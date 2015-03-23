using UnityEngine;
using System.Collections;

public class Kevin_Shader_Companion : MonoBehaviour {
		
	private Material AttachedMaterial;

	// Use this for initialization
	void Start () {
		AttachedMaterial = renderer.material;
	}

	public void UpdateShader(Vector4 a_newPos, float distance, float strength)
	{
		AttachedMaterial.SetVector("_root_position", a_newPos);
		AttachedMaterial.SetFloat("_Strength", strength);
		AttachedMaterial.SetFloat("_distance", distance);
		
	}
}