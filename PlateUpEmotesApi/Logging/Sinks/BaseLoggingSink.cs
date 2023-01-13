namespace PlateUpEmotesApi.Logging.Sinks;

internal abstract class BaseLoggingSink : ILoggingSink
{
    public abstract LogLevel MinLogLevel { get; }
    public bool ShouldHandle(LogLevel level) => level >= MinLogLevel;

    public abstract void Log(string message);
}