using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace New_Lands
{

    public class UserInterface 
    {

        public bool loaded = false;

        public KCModHelper helper;
        public AssetBundle AssetBundle;
        public GameObject Panel;
        public GameObject PrefAb;
        public GameObject goUi;
        private GameObject EventSystem;

        public Button DecreaseGridButton;   
        public Button IncreaseGridButton;

        public GameObject GeneratorObj;
        public TMP_Dropdown GeneratorDropDown;
        public static int GeneratorDropValue;
        
        public TextMeshProUGUI GridText;
        
        private readonly String PrefAbPath = "assets/workspace/NewLandsUI.prefab";
        private readonly String AssetPath = "/assets/ui";
        private readonly String AssetName = "newlandsbundle";

        private Transform Parent;

        public float ButtonPressTimer = 0f;
        public float ButtonPressThresh = 0.2f;
        public bool ButtonClick = false;
        public bool ButtonHold = false;


        // Constructor loads the Asset and initialises the PrefAb
        public UserInterface( KCModHelper _helper)
        {
            helper = _helper;
            LoadAssetBundle();
            LoadPrefAb();
        }

        // Update the grid text to display the current size of the world grid.
        public void Update()
        {
            if (!loaded)
                return;
            UpdateGridText();
            // TODO
            //update only if Game State is main menu. 
        }

        //Get the neccessary parent for the user interface object.
        public void GetParent()
        {
            if (GameState.inst.mainMenuMode.newMapUI != null)
                Parent = GameState.inst.mainMenuMode.newMapUI.transform;
        }

        //Sets the parent, inits the EventSystem and sets up the Button components for the grid right now.
        // When Instantiate is finished, the Update function gets executed.
        public void Instantiate()
        {
            GetParent();
            goUi = GameObject.Instantiate(PrefAb) as GameObject;
            goUi.transform.SetParent(Parent, false);

          
            GetButtonComponents();
            GetGeneratorDropDown();

            loaded = true;
        }

        
        //Function for updating the TextMesh, displaying the current Grid size.
        private void UpdateGridText()
        {
            GridText.text = Main.Grid.current.ToString();
        }

        //Sets up the Button Components
        private void GetButtonComponents()

        {
            IncreaseGridButton = goUi.transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<Button>();
            IncreaseGridButton.onClick.AddListener(IncreaseGridButtonPressed);

            EventTrigger IncreaseTrigger = IncreaseGridButton.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry IncreaseDown = new EventTrigger.Entry();
            IncreaseDown.eventID = EventTriggerType.PointerDown;
            IncreaseDown.callback.AddListener((e) => { HoldButtonDown((PointerEventData)e, IncreaseGridButton); });
           

            EventTrigger.Entry IncreaseUp = new EventTrigger.Entry();
            IncreaseUp.eventID = EventTriggerType.PointerUp;
            IncreaseUp.callback.AddListener((e) => { ButtonUp((PointerEventData)(e), DecreaseGridButton); });

            IncreaseTrigger.triggers.Add(IncreaseDown);
            IncreaseTrigger.triggers.Add(IncreaseUp);

            DecreaseGridButton = goUi.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Button>();
            DecreaseGridButton.onClick.AddListener(DecreaseGridButtonPressed);

            EventTrigger DecreaseTrigger = DecreaseGridButton.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry DecreaseDown = new EventTrigger.Entry();
            DecreaseDown.eventID = EventTriggerType.PointerDown;
            DecreaseDown.callback.AddListener((e) => { HoldButtonDown((PointerEventData)(e), DecreaseGridButton); });

            EventTrigger.Entry DecreaseUp = new EventTrigger.Entry();
            DecreaseUp.eventID = EventTriggerType.PointerUp;
            DecreaseUp.callback.AddListener((e) => { ButtonUp((PointerEventData)(e), DecreaseGridButton); });

            DecreaseTrigger.triggers.Add(DecreaseDown);
            DecreaseTrigger.triggers.Add(DecreaseUp);

            GridText = goUi.transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        }

        private void GetGeneratorDropDown()
        {
            GeneratorObj = goUi.transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
            GeneratorDropDown = GeneratorObj.GetComponent<TMP_Dropdown>();

            GeneratorDropValue = GeneratorDropDown.value;
            GeneratorDropDown.onValueChanged.AddListener(
                delegate
                {      
                    SelectAndSetGeneratorValue();
                }
            );
        }


        // Loading Asset and prefab
        private void LoadAssetBundle()
        {
            try
            {
                String p = helper.modPath + AssetPath;
                AssetBundle = KCModHelper.LoadAssetBundle(p, AssetName);
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        private void LoadPrefAb()
        {
            try
            {
                PrefAb = AssetBundle.LoadAsset(PrefAbPath) as GameObject;
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }


        private void SelectAndSetGeneratorValue()
        {
            GeneratorDropValue = GeneratorDropDown.value;
            Main.Generator.SetGenType(GeneratorDropValue);
        
        }

        // Handle the single button presses to increase and decrease the grid scale.
        private void IncreaseGridButtonPressed()
        {
            Main.IncreaseGrid();
            UpdateGridText();
        }

        private void DecreaseGridButtonPressed()
        {
            Main.DecreaseGrid();
            UpdateGridText();
        }


        // Handle increasing and decreasing of the grid value when holding the buttons.
        private void HoldButtonDown( PointerEventData eventData, Button button)
        {

            if( button == IncreaseGridButton )
            {
                Main.IncreaseSteady = true;
                Main.DecreaseSteady = false;

            }
            else if ( button == DecreaseGridButton )
            {
                Main.IncreaseSteady = false;
                Main.DecreaseSteady = true;

            }
        }

        // Same as HoldButtonDown but for releasing.
        private void ButtonUp(PointerEventData eventData, Button button)
        {
                ButtonPressTimer = 0f;
                Main.IncreaseSteady = false;
                Main.DecreaseSteady = false;
        }


    }

   
}
