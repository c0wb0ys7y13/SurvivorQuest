using UnityEngine;
using System.Collections;

public class BackWallFader : MonoBehaviour 
{
	public float FadeInRange = 5f;
	private GameObject Player;
	
	// Use this for initialization
	void Start () 
	{
		Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 closestPoint;
		
		if(collider != null)
			closestPoint = collider.ClosestPointOnBounds(Player.transform.position);
		else
			closestPoint = transform.position;
		
		if(Vector3.Distance(closestPoint, Player.transform.position) <= FadeInRange)
		{
			float RangePercent = Vector3.Distance(closestPoint, Player.transform.position) / FadeInRange;
			GetComponentInChildren<MeshRenderer>().material.color = new Color(1, 1, 1, 0.5f - RangePercent * 0.5f);
		}
		else
		{
			GetComponentInChildren<MeshRenderer>().material.color = new Color(1, 1, 1, 0);
		}
	}
}
