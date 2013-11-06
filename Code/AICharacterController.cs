using UnityEngine;
using System.Collections;

public class AICharacterController : rpgCharController 
{
	public GameObject VisionCone;
	protected AIStateMachine MyAIStateMachine; 
	
	// Use this for initialization
	void Start () 
	{
		MyStateMachine = GetComponent<PlayerStateMachine>();
		MyAIStateMachine = GetComponent<AIStateMachine>();
	}
	
	// Update is called once per frame
	void Update () 
	{		
		//choose my facing
		if(MyAIStateMachine.MyAIState == AIStateMachine.AIState.Agro && Velocity.magnitude == 0)
		{
			Vector3 targetPos = GetComponent<AIPathFinder>().target.position;
			float DotProduct = Vector3.Dot(new Vector3(-1, 0, 0),targetPos - transform.position );
			MyFacing = Facing.Right;
			
			if(DotProduct < Vector3.Dot(new Vector3(1, 0, 0), targetPos - transform.position))
			{
				DotProduct = Vector3.Dot(new Vector3(1, 0, 0), targetPos - transform.position);
				MyFacing = Facing.Left;
			}
			if(DotProduct < Vector3.Dot(new Vector3(0, 0, -1), targetPos - transform.position))
			{
				DotProduct = Vector3.Dot(new Vector3(0, 0, -1), targetPos - transform.position);
				MyFacing = Facing.Up;
			}
			if(DotProduct < Vector3.Dot(new Vector3(0, 0, 1), targetPos - transform.position))
			{
				DotProduct = Vector3.Dot(new Vector3(0, 0, 1), targetPos - transform.position);
				MyFacing = Facing.Down;
			}
		}
		else
		{
			float DotProduct = Vector3.Dot(new Vector3(-1, 0, 0), Velocity);
			MyFacing = Facing.Right;
			
			if(DotProduct < Vector3.Dot(new Vector3(1, 0, 0), Velocity))
			{
				DotProduct = Vector3.Dot(new Vector3(1, 0, 0), Velocity);
				MyFacing = Facing.Left;
			}
			if(DotProduct < Vector3.Dot(new Vector3(0, 0, -1), Velocity))
			{
				DotProduct = Vector3.Dot(new Vector3(0, 0, -1), Velocity);
				MyFacing = Facing.Up;
			}
			if(DotProduct < Vector3.Dot(new Vector3(0, 0, 1), Velocity))
			{
				DotProduct = Vector3.Dot(new Vector3(0, 0, 1), Velocity);
				MyFacing = Facing.Down;
			}
		}
		
		if(VisionCone != null)
		{
			if(MyFacing == rpgCharController.Facing.Right)
				VisionCone.transform.LookAt(VisionCone.transform.position + Vector3.left);
			else if(MyFacing == rpgCharController.Facing.Left)
				VisionCone.transform.LookAt(VisionCone.transform.position + Vector3.right);
			else if(MyFacing == rpgCharController.Facing.Up)
				VisionCone.transform.LookAt(VisionCone.transform.position + Vector3.back);
			else if(MyFacing == rpgCharController.Facing.Down)
				VisionCone.transform.LookAt(VisionCone.transform.position + Vector3.forward);
		}
		
		if(rigidbody.velocity != Velocity)
		{
			if(Velocity.magnitude != 0)
			{
				if(rigidbody.velocity.magnitude > Velocity.magnitude)
				{
					rigidbody.velocity = Velocity;
				}
				else
				{
					rigidbody.velocity += Velocity.normalized * Acceleration * Time.deltaTime;
				}
			}
			else if(rigidbody.velocity.magnitude != 0)
			{
				if(rigidbody.velocity.magnitude < rigidbody.velocity.magnitude * Acceleration * 2 * Time.deltaTime)
				{
					rigidbody.velocity = Vector3.zero;
				}
				else
				{
					rigidbody.velocity -= rigidbody.velocity * Acceleration * DecelerationMultiplyer * Time.deltaTime;
				}
			}
		}
	}
}
