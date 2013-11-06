using UnityEngine;
using System.Collections;

public class HarvestPoint : MonoBehaviour 
{
	public enum HarvistTools
	{
		Generic,
		Pick,
		Axe,
		Shovel,
		GoldPan
	}
	
	public HarvistTools NeededTool;
	public GameObject[] YieldsOnHit;
	public float[] OnHitProbability;
	public GameObject[] YieldsOnDestroy;
	public float[] OnDestroyProbability;
	public int MaxHarvests = 1;
	public int CurrentHarvists = 1;
	public float RegenTime = 10;
	[HideInInspector] public float CurrentRegenTime = 0;
	public Texture HarvistedTexture;
	private Texture UnHarvistedTexture;
	
	// Use this for initialization
	void Start () 
	{
		UnHarvistedTexture = GetComponentInChildren<MeshRenderer>().material.mainTexture;
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateHarvists();
	}
	
	void UpdateHarvists()
	{
		if(CurrentHarvists == 0)
		{
			//swap to harvested texture
			if(GetComponentInChildren<MeshRenderer>().material.mainTexture != HarvistedTexture)
			{
				GetComponentInChildren<MeshRenderer>().material.mainTexture = HarvistedTexture;
			}
			
			//update regen
			if(CurrentRegenTime < RegenTime)
				CurrentRegenTime += Time.deltaTime;
			else
			{
				CurrentHarvists = MaxHarvests;
				CurrentRegenTime = 0;
				GetComponentInChildren<MeshRenderer>().material.mainTexture = UnHarvistedTexture;
			}
		}
	}
}
