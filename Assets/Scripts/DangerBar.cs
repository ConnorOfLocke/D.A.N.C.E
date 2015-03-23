using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DangerBar : MonoBehaviour {

	public string WarningMessage = "Woah there";
	public string SafeMessage = "Groovy";
	public string DanceMessage = "AWW YEAH";
	public string JustFoundMessage = "...crap";
	public string ChaseMessage  = "RRRUNNNN!";

	public Color WarningColor = Color.white;
	public Color SafeColor  = Color.white;
	public Color DanceColor = Color.white;
	public Color FoundColor = Color.white;

	public Color TextWarningColor = Color.white;
	public Color TextSafeColor  = Color.white;
	public Color TextDanceColor = Color.white;
	public Color TextFoundColor = Color.white;

	public float ColorSwitchLerpValue = 0.8f;

	public Text WarningText;
	public GameObject WarningBarPlane;
	
	private GameStateManager Manager;

	void Start () 
	{
		Manager = FindObjectOfType<GameStateManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Color ColorToSwitchTo = Color.white;
		Color TextColorToSwitchTo = Color.white;
		switch (Manager.curState)
		{
			case GAME_STATE.GAME_NORMAL:
				WarningText.text = SafeMessage;
				ColorToSwitchTo = SafeColor;
				TextColorToSwitchTo = TextSafeColor;
			break;
			
			case GAME_STATE.GAME_WARNING:
				WarningText.text = WarningMessage;
				ColorToSwitchTo = WarningColor;
				TextColorToSwitchTo = TextWarningColor;
			break;
			
			case GAME_STATE.GAME_DANCING:
				WarningText.text = DanceMessage;
				ColorToSwitchTo = DanceColor;
				TextColorToSwitchTo = TextDanceColor;
			break;
			
			case GAME_STATE.GAME_JUSTFOUND:
				WarningText.text = JustFoundMessage;
				ColorToSwitchTo = FoundColor;
				TextColorToSwitchTo = TextFoundColor;
			break;
		};	
		WarningBarPlane.renderer.material.color =
			Color.Lerp(WarningBarPlane.renderer.material.color, ColorToSwitchTo, ColorSwitchLerpValue );
		WarningText.color =
			Color.Lerp(WarningText.color, TextColorToSwitchTo, ColorSwitchLerpValue);
	}
}
