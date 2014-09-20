using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]

public class FPSController : MonoBehaviour
{
	public Player player;

	Animator anim = null;

	public enum PlayerState {idle, running};
	public PlayerState playerState = PlayerState.idle;

	public AnimatorStateInfo animState;

	static int idleHash = Animator.StringToHash("Base.Idle");
	static int movingHash = Animator.StringToHash("Base.Moving");
	static int chargingHash = Animator.StringToHash ("Base.holding clasped");

	public float moveSpeed = 1;
	public float runSpeed = 1;
	public float mouseSensitivity = 1;
	public Vector3 moveDirection = new Vector3();
	public float pitch = 0;
	public float pitchRange = 60.0f;
	CharacterController controller;
	public float throwPower = 10.0f;
	
	public Material highlightMaterial;

	float verticalVelocity = 0;
	public float jumpSpeed = 5;
	
	void Start ()
	{
		anim = this.gameObject.GetComponentsInChildren<Animator>()[0];
		controller = this.gameObject.GetComponent<CharacterController>();
		player = this.gameObject.GetComponent<Player>();
		Screen.lockCursor = true;
	}

	void Update ()
	{
		animState = anim.GetCurrentAnimatorStateInfo(0);

		float yaw = Input.GetAxis ("Mouse X") * mouseSensitivity;
		this.gameObject.transform.Rotate (0, yaw, 0);
		pitch -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
		pitch = Mathf.Clamp (pitch, -pitchRange, pitchRange);
		Camera.main.transform.localRotation = Quaternion.Euler (pitch, 0, 0);

		float forwardMovement = Input.GetAxis ("Vertical"); 
		float sideMovement = Input.GetAxis ("Horizontal"); 

		moveDirection = new Vector3 (sideMovement, 0, forwardMovement);
		moveDirection = transform.rotation * moveDirection.normalized;
		//anim.SetFloat("moveSpeed", moveDirection.magnitude);
		moveDirection.y = verticalVelocity;
		if(playerState == PlayerState.idle)
		{
			controller.Move (moveDirection * (moveSpeed) * Time.deltaTime);
		}
		else
		{
			controller.Move (moveDirection * (moveSpeed + runSpeed) * Time.deltaTime);
		}
		if(controller.isGrounded)
		{
			if(Input.GetButtonDown("Jump"))
			{
				verticalVelocity = jumpSpeed;
			}
		} else {
			verticalVelocity += Physics.gravity.y * Time.deltaTime;
		}


		if (animState.IsName ("idle") && Input.GetButtonDown ("Interact")) 
		{
			if (!Screen.lockCursor) 
			{
				Screen.lockCursor = true;
			}
			//player.PrimaryAction();
			anim.SetTrigger ("interact");
		}
		
		if(animState.IsName ("idle") || animState.IsName ("holding clasped"))
		{
			if (Input.GetButtonDown ("Ability")) 
			{
				if (!Screen.lockCursor) 
				{
					Screen.lockCursor = true;
				}
				//player.SecondaryAction();
				anim.SetTrigger ("ability");
			}
		}

		if(Input.GetButton ("Run"))
		{
			playerState = PlayerState.running;
			anim.SetBool("running", true);
		}
		else
		{
			playerState = PlayerState.idle;
			anim.SetBool("running", false);
		}

		if (animState.IsName ("holding clasped"))
		{
				player.charging = true;
		}
		else
		{
				player.charging = false;
		}

	}
	
	
}
