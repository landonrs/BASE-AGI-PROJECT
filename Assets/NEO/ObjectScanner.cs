using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScanner : MonoBehaviour {

	NeoMemory NeoBrain;
	LocationScanner locator;

	// Use this for initialization
	void Start () {
		NeoBrain = NeoMemory.getInstance();
		locator = (LocationScanner) gameObject.GetComponent ("LocationScanner");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void learnObject(ObjectData data){
		Debug.Log("I just collided with the " + data.objectName);
		string objectLocation = locator.currentLocation;
		NeoBrain.memorizeObject (data, objectLocation);

	}
}
