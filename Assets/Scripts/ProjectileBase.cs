using UnityEngine;
using System.Collections;

// base functionality that is used in all projectiles
public class ProjectileBase : MonoBehaviour
{
	public float speed = 20f;
	public float lifeTime = 2f;
	public float damage = 10f;
	public GameObject hitFX = null;
	public GameObject enemyHitFX = null;
	public GameObject lightBleedFX = null;
	public ParticleSystem particles = null;
	
	public bool enemyProjectile = false;

	public GameObject hitEnemy = null;

	private double creationTime;
	private Vector3 startPosition;

	private Vector3 forwardOffset;
	
	void Start()
	{
		forwardOffset = Vector3.zero;
		Invoke ("Destroy", lifeTime);
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
		transform.position += transform.forward * speed * Time.deltaTime;
	}
	
	void CreatehitFX()
	{
		if (hitFX != null)
		{

			if(hitEnemy && lightBleedFX != null) {
				Instantiate(enemyHitFX, transform.position, transform.rotation);
				GameObject fx = Instantiate(lightBleedFX, transform.position, transform.rotation) as GameObject;
				fx.transform.parent = hitEnemy.transform.parent;

			} else {
				GameObject fx = Instantiate(hitFX, transform.position, transform.rotation) as GameObject;
			}
		}
	}
	
	public void OnProjectileHit()
	{
		Destroy ();
		CreatehitFX();
		Destroy( gameObject );
	}
	
	public void Destroy()
	{
		particles.transform.parent.parent = null;
		particles.Stop ();
		
		Destroy( gameObject );
	}
	
	void OnCollisionEnter( Collision collision )
	{	
		if(!enemyProjectile)
		{
			if( collision.collider.gameObject.CompareTag("Enemy"))
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
		else
		{
			if( collision.collider.gameObject.CompareTag("Player"))
			{
				Player player = collision.collider.GetComponent<Player>();
				if(player) player.OnProjectileHit( this );
				hitEnemy = collision.collider.gameObject;
				OnProjectileHit();
			}
		}
	}
}