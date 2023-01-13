using Controllers;
using HarmonyLib;
using PlateUpEmotesApi.Input;
using PlateUpEmotesApi.Logging;
using PlateUpEmotesApi.Logging.Loggers;
using UnityEngine.InputSystem;
// ReSharper disable InconsistentNaming

namespace PlateUpEmotesApi.Patches;

public static class MapsPatches
{
    private static readonly ILogger Logger = LogUtils.CreateDefaultLogger("MapsPatches");
    
    [HarmonyPatch(typeof(Maps), "Actions")]
    public class ActionsPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref InputActionMap __result)
        {
            Logger.Debug("Adding EmotesWheel to default actions");
            __result.AddAction(EmoteControls.EmotesWheel, InputActionType.Button);
        }
    }

    [HarmonyPatch(typeof(Maps), nameof(Maps.NewGamepad))]
    public class NewGamepadPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref InputActionMap __result)
        {
            Logger.Debug("Adding default binds for Gamepad");
            __result.FindAction(EmoteControls.EmotesWheel).AddBinding("<Gamepad>/rightShoulder");
        }
    }

    [HarmonyPatch(typeof(Maps), nameof(Maps.NewKeyboard))]
    public class NewKeyboardPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref InputActionMap __result)
        {
            Logger.Debug("Adding default binds for Keyboard");
            __result.FindAction(EmoteControls.EmotesWheel).AddBinding("<Keyboard>/c");
        }
    }
}