using UnityEngine;
using System.Collections;

public class Story : MonoBehaviour 
{
	
	public GameObject textPrefab;
	public bool first, second, third, fourth;
	private GameObject uiCamera;
	private bool finished = false;

	void Start () 
	{
		uiCamera = GameObject.Find ("UICamera");
	}
	
	void OnTriggerStay (Collider c) 
	{
		if(c.CompareTag("Player"))
		{
			if(first && !finished) FirstStory();
			if(second && !finished) SecondStory();
			if(third && !finished) ThirdStory();
			if(fourth && !finished) FourthStory();
		}
	}
	
	float storyDelay = 2.0f;
	int storyIndex = 0;
	
	public void FirstStory()
	{	
		string[] storyText = { "Where am I?...", "What was that?...", "I'd better follow it..." };
		
		storyDelay += 0.01f;
		if(storyDelay > 2.25f && storyIndex <= storyText.Length-1)
		{
			storyDelay = 0.0f;
			GameObject text = (GameObject)Instantiate(textPrefab, uiCamera.transform.position + uiCamera.transform.forward * 2, uiCamera.transform.rotation);
			text.transform.parent = uiCamera.transform;
			TextMesh textMesh = text.GetComponentInChildren<TextMesh>();
			textMesh.text = storyText[storyIndex];
			Destroy (text, 2);
			
			storyIndex++;
		}
		
		if(storyIndex >= storyText.Length)
		{
			finished = true;
		}
	}
	
	public void SecondStory()
	{
		string[] storyText = { "Guards! Better be careful...", "Look at the walls..." };
		
		storyDelay += 0.01f;
		if(storyDelay > 2.25f && storyIndex <= storyText.Length-1)
		{
			storyDelay = 0.0f;
			GameObject text = (GameObject)Instantiate(textPrefab, uiCamera.transform.position + uiCamera.transform.forward * 2, uiCamera.transform.rotation);
			text.transform.parent = uiCamera.transform;
			TextMesh textMesh = text.GetComponentInChildren<TextMesh>();
			textMesh.text = storyText[storyIndex];
			Destroy (text, 2);
			
			storyIndex++;
		}
		
		if(storyIndex >= storyText.Length)
		{
			finished = true;
		}
	}
	
	public void ThirdStory()
	{
		string[] storyText = { "I feel... ill...", "Something isn't right here..." };
		
		storyDelay += 0.01f;
		if(storyDelay > 2.25f && storyIndex <= storyText.Length-1)
		{
			storyDelay = 0.0f;
			GameObject text = (GameObject)Instantiate(textPrefab, uiCamera.transform.position + uiCamera.transform.forward * 2, uiCamera.transform.rotation);
			text.transform.parent = uiCamera.transform;
			TextMesh textMesh = text.GetComponentInChildren<TextMesh>();
			textMesh.text = storyText[storyIndex];
			Destroy (text, 2);
			
			storyIndex++;
		}
		
		if(storyIndex >= storyText.Length)
		{
			finished = true;
		}
	}
	
	public void FourthStory()
	{
		DataCore.player.fadeOut = true;
	
		string[] storyText = { "Ahh...", "What's happening to me?..." };
		
		storyDelay += 0.01f;
		if(storyDelay > 2.25f && storyIndex <= storyText.Length-1)
		{
			storyDelay = 0.0f;
			GameObject text = (GameObject)Instantiate(textPrefab, uiCamera.transform.position + uiCamera.transform.forward * 2, uiCamera.transform.rotation);
			text.transform.parent = uiCamera.transform;
			TextMesh textMesh = text.GetComponentInChildren<TextMesh>();
			textMesh.text = storyText[storyIndex];
			Destroy (text, 2);
			
			storyIndex++;
		}
		
		if(storyIndex >= storyText.Length)
		{
			finished = true;
		}
	}
}
