using UnityEngine;
using System.Collections;

public class FadeInText : MonoBehaviour 
{
	public float FadeRate = 0.1f;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnTriggerStay(Collider other)
	{
		//if(other.gameObject.GetComponent<TextMesh>() != null)
		//{
		other.gameObject.GetComponent<TextMesh>().color += new Color(0,0,0, FadeRate * Time.deltaTime);	
		Debug.Log("taco");
		//}
	}
}
