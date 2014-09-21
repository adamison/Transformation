﻿using UnityEngine;
using System.Collections;

public class Enemy : EnemyBase 
{
	public CharacterController controller;

	public enum EnemyState { patrolling, searching, chasing, idle };

	public EnemyState enemyState = EnemyState.patrolling;

	public float currentHealth;
	public float maxHealth = 100f;
	public Vector3 startPosition;
	public GameObject chaseTarget;

	public float walkSpeed = 1.25f;
	public float runSpeed = 3.25f;
	
	public float searchTime = 4.0f;

	protected Animator animator;	
	private float directionDampTime = 0.25f;
	
	private GameObject player;

	// Pathfinding
	private int _CURRENT_STATE = 0;
	private int _STATE_INITIALIZE = 0;
	private int _STATE_WAIT_FOR_TARGET = 1;
	private int _STATE_FIND_PATH = 2;
	private int _STATE_DETERMINE_NEXT_WAYPOINT = 3;
	private int _STATE_TURN_TO_WAYPOINT = 4;
	private int _STATE_SEEK_WAYPOINT = 5;
	
	private PathCore.PathFinder _PathFinder = new PathCore.PathFinder();
	
	public Component[] _AllWaypoints = null;
	public Component[] _TempTargetWayPoints = null;
	public Component[] _TargetWayPoints = null;
	private int _next_target = 0;
	
	private Component _Target = null;
	// end pathfinding

	void Start()
	{
		player = DataCore.player.gameObject;
		controller = GetComponent<CharacterController> ();
		animator = GetComponentInChildren<Animator>();
		startPosition = transform.position;
		currentHealth = maxHealth;
		chaseTarget = null;
		
		_AllWaypoints = GameObject.FindObjectsOfType<WayPoint>();
		_TempTargetWayPoints = _TargetWayPoints;
	}

	void FixedUpdate()
	{
		StateManager();
	}

	
	void StateManager()
	{
		switch(enemyState)
		{
			case EnemyState.idle:
				//Relax and look around (rotate occasionally)
				break;
			case EnemyState.patrolling:
				//if(_TempTargetWayPoints != null) _TargetWayPoints = _TempTargetWayPoints;
			
				animator.SetFloat("Speed", walkSpeed);
				//animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
				DoPathfinding();
				
				break;		
			case EnemyState.chasing:
				animator.SetFloat("Speed", runSpeed);
				
				//if I see the player, chase him
				enemyState = EnemyState.chasing;
				chaseTarget = player;
				
				if(chaseTarget != null) _SeekTarget(chaseTarget.transform.position, runSpeed);
				
				//else go to the last seen position and enter search state
				if(!EnemySight.playerInSight)
				{
					StateSearchingEnter();														
				}
				break;				
			case EnemyState.searching:
				animator.SetFloat("Speed", walkSpeed);			
				
				DoPathfinding();
				
				//_RotateYaw(2f);
				break;			
		}
		
		if(EnemySight.playerInSight)
		{
			StateChasingEnter();
		}
	}
	
	void StatePatrolEnter()
	{
		_TargetWayPoints = _TempTargetWayPoints;
		enemyState = Enemy.EnemyState.patrolling;		
	}
	void StateSearchingEnter()
	{
		
		// Get the nearest waypoint to us, and the nearest waypoint to the lastSeenPos
		Component closestWp = null;
		Component closestWpToLastSeen = null;
		foreach (Component wp in _AllWaypoints)
		{
			if(closestWp == null) closestWp = wp;
			if(closestWpToLastSeen == null) closestWpToLastSeen = wp;
			
			if(Vector3.Distance(transform.position, wp.transform.position) < Vector3.Distance(transform.position, closestWp.transform.position))
			{
				closestWp = wp;
			}
			
			if(Vector3.Distance(EnemySight.lastSeenPosition, wp.transform.position) < Vector3.Distance(EnemySight.lastSeenPosition, closestWpToLastSeen.transform.position))
			{
				closestWpToLastSeen = wp;
			}
		}				
		//_SeekTarget(EnemySight.lastSeenPosition, runSpeed);
		_TargetWayPoints = new Component[] { closestWp, closestWpToLastSeen };
			
		Invoke ("StatePatrolEnter", searchTime);
		enemyState = EnemyState.searching;
		//set animator
	}
	void StateChasingEnter()
	{
		CancelInvoke("StatePatrolEnter");
		enemyState = EnemyState.chasing;
	}
	
