using UnityEngine;
using System.Collections;

public class ProximityCollectBehavior : MonoBehaviour
{
	public float rateModifier = 1.0f;
	public float speedupRate = 0.2f;
	
	private bool isTriggered;
	
	void DeleteAllPhysicsColliders()
	{
		Collider[] arr = this.GetComponentsInChildren<Collider>();
		if(arr != null)
		{
			foreach(Collider c in arr)
			{
				if(c.isTrigger) continue;
				Destroy(c);
			}
		}
	}
	
	void OnTriggerStay(Collider other) 
	{
		if(other.tag == "Player")
		{
			isTriggered = true;	
			DeleteAllPhysicsColliders();
		}
	}
	
	void Update ()
	{
					
		if(isTriggered)
		{		
			if(DataCore.player != null)
			{	
				transform.root.position = Vector3.Lerp(
					transform.root.position,
					DataCore.player.transform.position + 0.0f * Vector3.up,
					rateModifier * Time.deltaTime
				);
				
				rateModifier += speedupRate;
			
				if(transform.root.position == DataCore.player.transform.position) Destroy(this.gameObject);
			}
		}
		
	}
	
}
