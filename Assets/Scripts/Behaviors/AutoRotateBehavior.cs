using UnityEngine;
using System.Collections;

public class AutoRotateBehavior : MonoBehaviour {

	public float Pitch  = 50.0f;
	public float Yaw    = 50.0f;
	public float Roll   = 50.0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Pitch*Time.deltaTime, Yaw*Time.deltaTime, Roll*Time.deltaTime);
	}
}
