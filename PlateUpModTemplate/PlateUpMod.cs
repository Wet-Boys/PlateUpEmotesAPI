using HarmonyLib;
using UnityEngine;
using KitchenMods;

namespace PlateUpModTemplate;

public class PlateUpMod : IModInitializer
{
    public const string AUTHOR = "AUTHOR";
    public const string MOD_NAME = "MOD_NAME";
    public const string MOD_ID = $"com.{AUTHOR}.{MOD_NAME}";
    
    public void PostActivate(Mod mod)
    {
        Harmony.DEBUG = true;
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