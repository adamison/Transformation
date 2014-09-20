using UnityEngine;
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

	protected Animator animator;	
	private float directionDampTime = 0.25f;

	// Pathfinding
	private int _CURRENT_STATE = 0;
	private int _STATE_INITIALIZE = 0;
	private int _STATE_WAIT_FOR_TARGET = 1;
	private int _STATE_FIND_PATH = 2;
	private int _STATE_DETERMINE_NEXT_WAYPOINT = 3;
	private int _STATE_TURN_TO_WAYPOINT = 4;
	private int _STATE_SEEK_WAYPOINT = 5;
	
	private PathCore.PathFinder _PathFinder = new PathCore.PathFinder();
	
	public Component[] _TargetWayPoints = null;
	private int _next_target = 0;
	
	private Component _Target = null;
	// end pathfinding

	void Start()
	{
		controller = GetComponent<CharacterController> ();
		animator = GetComponentInChildren<Animator>();
		startPosition = transform.position;
		currentHealth = maxHealth;
		chaseTarget = null;
	}

	void FixedUpdate()
	{
		if(enemyState == EnemyState.chasing && chaseTarget != null)
		{
			_SeekTarget(chaseTarget.transform.position, runSpeed);
		}

		if (enemyState == EnemyState.patrolling) 
		{		
			animator.SetFloat("Speed", walkSpeed);
			//animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);

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
