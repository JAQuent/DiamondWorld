using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;
using UnityEngine.UI;

public class ExperimentController : MonoBehaviour{
    // Static variables
    public static List<float> diamondTimings;
    public static List<float> trapTimings;
    public static bool warningShow = false;

    // HTTPpost script
    public HTTPPost HTTPPostScript;

	// Public vars
	public GameObject diamond;
    public GameObject trap;
	public float yOfCube = 2.08f;
	public string objName1 = "diamond";
    public string objName2 = "trap";
    public Session session;
    public GameObject endScreen;
    public GameObject fixationMarker;
    public GameObject countdownText;
    public GameObject scoreText;
    public GameObject warningIcon;
    public float iconTime;

    // Private vars
    private List<float> diamond_x;
    private List<float> diamond_z;
    private List<float> trap_x;
    private List<float> trap_z;
    private int numberOfDiamonds;
    private int numberOfTrap;
    private List<GameObject> diamonds = new List<GameObject>();
    private List<GameObject> traps = new List<GameObject>();
    private bool startEndCountDown; // If true it starts the end countdown
    private float endCountDown = 60; // End countdown if zero, application closes. 
    private Text endScreenText; // Text component of the end screen
    private string endMessage; // String for the end message that is used.
    private bool useHTTPPost = false; // Is HTTPPost to be used? If so it needs input from the .json
    private GameObject FPS_Counter; // Game object for FPS counter
    private bool foundFPS_Counter = false; // Has the FPS counter been found?

    void Start(){
    	// Start with no movement
        ThreeButtonMovement.movementAllowed = false;

		// Deactivate FPS by default
        activateFPS_Counter(false);

#if UNITY_WEBGL
        // If WebGL also make full screen
        Screen.fullScreen = true;
#endif
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

        // Check if warning should be displayed
        if(warningShow){
            Debug.Log("Warning showed!");
            StartCoroutine(warning());
            warningShow = false;
        }
    }


    // Method to be run at the beginning of trial to get settings and spawn objects.
    public void spawnOnTrialBegin(){
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
        trap_x = session.settings.GetFloatList("trap_x");
        trap_z = session.settings.GetFloatList("trap_z");
        trapTimings = session.settings.GetFloatList("trapTimings");
        numberOfTrap = trap_x.Count;

        // Spawn objects
        // Diamonds
    	for(int i = 0; i < numberOfDiamonds; i++){
    		diamonds.Add(Spawn(diamond, i, objName1, diamond_x[i], diamond_z[i]));
    	}

        // Trap
        for(int i = 0; i < numberOfTrap; i++){
            traps.Add(Spawn(trap, i, objName2, trap_x[i], trap_z[i]));
        }
    }


