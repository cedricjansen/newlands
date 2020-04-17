using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Harmony;
using UnityEngine;

namespace New_Lands.patches
{

    //TREE GENERATION PATCH
    // Increases the amount of placable trees.
    [HarmonyPatch(typeof(TreeSystem))]
    [HarmonyPatch("TryInit")]
    public static class TreeGenerationPatch
    {

        // treeLimit should be a number between 1 and 100 since it gets
        // multiplied with 1023 in the original code later on.  
        private static int treeLimit = Main.Forest.maxTree + 1;
        private static int iterator = 0;
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            
            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)              
            {     
                if (codes[i].opcode == OpCodes.Callvirt)
                {
                   
                    if (iterator == 1)
                    {
                        
                        codes[i] = new CodeInstruction(OpCodes.Ldc_I4_S, treeLimit);
                        codes[i - 1] = new CodeInstruction(OpCodes.Nop);
                        codes[i + 1] = new CodeInstruction(OpCodes.Nop);
                        codes[i + 2] = new CodeInstruction(OpCodes.Nop);
                        codes[i + 3] = new CodeInstruction(OpCodes.Nop);
                        codes[i + 4] = new CodeInstruction(OpCodes.Nop);
                    }
                    iterator++;
                }       
                    // TODO Insert implementation later    
            }
            return codes.AsEnumerable();
        }
    }

    //Readress value
    [HarmonyPatch(typeof(TreeGrowth))]
    [HarmonyPatch("Start")]
    public static class TreeGrowthPatch
    {
        static void PostFix()
        {
            TreeGrowth.inst.MaxTreesOnMap = Main.Forest.maxTree;
        }
    }
}
