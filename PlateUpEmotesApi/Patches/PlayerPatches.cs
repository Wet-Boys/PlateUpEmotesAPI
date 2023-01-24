using HarmonyLib;
using Kitchen;
using PlateUpEmotesApi.Emote;
using PlateUpEmotesApi.Input;
using PlateUpEmotesApi.Logging;
using PlateUpEmotesApi.Utils;
using Unity.Entities;
using ILogger = PlateUpEmotesApi.Logging.Loggers.ILogger;
// ReSharper disable InconsistentNaming

namespace PlateUpEmotesApi.Patches;

public static class PlayerPatches
{
    private static readonly ILogger Logger = LogUtils.CreateDefaultLogger("PlayerPatches");
    
    [HarmonyPatch(typeof(Player), "CreateFromEntity")]
    public class CreateFromEntityPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref Player __instance)
        {
            if (__instance.State == PlayerState.Null)
                return;
        
            EntityManager entityManager = __instance.GetProperty<EntityManager>("EntMan");
            entityManager.AddComponent<CEmoteInputData>(__instance.Entity);
            entityManager.AddComponent<CEmoteEnjoyer>(__instance.Entity);
        
            Logger.Debug($"Added Emote related components to player {__instance.Index}");
        }
    }
    
    [HarmonyPatch(typeof(Player), nameof(Player.CompleteJoining))]
    public class CompleteJoiningPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref Player __instance)
        {
            if (__instance.State == PlayerState.Null)
                return;

            EntityManager entityManager = __instance.GetProperty<EntityManager>("EntMan");
            entityManager.AddComponent<CEmoteInputData>(__instance.Entity);
            entityManager.AddComponent<CEmoteEnjoyer>(__instance.Entity);
            
            Logger.Debug($"Added Emote related components to player {__instance.Index}");
        }
    }
}