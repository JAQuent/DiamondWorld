using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UXF;
using System.Linq;

public class screenTracker : Tracker {
	// Public vars
    public Camera cam;
    public List<float> x  = new List<float>();
    public List<float> y  = new List<float>();
    public int numRays;
    public bool debugMode = true; // Prints log entry and shows debug rays
    public string noObjectString = "NA";
    public bool saveNoObject = false;

    // Private vars
    public List<string> objectDetected = new List<string>();
    private UXFDataRow currentRow;
    private bool recording = false;

    // Start calc
    void Start(){
    	// Read .txt file
    	string[] rayCoordinates = readText("input_data/rayCoordinates.txt");

    	// Parsing information for rays
		int indexCount = 2; // because there are 7 variables with 1 column for each variable
		int n = rayCoordinates.Length/indexCount;
		
		// First var
		int indexMin = 0;
		x     = parseFloatListFromString(indexMin, indexCount, rayCoordinates);

		// Second var
		indexMin = 1;
		y     = parseFloatListFromString(indexMin, indexCount, rayCoordinates);

    	// Start the recoding
        StartCoroutine(RecordRoutine());
    }

    // Start and stop recoding functions to be added to UXF.rig Events (Start and End of trial)
    void StartRecording(){
    	recording = true;
    }

    // See above
    void StopRecording(){
    	recording = false;
    }

    IEnumerator RecordRoutine(){
        while (true){
            if (recording){
                objectDetected = ray2detectObjects(x, y, cam);
                for(int i = 0; i < numRays; i++){
                    // When no object was detected save only if saveNoObject is true
                    if(objectDetected[i] == noObjectString){
                        if(saveNoObject){
                            var values = new UXFDataRow();
                            values.Add(("rayIndex", i));
                            values.Add(("x", x[i]));
                            values.Add(("y", y[i]));
                            values.Add(("objectDetected", objectDetected[i]));
                            currentRow = values;
                            RecordRow(); // record for each ray
                            currentRow = null;
                        }
                    } else {
                            var values = new UXFDataRow();
                            values.Add(("rayIndex", i));
                            values.Add(("x", x[i]));
                            values.Add(("y", y[i]));
                            values.Add(("objectDetected", objectDetected[i]));
                            currentRow = values;
                            RecordRow(); // record for each ray
                            currentRow = null;
                    }
                }
            }
            yield return null; // wait until next frame
        }
    }

    // Set up header
    protected override void SetupDescriptorAndHeader(){
        measurementDescriptor = "objectsOnScreenTracker";
        
        customHeader = new string[]{
            "rayIndex",
            "x",
            "y",
            "objectDetected"
        };
    }

    // Get values
    protected override UXFDataRow GetCurrentValues(){
        return currentRow;
    }

    string[] readText(string fileName){
		// Loads .txt file and split by \t and \n
		string inputText = System.IO.File.ReadAllText(fileName);
		string[] stringList = inputText.Split('\t', '\n'); //splits by tabs and lines
	 	return stringList;
	}

	// Parsing informatiom from text file converting to list of floats
	List<float> parseFloatListFromString(int indexMin, int indexCount, string[] stringList){
		// Selects items, converts strings to floats and then creates a list for floats
		int indexMax = stringList.Length - indexCount + indexMin;
		var index = Enumerable.Repeat(indexMin, (int)((indexMax - indexMin) / indexCount) + 1).Select((tr, ti) => tr + (indexCount * ti)).ToList();
		List<float> floatList = index.Select(x => float.Parse(stringList[x])).ToList();
		return floatList;
	}

    // Function to detect objects on screen by rays
    List<string> ray2detectObjects(List<float> x, List<float> y, Camera cam){
    	// Get number of rays
    	numRays = y.Count;

    	// Create var
    	List<string> nameOfObjects = new List<string>();

    	for (int i = 0; i < numRays; i++){
    		// Cast the ray and add to list
    		Ray ray = cam.ViewportPointToRay(new Vector3(x[i], y[i], 0));

    		// Display ray for debugging
            if(debugMode){
            	Debug.DrawRay(ray.origin, ray.direction * 50, Color.red);
            }

            // Raycast and check if something is hit
            RaycastHit hit1;
            if (Physics.Raycast(ray, out hit1)){
            	if(debugMode){
            		Debug.DrawRay(ray.origin, ray.direction * 50, Color.green);
            		print("I'm looking at " + hit1.transform.name + " with ray " + i);
            	}

            	// Add name of GameObject that was hit
                nameOfObjects.Add(hit1.transform.name);
            } else {
            	// Add noObjectString becuase no object was hit by ray
            	nameOfObjects.Add(noObjectString);
            }
        }
    	return nameOfObjects;
    }

}