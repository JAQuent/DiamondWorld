using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

public class targetCube : MonoBehaviour{
	// Public vars
	public ParticleSystem pickParticle;
	public AudioClip collectSound; 
	public AudioClip deathCubeSound; 
	public Material baseMat;
	public Material redMat;
	public Material greenMat;
	public float uncoverDist = 10.0f;
	public bool deathCube = false;

	// Private vars
	private GameObject FPC;
	private GameObject experiment;

	void Start(){
		experiment = GameObject.Find("Experiment");
		FPC = GameObject.Find("First Person Controller");
	}
	void Update(){
		// Get distance between cube and FPC
		float dist = Vector3.Distance(FPC.transform.position, transform.position);

		// Use green material if close and not deathCube
		if(dist < uncoverDist & !deathCube){
			 this.GetComponent<Renderer>().material = greenMat;
		} 
		// Use red material if close and deathCube
		if(dist < uncoverDist & deathCube){
			 this.GetComponent<Renderer>().material = redMat;
		} 
		// Use blue matrial if further away
		if(dist >= uncoverDist){
			 this.GetComponent<Renderer>().material = baseMat;
		} 
	}

    /// OnTriggerEnter is called  if FPC enters it check whether it's a death cube
    void OnTriggerEnter(Collider other){
    	if (other.name == "First Person Controller"){
    		if(deathCube){
    			Debug.Log("Oh, no you picked up the wrong cube!");
    			AudioSource.PlayClipAtPoint(deathCubeSound, gameObject.transform.position, 1.0f);
    			Destroy(gameObject);
    			Session.instance.CurrentTrial.End();
    		} else {
	    		Debug.Log("Item picked up");
	            //experiment.GetComponent<scoreCounter>().score ++;
	            pickParticle.Play();
	            AudioSource.PlayClipAtPoint(collectSound, gameObject.transform.position, 1.0f);
	            Destroy(gameObject);
    		}
    	} 
    }
}
