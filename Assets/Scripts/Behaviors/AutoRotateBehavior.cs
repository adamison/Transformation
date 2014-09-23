using UnityEngine;
using System.Collections;

public class AutoRotateBehavior : MonoBehaviour {

	public float Pitch  = 50.0f;
	public float Yaw    = 50.0f;
	public float Roll   = 50.0f;

	public bool IntermittentChange = false;
	public float changeRate = 1.0f;
	public float pitchChange = 0.0f;
	public float yawChange = 0.0f;
	public float rollChange = 0.0f;
	// Use this for initialization
	void Start () {
		if (IntermittentChange) {
			InvokeRepeating ("ChangeDirection", changeRate, changeRate);
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Pitch*Time.deltaTime, Yaw*Time.deltaTime, Roll*Time.deltaTime);
	}

	void ChangeDirection() 
	{
		Pitch += Random.Range (-pitchChange, pitchChange);
		Yaw += Random.Range (-yawChange, yawChange);
		Roll += Random.Range (-rollChange, rollChange);

	}
}
