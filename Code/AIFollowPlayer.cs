using UnityEngine;
using System.Collections;

public class AIFollowPlayer : MonoBehaviour 
{
	public GameObject Target = null;
	public float TargetDistance = 2;
	protected AIStateMachine MyAIStateMachine;
	protected AICharacterController MyAICharacterController;
	
	// Use this for initialization
	void Start () 
	{
		MyAIStateMachine = GetComponent<AIStateMachine>();
		MyAICharacterController = GetComponent<AICharacterController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Target!= null)
		{
			if(Vector3.Distance(Target.transform.position, transform.position) > TargetDistance)
			{
				Vector3 Direction = Target.transform.position - transform.position;
				Direction = new Vector3(Direction.x, 0, Direction.z);
				Direction.Normalize();
				if(MyAIStateMachine.MyMovementState == PlayerStateMachine.MovementState.Running)
					rigidbody.velocity = Direction * MyAICharacterController.RunSpeed;
				else if(MyAIStateMachine.MyMovementState == PlayerStateMachine.MovementState.Walking)
					rigidbody.velocity = Direction * MyAICharacterController.WalkSpeed;
			}
			else
			{
				rigidbody.velocity = new Vector3(0, 0, 0);
				if(Target.tag == "AIWaypoint")
				{
					Target = Target.GetComponent<WaypointLinklist>().NextWaypoint;
				}
			}
		}
	}
}
