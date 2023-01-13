using HarmonyLib;
using Kitchen.Modules;
using PlateUpEmotesApi.Input;
using PlateUpEmotesApi.Localization;
using PlateUpEmotesApi.Utils;
// ReSharper disable InconsistentNaming

namespace PlateUpEmotesApi.Patches;

public static class ControlRebindElementPatches
{
    [HarmonyPatch(typeof(ControlRebindElement), nameof(ControlRebindElement.Setup))]
    public class SetupPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref ControlRebindElement __instance)
        {
            __instance.AddRebindOption(LocaleTokens.BindEmoteWheel, EmoteControls.EmotesWheel);

            PanelElement panel = __instance.GetField<PanelElement>("Panel");
            ModuleList modules = __instance.GetField<ModuleList>("ModuleList");
        
            panel.SetTarget(modules);
        }
    }
}