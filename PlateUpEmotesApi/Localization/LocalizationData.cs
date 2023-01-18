using System.Reflection;
using KitchenData;
using PlateUpEmotesApi.Logging;
using PlateUpEmotesApi.Logging.Loggers;
using PlateUpEmotesApi.Utils;

namespace PlateUpEmotesApi.Localization;

public static class LocalizationData
{
    private static readonly ILogger Logger = LogUtils.CreateDefaultLogger("LocalizationData");

    private static readonly Dictionary<Locale, string> LocalePathMap = new()
    {
        { Locale.Default, "en_US.json" },
        { Locale.English, "en_US.json" },
        { Locale.BlankText, "en_US.json" },
        { Locale.French, "en_US.json" },
        { Locale.German, "en_US.json" },
        { Locale.Spanish, "en_US.json" },
        { Locale.Polish, "en_US.json" },
        { Locale.Russian, "en_US.json" },
        { Locale.PortugueseBrazil, "en_US.json" },
        { Locale.Japanese, "en_US.json" },
        { Locale.ChineseSimplified, "en_US.json" },
        { Locale.ChineseTraditional, "en_US.json" }
    };

    internal static void LoadLocale(Locale locale, Dictionary<string, string> text)
    {
        Logger.Debug($"Loading locale data for {locale.ToString()}");
        
        string localePath = LocalePathMap[locale];

        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{nameof(PlateUpEmotesApi)}.Locales.{localePath}");
        if (stream is null)
        {
            Logger.Error($"Couldn't find embedded locale asset {localePath}!");
            throw new NullReferenceException();
        }
        
        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        var data = JsonUtils.Deserialize<Dictionary<string, string>>(json);

        foreach (var (key, localizedString) in data)
        {
            string token = $"{PlateUpEmotesApiMod.MOD_ID}.{key}";
            text[token] = localizedString;
        }
    }
}