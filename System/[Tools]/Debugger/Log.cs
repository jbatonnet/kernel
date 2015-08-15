using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Debugger
{
    public static class Log
    {
        public class LogMessage
        {
            public string Content { get; private set; }
            public Color Color { get; private set; } = Color.Black;

            public LogMessage(string content)
            {
                Content = content;
            }
            public LogMessage(string content, Color color)
            {
                Content = content;
                Color = color;
            }
        }
        public delegate void MessageCallback(LogMessage message);

        public static event Action Resetted;
        public static event MessageCallback MessageSent;

        public static void Reset()
        {
            if (Resetted != null)
                Resetted();
        }

        public static void Message(string format, params object[] args)
        {
            if (MessageSent != null)
                MessageSent(new LogMessage(string.Format(format, args), Color.Gray));
        }
        public static void Warning(string format, params object[] args)
        {
            if (MessageSent != null)
                MessageSent(new LogMessage(string.Format(format, args), Color.DarkKhaki));
        }
        public static void Error(string format, params object[] args)
        {
            if (MessageSent != null)
                MessageSent(new LogMessage(string.Format(format, args), Color.DarkRed));
        }
    }
}