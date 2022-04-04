using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach this to an object and let it rotate

public class Rotate : MonoBehaviour{
	// Public vars
	public bool xRotation;
	public bool yRotation;
	public bool zRotation;
	public float rotationSpeed = 100.0f;

	// Private vars
	private float x = 0.0f;
	private float y =  0.0f;
	private float z = 0.0f;

	// Create rotation vector3 
	void Start(){
		if(xRotation){
			x = rotationSpeed;
		} 
		if(yRotation){
			y = rotationSpeed;
		}
		if(zRotation){
			z = rotationSpeed;
		}
	}

    // Update is called once per frame
    void Update(){
        transform.Rotate(new Vector3(x, y, z) * Time.deltaTime);
    }
}
