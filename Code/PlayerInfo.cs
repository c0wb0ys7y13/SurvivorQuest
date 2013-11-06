using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour 
{
	public float CurrentHunger;
	public float MaxHunger;
	public float HungerPerMin;
	public float HungerPainsPerMin;
	public float HungerHealthRegenPerMin;
	public float CurrentThurst;
	public float MaxThurst;
	public float ThurstPerMin;
	public float ThurstPainsPerMin;
	public float ThurstHealthRegenPerMin;
	public float CurrentHealth;
	public float MaxHealth;
	
	public bool isAlive;
	
	// Use this for initialization
	void Start () 
	{
		 
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(GetComponent<PlayerInfo>().isAlive)
		{
			if(CurrentHunger > 0)
			{
				CurrentHunger -= Time.deltaTime * (HungerPerMin / 60);
				CurrentHealth += Time.deltaTime * (HungerHealthRegenPerMin / 60);
			}
			else
			{
				CurrentHealth -= Time.deltaTime * (HungerPainsPerMin / 60);
				CurrentHunger = 0;
			}
			if(CurrentThurst > 0)
			{
				CurrentThurst -= Time.deltaTime * (ThurstPerMin / 60);
				CurrentHealth += Time.deltaTime * (ThurstHealthRegenPerMin * (CurrentThurst/MaxThurst) / 60);
			}
			else
			{
				CurrentHealth -= Time.deltaTime * (ThurstPainsPerMin / 60);
				CurrentThurst = 0;
			}
		}
		
		if(CurrentHealth > MaxHealth)
			CurrentHealth = MaxHealth;
		else if(CurrentHealth <= 0)
		{
			CurrentHealth = 0;
			if(isAlive)
			{
				isAlive = false;
				GetComponent<PlayerStateMachine>().UpdateState(PlayerStateMachine.PlayerState.Dead, Mathf.Infinity);
				//GetComponent<PlayerStateMachine>().MyPlayerState = PlayerStateMachine.PlayerState.Dead;
				//GetComponent<PlayerStateMachine>().InStateTill = Mathf.Infinity;
			}
		}
	}
}
