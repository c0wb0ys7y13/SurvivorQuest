using UnityEngine;
using System.Collections;

public class FadeInLight : MonoBehaviour 
{
	public float FadeInTarget;
	public float FadeInOverTime;
	private Light MyLight;
	
	// Use this for initialization
	void Start () 
	{
		MyLight = GetComponent<Light>();
		MyLight.intensity = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Time.time <= FadeInOverTime + 1)
		{
			if(MyLight.intensity < FadeInTarget)
				MyLight.intensity += FadeInTarget * (Time.deltaTime / FadeInOverTime);
			else if(MyLight.intensity > FadeInTarget)
				MyLight.intensity = FadeInTarget;
		}
	}
}
