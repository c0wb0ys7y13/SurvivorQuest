using UnityEngine;
using System.Collections;

public class TreeFader : MonoBehaviour 
{
	private GameObject Player;
	private Material myMaterial;
	public float zFadeRange;
	public float zMin;
	public float xFadeRange;
	public float TargetAlpha;
	public float FadeRate;
	
	// Use this for initialization
	void Start () 
	{
		Player = GameObject.FindGameObjectWithTag("Player").gameObject;
		myMaterial = GetComponentInChildren<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( myMaterial.color.a > TargetAlpha &&
			Player.transform.position.z + zMin < transform.position.z &&
			Mathf.Abs(Player.transform.position.z - transform.position.z) < zFadeRange &&
			Mathf.Abs(Player.transform.position.x - transform.position.x) < xFadeRange)
		{
			myMaterial.color -= new Color(0,0,0,FadeRate) * Time.deltaTime;
		}
		else if(myMaterial.color.a < 1f)
		{
			myMaterial.color += new Color(0,0,0,FadeRate) * Time.deltaTime;
		}
	}
}
