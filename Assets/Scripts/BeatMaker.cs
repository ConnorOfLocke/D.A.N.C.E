using UnityEngine;
using System.Collections;

public class BeatMaker : MonoBehaviour {

	public float BeatsPerMinute = 130.0f;
	private float BeatTimer = 0.0f;
	private BeatReciever[] Recievers;
	
	private bool TimeKeeping = false;
	private GlobalTimeKeeper TimeKeeper = null;
	private PlayerDance DanceScriptInScene;
	
	private MusicManager MusicInScene;
	private float TimeSinceStart = 0;
	
	private bool nextBeat = true;
	
	// Use this for initialization
	void Start () {
		Recievers = FindObjectsOfType<BeatReciever>();
		TimeKeeper = FindObjectOfType<GlobalTimeKeeper>();
		DanceScriptInScene = FindObjectOfType<PlayerDance>();
		MusicInScene = FindObjectOfType<MusicManager>();
		TimeKeeping = (TimeKeeper != null);
	}
	
	// Update is called once per frame
	void Update () {
		float DeltaTime;
		if (TimeKeeping)
			DeltaTime = TimeKeeper.EntityDeltaTime;
		else
			DeltaTime = Time.deltaTime;
	
		TimeSinceStart += DeltaTime;
	
		BeatTimer += DeltaTime;
		if (BeatTimer >= 60.0f / BeatsPerMinute)
		{
			BeatTimer = BeatTimer - 60.0f / BeatsPerMinute;
			
			if (nextBeat)
			{
				foreach (BeatReciever thing in Recievers)
				{
					if (!thing.BeatOnDance)
						thing.MainBeat();
					else if (DanceScriptInScene.CurrentState == DanceState.DANCE_DANCE ||
					         DanceScriptInScene.CurrentState == DanceState.DANCE_CHASE)
			         {
						thing.MainBeat();   
			         }
				}
				
				Syncronize();
			}
			else
			{
				foreach (BeatReciever thing in Recievers)
				{
					if (!thing.BeatOnDance)
						thing.SubBeat();
					else if (DanceScriptInScene.CurrentState == DanceState.DANCE_DANCE ||
					         DanceScriptInScene.CurrentState == DanceState.DANCE_CHASE)
					{
						thing.SubBeat();   
					}
				}
			}
			
			
			{

			}
			nextBeat = !nextBeat;
		}
	}
	
	public void Syncronize()
	{
		float TimeSinceMusicStart = MusicInScene.getCurrentMusicTime();
		float Distance = 0.0f;
		if (TimeSinceMusicStart != TimeSinceStart)
			Distance = TimeSinceMusicStart - TimeSinceStart;
			
		TimeSinceStart = TimeSinceMusicStart;
		BeatTimer += Distance;
		
	}
}
