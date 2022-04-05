using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

public class objectScript : MonoBehaviour{
    // Static vars
    public static int rewardValue = 1;
    public static int punishmentValue = 2;

	// Public vars
	public ParticleSystem pickParticle;
	public AudioClip rewardSound; 
	public AudioClip punishmentSound; 
	public bool rewardObject = false;
    public int index;

    // Private vars
    private List<float> diamondTimings;
    private List<float> icosahedronTimings;
    private bool activated = true;
    private Renderer rend;

    void Start(){
        // Get renderer of the child that contains the renderer
        rend = gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>();
    }

    /// OnTriggerEnter is called  if FPC enters it check whether it's a death cube
    void OnTriggerEnter(Collider other){
        // Only run if activated
        if(activated){
            if (other.name == "First Person Controller"){
                if(!rewardObject){
                    // Log entry
                    Debug.Log("Punishment: Icosahedron picked up " + gameObject.transform.position);

                    // Change score
                    scoreCounter.score  = scoreCounter.score - punishmentValue;
                    Debug.Log("Current score: " + scoreCounter.score);

                    // PLay particle
                    pickParticle.Play();

                    // Play sound
                    AudioSource.PlayClipAtPoint(punishmentSound, gameObject.transform.position, 1.0f);

                    // Set inactive
                    activated = false;
                    rend.enabled = false;

                    // Set reacivationcountdown
                    icosahedronTimings = ExperimentController.icosahedronTimings;
                    StartCoroutine(reactivationCountdown(icosahedronTimings[index]));
                } else {
                    // Log entry
                    Debug.Log("Reward: Diamond picked up " + gameObject.transform.position);

                    // Change score
                    scoreCounter.score  = scoreCounter.score + rewardValue;
                    Debug.Log("Current score: " + scoreCounter.score);

                    // PLay particle
                    pickParticle.Play();

                    // Play sound
                    AudioSource.PlayClipAtPoint(rewardSound, gameObject.transform.position, 1.0f);

                    // Set inactive
                    activated = false;
                    rend.enabled = false;

                    // Set reacivationcountdown
                    diamondTimings = ExperimentController.diamondTimings;
                    StartCoroutine(reactivationCountdown(diamondTimings[index]));
                } 
            }
        }
    }

    //  IEnumerator to handels reactivation
    IEnumerator reactivationCountdown(float timer){
        // Wait 
        yield return new WaitForSeconds(timer);

        // Reactivate
        activated = true;
        rend.enabled = true;

        // Log entry
        Debug.Log("Object reactivated " + gameObject.transform.position);
    }

}
