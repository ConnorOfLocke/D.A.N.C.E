using UnityEngine;
using System.Collections;

public class EndScreen : MonoBehaviour {

	public bool Active;
	public float CoolDownTime = 2.0f;

	public float TimeToSwitch = 1.0f;

	public Vector3 ActivePosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Active && Input.GetAxis("Dance") != 0 && CoolDownTime <= 0)
		{
			Application.LoadLevel("GameScene");
		}
		else if (Active && CoolDownTime > 0)
		{
			CoolDownTime -= Time.deltaTime;
			Vector3 Velocity = Vector3.zero;
			transform.localPosition = Vector3.SmoothDamp( transform.localPosition, ActivePosition, ref Velocity, TimeToSwitch);
		}

	}
}
