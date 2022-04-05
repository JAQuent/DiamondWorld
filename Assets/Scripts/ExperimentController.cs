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

    // Private vars
    private List<float> diamond_x;
    private List<float> diamond_z;
    private List<float> icosahedron_x;
    private List<float> icosahedron_z;
    private int numberOfDiamonds;
    private int numberOfIcosahedron;
    private List<GameObject> diamonds = new List<GameObject>();
    private List<GameObject> icosahedrons = new List<GameObject>();


    // Update is called once per frame
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

    public void SaveDataAtTrialEnd(){
        // Save data
        session.CurrentTrial.result["score"] = 2;

    }

   GameObject Spawn (GameObject myGameObject, int index, string baseName, float x, float z){
     	GameObject spawnedObj = Instantiate(myGameObject) as GameObject;
        spawnedObj.transform.position = transform.position;
        spawnedObj.name = objName1 + index;
		spawnedObj.transform.position = new Vector3(x, yOfCube, z);

        // Also unique name the child because the ray tracker won't be useful otherwise. 
        GameObject  firstChildOfSpawnedObj = spawnedObj.transform.GetChild(0).gameObject;
        firstChildOfSpawnedObj.name = baseName + index;

        // Set index value of objectScript
        firstChildOfSpawnedObj.gameObject.GetComponent<objectScript>().index = index;

        return spawnedObj;
    }
}

// hide cursor
// add flag or layer to ray cast
// add fixation marker
// end application
// trial version
// remove shadows