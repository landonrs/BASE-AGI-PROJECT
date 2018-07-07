using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationData : MonoBehaviour {

	public string locationName;
	public int centerX;
	public int centerZ;

	//constructor for testing
	public LocationData(string name, int centerX, int centerZ){
		this.locationName = name;
		this.centerX = centerX;
		this.centerZ = centerZ;
	}

	// Use this for initialization
	void Start () {
		centerX = (int) gameObject.transform.position.x;
		centerZ = (int) gameObject.transform.position.z;
		Debug.Log (locationName + " x:" + centerX + ", z:" + centerZ);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider colObject){
		LocationScanner locationScanner = (LocationScanner) colObject.gameObject.GetComponent ("LocationScanner");
		if (locationScanner != null) {
			locationScanner.NameLocation (locationName);
			locationScanner.LearnLocation (this);
		}

	}
		
}
