using UnityEngine;
using System.Collections;

public class SpriteAnimation : MonoBehaviour {

	private SpriteRenderer AttachedSpriteRender;
	
	public float FPS = 60.0f;
	
	public int StartFrame = 0;
	public int EndFrame = 0;
	
	public int CurrentSprite = 0;
	public Sprite[] Sprites;
	
	public DIRECTION curDirection = DIRECTION.LEFT;
	public bool ReverseDirection = false;
	
	private bool TimeKeeping = false;
	private GlobalTimeKeeper TimeKeeper = null;
	private float SpriteTimer = 0.0f;
	
	private Vector3 OriginalScale;
	
	public bool Active = true;
	
	void Start () {
		AttachedSpriteRender = GetComponent<SpriteRenderer>();
		TimeKeeper = FindObjectOfType<GlobalTimeKeeper>();
		
		if (EndFrame < StartFrame || EndFrame == 0)
		{
			EndFrame = Sprites.Length - 1;
		}
		
		
		TimeKeeping = (TimeKeeper != null);
		OriginalScale = transform.localScale;
		
		SpriteTimer += Random.value;
	}
	
	void Update () 
	{
		if (Active)
		{
			float DeltaTime;
			if (TimeKeeping)
				DeltaTime = TimeKeeper.EntityDeltaTime;
			else
				DeltaTime = Time.deltaTime;
			

			SpriteTimer += DeltaTime;
			if (SpriteTimer >  1 / FPS)
			{
				SpriteTimer = SpriteTimer - 1 / FPS;
				CurrentSprite ++;
				if (CurrentSprite > EndFrame)
					CurrentSprite = StartFrame;
			}
			
			if (curDirection == DIRECTION.LEFT)
			{
				if (ReverseDirection)
					transform.localScale = new Vector3(-OriginalScale.x, OriginalScale.y, OriginalScale.z);
				else
					transform.localScale = new Vector3(OriginalScale.x, OriginalScale.y, OriginalScale.z);
			}
			else if (curDirection == DIRECTION.RIGHT)
			{
				if (ReverseDirection)
					transform.localScale = new Vector3(OriginalScale.x, OriginalScale.y, OriginalScale.z);
				else
					transform.localScale = new Vector3(-OriginalScale.x, OriginalScale.y, OriginalScale.z);
			}
			
			AttachedSpriteRender.sprite = Sprites[CurrentSprite];
		}
	}
}
