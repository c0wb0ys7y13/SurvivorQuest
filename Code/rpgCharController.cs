using UnityEngine;
using System.Collections;

public class rpgCharController : MonoBehaviour 
{
	[HideInInspector] public Vector3 Velocity;
	public float RunSpeed;
	public float WalkSpeed;
	public enum Facing
	{
		Right,
		Left,
		Up,
		Down
	}
	public Facing MyFacing = Facing.Right;
	protected PlayerStateMachine MyStateMachine;
	public float Acceleration = 15f;
	public float DecelerationMultiplyer = 2;
		
	// Use this for initialization
	void Start () 
	{
		MyStateMachine = GetComponent<PlayerStateMachine>();
		MyFacing = Facing.Down;
	}
	
	// Update is called once per frame
	void Update()
	{
		if(MyStateMachine.MyPlayerState == PlayerStateMachine.PlayerState.None)
		{			
			if(Input.GetKey(KeyCode.A) &&
			   	!Input.GetKey(KeyCode.D) &&
				!(Velocity.x < 0))
			{
				Velocity += new Vector3(Acceleration * Time.deltaTime, 0, 0);
				MyFacing = Facing.Left;
			}
			else if(Input.GetKey(KeyCode.D) &&
				!Input.GetKey(KeyCode.A) &&
				!(Velocity.x > 0))
			{
				Velocity += new Vector3(Acceleration * Time.deltaTime * -1, 0, 0);
				MyFacing = Facing.Right;
			}
			else
			{
				if(Mathf.Abs(Velocity.x) < Mathf.Abs(Acceleration * DecelerationMultiplyer * Time.deltaTime))
				{
					Velocity = new Vector3(0, 0, Velocity.z);
				}
				else if(Velocity.x > 0)
				{
					Velocity -= new Vector3(Acceleration * DecelerationMultiplyer * Time.deltaTime, 0, 0);
				}
				else if(Velocity.x < 0)
				{
					Velocity -= new Vector3(Acceleration * DecelerationMultiplyer * Time.deltaTime  * -1, 0, 0);
				}
			}
			
			if(Input.GetKey(KeyCode.W) &&
				!Input.GetKey(KeyCode.S) &&
				!(Velocity.z > 0))
			{
				Velocity += new Vector3(0, 0, Acceleration * Time.deltaTime * -1);
				MyFacing = Facing.Up;
			}
			else if(Input.GetKey(KeyCode.S) &&
				!Input.GetKey(KeyCode.W) &&
				!(Velocity.z < 0))
			{
				Velocity += new Vector3(0, 0, Acceleration * Time.deltaTime);
				MyFacing = Facing.Down;
			}
			else
			{
				if(Mathf.Abs(Velocity.z) < Mathf.Abs(Acceleration * DecelerationMultiplyer * Time.deltaTime))
				{
					Velocity = new Vector3(Velocity.x, 0, 0);
				}
				else if(Velocity.z > 0)
				{
					Velocity -= new Vector3(0, 0, Acceleration * DecelerationMultiplyer * Time.deltaTime);
				}
				else if(Velocity.z < 0)
				{
					Velocity -= new Vector3(0, 0, Acceleration * DecelerationMultiplyer * Time.deltaTime  * -1);
				}
			}
			
			//clamp my velocity
			if(MyStateMachine.MyMovementState == PlayerStateMachine.MovementState.Running && Velocity.magnitude > RunSpeed)
			{
				Velocity = Velocity.normalized * RunSpeed;
			}
			else if(MyStateMachine.MyMovementState == PlayerStateMachine.MovementState.Walking && Velocity.magnitude > RunSpeed)
			{
				Velocity = Velocity.normalized * WalkSpeed;
			}
			
			//apply velocity to RB
			rigidbody.velocity = Velocity + new Vector3(0, rigidbody.velocity.y, 0);
		}
		else
		{
			Velocity = new Vector3(0,0,0);
			rigidbody.velocity = Velocity;
		}
	}
}
