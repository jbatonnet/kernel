using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Gdb
{
    public partial class GdbStub
    {
        public class GdbRegisters
        {
            private GdbStub gdbStub;

            internal GdbRegisters(GdbStub gdbStub)
            {
                this.gdbStub = gdbStub;
            }

            protected ulong ReadRegister(int index)
            {
                if (gdbStub.Running)
                    throw new NotSupportedException("Cannot read register while running");

                string command = string.Format("p{0:x}", index);
                string response = gdbStub.Query(command);

                byte[] buffer = new byte[response.Length / 2];
                for (int i = 0; i < buffer.Length; i++)
                    buffer[i] = Convert.ToByte(response.Substring(i * 2, 2), 16);

                switch (buffer.Length)
                {
                    case 8: return BitConverter.ToUInt64(buffer, 0);
                    case 4: return BitConverter.ToUInt32(buffer, 0);
                    case 2: return BitConverter.ToUInt16(buffer, 0);
                }

                throw new Exception("Could not find register size");
            }
            protected void WriteRegister(int index, ulong value)
            {
                if (gdbStub.Running)
                    throw new NotSupportedException("Cannot read register while running");

                byte[] buffer = BitConverter.GetBytes(value);
                string valueBuffer = "";

                for (int i = 0; i < buffer.Length; i++)
                    valueBuffer += buffer[i].ToString("x2");

                string command = string.Format("P{0:x}={1:x}", index, valueBuffer);
                string response = gdbStub.Query(command);

                if (response != "OK")
                    throw new Exception("Unable to write regsiter");
            }
        }
    }
}