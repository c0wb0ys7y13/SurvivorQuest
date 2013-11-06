using UnityEngine;
using System.Collections;

public class AIStateMachine : PlayerStateMachine 
{
	public enum AIState
	{
		Oblivious,
		Cautious,
		Agro
	}
	
	public AIState MyAIState = AIState.Oblivious;
	protected float AgroTimer = 0.0f;
	public float TimeToAgro = 1.0f;
	protected Transform MyLastWaypoint = null;
	public float CautiousFor = 8f;
	public GameObject CautiousIcon;
	private float CautiousTill;
	
	
	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			Ray ray = new Ray(transform.position, other.transform.position - transform.position);
			RaycastHit returnedRaycast;
			if(Physics.Raycast(ray, out returnedRaycast, 100, LayerMask.NameToLayer("AICollider")) && returnedRaycast.collider == other)
			{
				if(MyAIState == AIState.Oblivious)
				{
					MyAIState = AIState.Cautious;
					CautiousTill = Time.time + CautiousFor;
					
					//set new waypoint to investigate
					if(GetComponent<AIPathFinder>().target != null && GetComponent<AIPathFinder>().target.gameObject.tag == "AIWaypoint")
					{
						MyLastWaypoint = GetComponent<AIPathFinder>().target;
					}
					else
					{
						//if i wasnt seeking a waypoint, just return to my starting position
						MyLastWaypoint = transform;
					}
					GetComponent<AIPathFinder>().target = other.transform;
				}
				if(MyAIState == AIState.Cautious)
				{
					AgroTimer += Time.deltaTime;
					
					if(AgroTimer >= TimeToAgro)
					{
						AgroTimer = 0f;
						MyAIState = AIState.Agro;
						GetComponent<AIPathFinder>().target = other.gameObject.transform;
						Debug.Log("Agro");
					}
				}
			}
		}
	}
	
	void LateUpdate()
	{
		if(MyAIState == AIState.Oblivious)
		{
			MyMovementState = PlayerStateMachine.MovementState.Walking;
			AgroTimer = 0f;
		}
		else if(MyAIState == AIState.Cautious)
		{
			if(Time.time > CautiousTill)
			{
				MyAIState = AIState.Oblivious;
				GetComponent<AIPathFinder>().target = MyLastWaypoint;
			}
		}
		else if(MyAIState == AIState.Agro)
		{
			MyMovementState = PlayerStateMachine.MovementState.Running;
		}
	}
}
