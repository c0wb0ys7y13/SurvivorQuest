using UnityEngine;
using System.Collections;

public class PrisonTrigger1 : MonoBehaviour 
{
	public GameObject[] GuardsToTrigger;
	public GameObject Waypoint;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			foreach(GameObject tempObj in GuardsToTrigger)
			{
				if(tempObj.GetComponent<AIPathFinder>() != null)
				{
					tempObj.GetComponent<AIPathFinder>().target = Waypoint.transform;
				}
			}
		}
	}
	
}
