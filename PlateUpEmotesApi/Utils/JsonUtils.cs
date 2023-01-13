using Newtonsoft.Json;

namespace PlateUpEmotesApi.Utils;

internal static class JsonUtils
{
    private static readonly JsonSerializer Serializer = JsonSerializer.CreateDefault();

    public static T Deserialize<T>(string json)
    {
        using var stringReader = new StringReader(json);
        using var reader = new JsonTextReader(stringReader);
        
        return Serializer.Deserialize<T>(reader);
    }
}