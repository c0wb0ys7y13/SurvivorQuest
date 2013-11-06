using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(BoxCollider))]

public class LootManager : MonoBehaviour 
{
	public bool PickUp = true;
	public GameObject[] ReactsWith;
	public GameObject[] Creates;
	public bool[] DestroyOnCombine;
	//private SphereCollider mySphereCollider;
	[HideInInspector] public bool CanCombine = false;
	
	// Use this for initialization
	void Start ()
	{
		//ignore collision on the players box collider, so the player cant kick loot but can grab it
		if(GetComponent<BoxCollider>() != null && GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider>() != null)
			Physics.IgnoreCollision(GetComponent<BoxCollider>(), GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider>());
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	
	void OnTriggerEnter(Collider other)
	{
		//Pickup(other);
		Combine(other);	
	}
	
	void Combine(Collider other)
	{
		if(other.gameObject.tag == "Loot")
		{			
			for(int i = 0; i < ReactsWith.Length; i++)
			{
				if(other.gameObject.name == ReactsWith[i].name && 
					gameObject.GetInstanceID() < other.gameObject.GetInstanceID() && //makes sure only one of these objects spawns the new one and does the destroying
					(CanCombine || other.GetComponent<LootManager>().CanCombine)) 
				{
					GameObject newLoot = (GameObject)Instantiate(Creates[i], transform.position, transform.rotation);
					newLoot.name = Creates[i].name;
					newLoot.rigidbody.velocity = new Vector3(Random.Range(-2, 2), 2, Random.Range(-2, 2));
					
					//destroy other if need be
					for(int j = 0; j < other.GetComponent<LootManager>().DestroyOnCombine.Length; j++)
					{
						if(other.GetComponent<LootManager>().ReactsWith[j].name == gameObject.name)
						{
							if(other.GetComponent<LootManager>().DestroyOnCombine[j])
							{
								Destroy(other.gameObject);
							}
							break;
						}
					}
					
					if(DestroyOnCombine[i])
					{
						Destroy(gameObject);
						Debug.Log("Destroying Self");
					}
					return;
				}
			}
		}
	}
	
	/*
	void Pickup(Collider other)
	{
		
		if(other.gameObject.tag == "Player" && PickUp)
		{
			InventoryManager myInvManager = other.gameObject.GetComponent<InventoryManager>();
			//pick this item up and add it to their invintory
			//look for existing item stack
			for(int i = 0; i < myInvManager.ItemSlots; i++)
			{
				if(myInvManager.SlotObj[i] != null && 
					myInvManager.SlotObj[i].name == gameObject.name)
				{
					myInvManager.SlotQt[i] += 1;
					Debug.Log("Fuck i destroyed on object");
					Destroy(gameObject);
					return;
				}
			}
			//look for empty slot
			for(int i = 0; i < myInvManager.ItemSlots; i++)
			{
				if(myInvManager.SlotObj[i] == null)
				{
					myInvManager.SlotObj[i] = gameObject;
					myInvManager.SlotQt[i] = 1;
					//gameObject.SetActive(false);
					GetComponentInChildren<MeshRenderer>().enabled = false;
					mySphereCollider.enabled = false;
					GetComponent<BoxCollider>().enabled = false;
					this.enabled = false;
					return;
				}
			}
		}
	}
	*/
}
