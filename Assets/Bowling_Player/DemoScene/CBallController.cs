using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBallController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	public void ballDisappear(){
		gameObject.SetActive(false);
	}
	
	public void ballAppear(){
		gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
