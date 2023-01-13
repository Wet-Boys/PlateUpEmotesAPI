using PlateUpEmotesApi.Logging.Sinks;

namespace PlateUpEmotesApi.Logging.Loggers;

internal class Logger : ILogger
{
    private readonly ILoggingSink[] _sinks;

    public Logger(ILoggingSink[] sinks)
    {
        _sinks = sinks;
    }

    public virtual void Log(LogLevel level, string format, params object[] args)
    {
        foreach (var sink in _sinks)
        {
            if (!sink.ShouldHandle(level))
                continue;

            format = $"[PlateUpEmotesApi][{LogLevelAsString(level)}]{format}";
            string message = string.Format(format, args);
            
            sink.Log(message);
        }
    }

    public void Debug(string format, params object[] args) => 
        Log(LogLevel.Debug, format, args);

    public void Debug(string message) =>
        Log(LogLevel.Debug, message);

    public void Info(string format, params object[] args) => 
        Log(LogLevel.Info, format, args);

    public void Info(string message) => 
        Log(LogLevel.Info, message);

    public void Warn(string format, params object[] args) => 
        Log(LogLevel.Warn, format, args);

    public void Warn(string message) => 
        Log(LogLevel.Warn, message);

    public void Error(string format, params object[] args) => 
        Log(LogLevel.Error, format, args);

    public void Error(string message) => 
        Log(LogLevel.Error, message);

    private string LogLevelAsString(LogLevel level)
    {
        return level switch
        {
            LogLevel.Debug => "Dbg",
            LogLevel.Info => "Inf",
            LogLevel.Warn => "Wrn",
            LogLevel.Error => "Err",
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }
}