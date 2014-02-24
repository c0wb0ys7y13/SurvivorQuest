using UnityEngine;
using System.Collections;

public class OpenGate : MonoBehaviour 
{
	public GameObject Gate;
	public Vector3 OpenDisplacement;
		
	// Use this for initialization
	void Start () 
	{
		Gate.transform.position += OpenDisplacement;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
