using UnityEngine;
using System.Collections;

public class CameraFOV : MonoBehaviour {

	public float FOVScale = 1.5f;

	private PlayerDance DanceScriptInScene;
	private float OriginalFOV;
	void Start()
	{
		DanceScriptInScene = FindObjectOfType<PlayerDance>();
		OriginalFOV = camera.fieldOfView;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (DanceScriptInScene.CurrentState == DanceState.DANCE_DANCE)
		{
			float Velocity = 0.0f;
			camera.fieldOfView = Mathf.SmoothDamp(camera.fieldOfView, OriginalFOV * FOVScale, ref Velocity, 0.1f);
			
		}
		else if (DanceScriptInScene.CurrentState == DanceState.DANCE_WINDUP)
		{
			float Velocity = 0.0f;
			camera.fieldOfView = Mathf.SmoothDamp(camera.fieldOfView, OriginalFOV * FOVScale * 0.5f, ref Velocity, 0.1f);
		}
		else
		{
			float Velocity = 0.0f;
			camera.fieldOfView = Mathf.SmoothDamp(camera.fieldOfView, OriginalFOV, ref Velocity, 0.025f);
		}
	
	}
}
