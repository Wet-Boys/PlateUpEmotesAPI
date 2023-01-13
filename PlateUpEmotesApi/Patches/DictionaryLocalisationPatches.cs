using HarmonyLib;
using KitchenData;
using PlateUpEmotesApi.Localization;
using PlateUpEmotesApi.Logging;
using PlateUpEmotesApi.Logging.Loggers;
// ReSharper disable InconsistentNaming

namespace PlateUpEmotesApi.Patches;

public static class DictionaryLocalisationPatches
{
    private static readonly ILogger Logger = LogUtils.CreateDefaultLogger("DictionaryLocalisationPatches");

    [HarmonyPatch(typeof(DictionaryLocalisation), "Localise")]
    public class InitialSetupPatch
    {
        [HarmonyPostfix]
        public static void Postfix(Locale locale, ref Dictionary<string, string> ___Text)
        {
            Logger.Debug("Loading locale data...");
            LocalizationData.LoadLocale(locale, ___Text);
        }
    }
}