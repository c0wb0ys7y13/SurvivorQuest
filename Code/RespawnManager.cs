using UnityEngine;
using System.Collections;

public class RespawnManager : MonoBehaviour 
{
	private PlayerInfo MyPlayerInfo;
	//do i need to respawn?
	public bool Respawning = false;
	//what time to respawn at
	public float RespawnAt = 0;
	//the amount of time to wait before respawning
	public float respawnDelay = 5;
	
	// Use this for initialization
	void Start () 
	{
		MyPlayerInfo = GetComponent<PlayerInfo>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!MyPlayerInfo.isAlive)
		{
			//do a search for another players
			GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
			
			//if all the players are dead, set up respawn stuff
			if(Respawning == false)
			{
				Respawning = true;
				
				foreach(GameObject loopPlayers in Players)
				{
					if(loopPlayers.GetComponent<PlayerInfo>().isAlive)
					{
						Respawning = false;
					}
				}
				
				if(Respawning == true)
				{
					RespawnAt = Time.time + respawnDelay;
				}
			}
			else//handel actual respawn process
			{
				if(Time.time >= RespawnAt)
				{
					//locate the nearest appeased alter
					
				}
			}
		}
	}
}
