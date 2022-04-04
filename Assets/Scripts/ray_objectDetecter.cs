using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ray_objectDetecter : MonoBehaviour{
    // Public variable
    public Camera cam;
    public List<Ray> rays = new List<Ray>();
    public List<float> x = new List<float>();
    public List<float> y = new List<float>();
    public int numRays;
    public List<string> objectDetected = new List<string>();

    // Private variables
    private bool listPopulated = false; 

    void Start(){
        numRays = y.Count;
    }
  

    void Update(){   
        if(!listPopulated){
            for (int i = 0; i < numRays; i++){
                Ray ray = cam.ViewportPointToRay(new Vector3(x[i], y[i], 0));
                rays.Add (ray);

                Debug.DrawRay(rays[i].origin, rays[i].direction * 50, Color.red);
                RaycastHit hit1;

                if (Physics.Raycast(rays[i], out hit1)){
                    Debug.DrawRay(rays[i].origin, rays[i].direction * 50, Color.green);
                    print("I'm looking at " + hit1.transform.name + " with ray " + i);
                }
            }

            listPopulated = true; 

        } else {
            for (int i = 0; i < numRays; i++){
                Ray ray = cam.ViewportPointToRay(new Vector3(x[i], y[i], 0));
                rays[i] = ray;

                Debug.DrawRay(rays[i].origin, rays[i].direction * 50, Color.red);
                RaycastHit hit1;

                if (Physics.Raycast(rays[i], out hit1)){
                    print("I'm looking at " + hit1.transform.name + " with ray " + i);
                    Debug.DrawRay(rays[i].origin, rays[i].direction * 50, Color.green);
                }
            }
        }
    }
}

// Check if x and y values are the same
// might make function out of casting rays, visualising rays
// feed x and values in a way that they are not cast sequentially