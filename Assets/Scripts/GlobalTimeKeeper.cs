using UnityEngine;
using System.Collections;

public class GlobalTimeKeeper : MonoBehaviour {
	
	public float EntityDeltaTime = 0.0f;
	public float TimeSinceStart = 0.0f;
	public float CurDeltaRatio = 1.0f;
	public float PauseEaseTime = 0.5f;
	private float CurEaseTime = 0.0f;
	
	private bool isPaused = false;
	
	// Use this for initialization
	void Start () {
		EntityDeltaTime = Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isPaused)
		{
			CurDeltaRatio = (1.0f - CurEaseTime / PauseEaseTime);
			EntityDeltaTime = Time.deltaTime * CurDeltaRatio;
			if (CurEaseTime > 0)
				CurEaseTime -= Time.deltaTime;
			else if (CurEaseTime < 0)
				CurEaseTime = 0;
		}
		else
		{
			EntityDeltaTime = 0;
			CurDeltaRatio = (1.0f - CurEaseTime / PauseEaseTime);
			EntityDeltaTime = Time.deltaTime * CurDeltaRatio;
		}
		
		TimeSinceStart += EntityDeltaTime;
	}
	
	public void Pause()
	{
		isPaused = true;
		CurEaseTime = PauseEaseTime;
	}
	
	public void UnPause()
	{
		isPaused = false;
	}
}