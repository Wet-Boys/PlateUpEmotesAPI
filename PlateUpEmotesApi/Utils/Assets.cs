using System.Reflection;
using UnityEngine;

namespace PlateUpEmotesApi.Utils;

internal static class Assets
{
    private static readonly List<AssetBundle> AssetBundles = new();
    private static readonly Dictionary<string, int> AssetIndices = new();

    public static void AddBundle(string assetBundleLocation)
    {
        using var assetBundleStream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream($"{nameof(PlateUpEmotesApi)}.Assets.{assetBundleLocation}");

        if (assetBundleStream is null)
            throw new Exception($"Couldn't find embedded AssetBundle from path: {assetBundleLocation}");
        
        AssetBundle assetBundle = AssetBundle.LoadFromStream(assetBundleStream);
        
        int index = AssetBundles.Count;
        AssetBundles.Add(assetBundle);

        foreach (var assetName in assetBundle.GetAllAssetNames())
        {
            string path = assetName.ToLower();

            if (path.StartsWith("assets/"))
                path = path.Remove(0, "assets/".Length);

            AssetIndices[path] = index;
        }
    }

    public static T Load<T>(string assetName) where T : UnityEngine.Object
    {
        if (assetName.Contains(":"))
            assetName = assetName.Split(':')[1].ToLower();

        if (assetName.StartsWith("assets/"))
            assetName = assetName.Remove(0, "assets/".Length);

        int index = AssetIndices[assetName];
        return AssetBundles[index].LoadAsset<T>($"assets/{assetName}");
    }
}