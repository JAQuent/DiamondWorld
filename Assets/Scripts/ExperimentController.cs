using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;
using UnityEngine.UI;

public class ExperimentController : MonoBehaviour{
    // Static variables
    public static List<float> diamondTimings;
    public static List<float> icosahedronTimings;

	// Public vars
	public GameObject diamond;
    public GameObject icosahedron;
	public float yOfCube = 2.08f;
	public string objName1 = "diamond";
    public string objName2 = "icosahedron";
    public Session session;
    public GameObject endScreen;

    // Private vars
    private List<float> diamond_x;
    private List<float> diamond_z;
    private List<float> icosahedron_x;
    private List<float> icosahedron_z;
    private int numberOfDiamonds;
    private int numberOfIcosahedron;
    private List<GameObject> diamonds = new List<GameObject>();
    private List<GameObject> icosahedrons = new List<GameObject>();
    private bool startEndCountDown; // If true it starts the end countdown
    private float endCountDown = 60; // End countdown if zero, application closes. 
    private Text endScreenText; // Text component of the end screen
    private string endMessage; // String for the end message that is used.

    void Start(){
    	// Start with no movement
        ThreeButtonMovement.movementAllowed = false;
    }

    // Update method to quit application
    void Update(){
        // Stop Experiment
        if(Input.GetKey(KeyCode.Escape)){
            // Log entry
            Debug.Log("Session end time " + System.DateTime.Now);

            // Close application
            TheEnd();
        }

        // End countdown
        if(startEndCountDown){
            endCountDown -= Time.deltaTime;
            endScreenText.text = endMessage + Mathf.Round(endCountDown);

            // Quit if end count down over
            if(endCountDown <= 0){
                Application.Quit();
            }
        }
    }


    // Method to be run at the beginning of trial to get settings and spawn objects.
    void spawnOnTrialBegin(){
        // Get information from session after it started
        // Parse general information from .json
        ThreeButtonMovement.forwardSpeed = session.settings.GetFloat("forwardSpeed");
        ThreeButtonMovement.rotationSpeed = session.settings.GetFloat("rotationSpeed");
        objectScript.rewardValue = session.settings.GetInt("rewardValue");
        objectScript.punishmentValue = session.settings.GetInt("punishmentValue");

        // Object locations and timing
        diamond_x = session.settings.GetFloatList("diamond_x");
        diamond_z = session.settings.GetFloatList("diamond_z");
        diamondTimings = session.settings.GetFloatList("diamondTimings");
        numberOfDiamonds = diamond_x.Count;
        icosahedron_x = session.settings.GetFloatList("icosahedron_x");
        icosahedron_z = session.settings.GetFloatList("icosahedron_z");
        icosahedronTimings = session.settings.GetFloatList("icosahedronTimings");
        numberOfIcosahedron = icosahedron_x.Count;

        // Spawn objects
        // Diamonds
    	for(int i = 0; i < numberOfDiamonds; i++){
    		diamonds.Add(Spawn(diamond, i, objName1, diamond_x[i], diamond_z[i]));
    	}

        // Icosahedron
        for(int i = 0; i < numberOfIcosahedron; i++){
            icosahedrons.Add(Spawn(icosahedron, i, objName2, icosahedron_x[i], icosahedron_z[i]));
        }
    }


    // Method to be run at the end of the trial to destroy all objects
    public void destroyAllCubesAtTrialEnd(){
        // Destroy diamonds
    	for(int i = 0; i < numberOfDiamonds; i++){
    		Destroy(diamonds[i]);
    	}

        // Destroy icosahedrons
        for(int i = 0; i < numberOfIcosahedron; i++){
            string obj2delete = objName2 + i;
            Destroy(icosahedrons[i]);
        }
    }

    // Method to be run at the end of trial to save data
    public void SaveDataAtTrialEnd(){
        // Save data
        session.CurrentTrial.result["score"] = scoreCounter.score;

    }

    // Method to be run at the beginning of session to allow movement
    public void AllowMovement(){
    	// Allow movement
        ThreeButtonMovement.movementAllowed = true;
    }

    /// <summary>
    /// Function to end application. This needs to be attached to the On Session End Event of the UXF Rig.
    /// </summary>
    public void TheEnd(){
        // Set end screen active
        endScreen.SetActive(true);

        // Start end countdown
        startEndCountDown = true;

        // Get text
        endScreenText = endScreen.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text>();
    }


    // Method to hide the cursor at the beginning of the session
    public void HideCursor(){
        Cursor.visible = false;
    }

    // Method to spawn the objects
    GameObject Spawn (GameObject myGameObject, int index, string baseName, float x, float z){
     	GameObject spawnedObj = Instantiate(myGameObject) as GameObject;
        spawnedObj.transform.position = transform.position;
        spawnedObj.name = baseName + index;
		spawnedObj.transform.position = new Vector3(x, yOfCube, z);

        // Also unique name the child because the ray tracker won't be useful otherwise. 
        GameObject  firstChildOfSpawnedObj = spawnedObj.transform.GetChild(0).gameObject;
        firstChildOfSpawnedObj.name = baseName + index;

        // Set index value of objectScript
        firstChildOfSpawnedObj.gameObject.GetComponent<objectScript>().index = index;

        return spawnedObj;
    }

    // Set target framerate at the beginning of the session & also print system time
    public void sessionStart(){
        // Set frame rate
        Application.targetFrameRate = session.settings.GetInt("targetFrameRate");

        // Print system time
        Debug.Log("Session start time " + System.DateTime.Now);

        // Get endCountDown & countdown message
        endCountDown = session.settings.GetFloat("endCountDown");
        endMessage = session.settings.GetString("endMessage");

        // Which platform is used
        whichPlatform();
    }


    /// <summary>
    /// Method to log which platform is used. # More info here https://docs.unity3d.com/Manual/PlatformDependentCompilation.html
    /// </summary>
    void whichPlatform(){
        #if UNITY_EDITOR
            Debug.Log("Platform used is UNITY_EDITOR");
        #elif UNITY_STANDALONE_OSX
            Debug.Log("Platform used is UNITY_STANDALONE_OSX");
        #elif UNITY_STANDALONE_WIN
            Debug.Log("Platform used is UNITY_STANDALONE_WIN");
        #endif
    }
}