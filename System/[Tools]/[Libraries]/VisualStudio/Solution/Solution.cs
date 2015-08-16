using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.VisualStudio
{
    public class Solution
    {
        IEnumerable<Project> Projects
        {
            get
            {
                return null;
            }
        }
    }

    public abstract class SolutionItem
    {
        public abstract string Name { get; }
        public abstract Guid Guid { get; }
    }
}