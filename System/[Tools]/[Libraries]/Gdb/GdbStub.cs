using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Tools.Gdb
{
    public partial class GdbStub : IDisposable
    {
        public bool Running { get; private set; } = false;

        public event GdbBreakpointHitCallback BreakpointHit;

        public GdbBreakpointCollection Breakpoints { get; private set; }
        public GdbMemory Memory { get; private set; }
        public GdbRegisters Registers { get; private set; }

        private Socket gdbSocket;
        private NetworkStream gdbStream;
        private StreamReader gdbReader;
        private StreamWriter gdbWriter;

        private object gdbMutex = new object();
        private AutoResetEvent stopEvent = new AutoResetEvent(false);
        private bool stopPending = false;
        
        public GdbStub() : this("127.0.0.1", 8832) { }
        public GdbStub(string host) : this(host, 8832) { }
        public GdbStub(string host, int port)
        {
            gdbSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            gdbSocket.Connect(host, port);

            gdbStream = new NetworkStream(gdbSocket);
            gdbWriter = new StreamWriter(gdbStream);
            gdbWriter.AutoFlush = true;
            gdbReader = new StreamReader(gdbStream);
            
            Breakpoints = new GdbBreakpointCollection(this);
            Memory = new GdbMemory(this);
            Registers = new GdbRegisters(this);
        }
        
        private void WritePacket(string command)
        {
            byte checksum = unchecked((byte)command.Sum(c => (int)c));
            string packet = string.Format("${0}#{1:x2}", command, checksum);

            while (true)
            {
                // Send the request
                gdbWriter.Write(packet);

                // Read the acknowledgment
                char value = (char)gdbReader.Read();

                if (value == '+')
                    break;
                if (value == '-')
                    continue;

                throw new Exception("The stub did not answer correctly");
            }
        }
        private string ReadPacket(bool header = true)
        {
            char value;
            string response = null;

            while (true)
            {
                if (header)
                {
                    value = (char)gdbReader.Read();

                    if (value != '$')
                    {
                        gdbWriter.Write('-');
                        continue;
                    }
                }

                // Read the response
                StringBuilder responseBuilder = new StringBuilder();
                while (true)
                {
                    value = (char)gdbReader.Read();
                    if (value == '#')
                        break;

                    responseBuilder.Append(value);
                }

                response = responseBuilder.ToString();

                // Check the checksum
                string checksumString = new string(new[] { (char)gdbReader.Read(), (char)gdbReader.Read() });
                byte checksum = Convert.ToByte(checksumString, 16);
                if (checksum != unchecked((byte)response.Sum(c => (int)c)))
                {
                    gdbWriter.Write('-');
                    continue;
                }

                gdbWriter.Write('+');
                break;
            }

            return response;
        }

        protected void Execute(string command)
        {
            lock (gdbMutex)
                WritePacket(command);
        }
        protected string Query(string command)
        {
            lock (gdbMutex)
            {
                WritePacket(command);
                return ReadPacket();
            }
        }

        public void Continue()
        {
            if (Running)
                throw new NotSupportedException("Cannot continue while running");

            lock (gdbMutex)
            {
                WritePacket("c");
                Running = true;

                byte[] buffer = new byte[1];
                gdbStream.BeginRead(buffer, 0, 1, OnStop, null);
            }
        }
        public void Break()
        {
            if (!Running)
                throw new NotSupportedException("Cannot break when while stopped");

            lock (gdbMutex)
                gdbStream.WriteByte(0x03);

            lock (stopEvent)
                stopPending = true;
            stopEvent.WaitOne();
        }
        public void Step()
        {
            if (Running)
                throw new NotSupportedException("Cannot step when while running");

            string response = Query("s");
        }

        private void OnStop(IAsyncResult asyncResult)
        {
            string result = null;

            lock (gdbMutex)
            {
                try
                {
                    int count = gdbStream.EndRead(asyncResult);
                    if (count != 1)
                        return;
                }
                catch
                {
                    return;
                }

                Running = false;
                result = ReadPacket(false);

                bool pending = false;
                lock (stopEvent)
                    pending = stopPending;

                if (stopPending)
                {
                    stopEvent.Set();

                    lock (stopEvent)
                        stopPending = false;

                    return;
                }
            }

            OnNotification(result);
        }
        protected virtual void OnNotification(string data)
        {
            //string thread = data.Substring(0, 3);
            string[] fields = data.Substring(3).TrimEnd(';').Split(';');

            int thread = 0;

            foreach (string field in fields)
            {
                string[] values = field.Split(':');

                if (values[0] == "thread")
                    int.TryParse(values[1], out thread);
            }

            OnBreakpointHit(new GdbBreakpointHitData(thread, 0));
        }
        protected void OnBreakpointHit(GdbBreakpointHitData hitData)
        {
            if (BreakpointHit != null)
                BreakpointHit(this, hitData);
        }

        public void Dispose()
        {
            if (gdbSocket == null)
                return;

            try
            {
                if (gdbSocket.Connected)
                    gdbSocket.Disconnect(false);

                gdbSocket.Dispose();
            }
            finally
            {
                gdbSocket = null;
            }
        }

        protected static ulong DecodeBuffer(string data)
        {
            byte[] buffer = new byte[data.Length / 2];
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = Convert.ToByte(data.Substring(i * 2, 2), 16);

            switch (buffer.Length)
            {
                case 8: return BitConverter.ToUInt64(buffer, 0);
                case 4: return BitConverter.ToUInt32(buffer, 0);
                case 2: return BitConverter.ToUInt16(buffer, 0);
            }

            throw new NotSupportedException("Unable to decode buffer");
        }
    }
}