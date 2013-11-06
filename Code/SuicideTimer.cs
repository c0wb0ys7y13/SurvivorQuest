using UnityEngine;
using System.Collections;

public class SuicideTimer : MonoBehaviour 
{
	public float lifetime;
	private float deathTime;
	// Use this for initialization
	void Start () 
	{
		deathTime = Time.time + lifetime;
	}
	
	public void SetLifeTime(float l)
	{
		deathTime = Time.time + l;	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Time.time >= deathTime)
		{
			//GameObject.Destroy(transform);
			GameObject.DestroyObject(gameObject);
		}
	}
}
