using UnityEngine;
using System.Collections;

public class InventoryManager : MonoBehaviour 
{
	public int ItemSlots = 10;
	public int SelectedSlot = 0;
	public GameObject[] SlotObj;
	public int[] SlotQt;
	private rpgCharController MyRpgCharController;
	public Texture HudItemBoarder;
	public float ThrowDistance;
	[HideInInspector] public Collider CurrentLootTarget;
	public float PickUpDistance = 2;
	public float PickUpDurration = 0.5f;
	protected GameObject LastDroppedLoot = null;
	
	// Use this for initialization
	void Start () 
	{
		SlotObj = new GameObject[ItemSlots];
		SlotQt = new int[ItemSlots];
		MyRpgCharController = GetComponent<rpgCharController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		ToggleSelectedItem();
		
		PickUpLoot();
	}
	
	void PickUpLoot()
	{
		if(Input.GetKeyDown(KeyCode.E) && CurrentLootTarget != null && Vector3.Distance(CurrentLootTarget.transform.position, transform.position) <= PickUpDistance && GetComponent<PlayerStateMachine>().MyPlayerState == PlayerStateMachine.PlayerState.None &&
			(GetComponent<PlayerHarvistManger>().CurrentHarvestPoint == null  || GetComponent<PlayerHarvistManger>().CurrentHarvestPoint.GetComponent<HarvestPoint>().CurrentHarvists == 0 || Vector3.Distance(GetComponent<PlayerHarvistManger>().CurrentHarvestPoint.transform.position, transform.position) > GetComponent<PlayerHarvistManger>().HarvestRange))
		{						
			LootManager CurLootManager = CurrentLootTarget.GetComponent<LootManager>();
			if(CurLootManager.PickUp)
			{
				//pick this item up and add it to their invintory
				//look for existing item stack
				for(int i = 0; i < ItemSlots; i++)
				{
					if(SlotObj[i] != null && 
						SlotObj[i].name == CurrentLootTarget.gameObject.name)
					{
						GetComponent<PlayerStateMachine>().UpdateState(PlayerStateMachine.PlayerState.PickingUp, Time.time + PickUpDurration);
						//GetComponent<PlayerStateMachine>().MyPlayerState = PlayerStateMachine.PlayerState.PickingUp;
						//GetComponent<PlayerStateMachine>().InStateTill = Time.time + PickUpDurration;
						SlotQt[i] += 1;
						Destroy(CurrentLootTarget.gameObject);
						return;
					}
				}
				//look for empty slot
				for(int i = 0; i < ItemSlots; i++)
				{
					if(SlotObj[i] == null)
					{
						GetComponent<PlayerStateMachine>().UpdateState(PlayerStateMachine.PlayerState.PickingUp, Time.time + PickUpDurration);
						//GetComponent<PlayerStateMachine>().MyPlayerState = PlayerStateMachine.PlayerState.PickingUp;
						//GetComponent<PlayerStateMachine>().InStateTill = Time.time + PickUpDurration;
						SlotObj[i] = CurrentLootTarget.gameObject;
						SlotQt[i] = 1;
						//gameObject.SetActive(false);
						CurrentLootTarget.GetComponentInChildren<MeshRenderer>().enabled = false;
						CurrentLootTarget.GetComponent<SphereCollider>().enabled = false;
						CurrentLootTarget.GetComponent<BoxCollider>().enabled = false;
						CurLootManager.enabled = false;
						return;
					}
				}
			}
		}
	}
	
	void ToggleSelectedItem()
	{
		//toggle selected item
		if(Input.GetKeyDown(KeyCode.Equals))
		{
			SelectedSlot++;
			if(SelectedSlot >= ItemSlots)
				SelectedSlot = 0;
		}
		else if(Input.GetKeyDown(KeyCode.Minus))
		{
			SelectedSlot--;
			if(SelectedSlot < 0)
				SelectedSlot = ItemSlots - 1;
		}
		
		//press G to throw an item on the ground
		if(Input.GetKeyDown(KeyCode.G) && GetComponent<PlayerStateMachine>().MyPlayerState == PlayerStateMachine.PlayerState.None)
		{
			//confirm our selected inv slot has something in it
			if(SlotObj[SelectedSlot] != null && SlotQt[SelectedSlot] > 0)
			{
				//throw it the right direction
				Vector3 SpawnDir = transform.position;
				Vector3 ThrowDir = new Vector3(0,0,0);
				if(MyRpgCharController.MyFacing == rpgCharController.Facing.Left)
				{
					SpawnDir += new Vector3(ThrowDistance + SlotObj[SelectedSlot].GetComponent<SphereCollider>().radius, 1f, 0);
					ThrowDir += new Vector3(1, -1, 0);
				}
				else
				{
					SpawnDir += new Vector3((ThrowDistance + SlotObj[SelectedSlot].GetComponent<SphereCollider>().radius) * -1, 1f, 0);
					ThrowDir += new Vector3(-1, -1, 0);
				}
				//instantiate the object
				GameObject newLoot = (GameObject)Instantiate(SlotObj[SelectedSlot], SpawnDir, transform.rotation);
				newLoot.rigidbody.velocity = ThrowDir + rigidbody.velocity;
				//remove the (clone) from the name
				newLoot.name = SlotObj[SelectedSlot].name;
				//make sure we re-enable stuff we may have disabled
				newLoot.GetComponentInChildren<MeshRenderer>().enabled = true;
				newLoot.GetComponent<SphereCollider>().enabled = true;
				newLoot.GetComponent<BoxCollider>().enabled = true;
				newLoot.GetComponent<LootManager>().enabled = true;
				newLoot.GetComponent<LootManager>().CanCombine = true;
				//turn off combining from the last thing we threw
				if(LastDroppedLoot != null)
					LastDroppedLoot.GetComponent<LootManager>().CanCombine = false;
				//set this new object as my last dropped loot
				LastDroppedLoot = newLoot;
				
				//remove one from our invintory
				SlotQt[SelectedSlot]--;
				if(SlotQt[SelectedSlot] <= 0)
				{
					Destroy(SlotObj[SelectedSlot]);
					SlotObj[SelectedSlot] = null;
					SlotQt[SelectedSlot] = 0;
				}
			}
		}
	}
	
	void OnGUI()
	{
		GUIStyle MyGUIStyle = new GUIStyle();
		
		for(int i = 0; i < ItemSlots; i++)
		{
			
			if(SlotObj[i] != null)
				GUI.Box(new Rect(0 + i * 64, 0, 64, 64), SlotObj[i].GetComponentInChildren<MeshRenderer>().material.mainTexture, MyGUIStyle);
			if(SlotQt[i] > 1)
			{
				GUI.Box(new Rect(0 + i * 64, 0, 32, 32), SlotQt[i].ToString(), MyGUIStyle);	
			}
			if(SelectedSlot == i)
				GUI.Box(new Rect(0 + i * 64, 0, 64, 64), HudItemBoarder, MyGUIStyle);
		}
	}
	
	void OnTriggerStay(Collider other)
	{
		if(other.tag == "Loot")
		{
			CurrentLootTarget = other;	
		}
	}
}
