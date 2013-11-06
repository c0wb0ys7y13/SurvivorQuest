using UnityEngine;
using System.Collections;

public class SpecialRenderQueue : MonoBehaviour 
{
	public int myRenderQueue = 1;
	
	// Use this for initialization
	void Start () 
	{
		//GameObject sceneTerrain = GameObject.FindGameObjectWithTag("Terrain");
		
		//GetComponentInChildren<MeshRenderer>().material.renderQueue = sceneTerrain.GetComponent<Terrain>().renderer.material.renderQueue + -1;
	}
	
	// Update is called once per frame
	void Update () 
	{
		GetComponentInChildren<MeshRenderer>().material.renderQueue = myRenderQueue;
	}
}
