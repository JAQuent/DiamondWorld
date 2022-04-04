using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UXF;

public class countdown : MonoBehaviour{
	// Public vars
    public AudioClip failSound;
    public Session session;
    public float timeoutPeriod = 60.0f;
    public GameObject countdownText;

    // Prviate vars
    private float startTime;
    private bool drawText = false;
    private bool timeOut = false;
    private GameObject FPC;
    
    public void BeginCountdown(){
        StartCoroutine(Countdown());
        startTime = Time.time;
        drawText = true;
        Debug.Log("Start countdown");
    }

    public void StopCountdown(){
        StopAllCoroutines();
        drawText = false;
    }

    void Update(){
    	if(drawText){
    		float timeLeft = Mathf.Round(timeoutPeriod - (Time.time - startTime));
    		countdownText.GetComponent<Text>().text = timeLeft + " sec left";
    	}
    }

    IEnumerator Countdown(){
        timeoutPeriod = session.settings.GetFloat("timeoutPeriod");
        yield return new WaitForSeconds(timeoutPeriod);

        // Set timeOut to true so it can be played at the end of trial
        timeOut = true;

        // if we got to this stage, that means we moved too slow
        // session.CurrentTrial.result["outcome"] = "tooslow";
        session.EndCurrentTrial();        
    }

    void playTimeOutSound(){
    	if(timeOut){
    		FPC = GameObject.Find("First Person Controller");
    		// we will play a clip at position above origin, 100% volume
    		AudioSource.PlayClipAtPoint(failSound, FPC.transform.position, 2.0f);
    		timeOut = false;
    	}
    }
}
