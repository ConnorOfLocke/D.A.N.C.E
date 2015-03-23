using UnityEngine;
using System.Collections;

public class BeatScaleriser : BeatReciever {

	public Vector3 BeatScale = Vector3.one;
	public float CoolDown = 0.5f;
	private Vector3 OriginalScale;

	// Use this for initialization
	void Start () {
		OriginalScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 Velocity = Vector3.zero;
		transform.localScale  = Vector3.SmoothDamp( transform.localScale, OriginalScale, ref Velocity, CoolDown * 0.1f);
	}
	
	public override void MainBeat()
	{
		transform.localScale = BeatScale;
	}
	
	public override void SubBeat()
	{
		transform.localScale = BeatScale - (BeatScale - Vector3.one) * 0.5f;
	}
}
