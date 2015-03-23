using UnityEngine;
using System.Collections;

public class BooyahScreen : MonoBehaviour {

	public bool Active;
	
	public float TimeToSwitch = 1.0f;
	
	public Vector3 ActivePosition;
	public Vector3 InActivePosition;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Active)
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
}
