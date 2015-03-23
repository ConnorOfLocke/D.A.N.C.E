using UnityEngine;
using System.Collections;

public enum DIRECTION
{
	LEFT,
	RIGHT
}

public class PlayerMove : MonoBehaviour {
		
	public float walkingSpeed = 5.0f;
	public float danceSpeed = 10.0f;
	public float transitionTime = 0.1f;
	
	public float SpeedMultiplier = 1;
	public float ChaseMultiplier = 4;
	
	public DIRECTION CurDirection = DIRECTION.RIGHT;
	public bool Dancing = false;
	public bool Chased = false;
	
	private bool TimeKeeping = false;
	private GlobalTimeKeeper TimeKeeper = null;
	
	public bool LockMovement = false;
	
	private float CurSpeed;
	
	private SpriteAnimation AttachedAnim = null;
	
	void Start()
	{
		AttachedAnim = GetComponent<SpriteAnimation>();
		TimeKeeper = FindObjectOfType<GlobalTimeKeeper>();
		TimeKeeping = (TimeKeeper != null);
	}
	
	// Update is called once per frame
	void Update () 
	{
		float DeltaTime;
		if (TimeKeeping)
			DeltaTime = TimeKeeper.EntityDeltaTime;
		else
			DeltaTime = Time.deltaTime;

		if (!LockMovement)
		{
			float WantedSpeed = 0.0f;
			if (!Dancing)
			{
				if (CurDirection == DIRECTION.LEFT)
					WantedSpeed = -walkingSpeed * SpeedMultiplier;
	
				else if (CurDirection == DIRECTION.RIGHT)
					WantedSpeed = walkingSpeed * SpeedMultiplier;
			}
			else
			{
				if (CurDirection == DIRECTION.LEFT)
					WantedSpeed = -danceSpeed * SpeedMultiplier;
				
				else if (CurDirection == DIRECTION.RIGHT)
					WantedSpeed = danceSpeed * SpeedMultiplier;
			}
			
			AttachedAnim.curDirection = CurDirection;
			float Velocity = 0.0f;
			CurSpeed =  Mathf.SmoothDamp(CurSpeed, WantedSpeed, ref Velocity, transitionTime, 1000, DeltaTime);
			
			transform.position = new Vector3(transform.position.x + CurSpeed * DeltaTime,
			                                 transform.position.y,
			                                 transform.position.z);
         }
         
	}

}