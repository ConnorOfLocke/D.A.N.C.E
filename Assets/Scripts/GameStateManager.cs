using UnityEngine;
using System.Collections;

public enum GAME_STATE
{
	GAME_NORMAL,
	GAME_WARNING,
	GAME_DANCING,
	GAME_JUSTFOUND,
	GAME_OVER
};

public class GameStateManager : MonoBehaviour {

	public GAME_STATE curState;
	
	public GameObject[] LifeObject;
	private int Lives = 0;
	public GameObject ExplosionParticleEffect;
	
	private EndScreen EndScreenInScene;
	private BooyahScreen BooyahInScene;
	
	private FunPolice[] Police;
	private PlayerDance Player;
	private PlayerMove PlayerMovement;
	
	private float CatchTimer = 1.0f;
	
	private GlobalTimeKeeper TimeKeeper;
	
	public float DanceOnBeatInvincibilityTime = 5.0f;
	private float CurInvincibilityTime = 0.0f;
	
	public AudioSource BooyahSound;
	// Use this for initialization
	void Start () 
	{
		Police = 			FindObjectsOfType<FunPolice>();
		Player = 			FindObjectOfType<PlayerDance>();
		PlayerMovement = 	FindObjectOfType<PlayerMove>();
		TimeKeeper = 		FindObjectOfType<GlobalTimeKeeper>();
		EndScreenInScene = 	FindObjectOfType<EndScreen>();
		BooyahInScene = 	FindObjectOfType<BooyahScreen>();
		Lives = LifeObject.Length;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 PlayerPosition = Player.gameObject.transform.position;
		float NearestPoliceDistance = float.MaxValue;
		FunPolice NearestPolice = null;

		if (CurInvincibilityTime > 0)
			CurInvincibilityTime -= Time.deltaTime;
		else
			BooyahInScene.Active = false;
			
	
		foreach (FunPolice cop in Police)
		{
			float Distance = Mathf.Abs(PlayerPosition.x - cop.gameObject.transform.position.x);
			if ( Distance < NearestPoliceDistance)
			{
				NearestPolice = cop;
				NearestPoliceDistance = Distance;
			}	
		}
		
		switch (curState)
		{
		case GAME_STATE.GAME_NORMAL:
		
			if (NearestPoliceDistance < NearestPolice.WarningDistance)
				curState = GAME_STATE.GAME_WARNING;

			if (Player.CurrentState == DanceState.DANCE_DANCE)
			{
				curState = GAME_STATE.GAME_DANCING;
				if (Player.didStartOnBeat)
				{
					
					CurInvincibilityTime = DanceOnBeatInvincibilityTime;
					BooyahInScene.Active = true;
					Player.didStartOnBeat = false;
					BooyahSound.Play();	
				}
			
			}
			break;
			
		case GAME_STATE.GAME_WARNING:
			if (!(NearestPoliceDistance < NearestPolice.WarningDistance))
				curState = GAME_STATE.GAME_NORMAL;
				
			if (Player.CurrentState == DanceState.DANCE_DANCE)
				{
					curState = GAME_STATE.GAME_DANCING;
					if (Player.didStartOnBeat)
					{
						CurInvincibilityTime = DanceOnBeatInvincibilityTime;
						BooyahInScene.Active = true;
						Player.didStartOnBeat = false;
						BooyahSound.Play();
					}
				}
			break;
			
		case GAME_STATE.GAME_DANCING:
			if (NearestPoliceDistance < NearestPolice.CatchPlayerDistance && CurInvincibilityTime <= 0)
			{
				curState = GAME_STATE.GAME_JUSTFOUND;
				TimeKeeper.Pause();
				Player.StopDancing();
				CurInvincibilityTime = 0;
			}
			
			else if (!(Player.CurrentState == DanceState.DANCE_DANCE) && NearestPoliceDistance < NearestPolice.WarningDistance)
			{
				curState = GAME_STATE.GAME_WARNING;
				CurInvincibilityTime = 0;
				
			}
				
			else if (!(Player.CurrentState == DanceState.DANCE_DANCE))
			{
				curState = GAME_STATE.GAME_NORMAL;
				CurInvincibilityTime = 0;
			}
			
			break;
		case GAME_STATE.GAME_JUSTFOUND:
			CatchTimer -= Time.deltaTime;
			if (CatchTimer <= 0)
			{
				Lives--;
				if (Lives == 0)
				{
					curState = GAME_STATE.GAME_OVER;
					EndScreenInScene.Active = true;
				}
				else
				{
					curState = GAME_STATE.GAME_NORMAL;
					TimeKeeper.UnPause();
				}
				NearestPolice.Activate();
				GameObject.Instantiate( ExplosionParticleEffect, LifeObject[Lives].transform.position, LifeObject[Lives].transform.rotation);
				GameObject.Destroy(LifeObject[Lives]);
				CatchTimer = 1.0f;
			}
			break;
		case GAME_STATE.GAME_OVER:
			//end of days crap
		break;
			
		}

		
		
		

	
	}
}
