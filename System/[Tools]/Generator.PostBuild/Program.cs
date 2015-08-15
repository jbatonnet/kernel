using System;
using System.Collections.Generic;
using System.Generator;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Debugger
{
    public static class Program
    {
        public static string Root { get; private set; }
        public static VirtualMachine VirtualMachine { get; private set; }

        static void Main(string[] args)
        {
            Console.WriteLine("Generator.PostBuild: Begin");

            // Find the project root
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            while (true)
            {
                if (Directory.Exists(Path.Combine(path, "Root")) && Directory.Exists(Path.Combine(path, "System")) && Directory.Exists(Path.Combine(path, "Tools")) && Directory.Exists(Path.Combine(path, "VMware")))
                    break;

                if (path.Length == 2)
                {
                    MessageBox.Show("Unable to find project root", "Generator.PostBuild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                path = Path.GetDirectoryName(path);
            }
            Root = path;

            // Construct environment and run the interface
            using (VirtualMachine = new VMwareVirtualMachine(Path.Combine(Root, @"VMware\System.vmx")))
                Output.Generate();

            //Wrappers.Generate();

            Console.WriteLine("Generator.PostBuild: End");
        }
    }
}