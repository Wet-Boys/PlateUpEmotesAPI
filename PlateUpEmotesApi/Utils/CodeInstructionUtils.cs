using System.Reflection.Emit;
using HarmonyLib;

namespace PlateUpEmotesApi.Utils;

internal static class CodeInstructionUtils
{
    public static int IndexBeforeAllOpcodes(this IEnumerable<CodeInstruction> instructions, OpCode anchor, params OpCode[] opCodes)
    {
        List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

        bool anchorFound = false;
        int currentCondition = 0;
        
        int index = -1;
        bool foundAll = false;
        
        for (int i = 0; i < codes.Count; i++)
        {
            if (codes[i].opcode == anchor && !anchorFound)
            {
                anchorFound = true;
            }
            else if (codes[i].opcode == opCodes[currentCondition] && anchorFound)
            {
                if (currentCondition == 0)
                    index = i;
                
                currentCondition++;
            }
            else if (anchorFound)
            {
                currentCondition = 0;
            }

            if (currentCondition >= opCodes.Length && anchorFound)
            {
                foundAll = true;
                break;
            }
        }

        if (index < 0 || !foundAll)
            throw new Exception("Couldn't find index where all conditions match!");

        return index;
    }
    
    public static int IndexBeforeAllOpcodes(this IEnumerable<CodeInstruction> instructions, int offset, params OpCode[] opCodes)
    {
        List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

        int currentCondition = 0;
        
        int index = -1;
        bool foundAll = false;
        
        for (int i = offset; i < codes.Count; i++)
        {
            if (codes[i].opcode == opCodes[currentCondition])
            {
                if (currentCondition == 0)
                    index = i;
                
                currentCondition++;
            }
            else
            {
                currentCondition = 0;
            }

            if (currentCondition >= opCodes.Length)
            {
                foundAll = true;
                break;
            }
        }

        if (index < 0 || !foundAll)
            throw new Exception("Couldn't find index where all conditions match!");

        return index;
    }
}