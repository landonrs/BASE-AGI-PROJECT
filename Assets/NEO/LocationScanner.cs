using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationScanner : MonoBehaviour {

	public string currentLocation;
	NeoMemory neoBrain;
	public EntityData person;

	// Use this for initialization
	void Start () {
		neoBrain = NeoMemory.getInstance();
		person = (EntityData) gameObject.GetComponent ("EntityData");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void NameLocation(string room){
		if (!room.Equals(this.currentLocation)) {
			//Debug.Log("I just entered the: " + room); 
			this.currentLocation = room;
		}
	}

	public void LearnLocation(LocationData data){
		neoBrain.MemorizeLocation (data);
		person.entityLocation = data.locationName;
	}

	public void UpdateCurrentLocation(LocationData data){
		neoBrain.UpdatePersonLocation (person.entityName, data.locationName);
			
	}
}
