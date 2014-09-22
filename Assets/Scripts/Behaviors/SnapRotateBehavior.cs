using UnityEngine;
using System.Collections;

public class SnapRotateBehavior : MonoBehaviour {

	public float Pitch = 50.0f;
	public float Yaw = 50.0f;
	public float Roll = 50.0f;
	public float snapDelay = 1.0f;
	public Quaternion targetRotation;
	public float rotationRate = 10.0f;
	// Use this for initialization
	void Start () {
		//InvokeRepeating("SetNewRotationTarget", snapDelay, snapDelay);
		Invoke ("SetNewRotationTarget", snapDelay);
	}
	
	// Update is called once per frame
	void Update () {
		//transform.Rotate(Pitch*Time.deltaTime, Yaw*Time.deltaTime, Roll*Time.deltaTime);
		transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRotation, rotationRate);
	}

	void SetNewRotationTarget() {
		Quaternion newTarget = Quaternion.Euler (new Vector3(
			transform.rotation.eulerAngles.x + Pitch, 
			transform.rotation.eulerAngles.y + Yaw, 
			transform.rotation.eulerAngles.z + Roll)
		                                         );

		targetRotation = newTarget;
		Invoke ("SetNewRotationTarget", snapDelay);
	}
}
