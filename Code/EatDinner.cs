using UnityEngine;
using System.Collections;

public class EatDinner : MonoBehaviour 
{
	public GameObject dinner;
	public int SayAfterFed;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == dinner.name)
		{
			Destroy(other.gameObject);
			GetComponent<ConversationManager>().CurrentNPCChatText = SayAfterFed;
		}
	}
}
