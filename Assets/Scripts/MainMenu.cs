using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{

	void Update()
	{
		if(Input.GetKey (KeyCode.Escape)) Application.Quit();
		if(Input.anyKey) Application.LoadLevel(1);
	}
	
	void OnMouseUp()
	{
		Application.LoadLevel(1);
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
		
		GUI.Label( new Rect( ( Screen.width - labelWidth ) * 0.5f, ( Screen.height - labelHeight ), labelWidth, labelHeight ), label );
	}
	
}
