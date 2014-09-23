using UnityEngine;
using System.Collections;

public class FlickerBehavior : MonoBehaviour 
{
	public Color[] c;
	Color color;

	public float flickerRate = 1.0f;
	private float maxFlickerRate = 0.0f;
	private float t = 0;
	bool change = false;
	
	public GameObject proximityFromTarget = null;
	public GameObject proximityToTarget = null;
	public bool proximityFlicker = false;
	private float proximity;
	
	void Start()
	{
		color = c[0];
		
		maxFlickerRate = flickerRate;
		
		proximityFromTarget = GameObject.Find("SpawnPoint");
		proximityToTarget = GameObject.Find("BeaconGoal");
		
		if(proximityFromTarget != null && proximityToTarget != null)
		{
			proximity = Vector3.Distance(proximityFromTarget.transform.position, proximityToTarget.transform.position);
		}
	}
	
	void Update()
	{
		// if we have proximity targets, modify the flicker rate to a max, 
		// based on distance between the two
		if(proximityFlicker == true)
		{
			float currentDistance = Vector3.Distance(DataCore.player.transform.position, proximityToTarget.transform.position);
			flickerRate = AICore._IsItMin(currentDistance, 0.0f, proximity);
		}
		
		// flicker the color
		renderer.material.color = Color.Lerp(c[0], c[1], t);
		if(!change)
			t += flickerRate * Time.deltaTime;
		else
			t -= flickerRate * Time.deltaTime;
		if(t>=1)
			change = true;
		if(t<=0)
			change = false;
	}
}
