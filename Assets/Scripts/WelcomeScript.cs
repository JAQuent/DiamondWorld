using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using UnityEngine.Networking;


public class WelcomeScript : MonoBehaviour{
	// public vars
    public string fileName = "welcome.json";
    public Text label1Text;
    public Text label2Text;
    public Text instructions;
    public Text title;
    public Text version;
    public GameObject button;
    public GameObject inputField;

    // static vars
    public static string studyID = "None";
    public static string UXF_settings_url = "None";
    public static string fileNameForStartUpText = "None";

    // You need to set-up all variables that you want to get from the .json file.
    // The variable names have to correspond to the input names in that file. 
    [Serializable]
    public class UIDataClass {
        public string label1;
        public string label2;
        public string title;
        public string buttonLabel;
        public string instructionText;
    }

    // JSON Data for studyID dictionary for WebGL experiments
    [Serializable]
    public struct Study{
        public string studyID;
        public string UXF_settings_url;
        public string startupText;
    }
    [Serializable]
    public class StudyDictClass{
        public List<Study> studies;
    }

    // private vars
    private UIDataClass UIData;
    private StudyDictClass StudyDict;

    // Start is called before the first frame update
    void Start(){
        // Change version
        version.text = "Version " + Application.version;

#if UNITY_WEBGL
        StartCoroutine(SetUp_WebGLExperiment());
        Debug.Log("WebGL build");
#else
        SetUp_LocalExperiment();
#endif
    }

    /// <summary>
    /// Method to set up the Welcome UI for local (non-webGL) experiments
    /// </summary>
    void SetUp_LocalExperiment(){
        // Get path
        string path2file = Path.GetFullPath(Path.Combine(Application.streamingAssetsPath, fileName));

        // Get JSON input
        var sr = new StreamReader(path2file);
        var fileContents = sr.ReadToEnd();
        sr.Close();

        // Get the information from the JSON file
        GetDataFromJSON_UI(fileContents);

        // Change texts
        label1Text.text = UIData.label1;
        label2Text.text = UIData.label2;
        instructions.text = UIData.instructionText;
        button.SetActive(true);
        button.GetComponentInChildren<Text>().text = UIData.buttonLabel;
        title.text = UIData.title;
    }

    /// <summary>
    /// Method to set up the Welcome UI for WebGL experiments
    /// </summary>
    private IEnumerator SetUp_WebGLExperiment(){
        // Setting up UI while the settings are loading
        instructions.GetComponent<Text>().text = "Please wait for settings to load...";
        label1Text.text = "";
        label2Text.text = "";
        button.SetActive(false);

        //////////////////// Get the UI file
        // Get the URL for the UI file
        string UIURLPath = Path.Combine(Application.streamingAssetsPath, fileName);

        // download UI file from StreamingAssets folder
        UnityWebRequest www = UnityWebRequest.Get(UIURLPath);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success){
            Debug.LogError("Error downloading " + fileName + " file: " + www.error);
            yield break;
        }

        // Get the URL to download from
        string fileContents = www.downloadHandler.text;

        // Get the information from the JSON file
        GetDataFromJSON_UI(fileContents);

        //////////////////// Load URL from StreamingAssets
        // Get the URL for the dictionary of studies
        string StudyDictURLPath = Path.Combine(Application.streamingAssetsPath, "study_dict_url.txt");

        // download file from StreamingAssets folder
        www = UnityWebRequest.Get(StudyDictURLPath);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success){
            Debug.LogError("Error downloading study_dict_url.txt file: " + www.error);
            yield break;
        }

        // Get the URL to download from
        string StudyDictURL = www.downloadHandler.text;
        Debug.Log("Downloading Study Dictionary file from: " + StudyDictURL);

        //////////////////// Download the study dictionary
        // download file from the internet
        www = UnityWebRequest.Get(StudyDictURL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success){
            Debug.LogError("Error downloading Study Dictionary file: " + www.error);
            yield break;
        }

        // Get the URL to download from
        string StudyDictContent = www.downloadHandler.text;

        // Get information from the JOSN about the studies
        GetDataFromJSON_StudyDict(StudyDictContent);

        // Change texts
        label1Text.text = UIData.label1;
        label2Text.text = UIData.label2;
        instructions.text = UIData.instructionText;
        button.SetActive(true);
        button.GetComponentInChildren<Text>().text = UIData.buttonLabel;
        title.text = UIData.title;
        inputField.SetActive(true);
    }

    /// <summary>
    /// Method to read in the JSON file contaning the study dictionary for WebGL experiments
    /// </summary>
    void GetDataFromJSON_StudyDict(string fileContents){
        // Get instruction data profile
        StudyDict = JsonUtility.FromJson<StudyDictClass>(fileContents);
    }

    /// <summary>
    /// Method to load the scene
    /// </summary>
    public void LoadScene(){
    	SceneManager.LoadScene("DiamondWorld");
    }

    /// <summary>
    /// Method to read in the JSON file contaning the study dictionary for WebGL experiments
    /// </summary>
    public void ButtonPress(){
#if UNITY_WEBGL
        SubmitStudyID();
#else
        LoadScene();
#endif
    }


    /// <summary>
    /// Method to submit the study ID, set static variable and load scene.
    /// </summary>
    public void SubmitStudyID(){
        Debug.Log("The Study ID was submitted.");
        studyID = inputField.GetComponent<InputField>().text;
        Debug.Log("Study ID: " + studyID);

        // Loop through the study dictionary to find the correct study
        foreach (Study study in StudyDict.studies){
            if (study.studyID == studyID){
                UXF_settings_url = study.UXF_settings_url;
                fileNameForStartUpText = study.startupText;
                Debug.Log("UXF_settings_url: " + UXF_settings_url);
                Debug.Log("startupText: " + fileNameForStartUpText);
                LoadScene();
                return;
            }
        }

        // Throw error if study ID is not found
        string msg = "Study ID not found in study dictionary. Please contact experimenter.";
        Debug.LogError(msg);
        instructions.GetComponent<Text>().text = msg;
    }

    /// <summary>
    /// Method to read in the JSON file that is placed in the StreamingAssets folder for the file that is provided
    /// </summary>
    void GetDataFromJSON_UI(string fileContents) { 
        // Get instruction data profile
        UIData = JsonUtility.FromJson<UIDataClass>(fileContents);
    }
}
