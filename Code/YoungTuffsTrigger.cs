using UnityEngine;
using System.Collections;

public class YoungTuffsTrigger : MonoBehaviour 
{
	public GameObject[] Tuff;
	public GameObject[] Waypoint;
	public GameObject AttachTo;
	
	// Use this for initialization
	void Start () 
	{
		GetComponent<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			for(int i = 0; i < Tuff.Length; i++)
			{
			Tuff[i].GetComponent<AIPathFinder>().target = Waypoint[i].transform;
			Waypoint[i].transform.parent = AttachTo.transform;
			}
		}
	}
}
