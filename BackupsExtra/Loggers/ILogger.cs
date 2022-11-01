namespace BackupsExtra.Loggers
{
    public interface ILogger
    {
        bool TimeLog { get; set; }
        void Log(string info);
    }
}