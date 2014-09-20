using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour 
{
	
	bool ready = false;
	float readyTimer = 0.0f;
	
	void Update()
	{
		readyTimer += Time.deltaTime;
		if(readyTimer > 1.0f) ready = true;
		if(ready == true)
		{
			if(Input.GetKey (KeyCode.Escape)) Application.Quit();
			if(Input.anyKey) Application.LoadLevel(0);
		}
	}
	
	void OnMouseUp()
	{
		Application.LoadLevel(0);
	}
	
	void OnGUI()
	{
		DrawPulsingLabel();
	}
	
	
	void DrawPulsingLabel()
	{
		GUI.color = new Color( 1f, 1f, 1f, Mathf.Sin( Time.realtimeSinceStartup * 4f ) * 0.4f + 0.6f );
		
		float labelWidth = 150f;
		float labelHeight = 30f;
		
		string label = "- press any key -";
		
		GUI.Label( new Rect( ( Screen.width - labelWidth ) * 0.5f, ( Screen.height - labelHeight ) * 0.5f + 200f, labelWidth, labelHeight ), label );
	}
	
}
