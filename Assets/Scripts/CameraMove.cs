using UnityEngine;
using System.Collections;

public class CameraMove : BeatReciever {
	
	public GameObject FollowObject;
	public float MoveDelay = 1.0f;
	
	public float yBeat = 0.7f;
	
	public float DanceRotation = 0.5f;
	
	float yDistance = 0.0f;
	float zDistance = 0.0f;
	float curBeatY = 0.0f;
	
	private Vector3 Velocity = Vector3.zero;
	
	private Equalizer SceneEqualizer;
	
	private PlayerDance DanceScriptInScene;
	
	// Use this for initialization
	void Start () 
	{
		Velocity = Vector3.zero;
		yDistance = Mathf.Abs(FollowObject.transform.position.y - transform.position.y);
		zDistance = Mathf.Abs(FollowObject.transform.position.z - transform.position.z);
		SceneEqualizer = FindObjectOfType<Equalizer>();
		
		DanceScriptInScene = FindObjectOfType<PlayerDance>();
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 followPos = FollowObject.transform.position;
		followPos.y += yDistance - curBeatY;
		followPos.z -= zDistance;
		
		
		Vector3 newPostion = Vector3.SmoothDamp(transform.position,
		                                        followPos,
		                                        ref Velocity,
		                                        MoveDelay);
		transform.position = newPostion;
		
		if (curBeatY > 0)
			curBeatY -= Time.deltaTime * 40;
			
		
		if (DanceScriptInScene.CurrentState == DanceState.DANCE_DANCE )
		{
			Quaternion newRotation = Quaternion.identity;
			newRotation.eulerAngles = new Vector3(0,0, Mathf.Sin(Time.timeSinceLevelLoad * 4) * DanceRotation);
			transform.localRotation = newRotation;
		}
		else
			transform.localRotation = Quaternion.identity;	
		
		
			
	}
	
	public override void MainBeat()
	{
		if (SceneEqualizer.INTENSE)
			curBeatY = yBeat * 4;
		else
			curBeatY = yBeat;
		
	}
	
	public override void SubBeat()
	{
		if (SceneEqualizer.INTENSE)
			curBeatY = yBeat * 2;
		else
			curBeatY = yBeat * 0.5f;
	}
}