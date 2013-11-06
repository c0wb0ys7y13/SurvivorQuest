using UnityEngine;
using System.Collections;

public class PlayerStateMachine : MonoBehaviour 
{
	public enum PlayerState
	{
		None,
		Dead,
		Harvesting,
		PickingUp,
		Attack1
	}
	
	public enum MovementState
	{
		Walking,
		Running
	}
	
	public PlayerState MyPlayerState = PlayerState.None;
	public MovementState MyMovementState = MovementState.Running;
	public float InStateTill = 0.0f;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Time.time > InStateTill && MyPlayerState != PlayerState.None)
		{
			UpdateState(PlayerState.None, Mathf.Infinity);
		}
	}
	
	public void UpdateState(PlayerState newState, float Till)
	{
		if(newState != MyPlayerState)
		{
			MyPlayerState = newState;
			InStateTill = Till;
			GetComponent<SpriteAnimController>().UpdateSprite();
		}
	}
}