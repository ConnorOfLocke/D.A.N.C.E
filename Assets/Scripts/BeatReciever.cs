using UnityEngine;
using System.Collections;

public abstract class BeatReciever : MonoBehaviour {

	public abstract void MainBeat();
	public abstract void SubBeat();
	
	public bool BeatOnDance = true;
}
