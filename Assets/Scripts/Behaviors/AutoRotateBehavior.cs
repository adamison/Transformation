using UnityEngine;
using System.Collections;

public class AutoRotateBehavior : MonoBehaviour {

	public float YawRate = 50.0f;
	public float TiltRate = 50.0f;
	public float RollRate = 50.0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(TiltRate*Time.deltaTime, YawRate*Time.deltaTime, RollRate*Time.deltaTime);
	}
}
