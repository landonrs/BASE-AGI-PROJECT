using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityData : MonoBehaviour {

	public string entityName;
	public string entityLocation;
	public List<string> entityObjects;

	// Use this for initialization
	void Start () {
		//this should come from the gameobject
		entityName = "you";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
