using UnityEngine;
using System.Collections;

public class LightBehavior : MonoBehaviour
{
	private int maxRays = 50;

	// Cast rays from light position 
	// with random sampling within the light cone angle			
	void FixedUpdate () {
		for(int i = 0; i < maxRays; i++)
		{
			Vector3 target = RandomSpotlightPoint(light) - light.transform.position;
			
			RaycastHit hit;
			
			if(Physics.Raycast(light.transform.position, target, out hit))
			{
				if(hit.transform.gameObject.tag == "Player")
				{
					Player player = hit.transform.gameObject.GetComponent<Player>();
					player.IncrementPower(player.powerIncrementAmount * Time.deltaTime);
				}
			}
		}
	}
	
	Vector3 RandomSpotlightPoint(Light l)
	{
		float radius = Mathf.Tan(Mathf.Deg2Rad * l.spotAngle / 2f) * l.range;
		Vector2 circle = Random.insideUnitCircle * radius;
		Vector3 target = l.transform.position + l.transform.forward * l.range + l.transform.rotation * new Vector3(circle.x, circle.y);
		return target;
	}
	
}
