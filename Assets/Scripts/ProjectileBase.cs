using UnityEngine;
using System.Collections;

// base functionality that is used in all projectiles
public class ProjectileBase : MonoBehaviour
{
	public float speed = 20f;
	public float lifeTime = 2f;
	public float damage = 10f;
	public GameObject hitFX = null;
	
	private double creationTime;
	private Vector3 startPosition;

	private Vector3 forwardOffset;
	
	void Start()
	{
		forwardOffset = Vector3.zero;
	}
	
	public void SetStartPosition( Vector3 position )
	{
		startPosition = position + forwardOffset;
	}
	
	public void SetCreationTime( double time )
	{
		creationTime = time;
	}
		
	void FixedUpdate()
	{
		float timePassed = (float) (Time.time - creationTime);
		transform.position += transform.forward * speed * Time.deltaTime;
		
		if( timePassed > lifeTime )
		{
			Destroy( gameObject );
		}
	}
	
	void CreatehitFX()
	{
		if (hitFX != null)
		{
			Instantiate( hitFX, transform.position, transform.rotation );
		}
	}
	
	public void OnProjectileHit()
	{
		Destroy( gameObject );
		CreatehitFX();
	}
	
	void OnCollisionEnter( Collision collision )
	{
		if( collision.collider.tag == "Enemy" )
		{
			Enemy enemy = collision.collider.GetComponent<Enemy>();
			enemy.OnProjectileHit( this );
			OnProjectileHit();
		}
		else
		{
			OnProjectileHit();
		}
	}
}