using UnityEngine;
using HarmonyLib;
using SFS.Builds;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Linq;

public static class CustomPartMoveGrid_CucumberSP
{
    //Set grid size that you want
    const float grid_size = 0.1f;

    [HarmonyPatch(typeof(HoldGrid), "GetSnapPosition_Old")]
    public static class HoldGrid_GetSnapPosition_Old
    {
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            CodeInstruction[] codeInstructions = instructions.ToArray();
            for (int i = 0; i < codeInstructions.Length; i++)
                if (codeInstructions[i].opcode == OpCodes.Ldc_R4 && ((float)codeInstructions[i].operand) == 0.5f)
                    codeInstructions[i].operand = grid_size;
            return codeInstructions.AsEnumerable();
        }
    }

    public static void Load()
    {
        new Harmony("part_move_patch").PatchAll();
    }
}

//MainClassName: CustomPartMoveGrid_CucumberSP