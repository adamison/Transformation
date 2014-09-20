using UnityEngine;
using System.Collections;

public class PlayerEvents : MonoBehaviour 
{
	void Start () 
	{
	
	}

	void Interact() 
	{
		DataCore.player.PrimaryAction ();
	}

	void Ability () 
	{
		DataCore.player.SecondaryAction ();
	}
}
