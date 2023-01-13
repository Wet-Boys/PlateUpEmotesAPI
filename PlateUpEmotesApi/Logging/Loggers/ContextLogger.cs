using PlateUpEmotesApi.Logging.Sinks;

namespace PlateUpEmotesApi.Logging.Loggers;

internal class ContextLogger : Logger
{
    private readonly string _context;
    
    public ContextLogger(string context, ILoggingSink[] sinks) : base(sinks)
    {
        _context = context;
    }

    public override void Log(LogLevel level, string format, params object[] args)
    {
        format = $"[{_context}] {format}";
        base.Log(level, format, args);
    }
}