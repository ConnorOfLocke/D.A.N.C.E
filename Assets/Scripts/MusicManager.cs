using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public AudioSource DanceMusic;
	public AudioSource AmbientMusic;
	private PlayerDance DanceScriptInScene;
	public float TransitionTime = 0.5f;
	public float NormalVolume = 0.2f;
	public float DanceVolume = 1.0f;

	private bool TimeKeeping = false;
	private GlobalTimeKeeper TimeKeeper = null;

	// Use this for initialization
	void Start () {
		DanceScriptInScene = FindObjectOfType<PlayerDance>();
				
		TimeKeeper = FindObjectOfType<GlobalTimeKeeper>();
		TimeKeeping = (TimeKeeper != null);
		DanceMusic.volume = NormalVolume;
		DanceMusic.Play();
	}
	
	// Update is called once per frame
	void Update () 
	{	
		DanceMusic.pitch = TimeKeeper.CurDeltaRatio;
		if (DanceScriptInScene.CurrentState == DanceState.DANCE_DANCE ||
		    DanceScriptInScene.CurrentState == DanceState.DANCE_CHASE)
		{
			DanceMusic.volume = DanceVolume;
		}
		else
		{
			DanceMusic.volume = NormalVolume;
		}
	}
	
	public float getCurrentMusicTime()
	{
		return DanceMusic.time;
	}
}
