using UnityEngine;
using System.Collections;
using System;

public class Equalizer : BeatReciever {

	public GameObject EqualiserPart;
	public int samples = 512;
	public int numParts = 32;
	public Vector3 Seperation;
	
	public float scaleSmoothing = 0.01f;
	
	public GameObject FollowObject;
	
	private int sampleToPartRatio;
	public float CurrentMusicIntensity = 0.0f;
	private GameObject[] EqualiserParts;
	private float[] EqualiserHighestValues;
	
	private float BeatTimer = 0.0f;
	private Vector3 OriginalPartScale;

	private PlayerDance DanceScriptInScene;
	
	public bool INTENSE = false;
	private int INTENSErolloff = 12;
	private int INTENSEcurrent = 0; 
	
	void Start () {
		
		//rounds sample and numparts to power of two
		if (!(  samples == 2
			 || samples == 4
			 || samples == 8
			 || samples == 16
			 || samples == 32
			 || samples == 64
			 || samples == 128
			 || samples == 256
			 || samples == 512      
			 || samples == 1024 ))
				samples = 1024;
		
		if  (!(  numParts == 2
		       || numParts == 4
		       || numParts == 8
		       || numParts == 16
		       || numParts == 32
		       || numParts == 64
		       || numParts == 128
		       || numParts == 256
		       || numParts == 512      
		       || numParts == 1024 ))
			numParts = 32;
			
		//Set the ratio between the two
		sampleToPartRatio = samples / numParts;
		EqualiserParts = new GameObject[numParts];
		
		//sets all highest value to 0
		EqualiserHighestValues = new float[numParts];
		for (int i = 0; i < numParts; i++)
			EqualiserHighestValues[i] = 0.0f;
		
		//Instantiates all the EqualiserParts
		Vector3 RootPosition = FollowObject.transform.position - (Seperation * numParts) * 0.5f;
		for (int i = 0; i < numParts; i++)
		{
			EqualiserParts[i] = GameObject.Instantiate(EqualiserPart,
							 						RootPosition + i * Seperation,
							 						EqualiserPart.transform.rotation) as GameObject;
		}
		
		DanceScriptInScene = FindObjectOfType<PlayerDance>();
		OriginalPartScale = EqualiserPart.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
	
		//gets the audio values
		float[] spectrum = new float[samples];
		AudioListener.GetOutputData(spectrum, 0);
		float LastIntensity = CurrentMusicIntensity;
		CurrentMusicIntensity = 0;
		
		float AverageXPosition = 0.0f;
		
		bool IsDancing =  (DanceScriptInScene.CurrentState == DanceState.DANCE_DANCE);
	
		for (int i = 0; i < numParts; i++)
		{
			//gets absolute value of the sum of a chunk of the spectrum 
			float sampleValue = 0;
			for (int j = 0; j < sampleToPartRatio; j++)
			{
				if (IsDancing)
					sampleValue += spectrum[i * sampleToPartRatio + j] * 0.4f; //accounting for volume
				else
					sampleValue += spectrum[i * sampleToPartRatio + j] * 0.7f;
			}
			sampleValue = Mathf.Abs(sampleValue);
			
			Vector3 newScale = new Vector3(	OriginalPartScale.x,
										 	OriginalPartScale.y,
										 	OriginalPartScale.z + (spectrum[i]) * 2 - 1.0f);								
	        
	        //set the scale to the new scale or the highest value so far    
			if (newScale.z > EqualiserHighestValues[i])
			{
				EqualiserHighestValues[i] = newScale.z;
				EqualiserParts[i].transform.localScale = newScale;
			}
			else
			{
				EqualiserHighestValues[i] -= Time.deltaTime * 0.01f;
				EqualiserParts[i].transform.localScale =  new Vector3(OriginalPartScale.x,
			                                                         OriginalPartScale.y,
																	 OriginalPartScale.z + EqualiserHighestValues[i] - 1);
			}
			EqualiserHighestValues[i] = sampleValue;
			
			EqualiserParts[i].transform.localScale = newScale - new Vector3(0,0,1);
			
			CurrentMusicIntensity += (spectrum[i] * 2) + 1;
			AverageXPosition += EqualiserParts[i].transform.position.x;
		}
		
		CurrentMusicIntensity /= numParts;
			
		AverageXPosition /= numParts;
		
		CycleEqualiser(AverageXPosition);
		
		if (BeatTimer > 0)
			BeatTimer -= Time.deltaTime * 10;

	}

	private void CycleEqualiser(float AverageXPosition)
	{
		//move the last value to the start
		if ( FollowObject.transform.position.x - AverageXPosition > Seperation.x)
		{
			for (int i = 0; i < numParts - 1; i++)
			{
				GameObject temp = EqualiserParts[i + 1];
				EqualiserParts[i + 1] = EqualiserParts[i];
				EqualiserParts[i] = temp;
				
				float tempVal = EqualiserHighestValues[i + 1];
				EqualiserHighestValues[i + 1] = EqualiserHighestValues[i];
				EqualiserHighestValues[i] = tempVal;
			}
			EqualiserParts[numParts - 1].transform.position = EqualiserParts[numParts - 2].transform.position + Seperation;
		}
		//move the first value to the end
		else if (AverageXPosition - FollowObject.transform.position.x < -Seperation.x )
		{
			for (int i = numParts - 1; i > 0; i--)
			{
				GameObject temp = EqualiserParts[i];
				EqualiserParts[i] = EqualiserParts[i - 1];
				EqualiserParts[i - 1] = temp;
				
				float tempVal = EqualiserHighestValues[i];
				EqualiserHighestValues[i] = EqualiserHighestValues[i - 1];
				EqualiserHighestValues[i - 1] = tempVal;
			}
			EqualiserParts[0].transform.position = EqualiserParts[1].transform.position - Seperation;
		}
	}

	public override void MainBeat()
	{
		BeatTimer += 0.5f;
	}
	
	public override void SubBeat()
	{
		BeatTimer += 0.25f;
	}
}
