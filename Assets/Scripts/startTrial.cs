using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

public class startTrial : MonoBehaviour{
    // Public vars
	public Session session; 
	public bool trialEnded = true;
    public AudioClip startSound; 

    /// OnTriggerEnter is called when the Collider 'other' enters the trigger.
    void OnTriggerEnter(Collider other){
    	if (other.name != "Plane" & trialEnded){
    		Debug.Log("Start Trial");
        	session.BeginNextTrial();
            AudioSource.PlayClipAtPoint(startSound, gameObject.transform.position, 1.0f);
        	trialEnded = false;
    	} 
    }

    /// OnTriggerExit is called when the Collider 'other' has stopped touching the trigger.
    public void changeTrialEnded(){
        trialEnded = true;
    }
}
