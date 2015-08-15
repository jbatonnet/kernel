using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using Tools;
using Tools.Debugger;

namespace Debugger
{
    public static class Program
    {
        public static Dictionary<string, string> Parameters { get; private set; }
        public static string Root { get; private set; }

        [STAThread]
        static void Main(params string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Understand parameters
            Parameters = string.Join(" ", args)
                               .Split(new char[] { '&', '/', '-' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(p => new { Parameter = p.Trim(), Separator = p.Trim().IndexOfAny(new char[] { ':', '=' }) })
                               .ToDictionary(p => p.Separator == -1 ? p.Parameter : p.Parameter.Substring(0, p.Separator), p => p.Separator == -1 ? null : p.Parameter.Substring(p.Separator + 1));

            // Find the project root
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            while (true)
            {
                if (Directory.Exists(Path.Combine(path, "Root")) && Directory.Exists(Path.Combine(path, "System")) && Directory.Exists(Path.Combine(path, "Tools")) && Directory.Exists(Path.Combine(path, "VMware")))
                    break;

                if (path.Length == 2)
                {
                    MessageBox.Show("Unable to find project root", "Debugger", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                path = Path.GetDirectoryName(path);
            }
            Root = path;

            // Run the debugger
            Application.Run(new MainForm());

            // Force close the program
            Environment.Exit(0);
        }
    }
}