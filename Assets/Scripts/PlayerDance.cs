using UnityEngine;
using System.Collections;

public enum DanceState
{
	DANCE_NONE,
	DANCE_WINDUP,
	DANCE_DANCE,
	DANCE_COOLDOWN,
	DANCE_CHASE
}

public class PlayerDance : BeatReciever {

	public DanceState CurrentState;
	public float DanceWindUp = 0.25f;
	public float DanceCoolDown = 1.0f;
	
	public AudioClip WindUp;
	public AudioClip Dance;
	
	private float DanceTimer = 0.0f;

	private Kevin_Shader_Lord Shader_Lord;
	
	private bool TimeKeeping = false;
	private GlobalTimeKeeper TimeKeeper = null;
	private AudioSource AttachedAudioSource;
	
	private float TimeSinceLastBeat = 0.0f;
	public float DanceTimeToBeatAllowance = 0.01f;
	public bool didStartOnBeat = false;
	
	public GameObject DanceOnBeatObject;
	
	private SpriteAnimation AttachedSpriteAnim;

	void Start()
	{
		Shader_Lord = FindObjectOfType<Kevin_Shader_Lord>();
		TimeKeeper = FindObjectOfType<GlobalTimeKeeper>();
		TimeKeeping = (TimeKeeper != null);
		AttachedAudioSource = GetComponent<AudioSource>();
		AttachedSpriteAnim = GetComponent<SpriteAnimation>();
		BeatOnDance = false;
	}
	
	void Update () 
	{
		float DeltaTime;
		if (TimeKeeping)
			DeltaTime = TimeKeeper.EntityDeltaTime;
		else
			DeltaTime = Time.deltaTime;
	
		TimeSinceLastBeat += DeltaTime;
	
		if (Input.GetAxis("Dance") != 0.0f && CurrentState != DanceState.DANCE_CHASE)
		{
			switch (CurrentState)
			{
				case DanceState.DANCE_NONE:
					DanceTimer = DanceWindUp;
					CurrentState = DanceState.DANCE_WINDUP;
					AttachedAudioSource.Stop();
					AttachedAudioSource.PlayOneShot(WindUp);
				break;
				
				case DanceState.DANCE_WINDUP:
					DanceTimer -= DeltaTime;
					if (DanceTimer <= 0)
					{
						CurrentState = DanceState.DANCE_DANCE;
						AttachedAudioSource.Stop();
						AttachedAudioSource.PlayOneShot(Dance);
						AttachedSpriteAnim.StartFrame = 4;
						AttachedSpriteAnim.EndFrame = 7;
						AttachedSpriteAnim.FPS = 4.3f;
						AttachedSpriteAnim.CurrentSprite = 4;
					}
					
				break;
				case DanceState.DANCE_DANCE:
					//dance stuff
				break;
				
				case DanceState.DANCE_COOLDOWN:
				DanceTimer -= DeltaTime;
				if (DanceTimer <= 0)
				{
					CurrentState = DanceState.DANCE_NONE;
				}
				break;
			};
		}
		else
		{
			switch (CurrentState)
			{
			case DanceState.DANCE_WINDUP:
				DanceTimer = 0;
				
				if (TimeSinceLastBeat < DanceTimeToBeatAllowance)
				{
					GameObject particles = Instantiate(DanceOnBeatObject, transform.position, Quaternion.identity) as GameObject;
					particles.transform.parent = this.transform;
					didStartOnBeat = true;
				}
				
				CurrentState = DanceState.DANCE_NONE;
				AttachedAudioSource.Stop();
				break;
			case DanceState.DANCE_DANCE:
				DanceTimer = DanceCoolDown;
				CurrentState = DanceState.DANCE_COOLDOWN;
				AttachedSpriteAnim.StartFrame = 0;
				AttachedSpriteAnim.EndFrame = 3;
				AttachedSpriteAnim.FPS = 8;		
				AttachedSpriteAnim.CurrentSprite = 0;
				
				break;
			case DanceState.DANCE_COOLDOWN:
				DanceTimer -= DeltaTime;
				if (DanceTimer <= 0)
				{
					CurrentState = DanceState.DANCE_NONE;
				}
				break;
			case DanceState.DANCE_CHASE:
			
			break;
			};
		}
		Shader_Lord.UpdateShader(CurrentState);	
	}
	
	
	public void StopDancing()
	{
		DanceTimer = DanceCoolDown;
		CurrentState = DanceState.DANCE_COOLDOWN;
		AttachedSpriteAnim.StartFrame = 0;
		AttachedSpriteAnim.EndFrame = 3;
		AttachedSpriteAnim.FPS = 8;	
		AttachedSpriteAnim.CurrentSprite = 0;
	}
	
	public override void MainBeat()
	{
		TimeSinceLastBeat = 0.0f;
	}
	
	public override void SubBeat()
	{
		TimeSinceLastBeat = 0.0f;
	}
}
