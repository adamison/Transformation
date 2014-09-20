using UnityEngine;
using System.Collections;

public class HoveringObjectBehavior : MonoBehaviour 
{
	public float _Amplitude = 0.0125f;

	void FixedUpdate () 
	{	
		float ydelta = _Amplitude * Mathf.Cos(Time.time);
		this.transform.Translate(0f, ydelta, 0f);
	}
}
