using UnityEngine;
using System.Collections;

public class FadeOutScene : MonoBehaviour 
{
	public GameObject[] SceneLights;
	private bool HasShown = false;
	private float[] Intensity;
	private Color[] Colors;
	
	// Use this for initialization
	void Start () 
	{
		Intensity = new float[SceneLights.Length];
		Colors = new Color[SceneLights.Length];
		for(int i = 0; i < SceneLights.Length; i++)
		{
			Intensity[i] = SceneLights[i].GetComponent<FadeInLight>().FadeInTarget;	
		}
		for(int i = 0; i < SceneLights.Length; i++)
		{
			Colors[i] = SceneLights[i].GetComponent<Light>().color;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(GetComponent<TextMesh>().color.a == 1f)
			HasShown = true;
		
		if(HasShown)
		{
			for(int i = 0; i < SceneLights.Length; i++)
			{
				//obj.GetComponent<Light>().color = Colors[i] - new Color(0, 0, 0, Intensity[i] * GetComponent<TextMesh>().color.a);
				SceneLights[i].GetComponent<Light>().intensity = Intensity[i] * GetComponent<TextMesh>().color.a;
			}
		}
	}
}
