using HarmonyLib;
using Kitchen;
using PlateUpEmotesApi.Anim;
using PlateUpEmotesApi.Logging;
using PlateUpEmotesApi.Logging.Loggers;
// ReSharper disable InconsistentNaming

namespace PlateUpEmotesApi.Patches;

public static class GameCreatorPatches
{
    private static readonly ILogger Logger = LogUtils.CreateDefaultLogger("GameCreatorPatches");
    
    [HarmonyPatch(typeof(GameCreator), "PerformInitialSetup")]
    public static class PerformInitialSetupPatch
    {
        [HarmonyPostfix]
        public static void Postfix(AssetDirectory ___Directory)
        {
            Prefabs.PlayerPrefab = ___Directory.ViewPrefabs[ViewType.Player];

            if (Prefabs.PlayerPrefab is null)
                throw new NullReferenceException("Failed to get player prefab!!!!");
            
            AnimationReplacements.RunAll();
        }
    }
}