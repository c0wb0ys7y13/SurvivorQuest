using UnityEngine;
using System.Collections;

public class PlowStopperVolume : MonoBehaviour 
{
	public GameObject Plow;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Vector3.Distance(transform.position, Plow.transform.position) < 1)
		{
			Plow.GetComponent<PlowController>().PlowPullable = false;
		}
	}
}
