using UnityEngine;
using System.Collections;

public class PlowController : MonoBehaviour 
{
	public float PlowSpeed = 1;
	public GameObject TilledSoilPrefab;
	public float SoilScaleRate = 0.1f;
	private GameObject CurrentSoilPrefab;
	public GameObject PlowPuller;
	public GameObject Chain;
	public GameObject ChainAttachPoint;
	public float ChainDistanceScale;
	public float CollisionDetectionRange;
		
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//while isa is linked to the plow, keep her chained
		if(PlowPuller != null)
		{
			if(Chain.GetComponent<MeshRenderer>().enabled == false)
				Chain.GetComponent<MeshRenderer>().enabled = true;
			
			//set the position and length of the chain
			Chain.transform.localPosition = ((PlowPuller.transform.position + ChainAttachPoint.transform.localPosition) - transform.position) / 2;
			Chain.transform.localScale = new Vector3((PlowPuller.transform.position - ChainAttachPoint.transform.position).magnitude * ChainDistanceScale, Chain.transform.localScale.y, Chain.transform.localScale.z);
			
			//Set the angle of the chain
			Chain.transform.LookAt(PlowPuller.transform.position);
			Chain.transform.RotateAround(Chain.transform.position, Chain.transform.up, 90f);
		}
		else
		{
			Chain.GetComponent<MeshRenderer>().enabled = false;
		}
	}
	
	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "Player" && PlowPuller != null && collider.bounds.Contains(PlowPuller.transform.position))
		{
			transform.position += Vector3.left * Time.deltaTime * PlowSpeed;
			
			//as the plow moves, create tilled soil
			if(CurrentSoilPrefab == null)
			{
				CurrentSoilPrefab = (GameObject)Instantiate(TilledSoilPrefab, transform.position, transform.rotation);
				CurrentSoilPrefab.transform.localScale = new Vector3(0, 1, 1);
				CurrentSoilPrefab.transform.localScale = CurrentSoilPrefab.transform.localScale + (Vector3.right * Time.deltaTime * PlowSpeed * SoilScaleRate);
				CurrentSoilPrefab.transform.position += (Vector3.left * Time.deltaTime * PlowSpeed) / 2;
			}
			else if(CurrentSoilPrefab.transform.localScale.x < TilledSoilPrefab.transform.localScale.x)
			{
				CurrentSoilPrefab.transform.localScale = CurrentSoilPrefab.transform.localScale + (Vector3.right * Time.deltaTime * PlowSpeed * SoilScaleRate);
				CurrentSoilPrefab.transform.position += (Vector3.left * Time.deltaTime * PlowSpeed) / 2;
			}
			else
			{
				CurrentSoilPrefab = null;
			}
		}
	}
}
