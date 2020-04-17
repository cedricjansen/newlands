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
    // MAP GENERATION PATCH
    // This patch creates the land tiles and manipulates
    // the tree generation.
    [HarmonyPatch(typeof(MapGenTest))]
    [HarmonyPatch("Init")]
    static class MapTilesPatch
    {
        static bool Prefix(MapGenTest __instance, ref int w, ref int h,
           ref int desiredTiles, ref int islandCount)
        {
            // Calculate the amount of tiles that should be placed onto 
            // the grid.
            Main.Generator.UpdateValues();
            desiredTiles = Main.Generator.EstimatedUseableTiles;
            islandCount = Main.Generator.IslandCounts;
            TreeGrowth.inst.MaxTreesOnMap = desiredTiles;

            MapGenTest MapGen = (MapGenTest)GameObject.FindObjectOfType(typeof(MapGenTest));
            MapGen.riverStepPunch = Main.Generator.RiverStep; // original was 2f
            MapGen.riverPerpJitter = Main.Generator.RiverJitter; // original is 1f 

            MapGen.minIslandArea = Main.Generator.minIslandArea; // original is 64f
            MapGen.minCircleArea = Main.Generator.minCircleArea; // original is 80f

            MapGen.edgeHoleStep = Main.Generator.EdgeHole; // int, original is 5
            MapGen.waterFreq = Main.Generator.WaterFreq;
   
            return true;
        }
    }  
}



