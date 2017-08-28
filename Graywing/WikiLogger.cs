using System;
using System.Collections.Generic;
using System.Text;
using WikiClientLibrary;

namespace Graywing
{
    public class WikiLogger : ILogger
    {

        public static readonly WikiLogger Default = new WikiLogger();

        public void WriteLine(string severity, object source, string message)
        {
            Console.WriteLine("[{0}]{1} - {2}", severity, source, message);
        }

        public void Trace(object source, string message)
        {
            WriteLine("TRA", source, message);
        }

        public void Info(object source, string message)
        {
            WriteLine("INF", source, message);
        }

        public void Warn(object source, string message)
        {
            WriteLine("WAR", source, message);
        }

        public void Error(object source, Exception exception, string message)
        {
            WriteLine("ERR", source, message);
        }
    }
}
