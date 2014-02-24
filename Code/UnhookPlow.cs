using UnityEngine;
using System.Collections;

public class UnhookPlow : MonoBehaviour 
{
	public GameObject Plow;
	
	// Use this for initialization
	void Start () 
	{
		Plow.GetComponent<PlowController>().PlowPuller = null;
		Plow.GetComponentInChildren<MeshCollider>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
