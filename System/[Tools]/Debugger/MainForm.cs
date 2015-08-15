using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Debugger;

using Tools.Gdb;
using Tools.Pdb;

namespace Tools.Debugger
{
    public partial class MainForm : Form
    {
        public class Frame
        {
            public uint Eip { get; private set; }
            public uint Ebp { get; private set; }
            public PdbSymbol Function { get; private set; }

            public Frame(uint eip, uint ebp, PdbSymbol function)
            {
                Eip = eip;
                Ebp = ebp;
                Function = function;
            }
        }

        public VirtualMachine VirtualMachine { get; private set; }
        public NamedPipeServerStream SerialStream { get; private set; }
        public PdbFile PdbFile { get; private set; }
        public PdbSession PdbSession { get; private set; }
        public x86GdbStub Gdb { get; private set; }
        public VisualStudio VisualStudio { get; private set; }

        private PdbSymbol debuggerAttachedField, debuggerBreakFunction;

        private bool freshStart = false;
        private bool restartingVM = false;
        private bool vmRunning = false;

        private long address = 0;

        public MainForm()
        {
            InitializeComponent();

            // Load everything
            LoadSerial();
            LoadSymbols();
            LoadVS();
            
            // Load virtual machine
            VirtualMachine = new VMwareVirtualMachine(Path.Combine(Program.Root, @"VMware\System.vmx"));
            VirtualMachine.Started += VirtualMachine_Started;
            VirtualMachine.Stopped += VirtualMachine_Stopped;
            freshStart = !VirtualMachine.Running;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            bool isWindows10 = Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 2;

            Left = Screen.PrimaryScreen.WorkingArea.Right - Width - (isWindows10 ? 0 : 8);
            Top = 8;
            Height = Screen.PrimaryScreen.WorkingArea.Height - 8 - (isWindows10 ? 0 : 8);

            // Start virtual machine if needed
            if (Program.Parameters.ContainsKey("start"))
            {
                if (!freshStart)
                    VirtualMachine.Stop();
                VirtualMachine.Start();

                freshStart = true;
            }

            // Trigger events if needed
            if (!freshStart)
                VirtualMachine_Started(null, null);

            // Some missing events
            MemoryBox.MouseWheel += MemoryBox_MouseWheel;
            MemoryBox.AutoWordSelection = false;

            // Clean interface
            OnClear();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.Parameters.ContainsKey("start"))
            {
                if (Gdb != null && !Gdb.Running)
                    Gdb.Continue();

                VirtualMachine.Stop();
            }

            Application.Exit();
        }

