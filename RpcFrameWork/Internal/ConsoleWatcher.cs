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
                
            }
        }

        public void Error(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void ErrorFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void InfoFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void WarnFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
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
