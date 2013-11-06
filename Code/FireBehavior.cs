using UnityEngine;
using System.Collections;

public class FireBehavior : MonoBehaviour 
{
	private Light MyLightComponent;
	public float FireLifetime = 180;
	public float MaxFireLifetime = 300;
	public float MaximumFireIntensity = 2;
	public float MaximumFireRange = 20;
	public float MinimumFireIntensity = 0.5f;
	public float MinimumFireRange = 5;
	public GameObject[] Burns;
	public float[] AddsLifetime;
	public GameObject BurnsDownTo;
		
	// Use this for initialization
	void Start () 
	{
		MyLightComponent = GetComponentInChildren<Light>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(FireLifetime > MaxFireLifetime)
			FireLifetime = MaxFireLifetime;
		
		if(FireLifetime > 0)
			FireLifetime -= Time.deltaTime;
		
		if(FireLifetime < 0)
		{
			Instantiate(BurnsDownTo, transform.position, transform.rotation);
			Destroy(gameObject);
		}
		
		MyLightComponent.intensity = ((FireLifetime / MaxFireLifetime) * (MaximumFireIntensity - MinimumFireIntensity)) + MinimumFireIntensity;
		MyLightComponent.range = ((FireLifetime / MaxFireLifetime) * (MaximumFireRange - MinimumFireRange)) + MinimumFireRange;
	}
	
	void OnTriggerEnter(Collider other)
	{
		Burn(other);	
	}
	
	void Burn(Collider other)
	{
		if(other.gameObject.tag == "Loot")
		{			
			for(int i = 0; i < Burns.Length; i++)
			{
				if(other.name == Burns[i].name) 
				{
					FireLifetime += AddsLifetime[i];
					Destroy(other.gameObject);
					return;
				}
			}
		}
	}
}
