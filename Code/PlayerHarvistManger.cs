using UnityEngine;
using System.Collections;

public class PlayerHarvistManger : MonoBehaviour 
{	
	public HarvestPoint.HarvistTools[] MyHarvestTools;
	[HideInInspector] public Collider CurrentHarvestPoint;
	public float HarvestRange = 2f;
	public float HarvestDurration = 0.5f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.E) && GetComponent<PlayerStateMachine>().MyPlayerState == PlayerStateMachine.PlayerState.None)
		{
			if(CurrentHarvestPoint != null && 
				Vector3.Distance(CurrentHarvestPoint.transform.position, transform.position) <= HarvestRange && 
				GetComponent<PlayerStateMachine>().MyPlayerState == PlayerStateMachine.PlayerState.None)
			{
				HarvestPoint otherHarvestPoint = CurrentHarvestPoint.GetComponent<HarvestPoint>();
				bool CanHarvest = false;
				
				if(otherHarvestPoint.CurrentHarvists <= 0)
				{
					return;
				}
				
				foreach(int tempTools in MyHarvestTools)
				{
					if((int)tempTools == (int)otherHarvestPoint.NeededTool)
						CanHarvest = true;
				}				
				
				if(!CanHarvest)
				{
					return;
				}
					
				otherHarvestPoint.CurrentHarvists--;
				GetComponent<PlayerStateMachine>().UpdateState(PlayerStateMachine.PlayerState.Harvesting, Time.time + HarvestDurration);				
				
				if(otherHarvestPoint.CurrentHarvists == 0)
				{
					for(int i = 0; i < otherHarvestPoint.YieldsOnDestroy.Length; i++)
					{
						if(Random.Range(0f, 1f) < otherHarvestPoint.OnDestroyProbability[i])
						{
							//spawn this object
							GameObject newLoot = (GameObject)Instantiate(otherHarvestPoint.YieldsOnDestroy[i], otherHarvestPoint.transform.position + (0.5f * Vector3.up), CurrentHarvestPoint.transform.rotation);
							newLoot.name = otherHarvestPoint.YieldsOnDestroy[i].name;
							newLoot.rigidbody.velocity = new Vector3(Random.Range(-1f, 1f), 1f, 3f);
							if(otherHarvestPoint.GetComponent<BoxCollider>() != null)
								Physics.IgnoreCollision(newLoot.collider, otherHarvestPoint.GetComponent<BoxCollider>());
						}
					}
					return;
				}
				else
				{
					for(int i = 0; i < otherHarvestPoint.YieldsOnHit.Length; i++)
					{
						if(Random.Range(0f, 1f) < otherHarvestPoint.OnHitProbability[i])
						{
							//spawn this object
							GameObject newLoot = (GameObject)Instantiate(otherHarvestPoint.YieldsOnHit[i], otherHarvestPoint.transform.position + (0.5f * Vector3.up), CurrentHarvestPoint.transform.rotation);
							newLoot.name = otherHarvestPoint.YieldsOnHit[i].name;
							newLoot.rigidbody.velocity = new Vector3(Random.Range(-1f, 1f), 1f, 3f);
							if(otherHarvestPoint.GetComponent<BoxCollider>() != null)
								Physics.IgnoreCollision(newLoot.collider, otherHarvestPoint.GetComponent<BoxCollider>());
						}
					}
					return;
				}
			}
		}
	}
	
	void OnTriggerStay(Collider other)
	{
		if(other.tag == "Harvest")
		{
			CurrentHarvestPoint = other;
		}
	}
}
