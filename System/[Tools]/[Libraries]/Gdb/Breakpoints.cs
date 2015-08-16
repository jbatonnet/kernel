using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Gdb
{
    public enum GdbBreakpointType
    {
        Memory,
        Hardware,
        Write,
        Read,
        Access
    }

    public class GdbBreakpoint
    {
        public GdbBreakpointType Type { get; private set; }
        public ulong Address { get; private set; }
        public int Kind { get; private set; }

        public GdbBreakpoint(GdbBreakpointType type, ulong address) : this(type, address, 1) { }
        public GdbBreakpoint(GdbBreakpointType type, ulong address, int kind)
        {
            Type = type;
            Address = address;
            Kind = kind;
        }
    }
    public class GdbBreakpointHitData
    {
        public int Thread { get; private set; }
        public ulong Address { get; private set; }

        public GdbBreakpointHitData(int thread, ulong address)
        {
            Thread = thread;
            Address = address;
        }
    }

    public delegate void GdbBreakpointHitCallback(GdbStub gdbStub, GdbBreakpointHitData breakpointHitData);

    public partial class GdbStub
    {
        public class GdbBreakpointCollection : IEnumerable<GdbBreakpoint>
        {
            private GdbStub gdbStub;
            private List<GdbBreakpoint> breakpoints = new List<GdbBreakpoint>();

            public int Count
            {
                get
                {
                    return breakpoints.Count;
                }
            }

            internal GdbBreakpointCollection(GdbStub gdbStub)
            {
                this.gdbStub = gdbStub;
            }

            public void Add(GdbBreakpoint breakpoint)
            {
                string command, response = null;

                breakpoints.Add(breakpoint);

                switch (breakpoint.Type)
                {
                    case GdbBreakpointType.Memory:
                        command = string.Format("Z0,{0:x},{1:x}", breakpoint.Address, breakpoint.Kind);
                        response = gdbStub.Query(command);
                        break;

                    case GdbBreakpointType.Hardware:
                        command = string.Format("Z1,{0:x},{1:x}", breakpoint.Address, breakpoint.Kind);
                        response = gdbStub.Query(command);
                        break;

                    case GdbBreakpointType.Write:
                        command = string.Format("Z2,{0:x},{1:x}", breakpoint.Address, breakpoint.Kind);
                        response = gdbStub.Query(command);
                        break;

                    case GdbBreakpointType.Read:
                        command = string.Format("Z3,{0:x},{1:x}", breakpoint.Address, breakpoint.Kind);
                        response = gdbStub.Query(command);
                        break;

                    case GdbBreakpointType.Access:
                        command = string.Format("Z4,{0:x},{1:x}", breakpoint.Address, breakpoint.Kind);
                        response = gdbStub.Query(command);
                        break;
                }

                if (response != "OK")
                    throw new Exception("Could not add specified breakpoint");
            }
            public void Add(GdbBreakpointType type, ulong address)
            {
                GdbBreakpoint breakpoint = new GdbBreakpoint(type, address);
                Add(breakpoint);
            }
            public void Add(GdbBreakpointType type, ulong address, int kind)
            {
                GdbBreakpoint breakpoint = new GdbBreakpoint(type, address, kind);
                Add(breakpoint);
            }

            public bool Remove(GdbBreakpoint breakpoint)
            {
                string command, response = null;

                breakpoints.Remove(breakpoint);

                switch (breakpoint.Type)
                {
                    case GdbBreakpointType.Memory:
                        command = string.Format("z0,{0:x},{1:x}", breakpoint.Address, breakpoint.Kind);
                        response = gdbStub.Query(command);
                        break;

                    case GdbBreakpointType.Hardware:
                        command = string.Format("z1,{0:x},{1:x}", breakpoint.Address, breakpoint.Kind);
                        response = gdbStub.Query(command);
                        break;

                    case GdbBreakpointType.Write:
                        command = string.Format("z2,{0:x},{1:x}", breakpoint.Address, breakpoint.Kind);
                        response = gdbStub.Query(command);
                        break;

                    case GdbBreakpointType.Read:
                        command = string.Format("z3,{0:x},{1:x}", breakpoint.Address, breakpoint.Kind);
                        response = gdbStub.Query(command);
                        break;

                    case GdbBreakpointType.Access:
                        command = string.Format("z4,{0:x},{1:x}", breakpoint.Address, breakpoint.Kind);
                        response = gdbStub.Query(command);
                        break;
                }

                return response == "OK";
            }
            public void Clear()
            {
                foreach (GdbBreakpoint breakpoint in breakpoints)
                    Remove(breakpoint);
            }

            public IEnumerator<GdbBreakpoint> GetEnumerator()
            {
                return breakpoints.GetEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return breakpoints.GetEnumerator();
            }
        }
    }
}