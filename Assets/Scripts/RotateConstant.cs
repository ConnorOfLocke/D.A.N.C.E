using UnityEngine;
using System.Collections;

public class RotateConstant : MonoBehaviour {

	public Vector3 RotationAngle = Vector3.zero;

	private bool TimeKeeping = false;
	private GlobalTimeKeeper TimeKeeper = null;
	
	// Use this for initialization
	void Start () {
		TimeKeeper = FindObjectOfType<GlobalTimeKeeper>();
		TimeKeeping = (TimeKeeper != null);
	}
	
	// Update is called once per frame
	void Update () {
		float DeltaTime;
		if (TimeKeeping)
			DeltaTime = TimeKeeper.EntityDeltaTime;
		else
			DeltaTime = Time.deltaTime;
			
		transform.Rotate((RotationAngle * DeltaTime));
	}
}
