using UnityEngine;
using System.Collections;

public class DoorTriggerBehavior : MonoBehaviour 
{
	private Animator _anim;

	void Start()
	{
		_anim = this.gameObject.GetComponentsInChildren<Animator>()[0];
	}

	void OnTriggerEnter(Collider c)
	{
		if(c.tag == "Player" || c.tag == "Enemy" || c.tag == "NPC")
		{
			if(!_anim.GetBool("Activate")) {
				_anim.SetTrigger("Activate");
			}
			
		}
	}
	
	void OnTriggerExit(Collider c)
	{
		if(c.tag == "Player" || c.tag == "Enemy" || c.tag == "NPC")
		{
			if(!_anim.GetBool("Activate")) {
				_anim.SetTrigger("Activate");
			}
		}
	}
}
