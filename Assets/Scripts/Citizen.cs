using UnityEngine;
using System.Collections;

public class Citizen : BeatReciever
{
	private PlayerDance CurrentPlayer;
	private SpriteRenderer AttachedRenderer;
	private Color OriginalColor;
	private Vector3 OriginalPosition;
	private Vector3 OriginalScale;
	private Vector3 OriginalRotation;
	
	private InGameGUI GUIScriptInScene;
		
	public float DistanceToActivate = 3.0f;
	public bool IsActive = false;
	public GameObject ActivateExplosion;
	public float ShakeDistance = 6.0f;
	private Vector3 ActualPosition;
	
	public Vector3 BeatScale;
	private float BeatScaleTimer;

	public float SpeedFloor = 2.0f;
	public float SpeedCeil = 1.0f;
	private float Speed;
	public DIRECTION curDirection = DIRECTION.LEFT;
	public float PatrolDistance = 5.0f;
	private SpriteAnimation AttachedAnim = null;
	private AudioSource AttachedAudioSource = null;

	private bool TimeKeeping = false;
	private GlobalTimeKeeper TimeKeeper = null;

	// Use this for initialization
	void Start ()
	{
		CurrentPlayer = FindObjectOfType<PlayerDance>();
		GUIScriptInScene = FindObjectOfType<InGameGUI>();
		AttachedRenderer = GetComponent<SpriteRenderer>();
		
		AttachedAnim = GetComponent<SpriteAnimation>();
		AttachedAnim.curDirection = curDirection;
		
		OriginalColor = AttachedRenderer.color;
		OriginalPosition = transform.position;
		ActualPosition = transform.position;
		OriginalScale = transform.localScale;
	
		AttachedAudioSource = GetComponent<AudioSource>();
			
		BeatScale = new Vector3(0.1f ,0 ,0);
		Speed = Random.Range(SpeedFloor, SpeedCeil);
		
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
	
		
		AttachedAudioSource.pitch = TimeKeeper.CurDeltaRatio;
	
		if (curDirection == DIRECTION.LEFT)
		{
			AttachedAnim.curDirection = curDirection;
			
			transform.position = transform.position + new Vector3(-Speed * DeltaTime,0,0);
			if ( Vector3.Distance(transform.position, OriginalPosition) > PatrolDistance)
			{
				curDirection = DIRECTION.RIGHT;
				transform.position = transform.position - new Vector3(-Speed * DeltaTime,0,0);
			}
		}
		else if (curDirection == DIRECTION.RIGHT)
		{
			AttachedAnim.curDirection = curDirection;
			
			transform.position = transform.position + new Vector3( Speed * DeltaTime,0,0);
			if ( Vector3.Distance(transform.position, OriginalPosition) > PatrolDistance)
			{
				curDirection = DIRECTION.LEFT;
				transform.position = transform.position - new Vector3( Speed * DeltaTime,0,0);
			}
		}
	
		if (CurrentPlayer.CurrentState == DanceState.DANCE_DANCE)
		{
			if (Vector3.Distance(CurrentPlayer.gameObject.transform.position, transform.position) < DistanceToActivate)
			{
				AttachedRenderer.color = Color.green;
				if (!IsActive)
				{
					IsActive = true;
					GUIScriptInScene.IterateCounter();
					if (ActivateExplosion != null)
						GameObject.Instantiate(ActivateExplosion, transform.position, Quaternion.identity);
					AttachedAudioSource.Play();
				}
			}
		}
		else if(CurrentPlayer.CurrentState == DanceState.DANCE_WINDUP)
		{		
			float Distance = Vector3.Distance(CurrentPlayer.gameObject.transform.position, transform.position);
			if (Distance < DistanceToActivate)
			{
				AttachedRenderer.color = Color.yellow;
			}
				
		}
		else if (!IsActive)
			AttachedRenderer.color = OriginalColor;
		
		
		if (IsActive)
		{
			//transform.localScale = OriginalScale + BeatScale * BeatScaleTimer;
			
			if (BeatScaleTimer > 0)
				BeatScaleTimer -= Time.deltaTime * 4;
		}
	}
	
	public override void MainBeat()
	{
		BeatScaleTimer = 2.0f;
	}
	
	public override void SubBeat()
	{
		BeatScaleTimer = 1.5f;	
	}
}

