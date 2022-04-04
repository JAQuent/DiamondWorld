using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

public class objectSpawner : MonoBehaviour{
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


    // Update is called once per frame
    void spawnOnTrialBegin(){
        // Get information from session after it started
        diamond_x = session.settings.GetFloatList("diamond_x");
        diamond_z = session.settings.GetFloatList("diamond_z");
        diamondTimings = session.settings.GetFloatList("diamondTimings");
        numberOfDiamonds = diamond_x.Count;
        icosahedron_x = session.settings.GetFloatList("icosahedron_x");
        icosahedron_z = session.settings.GetFloatList("icosahedron_z");
        icosahedronTimings = session.settings.GetFloatList("icosahedronTimings");
        numberOfIcosahedron = icosahedron_x.Count;

        // Diamonds
    	for(int i = 0; i < numberOfDiamonds; i++){
    		Spawn(diamond, i, diamond_x[i], diamond_z[i]);
    	}

        // Icosahedron
        for(int i = 0; i < numberOfIcosahedron; i++){
            Spawn(icosahedron, i, icosahedron_x[i], icosahedron_z[i]);
        }
    }

    void destroyAllCubesAtTrialEnd(){
        // Destroy diamonds
    	for(int i = 0; i < numberOfDiamonds; i++){
    		string obj2delete = objName1 + i;
    		Destroy(GameObject.Find(obj2delete));
    	}

        // Destroy icosahedrons
        for(int i = 0; i < numberOfIcosahedron; i++){
            string obj2delete = objName2 + i;
            Destroy(GameObject.Find(obj2delete));
        }

    }

    void Spawn (GameObject myGameObject, int index, float x, float z){
     	GameObject spawnedObj = Instantiate(myGameObject) as GameObject;
        spawnedObj.transform.position = transform.position;
        spawnedObj.name = objName1 + index;
		spawnedObj.transform.position = new Vector3(x, yOfCube, z);

        // Also unique name the child because the ray tracker won't be useful otherwise. 
        GameObject  firstChildOfSpawnedObj = spawnedObj.transform.GetChild(0).gameObject;
        firstChildOfSpawnedObj.name = firstChildOfSpawnedObj.name + index;

        // Set index value of objectScript
        firstChildOfSpawnedObj.gameObject.GetComponent< objectScript >().index = index;
    }
}
