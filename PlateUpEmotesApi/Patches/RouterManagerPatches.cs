using HarmonyLib;
using Kitchen;
using PlateUpEmotesApi.Input;
using PlateUpEmotesApi.Logging;
using PlateUpEmotesApi.Logging.Loggers;

// ReSharper disable InconsistentNaming

namespace PlateUpEmotesApi.Patches;

public static class RouterManagerPatches
{
    private static readonly ILogger Logger = LogUtils.CreateDefaultLogger("RouterManagerPatches");
    
    [HarmonyPatch(typeof(RouterManager), nameof(RouterManager.AddInputSource))]
    public class AddInputSourcePatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref RouterManager __instance)
        {
            EmoteRouterManager.SetInstance(__instance);
        }
    }
}