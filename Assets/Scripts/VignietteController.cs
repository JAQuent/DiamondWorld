using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignietteController : MonoBehaviour{
    // Post processing value
    public GameObject GlobalVolume;
    public VolumeProfile volume;
    public float lerpTime = 0.1f;
    private Vignette vignette = null;
    private float max = 0.6f;
    private float min = 0.2f;
    public float intensity = 0.5f;
    public bool go2max = false;
    public bool go2min;
    public static bool vignetteEffectStart = false;

    // Start is called before the first frame update
    void Start(){
        // Find Global volume "Global Volume"
        GlobalVolume = GameObject.Find("Global Volume");
        volume = GlobalVolume.GetComponent<Volume>()?.profile;
        volume.TryGet(out vignette);
    }

    void Update(){
    	// Start
    	if(vignetteEffectStart){
    		go2max = true;
    		go2min = false;
    		vignetteEffectStart = false;
    	}

    	// go2max the vignette
    	if(go2max){
    		intensity = Mathf.Lerp(vignette.intensity.value, max,Time.deltaTime * lerpTime);
    		vignette.intensity.Override(intensity);
    		if(System.Math.Round(max - intensity, 2) <= 0.0f){
    			go2max = false;
    			go2min = true;
    		}
        } 


        // Go back to normal
        if(go2min && !go2max){
    		intensity = Mathf.Lerp(vignette.intensity.value, min,Time.deltaTime * lerpTime);
    		vignette.intensity.Override(intensity);
    		if(System.Math.Round(intensity - min, 2) <= 0.0f){
    			go2min = false;
    		}
        }
    }
}
