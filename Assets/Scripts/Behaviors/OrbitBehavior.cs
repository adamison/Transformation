using UnityEngine;
using System.Collections;

public class OrbitBehavior : MonoBehaviour {
	
	public Vector3 rotationMask = Vector3.up; 
	public float rotationSpeed = 5.0f; //degrees per second 
	public Component rotateAroundObject = null; 	

	// Use this for initialization
	void Start () {
		//rotationMask = Vector3(0, 1, 0); //which axes to rotate around 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	

	void FixedUpdate() { 
	
	   if (rotateAroundObject != null) {//If true in the inspector orbit <rotateAroundObject>: 
	
	    transform.RotateAround(rotateAroundObject.transform.position, 
	
	    rotationMask, rotationSpeed * Time.deltaTime); 
	
	   } 
	
	   else {//not set -> rotate around own axis/axes: 
	/*
	    transform.Rotate(Vector3( 
	
	    rotationMask.x * rotationSpeed * Time.deltaTime, 
	
	    rotationMask.y * rotationSpeed * Time.deltaTime, 
	
	    rotationMask.z * rotationSpeed * Time.deltaTime)); 
	*/
	   } 
	
	}	
}


