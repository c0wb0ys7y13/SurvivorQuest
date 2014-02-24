using UnityEngine;
using System.Collections;

public class CameraFadeIn : MonoBehaviour 
{
	//delay the fade in
	public float DelayFadeInFor;
	//fade in durration
	public float FadeInOverSeconds;
	private MeshRenderer myMeshRenderer;
	private Material myMaterial;
	
	
	
	// Use this for initialization
	void Start () 
	{
		myMeshRenderer = GetComponent<MeshRenderer>();
		myMaterial = myMeshRenderer.material;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Time.time > DelayFadeInFor)
		{
			myMaterial.color = myMaterial.color - new Color(0, 0, 0, Time.deltaTime / FadeInOverSeconds);
		}
		
		if(myMaterial.color.a <= 0)
			GameObject.Destroy(gameObject);
	}
}
