using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

	public float TimeToExist = 1.0f;
	private float Timer = 0.0f;
	
	// Update is called once per frame
	void Update () {
		Timer += Time.deltaTime;
		if (Timer > TimeToExist)
			Destroy(this.gameObject);
	
	}
}
