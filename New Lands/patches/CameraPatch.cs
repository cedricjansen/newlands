using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;

// CAMERA PATCH
// This patch injects some code before the exectuion
// of the Cam.inst.Update() function, manipulating and increasing the allowed distance.
// Helpful when playing on larger maps.

namespace New_Lands.patches
{
    [HarmonyPatch(typeof(Cam))]
    [HarmonyPatch("Update")]
    static class CameraPatch
    {
        // Amount of zoom that should be the maximum height;
        private static float zoomMax = 200;
       
        static bool Prefix(Cam __instance)
        {
            __instance.zoomRange.Max = zoomMax;
            return true;
        }
    }

    [HarmonyPatch(typeof(Keep))]
    [HarmonyPatch("OnPlayerPlacement")]
    public static class SwapViewToKeep
    {
        // Amount of zoom that should be the maximum height;
        public static Keep keepInstance;

        static void Postfix(Keep __instance)
        {
            keepInstance = __instance;
        }
    }



    public static class RegisterSwap 
    {
        // maybe replaceable to configure /later
        static KeyCode code = KeyCode.K;


        


        public static void TrackSwapKey()
        {
            if (Player.inst != null && World.inst != null)
            {
                if (Input.GetKeyDown(code))
                {
                        // Throws an non-important error if the keep is not initialised yet.
                        Keep keep = (Keep)GameObject.FindObjectOfType(typeof(Keep));
                    
                        FieldInfo keepBuildingField = typeof(Keep).GetField("b", BindingFlags.Instance | BindingFlags.NonPublic);
                        Building keepBuildingComponent = (Building)keepBuildingField.GetValue(keep);
                        Cam.inst.SetDesiredTrackingPos(keepBuildingComponent.Center());
                }
            }
        }
       
    }
}
