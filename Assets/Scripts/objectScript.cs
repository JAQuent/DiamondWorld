using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class objectScript : MonoBehaviour{
    // Static vars
    public static int rewardValue = 1;
    public static int punishmentValue = 2;

	// Public vars
	//public ParticleSystem pickParticle;
	public AudioClip rewardSound; 
	public AudioClip punishmentSound; 
	public bool rewardObject = false;
    public int index;

    // Private vars
    private List<float> diamondTimings;
    private List<float> trapTimings;
    private bool activated = true;
    private ParticleSystem particle;
    private Collider m_Collider;

    void Start(){
        // Get particle system of the parent object
        particle = gameObject.transform.parent.gameObject.GetComponent<ParticleSystem>();

        //Fetch the GameObject's Collider (make sure it has a Collider component)
        m_Collider = GetComponent<Collider>();
    }

    /// OnTriggerEnter is called  if FPC enters it check whether it's a death cube
    void OnTriggerEnter(Collider other){
        // Only run if activated
        if(activated){
            if (other.name == "First Person Controller"){
                if(!rewardObject){
                    // Log entry
                    Debug.Log("Punishment: Trap" + index + " picked up " + gameObject.transform.position);

                    // Punishment vignette & warning
                    VignietteController.vignetteEffectStart = true;
                    ExperimentController.warningShow = true;

                    // Change score
                    scoreCounter.score  = scoreCounter.score - punishmentValue;
                    Debug.Log("Current score: " + scoreCounter.score);

                    // Play sound
                    AudioSource.PlayClipAtPoint(punishmentSound, gameObject.transform.position, 1.0f);

                    // Set inactive
                    activated = false;
                    //end.enabled = false;
                    m_Collider.enabled = false;

                    // Set reacivationcountdown
                    trapTimings = ExperimentController.trapTimings;
                    StartCoroutine(reactivationCountdown(trapTimings[index]));
                } else {
                    // Log entry
                    Debug.Log("Reward: Diamond" + index + " picked up " + gameObject.transform.position);

                    // Change score
                    scoreCounter.score  = scoreCounter.score + rewardValue;
                    Debug.Log("Current score: " + scoreCounter.score);

                    // Play sound
                    AudioSource.PlayClipAtPoint(rewardSound, gameObject.transform.position, 1.0f);

                    // Set inactive
                    activated = false;
                    //rend.enabled = false;
                    m_Collider.enabled = false;

                    // Set reacivationcountdown
                    diamondTimings = ExperimentController.diamondTimings;
                    StartCoroutine(reactivationCountdown(diamondTimings[index]));
                }

                // Stop paricle system
                particle.Stop();
            }
        }
    }

    //  IEnumerator to handels reactivation
    IEnumerator reactivationCountdown(float timer){
        // Wait 
        yield return new WaitForSeconds(timer);

        // Reactivate
        activated = true;
        m_Collider.enabled = true;
        particle.Play();

        // Log entry
        Debug.Log("Object reactivated " + gameObject.transform.position);
    }

}
