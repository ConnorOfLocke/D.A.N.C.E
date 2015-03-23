using UnityEngine;
using System.Collections;

public class Kevin_Shader_Lord : MonoBehaviour {

	Kevin_Shader_Companion[] curShaders;
	public GameObject Shader_RootObject;
	public float Cur_Shader_Distance;
	public float Cur_Shader_Strength;
	
	public float TransitionTime = 0.25f;
	
	private float Shader_Distance;
	private float Shader_Strength;
	
	public float DANCE_NONE_Distance = 0.1f;
	public float DANCE_NONE_Strength = 0.1f;

	public float DANCE_WINDUP_Distance = 0.3f;
	public float DANCE_WINDUP_Strength = 0.3f;
	
	public float DANCE_DANCE_Distance = 10.0f;
	public float DANCE_DANCE_Strength = 1.0f;
	
	public float DANCE_COOLDOWN_Distance = 0;
	public float DANCE_COOLDOWN_Strength = 0;
	
	private bool TimeKeeping = false;
	private GlobalTimeKeeper TimeKeeper = null;
	
	// Use this for initialization
	void Start () {
		curShaders = FindObjectsOfType<Kevin_Shader_Companion> ();
		TimeKeeper = FindObjectOfType<GlobalTimeKeeper>();
		TimeKeeping = (TimeKeeper != null);
	}
	
	// Update is called once per frame
	void Update ()
	{
		float DeltaTime;
		if (TimeKeeping)
			DeltaTime = TimeKeeper.EntityDeltaTime;
		else
			DeltaTime = Time.deltaTime;
	
		float Velocity = 0.0f;
	
		Cur_Shader_Distance = Mathf.SmoothDamp(Cur_Shader_Distance, Shader_Distance, ref Velocity, TransitionTime, 100000, DeltaTime);
		Cur_Shader_Strength = Mathf.SmoothDamp(Cur_Shader_Strength, Shader_Strength, ref Velocity, TransitionTime, 100000, DeltaTime);
	
		foreach (Kevin_Shader_Companion thing in curShaders)
			thing.UpdateShader(Shader_RootObject.transform.position, Cur_Shader_Distance, Cur_Shader_Strength);
	}

	public void UpdateShaderList()
	{
		curShaders = FindObjectsOfType<Kevin_Shader_Companion> ();
	}
	
	public void UpdateShader(DanceState Player_State)
	{
		switch (Player_State)
		{
			case DanceState.DANCE_NONE:
				Shader_Distance = DANCE_NONE_Distance;
				Shader_Strength = DANCE_NONE_Strength;
				break;
			case DanceState.DANCE_WINDUP:
				Shader_Distance = DANCE_WINDUP_Distance;
				Shader_Strength = DANCE_WINDUP_Strength;
				break;
			case DanceState.DANCE_DANCE:
				Shader_Distance = DANCE_DANCE_Distance;
				Shader_Strength = DANCE_DANCE_Strength;
				break;
				
			case DanceState.DANCE_COOLDOWN:
				Shader_Distance = DANCE_COOLDOWN_Distance;
				Shader_Strength = DANCE_COOLDOWN_Strength;
				break;
		};
	}

}
