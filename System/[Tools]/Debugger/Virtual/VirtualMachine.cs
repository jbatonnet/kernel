using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Debugger
{
    public abstract class VirtualMachine : IDisposable
    {
        public string Path { get; protected set; }
        public abstract bool Running { get; }

        public abstract void Start();
        public abstract void Stop();
        public virtual void Restart()
        {
            Stop();
            Start();
        }

        public abstract void Dispose();

        public abstract event EventHandler Started;
        public abstract event EventHandler Stopped;
    }
}