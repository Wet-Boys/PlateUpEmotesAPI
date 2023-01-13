namespace PlateUpEmotesApi.Logging.Sinks;

internal interface ILoggingSink
{
    public LogLevel MinLogLevel { get; }

    public bool ShouldHandle(LogLevel level);
    
    public void Log(string message);
}