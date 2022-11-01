using System;

namespace BackupsExtra.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public bool TimeLog { get; set; } = false;

        public void Log(string info)
        {
            Console.WriteLine(TimeLog ? $"{DateTime.Now} {info}" : $"{info}");
        }
    }
}