        private void LoadVS()
        {
            VisualStudio = null;

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    VisualStudio = VisualStudio.Instances.FirstOrDefault(vs => vs.Instance.Solution != null && vs.Instance.Solution.FullName.EndsWith("System.sln"));
                }
                catch
                {
                    continue;
                }
            }

            if (VisualStudio == null)
                throw new Exception("Could not get Visual Studio instance");
        }
        private void LoadSerial()
        {
            SerialStream = new NamedPipeServerStream("System", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
            SerialStream.BeginWaitForConnection(SerialStream_WaitForConnection, null);
        }
        private void LoadSymbols()
        {
            string pdbPath = Path.Combine(Program.Root, @"System\Kernel\Kernel.pdb");
            if (!File.Exists(pdbPath))
                throw new Exception("Could not find kernel symbols");

            // Load and open PDB session
            PdbFile = new PdbFile(pdbPath);
            PdbSession = PdbFile.OpenSession(0x100000);
        }
        private void LoadGDB()
        {
            Gdb = new x86GdbStub();
            Gdb.BreakpointHit += Gdb_BreakpointHit;

            if (Gdb.Running)
                Gdb.Break();

            if (freshStart)
            {
                // Wait for OS to start
                Gdb.Continue();
                Thread.Sleep(1750);
                Gdb.Break();
            }

            // Load common symbols and wrappers
            LoadWrappers();

            // Replace OS breakpoint with GDB one
            Gdb.Breakpoints.Add(GdbBreakpointType.Memory, debuggerBreakFunction.VirtualAddress);
            
            // Tell the OS we are attached
            Gdb.Memory.Write(debuggerAttachedField.VirtualAddress, 0x01);

            // TODO: Replace VS breakpoints with GDB ones
            foreach (EnvDTE.Breakpoint breakpoint in VisualStudio.Instance.Debugger.Breakpoints)
                breakpoint.ToString();

            // Resume OS load
            bool running = Gdb.Running;
            Gdb.Continue();
        }
        private void LoadWrappers()
        {
            // Debugger symbols
            debuggerAttachedField = PdbSession.Global.FindChildren(PdbSymbolTag.Null, "Debugger::attached").Single();
            debuggerBreakFunction = PdbSession.Global.FindChildren(PdbSymbolTag.Function, "Debugger::Break").Single();
        }

        private void Gdb_BreakpointHit(GdbStub gdbStub, GdbBreakpointHitData breakpointHitData)
        {
            ulong address = breakpointHitData.Address;

            // Debugger::Break() function
            if (address == debuggerBreakFunction.VirtualAddress)
            {
                Gdb.Registers.Eip++;
                Gdb.Step();
            }

            // TODO: Autoselect right frame
            Invoke(new MethodInvoker(() =>
            {
                Activate();

                OnUpdateControls();
                OnUpdate();
            }));
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            if (Gdb.Running)
                return;

            Gdb.Continue();

            OnUpdateControls();
        }
        private void BreakButton_Click(object sender, EventArgs e)
        {
            if (!Gdb.Running)
                return;

            Gdb.Break();

            OnUpdateControls();
            OnUpdate();
        }
        private void StepGdbButton_Click(object sender, EventArgs e)
        {
            if (Gdb.Running)
                return;

            Gdb.Step();

            OnUpdateControls();
            OnUpdate();
            
            PdbLineNumber line = PdbSession.FindLinesByVirtualAddress(Gdb.Registers.Eip, 1).FirstOrDefault();
            if (line == null)
                return;

            PdbSourceFile sourceFile = line.SourceFile;
            if (sourceFile == null)
                return;

            EnvDTE.Window window = VisualStudio.Instance.ItemOperations.OpenFile(sourceFile.FileName);
            EnvDTE.TextSelection selection = window.Document.Selection as EnvDTE.TextSelection;
            if (selection == null)
                return;

            selection.GotoLine((int)line.LineNumber);
        }
        private void StepLineButton_Click(object sender, EventArgs e)
        {
            if (Gdb.Running)
                return;

            PdbLineNumber origin, line;
            origin = PdbSession.FindLinesByVirtualAddress(Gdb.Registers.Eip, 1).FirstOrDefault();

            while (true)
            {
                Gdb.Step();

                line = PdbSession.FindLinesByVirtualAddress(Gdb.Registers.Eip, 1).FirstOrDefault();
                if (line == null)
                    continue;

                if (origin == null && line != null)
                    break;
                if (line.SourceFileId != origin.SourceFileId)
                    break;
                if (line.LineNumber != origin.LineNumber || line.ColumnNumber != origin.ColumnNumber)
                    break;
            }
            
            OnUpdateControls();
            OnUpdate();

            PdbSourceFile sourceFile = line.SourceFile;
            if (sourceFile == null)
                return;

            EnvDTE.Window window = VisualStudio.Instance.ItemOperations.OpenFile(sourceFile.FileName);
            EnvDTE.TextSelection selection = window.Document.Selection as EnvDTE.TextSelection;
            if (selection == null)
                return;

            selection.GotoLine((int)line.LineNumber);
        }
        private void StepOverButton_Click(object sender, EventArgs e)
        {
            if (Gdb.Running)
                return;

            List<PdbLineNumber> lines = PdbSession.FindLinesByVirtualAddress(Gdb.Registers.Eip, 100).ToList();
            if (lines.Count < 1)
            {
                StepLineButton_Click(sender, e);
                return;
            }

            PdbLineNumber currentLine = lines.First();
            PdbLineNumber nextLine = currentLine == null ? null : lines.FirstOrDefault(l => l.SourceFileId == currentLine.SourceFileId && l.VirtualAddress > currentLine.VirtualAddress);
            if (nextLine == null)
            {
                StepLineButton_Click(sender, e);
                return;
            }

            PdbLineNumber line = null;
            while (true)
            {
                Gdb.Step();

                line = PdbSession.FindLinesByVirtualAddress(Gdb.Registers.Eip, 1).FirstOrDefault();
                if (line == null)
                    continue;

                if (line.SourceFileId == nextLine.SourceFileId && line.LineNumber == nextLine.LineNumber && line.ColumnNumber == nextLine.ColumnNumber)
                    break;
            }

            OnUpdateControls();
            OnUpdate();

            PdbSourceFile sourceFile = line.SourceFile;
            if (sourceFile == null)
                return;

            EnvDTE.Window window = VisualStudio.Instance.ItemOperations.OpenFile(sourceFile.FileName);
            EnvDTE.TextSelection selection = window.Document.Selection as EnvDTE.TextSelection;
            if (selection == null)
                return;

            selection.GotoLine((int)line.LineNumber);
        }
        private void StartVMButton_Click(object sender, EventArgs e)
        {
            VirtualMachine.Start();
        }
        private void StopVMButton_Click(object sender, EventArgs e)
        {
            if (!Gdb.Running)
                Gdb.Dispose();

            VirtualMachine.Stop();
        }
        private void RestartVMButton_Click(object sender, EventArgs e)
        {
            restartingVM = true;

            if (!Gdb.Running)
                Gdb.Dispose();

            DebuggerWorker.CancelAsync();
            VirtualMachine.Restart();

            restartingVM = false;
        }

        private void TaskList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Gdb.Running)
            {
                Invoke(new MethodInvoker(() =>
                {
                    OnUpdateCallstack();
                    OnUpdateRegisters();
                    OnUpdateVariables();
                }));
            }
        }
        private void FrameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Gdb.Running)
            {
                Invoke(new MethodInvoker(() =>
                {
                    OnUpdateRegisters();
                    OnUpdateVariables();
                }));
            }
        }
        private void MemoryBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (Gdb.Running)
                return;

            int delta = e.Delta / 40;
            address -= 8 * delta;

            if (address < 0)
                address = 0;
            if (address > 0xFFFFFFFF)
                address = 0xFFFFFFFF;

            OnUpdateMemory();
        }

        private void FrameList_DoubleClick(object sender, EventArgs e)
        {
            Frame selectedFrame = FrameList.SelectedItems.Count == 0 ? null : FrameList.SelectedItems[0].Tag as Frame;
            if (selectedFrame == null)
                return;

            PdbLineNumber line = PdbSession.FindLinesByVirtualAddress(selectedFrame.Eip, 1).FirstOrDefault();
            if (line == null)
                return;

            PdbSourceFile sourceFile = line.SourceFile;
            if (sourceFile == null)
                return;

            EnvDTE.Window window = VisualStudio.Instance.ItemOperations.OpenFile(sourceFile.FileName);
            EnvDTE.TextSelection selection = window.Document.Selection as EnvDTE.TextSelection;
            if (selection == null)
                return;

            selection.GotoLine((int)line.LineNumber);
        }
        private void RegisterList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = RegisterList.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
            if (item == null)
                return;

            uint address = 0;
            if (!uint.TryParse(item.SubItems[1].Text.Substring(2), NumberStyles.HexNumber, null, out address))
                return;

            this.address = address - (address % 8);
            Invoke(new MethodInvoker(OnUpdateMemory));
        }
        private void VariableList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = VariableList.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
            if (item == null)
                return;

            if (!item.SubItems[2].Text.EndsWith("*"))
                return;

            uint address = 0;
            if (!uint.TryParse(item.SubItems[1].Text.Substring(2), NumberStyles.HexNumber, null, out address))
                return;

            this.address = address - (address % 8);

            Invoke(new MethodInvoker(() =>
            {
                Tabs.SelectedIndex = 1;
                OnUpdateMemory();
            }));
        }

        byte[] buffer = new byte[1024];
        string line = "";

        private void SerialStream_WaitForConnection(IAsyncResult asyncResult)
        {
            try
            {
                SerialStream.EndWaitForConnection(asyncResult);
                SerialStream.BeginRead(buffer, 0, 1, SerialStream_Read, null);
            }
            catch { }
        }
        private void SerialStream_Read(IAsyncResult asyncResult)
        {
            int count = SerialStream.EndRead(asyncResult);
            if (count != 1)
            {
                try
                {
                    SerialStream.Close();
                    Gdb.Dispose();
                }
                catch { }

                SerialStream = new NamedPipeServerStream("System", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                SerialStream.BeginWaitForConnection(SerialStream_WaitForConnection, null);

                return;
            }

            SerialStream_ReadByte(buffer[0]);

            SerialStream.BeginRead(buffer, 0, 1, SerialStream_Read, null);
        }
        private void SerialStream_ReadByte(byte value)
        {
            // Carriage return
            if (value == '\r')
                return;

            // Full line
            if (value == '\n')
            {
                Log.Message(line);
                line = "";
                return;
            }

            // Simple character
            line += new string((char)value, 1).Replace("{", "{{").Replace("}", "}}");
        }

        private void VirtualMachine_Started(object sender, EventArgs e)
        {
            vmRunning = true;
            OnUpdateControls();
            
            LoadGDB();
            OnClear();
            DebuggerWorker.RunWorkerAsync();
        }
        private void VirtualMachine_Stopped(object sender, EventArgs e)
        {
            vmRunning = false;
            OnUpdateControls();

            DebuggerWorker.CancelAsync();

            try
            {
                SerialStream.Close();
            }
            catch { }

            try
            {
                Gdb.Dispose();
            }
            finally
            { 
                Gdb = null;
            }

            if (restartingVM)
                return;

            if (Program.Parameters.ContainsKey("start"))
                Application.Exit();
        }

        private void DebuggerWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!DebuggerWorker.CancellationPending)
            {
                Thread.Sleep(500);

                bool running = Gdb.Running;

                // Update controls
                OnUpdateControls();

                if (!running)
                    continue;

                /*Gdb.Break();

                // Update everything
                Invoke(new MethodInvoker(() => OnUpdate()));

                Gdb.Continue();*/
            }
        }

        private void OnClear()
        {
            try
            {
                Invoke(new MethodInvoker(() =>
                {
                    FrameList.Items.Clear();
                    VariableList.Items.Clear();
                    RegisterList.Items.Clear();
                }));
            }
            catch { }
        }
        private void OnUpdate()
        {
            OnUpdateCallstack();
            OnUpdateRegisters();
            OnUpdateMemory();
            OnUpdateVariables();
        }

        private void OnUpdateControls()
        {
            bool gdbRunning = Gdb != null && Gdb.Running;

            try
            {
                Invoke(new MethodInvoker(() =>
                {
                    ContinueButton.Enabled = vmRunning && !gdbRunning;
                    BreakButton.Enabled = vmRunning && gdbRunning;
                    StepGdbButton.Enabled = vmRunning && !gdbRunning;
                    StepLineButton.Enabled = vmRunning && !gdbRunning;
                    StepOverButton.Enabled = vmRunning && !gdbRunning;

                    StartVMButton.Enabled = !vmRunning;
                    StopVMButton.Enabled = vmRunning;
                    RestartVMButton.Enabled = vmRunning;
                }));
            }
            catch { }
        }
        private void OnUpdateCallstack()
        {
            string selectedFrame = FrameList.SelectedItems.Count == 0 ? null : FrameList.SelectedItems[0].Text;
            ListViewItem selectedItem = null;

            FrameList.Items.Clear();

            CallstackLabel.Text = "Callstack";

            uint eip = Gdb.Registers.Eip;
            uint ebp = Gdb.Registers.Ebp;

            // Add current method
            {
                PdbSymbol function = PdbSession.GetSymbolAtVirtualAddress(PdbSymbolTag.Function, eip);
                if (function == null)
                    return;

                Frame frame = new Frame(eip, ebp, function);
                ulong pointer = function.VirtualAddress;

                ListViewItem frameItem = new ListViewItem("0x" + pointer.ToString("X8"));

                frameItem.Tag = frame;
                frameItem.SubItems.Add(function.Name);
                frameItem.SubItems.Add("0x" + (eip - pointer).ToString("X"));

                FrameList.Items.Add(frameItem);

                if (selectedFrame != null && selectedFrame == frameItem.Text)
                    selectedItem = frameItem;
            }

            uint stackPointer = ebp;

            // Decode frames
            while (true)
            {
                uint ebp2 = Gdb.Memory.ReadUInt32(stackPointer);
                uint ret = Gdb.Memory.ReadUInt32(stackPointer + 4);

                if (ret == 0)
                    break;

                PdbSymbol function = PdbSession.GetSymbolAtVirtualAddress(PdbSymbolTag.Function, ret);
                if (function == null)
                    break;

                Frame frame = new Frame(ret, ebp2, function);
                ulong pointer = function.VirtualAddress;

                ListViewItem frameItem = new ListViewItem("0x" + pointer.ToString("X8"));

                frameItem.Tag = frame;
                frameItem.SubItems.Add(function.Name);
                frameItem.SubItems.Add("0x" + (ret - pointer).ToString("X"));

                FrameList.Items.Add(frameItem);

                if (selectedFrame != null && selectedFrame == frameItem.Text)
                    selectedItem = frameItem;

                stackPointer = ebp2;
            }

            if (selectedItem != null)
                FrameList.SelectedIndices.Add(FrameList.Items.IndexOf(selectedItem));
        }
        private void OnUpdateRegisters()
        {
            RegisterList.Items.Clear();

            ListViewItem item;

            item = RegisterList.Items.Add("EAX");
            item.SubItems.Add("0x" + Gdb.Registers.Eax.ToString("X8"));
            item = RegisterList.Items.Add("ECX");
            item.SubItems.Add("0x" + Gdb.Registers.Ecx.ToString("X8"));
            item = RegisterList.Items.Add("EDX");
            item.SubItems.Add("0x" + Gdb.Registers.Edx.ToString("X8"));
            item = RegisterList.Items.Add("EBX");
            item.SubItems.Add("0x" + Gdb.Registers.Ebx.ToString("X8"));

            item = RegisterList.Items.Add("ESP");
            item.SubItems.Add("0x" + Gdb.Registers.Esp.ToString("X8"));
            item = RegisterList.Items.Add("EBP");
            item.SubItems.Add("0x" + Gdb.Registers.Ebp.ToString("X8"));
            item = RegisterList.Items.Add("ESI");
            item.SubItems.Add("0x" + Gdb.Registers.Esi.ToString("X8"));
            item = RegisterList.Items.Add("EDI");
            item.SubItems.Add("0x" + Gdb.Registers.Edi.ToString("X8"));

            item = RegisterList.Items.Add("EIP");
            item.SubItems.Add("0x" + Gdb.Registers.Eip.ToString("X8"));
            item = RegisterList.Items.Add("EFLAGS");
            item.SubItems.Add("0x" + Gdb.Registers.Eflags.ToString("X8"));

            item = RegisterList.Items.Add("CS");
            item.SubItems.Add("0x" + Gdb.Registers.Cs.ToString("X8"));
            item = RegisterList.Items.Add("SS");
            item.SubItems.Add("0x" + Gdb.Registers.Ss.ToString("X8"));
            item = RegisterList.Items.Add("DS");
            item.SubItems.Add("0x" + Gdb.Registers.Ds.ToString("X8"));
            item = RegisterList.Items.Add("ES");
            item.SubItems.Add("0x" + Gdb.Registers.Es.ToString("X8"));
            item = RegisterList.Items.Add("FS");
            item.SubItems.Add("0x" + Gdb.Registers.Fs.ToString("X8"));
            item = RegisterList.Items.Add("GS");
            item.SubItems.Add("0x" + Gdb.Registers.Gs.ToString("X8"));
        }
        private void OnUpdateMemory()
        {
            MemoryLabel.Text = "Memory - 0x" + address.ToString("X8");

            int height = MemoryBox.Height / MemoryBox.Font.Height;

            ulong begin = (ulong)Math.Max(0, address - 8 * height / 2);
            ulong end = (ulong)Math.Min(0xFFFFFFFF, address + 8 * (height - height / 2));

            byte[] buffer = new byte[end - begin];
            Gdb.Memory.Read(begin, buffer, 0, buffer.Length);

            StringBuilder memory = new StringBuilder();

            for (ulong a = begin; a < end; a += 8)
            {
                byte[] bytes = buffer.Skip((int)((long)a - (long)begin)).Take(8).ToArray();
                string repr = new string(bytes.Select(b => (char)b).Select(c => c < 0x20 || c > 0x7F ? '.' : c).ToArray());

                memory.AppendLine(string.Format("{0:X8}  {1}  {2}", a, string.Join(" ", bytes.Select(b => b.ToString("X2"))), repr));
            }

            MemoryBox.Text = memory.ToString();
        }
        private void OnUpdateVariables()
        {
            VariableList.Items.Clear();

            Frame selectedFrame = FrameList.SelectedItems.Count == 0 ? null : FrameList.SelectedItems[0].Tag as Frame;

            VariablesLabel.Text = "Variables";
            if (selectedFrame != null)
                VariablesLabel.Text += " - " + selectedFrame.Function.Name;

            uint eip = selectedFrame != null ? selectedFrame.Eip : Gdb.Registers.Eip;
            uint ebp = selectedFrame != null ? selectedFrame.Ebp : Gdb.Registers.Ebp;

            PdbSymbol function = PdbSession.GetSymbolAtVirtualAddress(PdbSymbolTag.Function, eip);
            if (function == null)
                return;

            foreach (PdbSymbol variable in function.FindChildren(PdbSymbolTag.Data))
            {
                ListViewItem variableItem = new ListViewItem(variable.Name);

                PdbSymbol variableType = variable.Type;
                if (variableType == null)
                    continue;

                int offset = variable.Offset;
                ulong size = variableType.Length;

                byte[] buffer = new byte[size];
                Gdb.Memory.Read((ulong)(ebp + offset), buffer, 0, (int)size);

                Type type = GetTypeFromSymbol(variableType);
                string typeName = GetTypeNameFromSymbol(variableType);
                string value = "";

                if (type == null)
                    value = "";
                else if (type == typeof(string))
                    value = "{ String }";
                else if (type == typeof(bool))
                    value = buffer[0] != 0 ? "true" : "false";
                else if (type == typeof(sbyte))
                    value = ((sbyte)buffer[0]).ToString();
                else if (type == typeof(byte))
                    value = buffer[0].ToString();
                else if (type == typeof(short))
                    value = BitConverter.ToInt16(buffer, 0).ToString();
                else if (type == typeof(ushort))
                    value = BitConverter.ToUInt16(buffer, 0).ToString();
                else if (type == typeof(int))
                    value = BitConverter.ToInt32(buffer, 0).ToString();
                else if (type == typeof(uint))
                    value = BitConverter.ToUInt32(buffer, 0).ToString();
                else if (type == typeof(long))
                    value = BitConverter.ToInt64(buffer, 0).ToString();
                else if (type == typeof(ulong))
                    value = BitConverter.ToUInt64(buffer, 0).ToString();
                else if (type == typeof(IntPtr))
                    value = "0x" + BitConverter.ToUInt32(buffer, 0).ToString("x8");

                variableItem.SubItems.Add(value);
                variableItem.SubItems.Add(typeName);
                variableItem.SubItems.Add(string.Join(" ", buffer.Select(b => b.ToString("X2"))));

                VariableList.Items.Add(variableItem);
            }
        }

        public Type GetTypeFromSymbol(PdbSymbol type)
        {
            PdbSymbolTag tag = type.SymTag;

            if (tag == PdbSymbolTag.PointerType)
                return typeof(IntPtr);
            else if (tag == PdbSymbolTag.BaseType)
            {
                ulong size = type.Length;
                PdbSymbolBaseType baseType = type.BaseType;

                switch (baseType)
                {
                    case PdbSymbolBaseType.Char: return typeof(char);
                    case PdbSymbolBaseType.WChar: return typeof(char);
                    case PdbSymbolBaseType.Int: return size == 2 ? typeof(short) : size == 4 ? typeof(int) : size == 8 ? typeof(long) : null;
                    case PdbSymbolBaseType.UInt: return size == 2 ? typeof(ushort) : size == 4 ? typeof(uint) : size == 8 ? typeof(ulong) : null;
                    case PdbSymbolBaseType.Float: return typeof(float);
                    case PdbSymbolBaseType.Bool: return typeof(bool);
                    case PdbSymbolBaseType.Long: return typeof(long);
                    case PdbSymbolBaseType.ULong: return typeof(ulong);
                }

                return null;
            }
            else if (tag == PdbSymbolTag.ArrayType)
            {

            }
            else if (tag == PdbSymbolTag.FunctionType)
            {

            }
            else if (tag == PdbSymbolTag.CustomType)
            {

            }
            
            return null;
        }
        public string GetTypeNameFromSymbol(PdbSymbol type)
        {
            if (type == null)
                return "";

            PdbSymbolTag tag = type.SymTag;

            string name = type.Name;
            if (name != null)
                return name;

            if (tag == PdbSymbolTag.PointerType)
                return GetTypeNameFromSymbol(type.Type) + "*";
            else if (tag == PdbSymbolTag.BaseType)
            {
                ulong size = type.Length;
                PdbSymbolBaseType baseType = type.BaseType;

                switch (baseType)
                {
                    case PdbSymbolBaseType.Char: return "char";
                    case PdbSymbolBaseType.WChar: return "char";
                    case PdbSymbolBaseType.Int: return size == 2 ? "s16" : size == 4 ? "s32" : size == 8 ? "s64" : "";
                    case PdbSymbolBaseType.UInt: return size == 2 ? "u16" : size == 4 ? "u32" : size == 8 ? "u64" : "";
                    case PdbSymbolBaseType.Float: return "float";
                    case PdbSymbolBaseType.Bool: return "bool";
                    case PdbSymbolBaseType.Long: return "s64";
                    case PdbSymbolBaseType.ULong: return "u64";
                }

                return "";
            }
            else if (tag == PdbSymbolTag.ArrayType)
            {

            }
            else if (tag == PdbSymbolTag.FunctionType)
            {

            }
            else if (tag == PdbSymbolTag.CustomType)
            {

            }

            return "";
        }
    }
}