using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

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

    void Start(){
    	// Start with no movement
        ThreeButtonMovement.movementAllowed = false;
    }

    // Update method to quit application
    void Update(){
        // Stop Experiment
        if(Input.GetKey(KeyCode.Escape)){
            // Log entry
            Debug.Log("The end");

            // Close application
            Application.Quit();
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

    // Method to be run at the end of the session
    public void EndOfSession(){
        // Show screeen
        endScreen.SetActive(true);

        // End movement
        ThreeButtonMovement.movementAllowed = false;
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
}