	void DoPathfinding()
	{
		// Do PF
		if(_CURRENT_STATE == _STATE_INITIALIZE) {
			_PathFinder._OnInitialize();
			_CURRENT_STATE = _STATE_WAIT_FOR_TARGET;
			_Target = _TargetWayPoints[_next_target];
		}
		
		if(_CURRENT_STATE == _STATE_WAIT_FOR_TARGET) {
			_CURRENT_STATE = _STATE_FIND_PATH;	
		}
		
		if(_CURRENT_STATE == _STATE_FIND_PATH) {
			_PathFinder._FindPath(this.transform.position, _Target.transform.position);
			_CURRENT_STATE = _STATE_DETERMINE_NEXT_WAYPOINT;
		}
		
		if(_CURRENT_STATE == _STATE_DETERMINE_NEXT_WAYPOINT) {
			_Target = _PathFinder._GetNextUntraveledWayPointOnPath(this.transform.position);
			if(_Target != null) {
				_CURRENT_STATE = _STATE_TURN_TO_WAYPOINT;
			} else {
				_next_target++;
				if(_next_target >= _TargetWayPoints.Length) {
					_next_target = 0;	
				}
				_Target = _TargetWayPoints[_next_target];
				_CURRENT_STATE = _STATE_WAIT_FOR_TARGET;
			}
		}
		
		if(_CURRENT_STATE == _STATE_TURN_TO_WAYPOINT) {
			if(_SeekTarget(_Target.transform.position, 0.0f)) {
				_CURRENT_STATE = _STATE_SEEK_WAYPOINT;	
			}
		}
		
		if(_CURRENT_STATE == _STATE_SEEK_WAYPOINT) {
			if(_SeekTarget(_Target.transform.position, walkSpeed)) {
				_PathFinder._MarkWayPointAsTraveled(_Target);
				_CURRENT_STATE = _STATE_DETERMINE_NEXT_WAYPOINT;
			}
		}	
	}
	
	public void Respawn()
	{
		transform.position = startPosition;
		currentHealth = maxHealth;
	}

	public void OnProjectileHit(ProjectileBase projectile)
	{
		currentHealth = currentHealth - projectile.damage;
		//transform.position = Vector3.Lerp(transform.position, transform.position + (projectile.transform.forward * 5f), 0.1f );
	}





	// Pathfinding
	public void _RotateYaw(float fTurnRate) {
		if(fTurnRate > 6.0f) fTurnRate = 6.0f;
		if(fTurnRate < -6.0f) fTurnRate = -6.0f;
		transform.Rotate(fTurnRate * Vector3.up);
	}
	
	public void _MoveForward(float fVelocity) {
		//bot.transform.Translate(fVelocity * Vector3.forward, Space.Self);	
		//CharacterController controller = bot.GetComponent<CharacterController> ();
		controller.Move (fVelocity * transform.forward * Time.deltaTime);
	}
	
	// _SeekTarget : Seeks out the indicated target and returns true when reached
	public bool _SeekTarget(Vector3 target, float fMaxVelocity) {
		float fTargetDistance;
		float zIsTargetBehindMe, zIsTargetInFrontOfMe, zIsTargetToMyLeft, zIsTargetToMyRight;
		AICore._GetSpatialAwareness2D(this, target, out fTargetDistance, out zIsTargetBehindMe, out zIsTargetInFrontOfMe, out zIsTargetToMyLeft, out zIsTargetToMyRight);
		
		// Detect whether TARGET is sufficiently in front
		if(zIsTargetInFrontOfMe > 0.99) {
			// Satisfactally facing target	
			// No need to turn
		} else {
			// Should we turn right or left?
			if(zIsTargetToMyRight > zIsTargetToMyLeft) {
				// Turn right
				float fTurnRate;
				if(zIsTargetBehindMe > zIsTargetToMyRight) {
					fTurnRate = AICore._Defuzzify(zIsTargetBehindMe, 0.0f, 6.0f);					
				} else {
					fTurnRate = AICore._Defuzzify(zIsTargetToMyRight, 0.0f, 6.0f);
				}
				_RotateYaw(fTurnRate);
			} else {
				// Turn left
				float fTurnRate;
				if(zIsTargetBehindMe > zIsTargetToMyLeft) {
					fTurnRate = AICore._Defuzzify(zIsTargetBehindMe, 0.0f, 6.0f);					
				} else {
					fTurnRate = AICore._Defuzzify(zIsTargetToMyLeft, 0.0f, 6.0f);
				}
				_RotateYaw(-fTurnRate);
			}
		}
		
		if(fMaxVelocity > 0.0f) {
			// Only drive forward when facing nearly toward target	
			if(zIsTargetInFrontOfMe > 0.7) {
				// Only drive forward if we're far enough from target
				if(fTargetDistance >= 0.50f) {
					//float fVelocity = AICore._Defuzzify(zIsTargetInFrontOfMe, 0.0f, fMaxVelocity);
					_MoveForward(fMaxVelocity); //fVelocity
				}
			}
			
			// Return whether target is reached
			return fTargetDistance < 2.00f;
		} else {
			// Return whether we're facing the target
			// Also include whether target is reached because when
			// we're very close to the target we get weird look at information
			return zIsTargetInFrontOfMe > 0.9f || fTargetDistance < 5.00f;
		}
		
	}

}
