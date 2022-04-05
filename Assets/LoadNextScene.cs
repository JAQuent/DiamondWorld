using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour{
    public void LoadScene(){
    	Debug.Log("Start button pressed.");
    	SceneManager.LoadScene("DiamondWorld");
    }
}
