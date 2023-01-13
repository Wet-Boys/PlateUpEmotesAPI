using UnityEngine;

namespace PlateUpEmotesApi.Logging.Sinks;

internal class UnityLoggingSink : BaseLoggingSink
{
    public override LogLevel MinLogLevel => LogLevel.Debug;
    
    public override void Log(string message) => Debug.Log(message);
}