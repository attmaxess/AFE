using UnityEngine;
using System.Collections;

[RequireComponent (typeof (ThirdPersonControllerNET))]
public class AnimationController : MonoBehaviour
{
    [Header("Debug")]
    public bool isDebug = false;

	public enum CharacterState
	{
		Normal,
		Jumping,
		Falling,
		Landing
	}	
	
	public Animation target;
		// The animation component being controlled
	new public Rigidbody rigidbody;
		// The rigidbody movement is read from
	public Transform root, spine, hub;
		// The animated transforms used for lower body rotation
	public float
		walkSpeed = 0.2f,
		runSpeed = 1.0f,
			// Walk and run speed dictate at which rigidbody velocity, the animation should blend
		rotationSpeed = 6.0f,
			// The speed at which the lower body should rotate
		shuffleSpeed = 7.0f,
			// The speed at which the character shuffles his feet back into place after an on-the-spot rotation
		runningLandingFactor = 0.2f;
			// Reduces the duration of the landing animation when the rigidbody has hoizontal movement
		
	private ThirdPersonControllerNET controller;
	public CharacterState state = CharacterState.Falling;
	private bool canLand = true;
	private float currentRotation;
	private Vector3 lastRootForward;

    [Header("Movement")]
    public Vector3 _currentHoriMove = Vector3.zero;
    public float _currentHoriMoveMag = 0f;

    private Vector3 HorizontalMovement
	{
		get
		{
            _currentHoriMove = new Vector3(rigidbody.velocity.x, 0.0f, rigidbody.velocity.z);
            _currentHoriMoveMag = _currentHoriMove.magnitude;
            return _currentHoriMove;
		}
	}	
	
	void Reset ()
	// Run setup on component attach, so it is visually more clear which references are used
	{
		Setup ();
	}	
	
	void Setup ()
	// If target or rigidbody are not set, try using fallbacks
	{
		if (target == null)
		{
			target = GetComponent<Animation> ();
		}
		
		if (rigidbody == null)
		{
			rigidbody = GetComponent<Rigidbody> ();
		}
	}	
	
	void Start ()
	// Verify setup, configure
	{
		Setup ();
			// Retry setup if references were cleared post-add
			
		if (VerifySetup ())
		{
			controller = GetComponent<ThirdPersonControllerNET> ();
			controller.onJump += OnJump;
				// Have OnJump invoked when the ThirdPersonController starts a jump
			currentRotation = 0.0f;
			lastRootForward = root.forward;
		}
	}	
	
	bool VerifySetup ()
	{
		return VerifySetup (target, "target") &&
			VerifySetup (rigidbody, "rigidbody") &&
			VerifySetup (root, "root") &&
			VerifySetup (spine, "spine") &&
			VerifySetup (hub, "hub");
	}	
	
	bool VerifySetup (Component component, string name)
	{
		if (component == null)
		{
			Debug.LogError ("No " + name + " assigned. Please correct and restart.");
			enabled = false;
			
			return false;
		}
		
		return true;
	}	
	
	void OnJump ()
	// Start a jump
	{
        if (isDebug) Debug.Log("OnJump" + transform.name);
		canLand = false;
		state = CharacterState.Jumping;

        if (isDebug) Debug.Log("Invoke Fall in : " + target["Jump"].length);
        Invoke ("Fall", target["Jump"].length);
	}	
	
	void OnLand ()
	// Start a landing
	{
        if (isDebug) Debug.Log("OnLand" + transform.name);
        canLand = false;
		state = CharacterState.Landing;

        if (isDebug) Debug.Log("Invoke Land in " + target["Land"].length * (HorizontalMovement.magnitude < walkSpeed ? 1.0f : runningLandingFactor));
        Invoke (
			"Land",
			target["Land"].length * (HorizontalMovement.magnitude < walkSpeed ? 1.0f : runningLandingFactor)
				// Land quicker if we're moving enough horizontally to start walking after landing
		);
	}	
	
	void Fall ()
	// End a jump and transition to a falling state (ignore if already grounded)
	{
        if (isDebug) Debug.Log("Fall" + transform.name);
        if (isDebug) Debug.Log("Current controller.Grounded " + controller.Grounded);

        state = CharacterState.Falling;

        if (controller.Grounded)
		{
			return;
		}
	}	
	
	void Land ()
	// End a landing and transition to normal animation state (ignore if not currently landing)
	{
        if (isDebug) Debug.Log("Land" + transform.name);
        if (state != CharacterState.Landing)
		{
			return;
		}
		state = CharacterState.Normal;
	}	
	
