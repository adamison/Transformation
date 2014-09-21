using UnityEngine;
using System.Collections;

public class EnemySight : EnemyBase
{
	public float fieldOfViewAngle = 110f;
	public float captureDistance = 1.5f;
	public bool playerInSight;
	public bool playerInRange;
	public GameObject inSightPrefab;
	public GameObject inRangePrefab;
	public Vector3 lastSeenPosition;

	private SphereCollider col;
	private GameObject player;
	private GameObject inSightAlert;
	private GameObject inRangeAlert;

	void Start ()
	{
		col = GetComponentsInChildren<SphereCollider>()[0];
		player = DataCore.player.gameObject;//GameObject.FindGameObjectWithTag("Player");
		
		// create instances of alert prefabs
		inSightAlert = Instantiate(inSightPrefab) as GameObject;
		inRangeAlert = Instantiate(inRangePrefab) as GameObject;
		
		// parent prefabs to player
		inSightAlert.transform.position = new Vector3(transform.position.x, transform.position.y + 1.25f, transform.position.z);
		inRangeAlert.transform.position = inSightAlert.transform.position;
		
		inSightAlert.transform.parent = transform;
		inRangeAlert.transform.parent = transform;
		
		// hide the alerts
		inSightAlert.SetActive(false);
		inRangeAlert.SetActive(false);	
	}

	
	void FixedUpdate ()
	{	
		if (playerInRange == true)
		{
			inRangeAlert.SetActive(true);
			inSightAlert.SetActive(false);
			DataCore.player.health -= 5f;
		}
		else if (playerInSight == true)
		{
			inSightAlert.SetActive (true);
			inRangeAlert.SetActive(false);
		}
		
		if (playerInRange == false) inRangeAlert.SetActive(false);
		if (playerInSight == false) inSightAlert.SetActive(false);
		
	}

	
	
	void OnTriggerStay (Collider other)
	{
		// If the player has entered the trigger sphere...
		if(other.gameObject == player)
		{			
			playerInSight = false;
			playerInRange = false;
			
			// Create a vector from the enemy to the player and store the angle between it and forward.
			Vector3 direction = other.transform.position - transform.position;
			float angle = Vector3.Angle(direction, transform.forward);
					
			// If the angle between forward and where the player is, is less than half the angle of view...
			if(angle < fieldOfViewAngle * 0.5f)
			{
				RaycastHit hit;

				if(Physics.Raycast(transform.position, direction.normalized, out hit))
				{
					if(hit.collider.gameObject == player)
					{
						playerInSight = true;
						if(hit.distance < captureDistance) playerInRange = true;
					}
				}
			}
		}
	}
	
	void OnTriggerExit (Collider other)
	{
		// If the player leaves the trigger zone...
		if(other.gameObject == player)
		{
			// The player is not in sight.
			playerInSight = false;
			playerInRange = false;

			if (playerInSight) lastSeenPosition = player.transform.position;
		}
	}
	
}