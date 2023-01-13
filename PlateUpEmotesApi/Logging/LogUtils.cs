using PlateUpEmotesApi.Logging.Loggers;
using PlateUpEmotesApi.Logging.Sinks;

namespace PlateUpEmotesApi.Logging;

internal static class LogUtils
{
    private static readonly ILoggingSink[] DefaultSinks = { new UnityLoggingSink() };
    
    public static ILogger CreateDefaultLogger(string context)
    {
        return new ContextLogger(context, DefaultSinks);
    }

    public static ILogger CreateDefaultLogger()
    {
        return new Logger(DefaultSinks);
    }
}