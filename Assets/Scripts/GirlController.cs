using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlController : MonoBehaviour {

	private InstantTrackingController trackerScript;
	private GameObject ButtonsParent;
	
	void Start () 
	{
		trackerScript = GameObject.Find("Controller").gameObject.GetComponent<InstantTrackingController>();
		ButtonsParent = GameObject.Find("Buttons Parent");
		trackerScript._gridRenderer.enabled = false;
		ButtonsParent.SetActive(false);
	}
	void OnEnable()
	{
		trackerScript._gridRenderer.enabled = false;
		ButtonsParent.SetActive(false);
	}
	void OnDisable()
	{
		ButtonsParent.SetActive(true);
	}
	
	void Update()
	{

	}
	
}
