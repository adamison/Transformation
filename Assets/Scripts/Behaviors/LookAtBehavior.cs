using UnityEngine;
using System.Collections;

public class LookAtBehavior : MonoBehaviour {
	
	public Component target;
	
	void FixedUpdate () {
		RULECORE._SeekTarget(this, target.transform.position, 0.0f);
	}
}
