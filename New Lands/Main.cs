using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using Harmony;
using UnityEngine;


/*
 * Version 1.3
 * Created by Cedric Jansen
 * 18.11.2019
 * 
 */

namespace New_Lands
{
    public class Main : MonoBehaviour
    {

        public static readonly String Name = "New Lands";
        // The current version of the modification.
        public static readonly String Version = "1.3";


        public static UserInterface UserInterface;
            
        public static Grid Grid = new Grid(55, 200, 100);
        public static Forest Forest = new Forest(250, 100000, 250);
        public static Generator Generator = new Generator();

        // Used for the button increase time
        public static float ButtonTimer = 0f;
        public static float ButtonMax = 0.05f;

        public static bool IncreaseSteady = false;  // Accessed and changed by the UserInterface
        public static bool DecreaseSteady = false;  // Acessed any changed by the UserInterface


        // PreScriptLoad is a method that gets invoked by 
        // the Kingdoms and Castles app in the splash screen and should be 
        // used to load and patch stuff before the main scene.
        public void Preload(KCModHelper _helper)
        {
           
            var harmony = HarmonyInstance.Create(name + " " + Version);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Debug.LogError("[New Lands] Patching methods..");
        }


        // OnScriptLoad is a method that gets invoked by
        // the Kingdoms and Castles app some frames after the main
        // scene is loaded.
        public void SceneLoaded(KCModHelper _helper) {
            UserInterface = new UserInterface(_helper);
            UserInterface.Instantiate();   
        }
 
        // The Unity Update() method that gets called once
        // per frame ( Version 2019.2).
        public void Update()
        {
            UserInterface.Update();
            patches.RegisterSwap.TrackSwapKey();
            ButtonTimer += Time.deltaTime;

            if (IncreaseSteady)
                IncreaseGridSteady();
            else if (DecreaseSteady)
                DecreaseGridSteady();
        }

        public static void IncreaseGrid()
        {
            if( Grid.current < Grid.max)
            {
                Grid.current += 1;
            }
        }

        public static void DecreaseGrid()
        {
            if( Grid.current > Grid.min )
            {
                Grid.current -= 1;
            }
        }

        public static void IncreaseGridSteady()
        {
            if( Grid.current < Grid.max)
            { 
                if (EvalButtonTime())
                {
                    Grid.current += 1;
                    ButtonTimer = 0;
                }
            }
        }

        public static void DecreaseGridSteady()
        {
           if( Grid.current > Grid.min)
            {          
                if (EvalButtonTime())
                {
                    Grid.current -= 1;
                    ButtonTimer = 0;
                }
            }
        }

        public static bool EvalButtonTime()
        {
            return (ButtonTimer > ButtonMax ? true : false);
        }


    }
}
