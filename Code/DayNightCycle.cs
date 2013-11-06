using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour 
{
	public const int HoursInADay = 24;
	public float TimeOfDay = 12f;
	public int DayStartsAt = 8;
	public int DuskStartsAt = 18;
	public int DawnStartsAt = 4;
	public int NightStartsAt = 22;
	public int GameDayInRealMin = 24;
	private Light MyDirectionalLight;
	public float MaxDaylight = 0.65f;
	public float MinDaylight = 0.0f;
	
	
	// Use this for initialization
	void Start () 
	{
		MyDirectionalLight = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//convert real time into game time
		TimeOfDay += (Time.deltaTime / 60) * (HoursInADay / GameDayInRealMin);
		//reset the clock to 0 at midnight	
		if(TimeOfDay > (float)HoursInADay)
		{
			TimeOfDay = 0;
		}
		
		//fade the ambiant light in/out
		if(TimeOfDay >= (float)DayStartsAt && TimeOfDay < (float)DuskStartsAt)
		{
			MyDirectionalLight.intensity = MaxDaylight;
		}
		else if(TimeOfDay >= (float)DuskStartsAt && TimeOfDay < (float)NightStartsAt)
		{
			MyDirectionalLight.intensity = MaxDaylight - ((TimeOfDay - DuskStartsAt) / (NightStartsAt - DuskStartsAt) * (MaxDaylight - MinDaylight));
		}
		else if(TimeOfDay >= (float)NightStartsAt && TimeOfDay < (float)DawnStartsAt)
		{
			MyDirectionalLight.intensity = MinDaylight;
		}
		else if(TimeOfDay >= (float)DawnStartsAt && TimeOfDay < (float)DayStartsAt)
		{
			MyDirectionalLight.intensity = (TimeOfDay - DawnStartsAt) / (DayStartsAt - DawnStartsAt) * (MaxDaylight - MinDaylight);
		}
	}
}
