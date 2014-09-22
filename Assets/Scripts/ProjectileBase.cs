using UnityEngine;
using System.Collections;

// base functionality that is used in all projectiles
public class ProjectileBase : MonoBehaviour
{
	public float speed = 20f;
	public float lifeTime = 2f;
	public float damage = 10f;
	public GameObject hitFX = null;
	public ParticleSystem particles = null;

	public GameObject hitEnemy = null;

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
			GameObject fx = Instantiate( hitFX, transform.position, transform.rotation ) as GameObject;
			if(hitEnemy) fx.transform.parent = hitEnemy.transform;
		}
	}
	
	public void OnProjectileHit()
	{
		particles.transform.parent = null;
		particles.Stop ();
		Destroy (particles, particles.startLifetime);
		CreatehitFX();
		Destroy( gameObject );
	}
	
	void OnCollisionEnter( Collision collision )
	{
		if( collision.collider.tag == "Enemy" )
		{
			Enemy enemy = collision.collider.GetComponent<Enemy>();
			if(enemy) enemy.OnProjectileHit( this );
			hitEnemy = collision.collider.gameObject;
			OnProjectileHit();
		}
		else
		{
			OnProjectileHit();
		}
	}
}