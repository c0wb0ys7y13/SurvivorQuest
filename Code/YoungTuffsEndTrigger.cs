﻿using UnityEngine;
using System.Collections;

public class YoungTuffsEndTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			GetComponent<MeshRenderer>().enabled = false;	
		}
	}
}
