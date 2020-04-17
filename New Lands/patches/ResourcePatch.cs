using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;

namespace New_Lands.patches
{
    //RESOURCE GENERATION PATCH
    // Increases and decreases the amount of resources. 
    [HarmonyPatch(typeof(World))]
    [HarmonyPatch("GenLand")]

    public static class ResourcePatch
    {

        private static int MaxIron;
        private static int RandomMod;

       // private static readonly int ResetIron = World
        static bool Prefix(World __instance)
        {
            int Grid = Main.Grid.current;
            World.SizeToDepostsInfo Resources = new World.SizeToDepostsInfo();

            if(Main.Generator.NewBias == Generator.GeneratorBias.Land)
            {
                int MaxIron = CalcMaxIron(Grid);
              
                int Iron = (int) MaxIron;
                int Stone = (int) MaxIron;

                Resources.numIron =(int) Iron;
                Resources.numSmallStone = (int) Stone;
                Resources.numLargeStone = (int) Stone / 5;

                __instance.smallMapDepositInfo = Resources;
                __instance.mediumMapDepositInfo = Resources;
                __instance.largeMapDepositInfo = Resources;
            }
            
            return true;
        }

        private static int CalcMaxIron(int CurrentGrid)
        {
            return CurrentGrid / 15;
        }
    }
}
