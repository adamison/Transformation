using UnityEngine;
using System.Collections;

public class JitterTextBehavior : MonoBehaviour 
{

	int trailCount = 5;
	float textAlpha = 0.0f;
	TextMesh textMesh = null;
	TextMesh[] trail;
	
	bool fadeIn = true;

	void Start ()
	{
		textMesh = this.GetComponentInChildren<TextMesh>();
		textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, textAlpha);
		
		// clone the mesh at random postions around the oringinal
		trail = new TextMesh[trailCount];
		for (int i = 0; i < trailCount; i++)
		{
			// generate the postion
			Vector3 newPosition = new Vector3(
				Random.Range(textMesh.transform.position.x-0.75F, textMesh.transform.position.x+0.75F), 
				Random.Range(textMesh.transform.position.y-0.75F, textMesh.transform.position.y+0.75F), 
				textMesh.transform.position.z
			);
			trail[i] = Instantiate(textMesh, newPosition, textMesh.transform.rotation) as TextMesh;
			trail[i].transform.parent = transform;
			trail[i].color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 1.0f * Random.Range(0.1f, 0.5f));
		}
	}
	
	void Update ()
	{
		
		if(fadeIn && textAlpha < 1.0f)
		{
			textAlpha += 0.75f * Time.deltaTime;			
		}
		else
		{
			//textAlpha -= 0.5f * Time.deltaTime;
			fadeIn = false;
		}
		textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, textAlpha);
		
		for (int i = 0; i < trailCount; i++)
		{
			float newAlpha = trail[i].color.a;
			newAlpha -= 0.25f * Time.deltaTime;
			trail[i].color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, newAlpha);
		}
	}
}
