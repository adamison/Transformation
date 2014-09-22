﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	private Player instance;
	
	public float health = 100f;
	public float maxHealth = 100f;
	public float powerMP = 0.0f;
	public float powerIncrementAmount = 5.0f;	
	public float powerThreshold = 100.0f;
	public float powerChargeModifier = 2.0f;
	public bool charging = false;
	public bool chargeOn = false;
	public GameObject projectilePrefab;
	
	//private static GameObject hands;
	private ParticleSystem chargeEmitter;
	private Component characterController;
	private GameObject projectileEmitter;

	void Awake()
	{
		instance = this;

		chargeEmitter = gameObject.GetComponentInChildren<ParticleSystem>();
		chargeEmitter.enableEmission = false;
				
		//hands = gameObject.transform.FindChild("playerArms").gameObject;
		characterController = gameObject.GetComponent<FPSController> ();

		projectileEmitter = transform.FindChild ("Emitter").gameObject;
	}
	
	void Update()
	{
		if(powerMP > powerThreshold && DataCore._view == DataCore.VIEW.Physical)
		{
			DataCore._view = DataCore.VIEW.Metaphysical;
		}
		
		if(DataCore._view == DataCore.VIEW.Metaphysical)
		{
			// bloom / swap materials
		} 
		
		if (charging == true && chargeOn == false)
		{
			chargeEmitter.enableEmission = true;
			chargeOn = true;
		}
		if (charging == false && chargeOn == true)
		{
			chargeEmitter.enableEmission = false;
			chargeOn = false;
		}
		
		if(health <= 0f)
		{
			DataCore.GameOver();
		}
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if(hit.gameObject.CompareTag("Enemy"))
		{
			DataCore.GameOver();
		}
	}
		
	// Action functions
	public void IncrementPower(float amount)
	{
		powerMP += amount;
	}
	
	public void SwitchView()
	{
		if(DataCore._view == DataCore.VIEW.Physical) 
		{
			DataCore._view = DataCore.VIEW.Metaphysical;
		} else {
			DataCore._view = DataCore.VIEW.Physical;
		}
	}
	
	public void Interact()
	{
		Debug.Log ("interact");
	}
	
	public void Shoot()
	{
		GameObject projectile = (GameObject)Instantiate(projectilePrefab, projectileEmitter.transform.position, transform.rotation);
		projectile.transform.forward = Camera.main.transform.forward;
		
		ProjectileBase newProjectile = projectile.GetComponent<ProjectileBase> ();
		newProjectile.SetStartPosition(projectileEmitter.transform.position);
		newProjectile.SetCreationTime (Time.time);
	}
	
	public void PrimaryAction()
	{
		Interact ();
		//powerMP += powerIncrementAmount;
	}
	
	public void SecondaryAction()
	{
		Shoot ();
		//powerMP -= powerIncrementAmount * powerChargeModifier;
	}
	
}
