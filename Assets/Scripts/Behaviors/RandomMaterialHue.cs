using UnityEngine;
using System.Collections;

public class RandomMaterialHue : MonoBehaviour {

	//public Material[] materialsToChange;
	public float saturation = 1.0f;
	public float brightness = 1.0f;
	public float alpha = 1.0f;
	public float colorGenerationRate = 1.0f;
	public bool continuous = false;
	// Use this for initialization
	void Start () {
		if (continuous) {
			InvokeRepeating ("GenerateNewColor", 0.0f, colorGenerationRate);
		} else {
			Invoke ("GenerateNewColor", colorGenerationRate);
		}

	}
	
	// Update is called once per frame
	void Update () {
		//foreach(Material mat in materialsToChange) {
		//mat.color = newColor.ToColor();
		//}
	}

	void GenerateNewColor()
	{
		HSBColor newColor = new HSBColor(Random.value, saturation, brightness, alpha);

		renderer.material.color = newColor.ToColor();


	}
}
