using UnityEngine;
using System.Collections;

public class Attacks : MonoBehaviour 
{
	public GameObject Attack1Obj;
	private PlayerStateMachine MyStateMachine;
	public float Attack1Durration;
	
	// Use this for initialization
	void Start () 
	{
		MyStateMachine = GetComponent<PlayerStateMachine>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float PlayerDir = 0f;
		if(GetComponent<rpgCharController>().MyFacing == rpgCharController.Facing.Left)
		{
			PlayerDir = -1.57f;
		}
		else if(GetComponent<rpgCharController>().MyFacing == rpgCharController.Facing.Right)
		{
			PlayerDir = 1.57f;	
		}
		
		if(MyStateMachine.MyPlayerState != PlayerStateMachine.PlayerState.Dead)
		{
			if(Input.GetKeyDown(KeyCode.Return))
			{
				GameObject attackInstance = (GameObject)Instantiate(Attack1Obj, transform.position + new Vector3(0, 1, 0), new Quaternion(0, 0, 0, 0));
				attackInstance.transform.parent = transform;
				attackInstance.transform.RotateAroundLocal(Vector3.up, PlayerDir);
				attackInstance.GetComponent<SuicideTimer>().SetLifeTime(Attack1Durration);
				MyStateMachine.UpdateState(PlayerStateMachine.PlayerState.Attack1, Time.time + Attack1Durration);
				//MyStateMachine.MyPlayerState = PlayerStateMachine.PlayerState.Attack1;
				//MyStateMachine.InStateTill = Time.time + Attack1Durration;
			}
		}
	}
}
