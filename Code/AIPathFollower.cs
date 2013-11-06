using UnityEngine;
using System.Collections;

public class AIPathFollower : MonoBehaviour 
{
	//----------------------------------------------------------------------------------
	// The example script to follow path. 
	// It manages waypointed path from pathFindingScript and move object along it.
	//----------------------------------------------------------------------------------

	public AIPathFinder pathFindingScript;				// Path holder/generator script
	public float damping = 3.0f;								// Smooth facing/movement value
	//var movementSpeed: float = 5.0;					// Speed of object movement along the path
	public float waypointActivationDistance = 1.0f;	// How far should object be to waypoint for its activation and choosing new
	public float AttackRange = 2.0f;
	public float stuckDistance = 2;                   // Max distance of move per regenTimeout that supposed to indicate stuking
	public float stuckTimeout = 2;                    // How fast should path be regenerated if player stucks
	
	// Usefull internal variables, please don't change them blindly
	private int currentWaypoint = 0;
	private Vector3 targetPosition;
	private bool inMove= false;
	
	private Vector3 oldPosition;
	private float timeToRegen;
	protected AIStateMachine MyAIStateMachine;
	protected AICharacterController MyAICharacterController;
	//=============================================================================================================
	// Setup initial data according to specified parameters
	void Start() 
	{
	  	MyAIStateMachine = GetComponent<AIStateMachine>();
		MyAICharacterController = GetComponent<AICharacterController>();
	}
	
	//----------------------------------------------------------------------------------
	//Main loop
	void Update() 
	{
	  	Ray LOSRay;
		RaycastHit LOSRayHit;
		if(pathFindingScript.target != null)
		{
			LOSRay = new Ray(transform.position + Vector3.up, pathFindingScript.target.position - transform.position);
		}
		else
		{
			LOSRay = new Ray(transform.position + Vector3.up, transform.forward);
		}
		
		if(	pathFindingScript.target == null || //if no target
			Vector3.Distance(transform.position, pathFindingScript.target.position) < waypointActivationDistance || //or im at my destination waypoint
			(pathFindingScript.target.gameObject.tag == "Player" && //or my targets a player
			Vector3.Distance(transform.position, pathFindingScript.target.position) < AttackRange && //and im within attack range of that player
			Physics.Raycast(LOSRay, out LOSRayHit, AttackRange,LayerMask.NameToLayer("AICollider")) && //and i can see a collider that isnt myself
			LOSRayHit.collider.transform == pathFindingScript.target))//and that collider is the player
	  	{
			MyAICharacterController.Velocity = Vector3.zero;
			inMove = false;
			
			//if we arived at our waypoint, move to next waypoint
			if( pathFindingScript.target != null &&
				pathFindingScript.target.gameObject.tag == "AIWaypoint" &&
				pathFindingScript.target.gameObject.GetComponent<WaypointLinklist>() != null &&
				pathFindingScript.target.gameObject.GetComponent<WaypointLinklist>().NextWaypoint != null)
			{
				pathFindingScript.target = pathFindingScript.target.gameObject.GetComponent<WaypointLinklist>().NextWaypoint.transform;
				inMove = true;
				pathFindingScript.FindPath();
			}
		}
		else
		{
		  	// Activate waypoint when object is closer than waypointActivationDistance
		   	if(Vector3.Distance(transform.position, targetPosition) < waypointActivationDistance) 
		   	{					
				if (currentWaypoint > pathFindingScript.waypoints.Length-1) 
				{
					currentWaypoint = 1;
				}
				
		      	currentWaypoint ++;
		   	}
		  
		  
		   // Try to get next waypoint. If it is missed in some reason - set currentWaypoint to 1
		 	try
			{
		  		targetPosition = pathFindingScript.waypoints[currentWaypoint]; 
			}
			catch
			{
				pathFindingScript.FindPath();
				currentWaypoint = 1;
				targetPosition = pathFindingScript.waypoints[currentWaypoint]; 
			}
			
			/*  Old code turned the game object towards the waypoint, then set its speed forward
			// Look at and dampen the rotation
			var rotation = Quaternion.LookRotation(targetPosition - transform.position);
			
			 transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
			 transform.Translate(Vector3.forward*movementSpeed*Time.deltaTime);
			*/
			
			if(MyAIStateMachine.MyMovementState == PlayerStateMachine.MovementState.Running)
				MyAICharacterController.Velocity = (targetPosition - transform.position).normalized * MyAICharacterController.RunSpeed;
			else if(MyAIStateMachine.MyMovementState == PlayerStateMachine.MovementState.Walking)
				MyAICharacterController.Velocity = (targetPosition - transform.position).normalized * MyAICharacterController.WalkSpeed;
			
		    inMove = true;
		    
		    
		   	if (Time.time > timeToRegen)
		    {
		      	if (Vector3.Distance(transform.position, oldPosition) < stuckDistance) 
		       	{
		          	pathFindingScript.FindPath();
		          	currentWaypoint = 1;
		       	}
		      
		      	oldPosition = transform.position; 
		      	timeToRegen = Time.time + stuckTimeout;
		    }
		}
	}

	// Return true if object is moving now
	public bool isMoving() 
	{
		return inMove;
	}
}
