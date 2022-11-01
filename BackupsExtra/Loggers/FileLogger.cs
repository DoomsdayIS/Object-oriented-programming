using System;
using System.IO;
using BackupsExtra.Tools;

namespace BackupsExtra.Loggers
{
    public class FileLogger : ILogger
    {
        public FileLogger(string path = null)
        {
            if (path != null)
            {
                Path = path;
            }

            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
        }

        public bool TimeLog { get; set; } = false;
        public string Path { get; private set; } = "/Users/dsivan/RiderProjects/DoomsdayIS/BackupsExtra/log.txt";

        public void Log(string info)
        {
            try
            {
                var sw = new StreamWriter(Path, true);
                sw.WriteLine(TimeLog ? $"{DateTime.Now} {info}" : $"{info}");
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception while logging: " + e.Message);
            }
        }
    }
}