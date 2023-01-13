using System.Reflection;
using HarmonyLib;
using Kitchen;
using PlateUpEmotesApi.Input;
using PlateUpEmotesApi.Players;
using PlateUpEmotesApi.Utils;
// ReSharper disable InconsistentNaming

namespace PlateUpEmotesApi.Patches;

public static class PerformCommandRouterPatches
{
    [HarmonyPatch]
    public class BroadcastCommandPatch
    {
        public static MethodInfo TargetMethod()
        {
            return typeof(PerformCommandRouter)
                .GetMethod("BroadcastCommand", BindingFlags.Instance | BindingFlags.Public)!
                .MakeGenericMethod(typeof(EmoteInputUpdate));
        }
    
        [HarmonyPostfix]
        public static void Postfix(ref PerformCommandRouter __instance, ref EmoteInputUpdate update)
        {
            var playerManager = __instance.GetField<PlayerManager>("PlayerManager");
            bool found = playerManager.GetPlayer(new InputIdentifier(update.SourceIdentifier, update.Data.User).PlayerID, 
                out var player, false, update.SourceIdentifier);

            if (!found)
                return;
        
            EmotePlayerManager.Instance?.ReportNewInput(player, update.Data.State);
        }
    }
}