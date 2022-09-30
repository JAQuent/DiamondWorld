using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeScreen : MonoBehaviour{
    public void LoadScene(){
    	Debug.Log("Start button pressed.");
    	SceneManager.LoadScene("DiamondWorld");
    }
}
