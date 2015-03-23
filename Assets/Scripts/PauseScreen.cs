using UnityEngine;
using System.Collections;

public class PauseScreen : MonoBehaviour {

	public Vector3 ActivePosition;
	public Vector3 InActivePosition;
	public float TimeToSwitch = 0.1f;
	
	public bool Paused = false;
	private bool PauseButtonDown = false;

	private GlobalTimeKeeper TimeKeeper;

	// Use this for initialization
	void Start () {
		TimeKeeper = FindObjectOfType<GlobalTimeKeeper>();
	}
	
	// Update is called once per frame
	void Update () {
		//checks input
		if (Input.GetAxis("Pause") != 0.0f && !PauseButtonDown)
		{
			SwitchPause();
			PauseButtonDown = true;
		}
		else if (Input.GetAxis("Pause") == 0.0f && PauseButtonDown)
			PauseButtonDown = false;
			
		if (Paused)
		{
			Vector3 Velocity = Vector3.zero;
			transform.localPosition = Vector3.SmoothDamp( transform.localPosition, ActivePosition, ref Velocity, TimeToSwitch);
		}
		else
		{
			Vector3 Velocity = Vector3.zero;
			transform.localPosition = Vector3.SmoothDamp( transform.localPosition, InActivePosition, ref Velocity, TimeToSwitch);
		}
		
	}
	
	public void SwitchPause()
	{
		if (Paused)
			TimeKeeper.UnPause();
		else
			TimeKeeper.Pause();
			
		Paused = !Paused;
	}
}
