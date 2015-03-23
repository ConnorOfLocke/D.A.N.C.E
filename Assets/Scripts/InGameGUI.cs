using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGameGUI : MonoBehaviour {

	public Text CounterText;
	public float CounterJittter = 1.0f;
	public float CounterTimeJittering = 0.5f;
	
	private Vector3 CounterOriginalPosition;
	private float CurCounterJitterTime;
	
	public int Counter = 0;
	
	// Use this for initialization
	void Start () {
		CounterOriginalPosition = CounterText.gameObject.GetComponent<RectTransform>().localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		CounterText.text = Counter.ToString();
		
		if (CurCounterJitterTime > 0)
		{
			Vector2 Jitter = (Random.insideUnitCircle) * CurCounterJitterTime;
			CounterText.GetComponent<RectTransform>().localPosition = CounterOriginalPosition + new Vector3(Jitter.x,
																											Jitter.y,
																											0); 
			CurCounterJitterTime -= Time.deltaTime;
		}
	}
	
	public void IterateCounter()
	{
		Counter++;
		CurCounterJitterTime += CounterTimeJittering;
	}
}
