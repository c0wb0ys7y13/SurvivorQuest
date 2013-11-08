using UnityEngine;
using System.Collections;

public class LootEmitter : MonoBehaviour 
{
	public GameObject LootType;
	public float LootRegenTime;
	public Vector3 minRange;
	public Vector3 maxRange;
	public GameObject[] LootList;
	private bool needToSpawnLoot = false;
	private float SpawnNextLootAtThisTime;
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		//decide if we need to spawn any loot
		if(!needToSpawnLoot)
		{
			foreach(GameObject loopObj in LootList)
			{
				needToSpawnLoot = false;
				
				if(loopObj == null)
				{
					needToSpawnLoot = true;
					SpawnNextLootAtThisTime = Time.time + LootRegenTime;
					break;
				}
			}
		}
		//if an object needs spawning, check the timer and do so, adding it to the array
		if(needToSpawnLoot == true)
		{
			if(Time.time > SpawnNextLootAtThisTime)
			{
				Vector3 SpawnPos = new Vector3(Random.Range(minRange.x, maxRange.x), Random.Range(minRange.y, maxRange.y), Random.Range(minRange.z, maxRange.z));
				if(Random.Range(1f, 0f) < 0.5f)
					SpawnPos = new Vector3(SpawnPos.x * -1, SpawnPos.y, SpawnPos.z);
				if(Random.Range(1f, 0f) < 0.5f)
					SpawnPos = new Vector3(SpawnPos.x, SpawnPos.y * -1, SpawnPos.z);
				if(Random.Range(1f, 0f) < 0.5f)
					SpawnPos = new Vector3(SpawnPos.x, SpawnPos.y, SpawnPos.z * -1);
				SpawnPos += transform.position;
					
				GameObject newLoot = (GameObject)Instantiate(LootType, SpawnPos, transform.rotation);
				newLoot.name = LootType.name;
				
				for(int i = 0; i < LootList.Length; i++)
				{
					if(LootList[i] == null)
					{
						LootList[i] = newLoot;
						break;
					}
				}
				
				needToSpawnLoot = false;
			}
		}
	}
}
