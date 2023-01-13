using Kitchen;
using PlateUpEmotesApi.Input;
using PlateUpEmotesApi.Logging;
using ILogger = PlateUpEmotesApi.Logging.Loggers.ILogger;

namespace PlateUpEmotesApi.Systems;

public static class EmoteRouterManager
{
    private static readonly ILogger Logger = LogUtils.CreateDefaultLogger("EmoteRouterManager");
    
    private static RouterManager? _instance;

    public static void SetInstance(RouterManager instance)
    {
        Logger.Debug("Updated RouterManager instance");
        _instance = instance;
    }
    
    public static void OnEmoteInputUpdate(object _, EmoteInputUpdateEvent e)
    {
        _instance?.CommandEntrypoint?.BroadcastCommand((EmoteInputUpdate)e);
    }
}