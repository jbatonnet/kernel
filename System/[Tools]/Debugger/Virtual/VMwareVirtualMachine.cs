using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Debugger
{
    public class VMwareVirtualMachine : VirtualMachine
    {
        public override bool Running
        {
            get
            {
                return RunCommand("list").Contains(Path);
            }
        }

        private const string type = "player"; // ws, player

        private Thread startMonitorThread;
        private Thread stopMonitorThread;

        public VMwareVirtualMachine(string path)
        {
            if (!File.Exists(path))
                throw new IOException("The specified file could not be found");

            Path = System.IO.Path.GetFullPath(path);

            startMonitorThread = new Thread(StartMonitor);
            startMonitorThread.Start();
        }

        public override void Start()
        {
            RunCommand("-T " + type + " start \"" + Path + "\"");
        }
        public override void Stop()
        {
            RunCommand("-T " + type + " stop \"" + Path + "\"");
        }
        public override void Restart()
        {
            stopMonitorThread.Abort();

            Stop();
            Start();
        }

        public override event EventHandler Started;
        public override event EventHandler Stopped;

        private List<string> RunCommand(string parameters)
        {
            string path = System.IO.Path.Combine(Program.Root, @"Tools\VMware\vmrun.exe");
            List<string> result = new List<string>();

            ProcessStartInfo processStartInfo = new ProcessStartInfo(path, parameters);
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;

            Process process = Process.Start(processStartInfo);
            while (!process.StandardOutput.EndOfStream)
                result.Add(process.StandardOutput.ReadLine());
            process.WaitForExit();

            return result;
        }

        private void StartMonitor()
        {
            bool lastState = Running;

            while (Thread.CurrentThread.ThreadState == System.Threading.ThreadState.Running)
            {
                bool state = Running;

                if (!lastState && state && Started != null)
                {
                    Started(this, new EventArgs());

                    stopMonitorThread = new Thread(StopMonitor);
                    stopMonitorThread.Start();
                }

                lastState = state;
                Thread.Sleep(1000);
            }
        }
        private void StopMonitor()
        {
            Process[] processes = Process.GetProcessesByName("vmware-vmx");
            Process process = processes.FirstOrDefault();
            if (process == null)
                return;

            while (Thread.CurrentThread.ThreadState == System.Threading.ThreadState.Running)
                if (process.WaitForExit(1000))
                {
                    Stopped(this, new EventArgs());
                    return;
                }
        }

        public override void Dispose()
        {
            if (startMonitorThread != null)
                startMonitorThread.Abort();
            if (stopMonitorThread != null)
                stopMonitorThread.Abort();
        }
    }
}