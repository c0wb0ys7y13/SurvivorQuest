using UnityEngine;
using System.Collections;

public class RoofController : MonoBehaviour 
{
	private bool FadeRoofIn;
	private bool FadeRoofOut;
	private Material RoofMaterial;
	public float FadeInOverTime;
	private bool IsTrans = false;
	
	// Use this for initialization
	void Start () 
	{
		RoofMaterial = GetComponentInChildren<MeshRenderer>().material;
		
		if(RoofMaterial.shader.name == "Transparent/Diffuse")
			IsTrans = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(FadeRoofIn && RoofMaterial.color.a < 1)
		{
			FadeRoofOut = false;
			RoofMaterial.color += new Color(0, 0, 0, Time.deltaTime * FadeInOverTime);
		}
		else if(FadeRoofIn && RoofMaterial.color.a >= 1)
		{
			FadeRoofIn = false;
			RoofMaterial.color = new Color(1, 1, 1, 1);
			if(!IsTrans)
				RoofMaterial.shader = Shader.Find("Diffuse");
		}
		else if(FadeRoofOut && RoofMaterial.color.a > 0)
		{
			FadeRoofIn = false;
			RoofMaterial.color -= new Color(0, 0, 0, Time.deltaTime * FadeInOverTime);
			RoofMaterial.shader = Shader.Find("Transparent/Diffuse");
		}
		else if(FadeRoofOut && RoofMaterial.color.a <= 0)
		{
			FadeRoofIn = false;
			RoofMaterial.color = new Color(1, 1, 1, 0);
		}
		
		
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			FadeRoofOut = true;
			FadeRoofIn = false;
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			FadeRoofIn = true;
			FadeRoofOut = false;
		}	
	}
}
