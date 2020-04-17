using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;

namespace New_Lands.patches
{
    // GRID PATCH
    // This patch manipulates the width and height of the grid in which
    // the playable area will be generated. When successfull, larger and smaller
    // maps are created, depending on user input.
    [HarmonyPatch(typeof(World))]
    [HarmonyPatch("Setup")]
    static class GridPatch
    {
        // Passes manipulated values to the World.inst.Setup() function.
        static bool Prefix(World __instance, ref int width, ref int height)
        {
            if (!SavePatch.isLoading)
            {
                width = Main.Grid.current;
                height = width;

                var WorldWidth = typeof(World).GetField("gridWidth", BindingFlags.Instance | BindingFlags.NonPublic);
                WorldWidth.SetValue(World.inst, Main.Grid.current);
                var WorldHeight = typeof(World).GetField("gridHeight", BindingFlags.Instance | BindingFlags.NonPublic);
                WorldHeight.SetValue(World.inst, Main.Grid.current);    
            }
          

            return true;
        }
    }
}
