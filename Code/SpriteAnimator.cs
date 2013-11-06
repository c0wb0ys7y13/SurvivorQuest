using UnityEngine;
using System.Collections;

public class SpriteAnimator : MonoBehaviour 
{
	//the plane 
	private MeshRenderer CharMesh;
	//our current sprite sheet
	public Material SpriteSheet;
	public bool playBackwards = false;
	public bool FlipMaterial = false;
	//next sprite sheet to play after this ones finished, if any
	public Material NextSpriteSheet = null;
	public bool NextPlayBackwards = false;
	public bool NextFlipMaterial = false;
	//The number of frames in the sprite sheet
	private int FramesInSpriteSheet;
	//Current frame our sprite is in
	private int CurrentSpriteFrame;
	//The length of the animation in seconds
	private float AnimLength;
	//The amount of time that has passed since the last frame was set
	private float CurrentFrameTime;
	//a const value representing the rate that frames are played
	public int FramesPerSec = 16;
	private bool newAnim = false;
	
	public void SetPlayBackwards(bool b)
	{
		if(b != playBackwards)
		{
			playBackwards = b;
			newAnim = true;
		}
	}
	
	public void SetFlipMaterial(bool b)
	{
		if(b != FlipMaterial)
		{
			FlipMaterial = b;
			newAnim = true;
		}
	}
	
	// Use this for initialization
	void Start () 
	{
		CharMesh = GetComponentInChildren<MeshRenderer>();
		
		if(CharMesh.material != null)
		{
			SpriteSheet = CharMesh.material;
		}
		
		if(SpriteSheet != null)
		{
			//if new texture is assigned, set up innitial values
			CharMesh.material = SpriteSheet;
			FramesInSpriteSheet = SpriteSheet.mainTexture.width / SpriteSheet.mainTexture.height;
			AnimLength = (1f / FramesPerSec) * (float)FramesInSpriteSheet;
			CurrentSpriteFrame = 0;
			CurrentFrameTime = 0;
			CharMesh.material.mainTextureScale = new Vector2(1f/(float)FramesInSpriteSheet, 1);
			CharMesh.material.mainTextureOffset = new Vector2((1f/(float)FramesInSpriteSheet)*CurrentSpriteFrame, 1);
			if(FlipMaterial)
			{
				CharMesh.material.mainTextureScale = new Vector2(CharMesh.material.mainTextureScale.x * -1, CharMesh.material.mainTextureScale.y);
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(SpriteSheet != null)
		{
			if(CharMesh.material.mainTexture != SpriteSheet.mainTexture || newAnim)
			{
				//if new texture is assigned, set up innitial values
				CharMesh.material = SpriteSheet;
				FramesInSpriteSheet = SpriteSheet.mainTexture.width / SpriteSheet.mainTexture.height;
				AnimLength = (1f / FramesPerSec) * (float)FramesInSpriteSheet;
				CurrentSpriteFrame = 0;
				CurrentFrameTime = 0;
				CharMesh.material.mainTextureScale = new Vector2(1f/(float)FramesInSpriteSheet, 1);
				CharMesh.material.mainTextureOffset = new Vector2((1f/(float)FramesInSpriteSheet)*CurrentSpriteFrame, 1);
				if(FlipMaterial)
				{
					CharMesh.material.mainTextureScale = new Vector2(CharMesh.material.mainTextureScale.x * -1, CharMesh.material.mainTextureScale.y);
				}
				newAnim = false;
			}
			
			//set the offset
			CharMesh.material.mainTextureOffset = new Vector2((1f/(float)FramesInSpriteSheet)*CurrentSpriteFrame, 1);
			if(playBackwards)
			{
				CharMesh.material.mainTextureOffset = new Vector2(CharMesh.material.mainTextureOffset.x * -1, CharMesh.material.mainTextureOffset.y);
			}
			
			//increment the fame time
			CurrentFrameTime += Time.deltaTime;
			if(CurrentFrameTime > 1f / FramesPerSec)
			{
				//if the correct amount of time has passed, decrement the frame time and increment the next sprite frame to be shown
				CurrentFrameTime -= 1f / (float)FramesPerSec;
				CurrentSpriteFrame += 1;
			}
			if(CurrentSpriteFrame > FramesInSpriteSheet && NextSpriteSheet != null)
			{
				SpriteSheet = NextSpriteSheet;
				NextSpriteSheet = null;
				newAnim = true;
				SetPlayBackwards(NextPlayBackwards);
				SetFlipMaterial(NextFlipMaterial);	
			}
		}
	}
}
