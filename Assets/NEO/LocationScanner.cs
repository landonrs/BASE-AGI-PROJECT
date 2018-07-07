using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationScanner : MonoBehaviour {

	public string currentLocation;
	NeoMemory neoBrain;

	// Use this for initialization
	void Start () {
		neoBrain = NeoMemory.getInstance();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void NameLocation(string room){
		Debug.Log("I just entered the: " + room); 
		this.currentLocation = room;
	}

	public void LearnLocation(LocationData data){
		neoBrain.MemorizeLocation (data);
	}
}
