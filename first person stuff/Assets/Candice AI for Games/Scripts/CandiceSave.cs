using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Candice AI
using CandiceAIforGames.AI;
using CandiceAIforGames.Data;

namespace CandiceAIforGames.AI
{
    public class CandiceSave : MonoBehaviour
    {

        //bring in Candice UI
        private CandiceUI candiceUI;
        private Dropdown SaveItemsDrop;
        public GameObject SaveManager;
        private CandiceSaveManager sm;

        //things to save
        public GameObject[] SaveObjects;
        public bool Transforms = true;
        public bool Health = true;
        public bool CandiceAIStats = false;
        private CandiceAIController CandiceAIStatsObject;



        //autosave
        private float timer = 0.0f;
        public static int increment = 0;
        public static string saveFileName = "";
        public int saveFileLimit = 1;

        //load data
        public static string loadData;
        public static string returnObject= "";
        private char delimiter = '_';
        private char masterDelimiter = '?';
        private static string dataString = "";

        //load vsfx
        public GameObject CandiceVSFX;

        // Start is called before the first frame update
        void Start()
        {
            //new candice UI
            candiceUI = new CandiceUI();
            if (SaveManager == null)
            {
                SaveManager = GameObject.Find("SaveManager");
                if (SaveManager != null)
                {
                    sm = SaveManager.GetComponent<CandiceSaveManager>();
                }
            }
            else {

                sm = SaveManager.GetComponent<CandiceSaveManager>();

            }
            //autosave dropdown
            SaveItemsDrop = GetComponent<Dropdown>();
            SaveItemsDrop.ClearOptions();
            //initial savefile
            saveFileName = "Candice_AutoSave_" + increment.ToString() + ".bin";
            //autosave on scene load
            if (SaveObjects != null)
            {
                foreach (GameObject saveObject in SaveObjects)
                {
                    CandiceAIStatsObject = saveObject.GetComponent<CandiceAIController>();
                    if (CandiceAIStats != null) {
                        if (Health)
                        {
                            dataString += string.Concat(saveObject.name, delimiter, "health", delimiter, CandiceAIStatsObject.hitPoints, masterDelimiter);

                        }
                        if (Transforms)
                        {
                            dataString += string.Concat(saveObject.name, delimiter, "transform", delimiter, saveObject.transform.position.x.ToString(), delimiter, saveObject.transform.position.y.ToString(), delimiter, saveObject.transform.position.z.ToString(), masterDelimiter);                            
                        }
                    }                    
                    SaveGame(dataString, saveFileName);

                }
            }
            //load first the save and add the file to the ui
            LoadSelectedSave(SaveItemsDrop, saveFileName);

            //add listener on dropdown value change to load the save            
            SaveItemsDrop.onValueChanged.AddListener(delegate { LoadDelegate(); });

        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;            
            if (timer >= sm.autoSaveInterval) {
                increment++;
                saveFileName = "Candice_AutoSave_" + increment.ToString() + ".bin";
                dataString = "";
                if (SaveObjects != null) {
                    foreach (GameObject saveObject in SaveObjects) {

                        CandiceAIStatsObject = saveObject.GetComponent<CandiceAIController>();
                        if (CandiceAIStats != null)
                        {
                            if (Health)
                            {
                                dataString += string.Concat(saveObject.name, delimiter, "health", delimiter, CandiceAIStatsObject.hitPoints, masterDelimiter);

                            }
                            if (Transforms)
                            {
                                dataString += string.Concat(saveObject.name, delimiter, "transform", delimiter, saveObject.transform.position.x.ToString(), delimiter, saveObject.transform.position.y.ToString(), delimiter, saveObject.transform.position.z.ToString(), masterDelimiter);
                            }
                        }
                        SaveGame(dataString, saveFileName);
                    }
                }                                
                timer = 0;
                if (increment >= saveFileLimit) {
                    increment = 0;
                }
            }
        }

        public void LoadSelectedSave(Dropdown saveItemsUI, string file)
        {
            //get that save data
            returnObject = CandiceSaveSystem.Instance.LoadFromFile(file) as string;
            //if no data
            if (returnObject == null)
            {
                Debug.Log("No save data loaded!");
            }
            else 
            {

                //now implement the changes
                if (SaveObjects != null)
                {
                    string[] statObjects = SplitThis(masterDelimiter, returnObject);
                    string saveObjectName = "";                    
                    foreach (GameObject saveObject in SaveObjects)
                    {
                        //find the save gameObject in the scene (usually the player)
                        GameObject thisGameObject = GameObject.Find(saveObject.name);
                        //apply the save values to the gameObject, such as transform.position, health and stats.
                        foreach (string stat in statObjects) {
                            Debug.Log(stat);
                            if (thisGameObject != null) {
                                if (thisGameObject.name == SplitThis(delimiter, stat)[0])
                                {
                                    //disable the save gameObject (required to disable any overriding controllers)
                                    thisGameObject.SetActive(false);
                                    if (Health && SplitThis(delimiter, stat)[1] == "health")
                                    {
                                        Debug.Log(thisGameObject.name + "Attempting Health load of: " + SplitThis(delimiter, stat)[2]);
                                        thisGameObject.GetComponent<CandiceAIController>().hitPoints = float.Parse(SplitThis(delimiter, stat)[2]);
                                        candiceUI.ResetBar(GameObject.FindWithTag("PlayerHealthBar"), thisGameObject.GetComponent<CandiceAIController>().hitPoints);
                                        Debug.Log(thisGameObject.name + " Loaded Health " + thisGameObject.GetComponent<CandiceAIController>().hitPoints.ToString());
                                    }
                                    if (Transforms && SplitThis(delimiter, stat)[1] == "transform")
                                    {
                                        //find the active gameObject in the scene and set new velocity vector to load teleport to autosave position
                                        thisGameObject.transform.position = new Vector3(float.Parse(SplitThis(delimiter, stat)[2]), float.Parse(SplitThis(delimiter, stat)[3]), float.Parse(SplitThis(delimiter, stat)[4]));
                                        Debug.Log(thisGameObject.name + " Loaded Transform " + thisGameObject.transform.position.ToString());
                                    }
                                }
                            }
                        }
                        if (thisGameObject != null) {
                            //re-enable the gameObject
                            thisGameObject.SetActive(true);
                            //add a CandiceVSFX prefab for some load VSFX
                            GameObject loadVSFX = Instantiate(CandiceVSFX, thisGameObject.transform.position, Quaternion.identity);
                            //then clear out the screen after the changes have been made
                            Destroy(loadVSFX, 2f);
                        }    
                    }
                }
            }
        }

        public void LoadDelegate() {


            saveFileName = SaveItemsDrop.captionText.text;            
            LoadSelectedSave(SaveItemsDrop, saveFileName);
        }

        public void SaveGame(string saveData, string fileName) 
        {
            //show me the save file in the ui element            
            CandiceSaveSystem.Instance.SaveToFile(saveData, fileName);
            PopulateAutoSaveInUi(SaveItemsDrop, fileName);

        }

        public void PopulateAutoSaveInUi(Dropdown SaveItemDrop, string value) {
            if (SaveItemDrop.options.Count < saveFileLimit)
            {
                if (SaveItemDrop.captionText.text != value) {                    
                    SaveItemDrop.AddOptions(new List<string> { value });
                }                
            }
            else {
                SaveItemDrop.ClearOptions();
                SaveItemDrop.AddOptions(new List<string> { value });
            }
        }

        public string[] SplitThis(char splitter, string value) {

            return value.Split(splitter);

        }

    }
}