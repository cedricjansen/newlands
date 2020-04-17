using System;
using UnityEngine;
using Harmony;

namespace New_Lands.patches
{
    [HarmonyPatch(typeof(LoadSaveContainer))]
    [HarmonyPatch("Unpack")]
    public static class SavePatch
    {

        public static bool isLoading = false;
        static bool Prefix(object __instance)
        {
            isLoading = true;
            return true;
        }

        static void Postfix()
        {
            isLoading = false;    
        }
    }
}
