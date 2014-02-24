using UnityEngine;
using System.Collections;

public class ScrollUpwards : MonoBehaviour 
{
	public float scrollSpeed = 1f;
	public float FadeInStart = -5f;
	public float FadeInEnd = -1f;
	public float FadeOutStart = 1f;
	public float FadeOutEnd = 5f;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position += new Vector3(0, scrollSpeed * Time.deltaTime, 0);
		
		if(transform.position.y >= FadeInStart && transform.position.y <= FadeInEnd)
		{
			GetComponent<TextMesh>().color = new Color(1, 1, 1, (((FadeInEnd - FadeInStart) - (Mathf.Abs(transform.position.y) - FadeInEnd)) / (FadeInEnd - FadeInStart)) * 2);
		}
		else if(transform.position.y > FadeInEnd && transform.position.y < FadeOutStart)
		{
			GetComponent<TextMesh>().color = Color.white;
		}
		else if(transform.position.y >= FadeOutStart && transform.position.y <= FadeOutEnd)
		{
			GetComponent<TextMesh>().color = new Color(1, 1, 1, ((FadeOutEnd - FadeOutStart) - (Mathf.Abs(transform.position.y) - FadeOutStart)) / (FadeOutEnd - FadeOutStart));
		}
	}
}