	void FixedUpdate ()
	// Handle changes in groundedness
	{
		if (controller.Grounded)
		{
			if (state == CharacterState.Falling || (state == CharacterState.Jumping && canLand))
			{
                if (isDebug) Debug.Log("OnLand() because of state = " + state + "canLand = " + canLand);
				OnLand ();
			}
		}
		else if (state == CharacterState.Jumping)
		{
            if (isDebug) Debug.Log("canLand = true because of state = " + state);
            canLand = true;
		}
	}
    
	void Update ()
	// Animation control
	{
        if (isDebug) Debug.Log("Current state : " + state);

        switch (state)
		{
			case CharacterState.Normal:
                Vector3 movement = HorizontalMovement; 
			
				if (movement.magnitude < walkSpeed)
				{
                    if (isDebug) Debug.Log("movement.magnitude : " + movement.magnitude + " < " + "walkSpeed : " + walkSpeed);

                    if (Vector3.Angle (lastRootForward, root.forward) > 1.0f)
					// If the character has rotated on the spot, shuffle his feet a bit
					{
                        if (isDebug) Debug.Log("target.CrossFade (Shuffle) because of Angle(lastRootForward, root.forward) > 1 : " + Vector3.Angle(lastRootForward, root.forward));
                        target.CrossFade ("Shuffle");
						
						lastRootForward = Vector3.Slerp (lastRootForward, root.forward, Time.deltaTime * shuffleSpeed);
					}
					else
					{
                        if (isDebug) Debug.Log("target.CrossFade (Shuffle) because of Angle(lastRootForward, root.forward) <= 1 : " + Vector3.Angle(lastRootForward, root.forward));
                        target.CrossFade ("Idle");
					}
				}
				else
				{
                    if (isDebug) Debug.Log("movement.magnitude : " + movement.magnitude + " >= " + "walkSpeed : " + walkSpeed);

                    target["Walk"].speed = target["Run"].speed =
						Vector3.Angle (root.forward, movement) > 91.0f ? -1.0f : 1.0f;
						// If the direction if backwards, play the animations backwards
					
					if (movement.magnitude < runSpeed)
					{
                        if (isDebug) Debug.Log("target.CrossFade (Walk) because " + movement.magnitude + " < " + "runSpeed : " + runSpeed);

                        target.CrossFade ("Walk");
					}
					else
					{
                        if (isDebug) Debug.Log("target.CrossFade (Run) because " + movement.magnitude + " > " + "runSpeed : " + runSpeed);

                        target.CrossFade ("Run");
					}
					
					lastRootForward = root.forward;
				}
			break;

			case CharacterState.Jumping:
                if (isDebug) Debug.Log("target.CrossFade (Jump) because state = " + state);
                target.CrossFade ("Jump");
			break;

			case CharacterState.Falling:
                if (isDebug) Debug.Log("target.CrossFade (Fall) because state = " + state);
                target.CrossFade ("Fall");
			break;

			case CharacterState.Landing:
                if (isDebug) Debug.Log("target.CrossFade (Land) because state = " + state);
                target.CrossFade ("Land");
			break;
		}
	}	
	
	void LateUpdate ()
	// Apply directional rotation of lower body
	{
		float targetAngle = 0.0f;
		
		Vector3 movement = HorizontalMovement;
		
		if (movement.magnitude >= walkSpeed)
		// Only calculate the target angle if we're moving sufficiently
		{
			targetAngle = Vector3.Angle (movement, new Vector3 (root.forward.x, 0.0f, root.forward.z));
			
			if (Vector3.Angle (movement, root.right) > Vector3.Angle (movement, root.right * -1))
			// Negative rotation if shortest route is counter-clockwise
			{
				targetAngle *= -1.0f;
			}
			
			if (Mathf.Abs (targetAngle) > 91.0f)
			// When walking backwards, don't rotate over 90 degrees and rotate opposite
			{
				targetAngle = targetAngle + (targetAngle > 0 ? -180.0f : 180.0f);
			}
		}
		
		currentRotation = Mathf.Lerp (currentRotation, targetAngle, Time.deltaTime * rotationSpeed);
			// Update our current rotation score
		
		hub.RotateAround (hub.position, root.up, currentRotation);
			// Rotate the dude
		spine.RotateAround (spine.position, root.up, currentRotation * -1.0f);
			// Rotate the upper-body to face forward
	}
}
