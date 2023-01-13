using System.Reflection;
using System.Reflection.Emit;
using Controllers;
using HarmonyLib;
using PlateUpEmotesApi.Input;
using PlateUpEmotesApi.Utils;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
// ReSharper disable InconsistentNaming

namespace PlateUpEmotesApi.Patches;

public static class InputSourcePatches
{
    [HarmonyPatch]
    public class CtorPatch
    {
        public static MethodBase TargetMethod()
        {
            return typeof(InputSource).GetConstructor(Array.Empty<Type>())!;
        }

        [HarmonyPostfix]
        public static void Postfix(ref InputSource __instance)
        {
            new InputSourceProxy(__instance);
        }
    }

    [HarmonyPatch(typeof(InputSource), "Setup")]
    public class SetupPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref bool register_as_default)
        {
            InputSourceProxy.Instance.Setup(register_as_default);
        }
    }

    [HarmonyPatch(typeof(InputSource), "Destroy")]
    public class DestroyPatch
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            InputSourceProxy.Instance.Destroy();
        }
    }

    [HarmonyPatch(typeof(InputSource), "Update")]
    public class UpdatePatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();

            int firstIndex = codes.IndexBeforeAllOpcodes(OpCodes.Br, OpCodes.Ldarg_0, OpCodes.Ldloc_2, OpCodes.Ldloc_S,
                OpCodes.Call, OpCodes.Nop, OpCodes.Nop);
            int secondIndex = codes.IndexBeforeAllOpcodes(firstIndex, OpCodes.Ldarg_0, OpCodes.Ldloc_2, OpCodes.Ldloc_S,
                OpCodes.Call, OpCodes.Nop, OpCodes.Nop);

            if (firstIndex < 0 || secondIndex < 0)
                throw new Exception();

            MethodInfo getInstance = typeof(InputSourceProxy).GetProperty(nameof(InputSourceProxy.Instance))!.GetMethod;
            MethodInfo inputUpdate = typeof(InputSourceProxy).GetMethod(nameof(InputSourceProxy.InputSourceUpdate))!;

            var additionalCodes = new List<CodeInstruction>
            {
                new(OpCodes.Call, getInstance),
                new(OpCodes.Ldloc_2),
                new(OpCodes.Ldloc_S, 4),
                new(OpCodes.Callvirt, inputUpdate),
                new(OpCodes.Nop)
            };

            codes.InsertRange(firstIndex, additionalCodes);
            codes.InsertRange(secondIndex + additionalCodes.Count, additionalCodes);

            return codes;
        }
    }

    [HarmonyPatch(typeof(InputSource), "NewDeviceUsed")]
    public class NewDeviceUsedPatch
    {
        [HarmonyPostfix]
        public static void Postfix(InputControl control, InputEventPtr eventPtr)
        {
            InputSourceProxy.Instance.NewDeviceUsed(control, eventPtr);
        }
    }
}