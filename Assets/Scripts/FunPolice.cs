using UnityEngine;
using System.Collections;

public class FunPolice : MonoBehaviour {

	public float CatchPlayerDistance = 1.0f;
	public float WarningDistance = 3.0f;
	public float PatrolDistance = 10.0f;
	
	public DIRECTION curDirection = DIRECTION.LEFT;
	public float SpeedFloor = 2.0f;
	public float SpeedCeil = 1.0f;

	private Vector3 OriginalPosition;
	private float Speed;
	
	private bool TimeKeeping = false;
	private GlobalTimeKeeper TimeKeeper = null;
	
	public float ReactionTime = 3.0f;
	public bool Active = false;

	private SpriteAnimation AttachedSpriteAnim;
	private SpriteRenderer AttachedSpriteRend;
	public Sprite ActiveSprite;

	// Use this for initialization
	void Start () {
		OriginalPosition = transform.position;
		TimeKeeper = FindObjectOfType<GlobalTimeKeeper>();
		TimeKeeping = (TimeKeeper != null);
		
		Speed = Random.Range(SpeedFloor, SpeedCeil);
		AttachedSpriteAnim = GetComponent<SpriteAnimation>();
		AttachedSpriteRend = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
		float DeltaTime;
		if (TimeKeeping)
			DeltaTime = TimeKeeper.EntityDeltaTime;
		else
			DeltaTime = Time.deltaTime;
			
		if (!Active)
		{
			AttachedSpriteAnim.curDirection = curDirection;
			if (curDirection == DIRECTION.LEFT)
			{
				transform.position += new Vector3( -Speed * DeltaTime,0,0);
				if (transform.position.x < OriginalPosition.x - PatrolDistance)
					curDirection = DIRECTION.RIGHT;
				
			}
			else if (curDirection == DIRECTION.RIGHT)
			{
				transform.position += new Vector3( Speed * DeltaTime,0,0);
				if (transform.position.x > OriginalPosition.x + PatrolDistance)
					curDirection = DIRECTION.LEFT;
				
			}
		}
	}
	
	public void Activate()
	{
		AttachedSpriteAnim.Active = false;
		AttachedSpriteRend.sprite = ActiveSprite;
		Active = true;
		
	}
}
