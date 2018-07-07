using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour {

	public string objectName;
	public string color;
	public List<string> categories = new List<string>();
	public int x_pos;
	public int z_pos;

	// constructor for testing
	public ObjectData(string name, string color, List<string> categories, int x_pos, int z_pos) {
		this.objectName = name;
		this.color = color;
		this.categories = categories;
		this.x_pos = x_pos;
		this.z_pos = z_pos;
	}

	// Use this for initialization
	void Start () {
		x_pos = (int) gameObject.transform.position.x;
		z_pos = (int) gameObject.transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
		x_pos = (int)gameObject.transform.position.x;
		z_pos = (int)gameObject.transform.position.z;
	}

	private void OnTriggerEnter(Collider colObject){
		ObjectScanner test = (ObjectScanner) colObject.gameObject.GetComponent ("ObjectScanner");
		if (test != null) {
			test.learnObject (this);
		}
	}
}
