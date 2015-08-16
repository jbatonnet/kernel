using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using EnvDTE;
using EnvDTE80;

namespace Tools
{
    using Process = System.Diagnostics.Process;

    public class VisualStudio
    {
        [DllImport("ole32.dll")]
        private static extern void CreateBindCtx(int reserved, out IBindCtx ppbc);

        [DllImport("ole32.dll")]
        private static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable prot);


        private static Regex DisplayNameRegex = new Regex("^!VisualStudio.DTE.(?<Version>[0-9]+.[0-9]+):(?<ProcessId>[0-9]+)", RegexOptions.Compiled);

        public static IEnumerable<VisualStudio> Instances
        {
            get
            {
                IRunningObjectTable rot;
                IEnumMoniker enumMoniker;

                int result = GetRunningObjectTable(0, out rot);
                if (result == 0)
                {
                    rot.EnumRunning(out enumMoniker);

                    IntPtr fetched = IntPtr.Zero;
                    IMoniker[] moniker = new IMoniker[1];

                    while (enumMoniker.Next(1, moniker, fetched) == 0)
                    {
                        IBindCtx bindCtx;
                        CreateBindCtx(0, out bindCtx);

                        string displayName;
                        moniker[0].GetDisplayName(bindCtx, null, out displayName);

                        Match match = DisplayNameRegex.Match(displayName);
                        if (!match.Success)
                            continue;

                        Version version = new Version(match.Groups["Version"].Value);

                        int processId = int.Parse(match.Groups["ProcessId"].Value);
                        Process process = Process.GetProcessById(processId);

                        object dte;
                        rot.GetObject(moniker[0], out dte);

                        yield return new VisualStudio(dte as DTE2, version, process);
                    }
                }
            }
        }

        public DTE2 Instance { get; private set; }
        public Version Version { get; private set; }
        public Process Process { get; private set; }

        internal VisualStudio(DTE2 instance, Version version, Process process)
        {
            Instance = instance;
            Version = version;
            Process = process;
        }
    }
}