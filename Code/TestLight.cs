using UnityEngine;
using System.Collections;

public class TestLight : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		GetComponent<Light>().intensity = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
