using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpcFrameWork.Internal
{
    public class ConsoleWatcher : IRabbitWatcher
    {
        private readonly object _consoleLock = new object();
        public bool IsDebugEnable { get; set; }

        public void DebugFormat(string format, params object[] args)
        {
            if (IsDebugEnable)
            {
                Write(ConsoleColor.Gray, format, args);
            }
        }

        public void Error(Exception exception)
        {
            Write(ConsoleColor.Red, exception.ToString());
        }

        public void ErrorFormat(string format, params object[] args)
        {
            Write(ConsoleColor.Red, format, args);
        }

        public void InfoFormat(string format, params object[] args)
        {
            Write(ConsoleColor.Green, format, args);
        }

        public void WarnFormat(string format, params object[] args)
        {
            Write(ConsoleColor.Yellow,for, args) ;
        }

        private void Write(ConsoleColor color, string format, params object[] args)
        {
            string msg;
            try
            {
                msg = string.Format(format, args);
            }
            catch (Exception)
            {
                msg = format;
            }
            lock (_consoleLock)
            {
                var backupColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine(msg);
                Console.ForegroundColor = backupColor;
            }
        }
    }
}
