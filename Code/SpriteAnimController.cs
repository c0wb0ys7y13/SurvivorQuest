using UnityEngine;
using System.Collections;

public class SpriteAnimController : MonoBehaviour 
{
	public Material IdleDownSprite;
	public Material IdleSideSprite;
	public Material IdleUpSprite;
	
	public Material RunSideSprite;
	public Material RunDownSprite;
	public Material RunUpSprite;
	
	public Material WalkDownSprite;
	public Material WalkSideSprite;
	public Material WalkUpSprite;
	
	public Material DeathSprite;
	public Material Attack1Sprite;
	private SpriteAnimator MySpriteAnimator;
	private rpgCharController MyRpgCharController;
	private PlayerStateMachine MyStateMachine;
	
	
	// Use this for initialization
	void Start () 
	{
		MySpriteAnimator = GetComponent<SpriteAnimator>();
		MyRpgCharController = GetComponent<rpgCharController>();
		MyStateMachine = GetComponent<PlayerStateMachine>();
	}
	
	void Update()
	{
		if(MyStateMachine.MyPlayerState == PlayerStateMachine.PlayerState.None && 
			MyRpgCharController.Velocity.magnitude > 0)
		{
			if(MyRpgCharController.MyFacing == rpgCharController.Facing.Right)
			{
				MySpriteAnimator.SpriteSheet = 	RunSideSprite;
				MySpriteAnimator.SetPlayBackwards(false);
				MySpriteAnimator.SetFlipMaterial(false);
			}
			else if(MyRpgCharController.MyFacing == rpgCharController.Facing.Left)
			{
				MySpriteAnimator.SpriteSheet = 	RunSideSprite;
				MySpriteAnimator.SetPlayBackwards(false);
				MySpriteAnimator.SetFlipMaterial(true);
			}
			else if(MyRpgCharController.MyFacing == rpgCharController.Facing.Down)
			{
				MySpriteAnimator.SpriteSheet = 	RunDownSprite;
				MySpriteAnimator.SetPlayBackwards(false);
				MySpriteAnimator.SetFlipMaterial(false);
			}
			else if(MyRpgCharController.MyFacing == rpgCharController.Facing.Up)
			{
				MySpriteAnimator.SpriteSheet = 	RunUpSprite;
				MySpriteAnimator.SetPlayBackwards(false);
				MySpriteAnimator.SetFlipMaterial(false);
			}
		}
		else if(MyStateMachine.MyPlayerState == PlayerStateMachine.PlayerState.None)
		{
			if(MyRpgCharController.MyFacing == rpgCharController.Facing.Right)
			{
				MySpriteAnimator.SpriteSheet = 	IdleSideSprite;
				MySpriteAnimator.SetPlayBackwards(false);
				MySpriteAnimator.SetFlipMaterial(false);
			}
			else if(MyRpgCharController.MyFacing == rpgCharController.Facing.Left)
			{
				MySpriteAnimator.SpriteSheet = 	IdleSideSprite;
				MySpriteAnimator.SetPlayBackwards(false);
				MySpriteAnimator.SetFlipMaterial(true);
			}
			if(MyRpgCharController.MyFacing == rpgCharController.Facing.Up)
			{
				MySpriteAnimator.SpriteSheet = 	IdleUpSprite;
				MySpriteAnimator.SetPlayBackwards(false);
				MySpriteAnimator.SetFlipMaterial(false);
			}
			else if(MyRpgCharController.MyFacing == rpgCharController.Facing.Down)
			{
				MySpriteAnimator.SpriteSheet = 	IdleDownSprite;
				MySpriteAnimator.SetPlayBackwards(false);
				MySpriteAnimator.SetFlipMaterial(false);
			}
			
		}
	}
	
	public void UpdateSprite() 
	{
		if(MyStateMachine.MyPlayerState == PlayerStateMachine.PlayerState.Attack1)
		{
			MySpriteAnimator.SpriteSheet = Attack1Sprite;
			MySpriteAnimator.SetPlayBackwards(false);
			MySpriteAnimator.SetFlipMaterial(false);
		}
		else if(MyStateMachine.MyPlayerState == PlayerStateMachine.PlayerState.Dead)
		{
			MySpriteAnimator.SpriteSheet = DeathSprite;
			MySpriteAnimator.SetPlayBackwards(false);
			MySpriteAnimator.SetFlipMaterial(false);	
		}
		else if(MyStateMachine.MyPlayerState == PlayerStateMachine.PlayerState.None)
		{
			if(MyRpgCharController.MyFacing == rpgCharController.Facing.Down)
			{
				MySpriteAnimator.SpriteSheet = 	IdleDownSprite;
				MySpriteAnimator.SetPlayBackwards(false);
				MySpriteAnimator.SetFlipMaterial(false);
			}
			else if(MyRpgCharController.MyFacing == rpgCharController.Facing.Up)
			{
				MySpriteAnimator.SpriteSheet = 	IdleUpSprite;
				MySpriteAnimator.SetPlayBackwards(false);
				MySpriteAnimator.SetFlipMaterial(false);
			}
			else if(MyRpgCharController.MyFacing == rpgCharController.Facing.Left)
			{
				MySpriteAnimator.SpriteSheet = 	IdleSideSprite;
				MySpriteAnimator.SetPlayBackwards(true);
				MySpriteAnimator.SetFlipMaterial(false);
			}
			else if(MyRpgCharController.MyFacing == rpgCharController.Facing.Right)
			{
				MySpriteAnimator.SpriteSheet = 	IdleSideSprite;
				MySpriteAnimator.SetPlayBackwards(false);
				MySpriteAnimator.SetFlipMaterial(false);
			}
		}
	}
}
