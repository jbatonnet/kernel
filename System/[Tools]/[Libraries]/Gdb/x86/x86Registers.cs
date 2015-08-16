using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Gdb
{
    public partial class x86GdbStub
    {
        public class x86GdbRegisters : GdbRegisters
        {
            internal x86GdbRegisters(GdbStub gdbStub) : base(gdbStub)
            {
            }

            public uint Eax
            {
                get
                {
                    return (uint)ReadRegister(0);
                }
                set
                {
                    WriteRegister(0, value);
                }
            }
            public uint Ecx
            {
                get
                {
                    return (uint)ReadRegister(1);
                }
                set
                {
                    WriteRegister(1, value);
                }
            }
            public uint Edx
            {
                get
                {
                    return (uint)ReadRegister(2);
                }
                set
                {
                    WriteRegister(2, value);
                }
            }
            public uint Ebx
            {
                get
                {
                    return (uint)ReadRegister(3);
                }
                set
                {
                    WriteRegister(3, value);
                }
            }

            public uint Esp
            {
                get
                {
                    return (uint)ReadRegister(4);
                }
                set
                {
                    WriteRegister(4, value);
                }
            }
            public uint Ebp
            {
                get
                {
                    return (uint)ReadRegister(5);
                }
                set
                {
                    WriteRegister(5, value);
                }
            }
            public uint Esi
            {
                get
                {
                    return (uint)ReadRegister(6);
                }
                set
                {
                    WriteRegister(6, value);
                }
            }
            public uint Edi
            {
                get
                {
                    return (uint)ReadRegister(7);
                }
                set
                {
                    WriteRegister(7, value);
                }
            }

            public uint Eip
            {
                get
                {
                    return (uint)ReadRegister(8);
                }
                set
                {
                    WriteRegister(8, value);
                }
            }
            public uint Eflags
            {
                get
                {
                    return (uint)ReadRegister(9);
                }
                set
                {
                    WriteRegister(9, value);
                }
            }

            public uint Cs
            {
                get
                {
                    return (uint)ReadRegister(10);
                }
                set
                {
                    WriteRegister(10, value);
                }
            }
            public uint Ss
            {
                get
                {
                    return (uint)ReadRegister(11);
                }
                set
                {
                    WriteRegister(11, value);
                }
            }
            public uint Ds
            {
                get
                {
                    return (uint)ReadRegister(12);
                }
                set
                {
                    WriteRegister(12, value);
                }
            }
            public uint Es
            {
                get
                {
                    return (uint)ReadRegister(13);
                }
                set
                {
                    WriteRegister(13, value);
                }
            }
            public uint Fs
            {
                get
                {
                    return (uint)ReadRegister(14);
                }
                set
                {
                    WriteRegister(14, value);
                }
            }
            public uint Gs
            {
                get
                {
                    return (uint)ReadRegister(15);
                }
                set
                {
                    WriteRegister(15, value);
                }
            }
        }
    }
}