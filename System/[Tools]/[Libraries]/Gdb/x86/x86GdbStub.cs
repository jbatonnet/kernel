using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Gdb
{
    public partial class x86GdbStub : GdbStub
    {
        public new x86GdbRegisters Registers { get; private set; }

        public x86GdbStub() : this("127.0.0.1", 8832) { }
        public x86GdbStub(string host) : this(host, 8832) { }
        public x86GdbStub(string host, int port) : base(host, port)
        {
            Registers = new x86GdbRegisters(this);
        }

        protected override void OnNotification(string data)
        {
            //string thread = data.Substring(0, 3);
            string[] fields = data.Substring(3).TrimEnd(';').Split(';');

            int thread = 0;
            ulong address = 0;

            foreach (string field in fields)
            {
                string[] values = field.Split(':');

                if (values[0] == "thread")
                    int.TryParse(values[1], NumberStyles.HexNumber, null, out thread);
                if (values[0] == "08")
                    address = DecodeBuffer(values[1]);
            }

            OnBreakpointHit(new GdbBreakpointHitData(thread, address));
        }
    }
}