    // Method to be run at the end of the trial to destroy all objects
    public void destroyAllObjectsAtTrialEnd(){
        // Destroy diamonds
    	for(int i = 0; i < numberOfDiamonds; i++){
    		Destroy(diamonds[i]);
    	}

        // Destroy traps
        for(int i = 0; i < numberOfTrap; i++){
            string obj2delete = objName2 + i;
            Destroy(traps[i]);
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
        // If useHTTPPost not used than quit immediately
        if(!useHTTPPost){
            Debug.Log("Application closed now.");
            Application.Quit();
        }

        // End session/trial if necessary
        if(session.InTrial){
            // End the trial
            session.EndCurrentTrial();  
        }
        if(!session.isEnding){
            // End the session
            session.End();
        }

        // Set other canvases inactive
        fixationMarker.SetActive(false);
        countdownText.SetActive(false);
        scoreText.SetActive(false);

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
        GameObject  childOfSpawnedObj = spawnedObj.transform.GetChild(3).gameObject;
        childOfSpawnedObj.name = baseName + index;

        // Set index value of objectScript
        childOfSpawnedObj.gameObject.GetComponent<objectScript>().index = index;

        return spawnedObj;
    }

    // Set target framerate at the beginning of the session & also print system time
    public void sessionStart(){
        // Set frame rate
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = session.settings.GetInt("targetFrameRate");
        // Log entry
        Debug.Log("Session start time " + System.DateTime.Now);
        // Screen resolution
        Debug.Log(Screen.currentResolution);
        // Version of the task
        Debug.Log("Application Version : " + Application.version);

        // Get endCountDown & countdown message
        endCountDown = session.settings.GetFloat("endCountDown");
        endMessage = session.settings.GetString("endMessage");

        // Check if the keys have to be changed
        bool changeKeys = session.settings.GetBool("changeKeys");
        if(changeKeys){
            changeKeyboardKeys();
        }

        // Check if HTTPPost needs to be set.
        useHTTPPost = session.settings.GetBool("useHTTPPost");
        if(useHTTPPost){
            configureHTTPPost();
        }

        // Which platform is used
        whichPlatform();

        // Detect which input devices are presented
        detectInputDevices();

        // Activate FPS counter if configured so.
        activateFPS_Counter(session.settings.GetBool("showFPS"));

        ////////////////////////////////////////////////////////////////////////////////////
        // Newly added features that if not specified in the .json file get a default value.
        // Check whether actionNeedToBeEnded should be controlled
        string tempKey = "actionNeedToBeEnded";
        if(containsThisKeyInSessionSettings(tempKey)){
            ThreeButtonMovement.actionNeedToBeEnded = session.settings.GetBool(tempKey); 
        } 
    }


    /// <summary>
    /// Method to activate FPS counter
    /// </summary>
    void activateFPS_Counter(bool activateFPS_Recording){
        // Find the game object
        if(!foundFPS_Counter){
            FPS_Counter = GameObject.Find("FPS_Counter");
            foundFPS_Counter = true; // Found the game object
        }

        // Set in/active
        FPS_Counter.SetActive(activateFPS_Recording);
    }

    /// <summary>
    /// Method to change keys 
    /// </summary>  
    public void changeKeyboardKeys(){
        // Get List of string from .json
        List<string> newKeys = session.settings.GetStringList("keys");
        ThreeButtonMovement.leftTurn = (KeyCode) System.Enum.Parse(typeof(KeyCode), newKeys[0]);
        ThreeButtonMovement.forwardKey = (KeyCode) System.Enum.Parse(typeof(KeyCode), newKeys[1]);
        ThreeButtonMovement.rightTurn = (KeyCode) System.Enum.Parse(typeof(KeyCode), newKeys[2]);
    }

    /// <summary>
    /// Method to configure the HTTPPost script. Needs public UXF.HTTPPost HTTPPostScript;
    /// </summary>
    public void configureHTTPPost(){
        string url = session.settings.GetString("url");
        string username = session.settings.GetString("username");
        string password = session.settings.GetString("password");

        // Set the variables
        HTTPPostScript.url = url;
        HTTPPostScript.username = username;
        HTTPPostScript.password = password;
        HTTPPostScript.active = true;
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

    /// <summary>
    /// Display warning icon for a short time
    /// </summary>
    IEnumerator warning(){
        warningIcon.SetActive(true);

        yield return new WaitForSeconds(iconTime);

        warningIcon.SetActive(false);
    }

    /// <summary>
    /// Check which input devices are available
    /// </summary>
    void detectInputDevices(){
        //Debug.Log("Mouse connected to computer: " + Input.mousePresent);
        Debug.Log("Computer supports touchscreen: " + Input.touchSupported);
        Debug.Log("Device type: " + SystemInfo.deviceType);
    }

    /// <summary>
    /// Method to check if a certain key can be found in the settings heirarchy
    /// </summary>  
    bool containsThisKeyInSessionSettings(string targetKey){
        // Initialise result
        bool contained = false;

        // Loop through all keys in settings
        foreach(var key in session.settings.Keys){
            if(key == targetKey){
                contained = true;
                break;
            }
        }

        // Retun the value
        return(contained);
    }
}