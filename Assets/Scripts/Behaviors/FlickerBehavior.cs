using UnityEngine;
using System.Collections;

public class FlickerBehavior : MonoBehaviour 
{
	public Color[] c;
	Color color;

	public float flickerRate = 1.0f;
	private float t = 0;
	bool change = false;
	
	void Awake()
	{
		color = c[0]; 
	}
	
	void Update()
	{
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
