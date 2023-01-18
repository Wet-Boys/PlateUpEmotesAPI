using HarmonyLib;
using KitchenMods;

namespace PlateUpEmotesApi;

public class PlateUpEmotesApiMod : IModInitializer
{
    public const string AUTHOR = "wetboys";
    public const string MOD_NAME = "emotes_api";
    public const string MOD_ID = $"com.{AUTHOR}.{MOD_NAME}";
    
    public void PostActivate(Mod mod)
    {
        Harmony harmony = new Harmony(MOD_ID);
        
        harmony.PatchAll();
    }

    public void PreInject()
    {
    }

    public void PostInject()
    {
        
    }
}