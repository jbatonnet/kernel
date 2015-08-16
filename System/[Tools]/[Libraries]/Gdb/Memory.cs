using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Gdb
{
    public partial class GdbStub
    {
        public class GdbMemory
        {
            private GdbStub gdbStub;

            internal GdbMemory(GdbStub gdbStub)
            {
                this.gdbStub = gdbStub;
            }

            public byte ReadUInt8(ulong address)
            {
                byte[] buffer = new byte[1];
                Read(address, buffer, 0, 1);
                return buffer[0];
            }
            public ushort ReadUInt16(ulong address)
            {
                byte[] buffer = new byte[2];
                Read(address, buffer, 0, 2);
                return BitConverter.ToUInt16(buffer, 0);
            }
            public uint ReadUInt32(ulong address)
            {
                byte[] buffer = new byte[4];
                Read(address, buffer, 0, 4);
                return BitConverter.ToUInt32(buffer, 0);
            }
            public ulong ReadUInt64(ulong address)
            {
                byte[] buffer = new byte[8];
                Read(address, buffer, 0, 8);
                return BitConverter.ToUInt64(buffer, 0);
            }

            public void Write(ulong address, byte value)
            {
                byte[] buffer = new byte[] { value };
                Write(address, buffer, 0, 1);
            }
            public void Write(ulong address, ushort value)
            {
                byte[] buffer = BitConverter.GetBytes(value);
                Write(address, buffer, 0, buffer.Length);
            }
            public void Write(ulong address, uint value)
            {
                byte[] buffer = BitConverter.GetBytes(value);
                Write(address, buffer, 0, buffer.Length);
            }
            public void Write(ulong address, ulong value)
            {
                byte[] buffer = BitConverter.GetBytes(value);
                Write(address, buffer, 0, buffer.Length);
            }

            public int Read(ulong address, byte[] buffer, int offset, int count)
            {
                if (gdbStub.Running)
                    throw new Exception("Cannot read memory while running");

                string command = string.Format("m{0:x},{1:x}", address, count);
                string response = gdbStub.Query(command);

                for (int i = 0; i < response.Length / 2; i++)
                    buffer[offset + i] = Convert.ToByte(response.Substring(i * 2, 2), 16);

                return response.Length / 2;
            }
            public void Write(ulong address, byte[] buffer, int offset, int count)
            {
                if (gdbStub.Running)
                    throw new Exception("Cannot write memory while running");

                StringBuilder command = new StringBuilder(count * 2 + 30);

                command.AppendFormat("M{0:x},{1:x}:", address, count);
                for (int i = 0; i < count; i++)
                    command.Append(buffer[offset + i].ToString("x2"));

                string response = gdbStub.Query(command.ToString());
            }
        }
    }
}