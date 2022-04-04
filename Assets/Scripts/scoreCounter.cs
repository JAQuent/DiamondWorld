using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UXF;

public class scoreCounter : MonoBehaviour{
    // Static variables
    public static int score; 

	// Public vars
	public Session session; 
	public GameObject scoretext;

	// Private vars
	private bool drawScore = false;

    // Update is called once per frame
    void Update(){
    	// Display score on screen
    	if(drawScore){
    		scoretext.GetComponent<Text>().text = "Score: "+ score;
    	}

    }

    // Set draw to be added as event  in UXF rig
    void setDrawScoreTrue(){
    	drawScore = true;
    	score = 0;
    }

    void setDrawScoreFalse(){
    	drawScore = false;
    }
}
