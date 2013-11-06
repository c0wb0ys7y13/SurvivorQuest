using UnityEngine;
using System.Collections;

public class PlayerDebugCommands : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			GetComponent<PlayerInfo>().CurrentHealth -= 30f;
		}
	}
}
