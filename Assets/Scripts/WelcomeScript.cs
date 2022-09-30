using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


public class WelcomeScript : MonoBehaviour{
	// public vars
    public string fileName = "welcome.json";
    public Text label1Text;
    public Text label2Text;
    public Text instructions;
    public Text button;
    public Text title;

    // You need to set-up all variables that you want to get from the .json file.
    // The variable names have to correspond to the input names in that file. 
    [System.Serializable]
    public class JSONDataClass {
        public string label1;
        public string label2;
        public string title;
        public string buttonLabel;
        public string instructionText;
    }

 	// private vars
    private JSONDataClass JSONData;

        // Start is called before the first frame update
    void Start(){
        // Get the information from the JSON file
        GetDataFromJSON(fileName);

        // Change texts
        label1Text.text = JSONData.label1;
        label2Text.text = JSONData.label2;
		instructions.text = JSONData.instructionText;
		button.text = JSONData.buttonLabel;
        title.text = JSONData.title;
    }


	// Method to load the main scene
    public void LoadScene(){
    	Debug.Log("Start button pressed.");
    	SceneManager.LoadScene("DiamondWorld");
    }

    /// <summary>
    /// Method to read in the JSON file that is placed in the StreamingAssets folder for the file that is provided
    /// </summary>
    void GetDataFromJSON(string fileName){
        // Get path
        string path2file = Path.GetFullPath(Path.Combine(Application.streamingAssetsPath, fileName));

        // Get JSON input
        var sr = new StreamReader(path2file);
        var fileContents = sr.ReadToEnd();
        sr.Close();

        // Get instruction data profile
        JSONData = JsonUtility.FromJson<JSONDataClass>(fileContents);
    }
}
