﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;

public class WernickeArea {

	//private static NeoMemory memory;
	private static WernickeArea instance;
	private GameObject Neo;

	private WernickeArea(){ 
		Neo = GameObject.FindGameObjectWithTag ("NEO");
	}

	public static WernickeArea getInstance(){
		if (instance == null) {
			instance = new WernickeArea ();
			//memory = NeoMemory.getInstance ();
		}
		return instance;

	}
		
	public string AnalyzeSentence(string sentence) {
		string response = "";
		//string verb = "";
		string[] words = sentence.Split ();
		foreach (string word in words) {
			if (word.ToLower ().Equals ("where")) {
				response = AnalyzeWhereQuery (sentence);
				//Debug.Log (response);
				return response;
			}
			if (word.ToLower ().Equals ("what")) {
				response = AnalyzeWhatQuery (sentence);
				//Debug.Log (response);
				return response;
			}
			if (word.ToLower ().Equals ("go")) {
				Vector2 targetLocation = getTargetLocation (sentence); 
				Neo.GetComponent<Navigator>().moveToLocation(targetLocation);
				//Debug.Log (response);
				return "Moving to location";
			}
		}

		return null;
	}

	public Vector2 getTargetLocation(string sentence) {
		string target = "";
		Vector2 targetLocation = new Vector2();
		string[] words = sentence.Split ();
				foreach (string word in words) {
					//check if word is an object
					if(WordIsObject(word)) {
						target = word;
						targetLocation = DBUtils.getObjectCoordinates (target);
					}
					else if (WordIsLocation (word)) {
						targetLocation = DBUtils.getLocationCoordinates (word);
						break;
					}
				}
		return targetLocation;
	}

	public string AnalyzeWhereQuery(string sentence) {
		string location = "";
		string[] words = sentence.Split ();
		foreach (string word in words){
			//check if word is an object
			if(DBUtils.ItemExistsInMemory(word, DBUtils.OBJECTS_COL, DBUtils.OBJECTS_TABLE)) {
				location = DefineObjectLocation(word);
			}
			else if(DBUtils.ItemExistsInMemory(word, DBUtils.PEOPLE_COL, DBUtils.PEOPLE_TABLE)) {
				location = DefinePersonLocation(word);
			}
		}
		return location;
	}

	public string AnalyzeWhatQuery(string sentence) {
		string objectName = null;
		string attribute = null;
		string response = null; 
		string[] words = sentence.Split ();
		foreach (string word in words){
			//check if word is an object
			if(DBUtils.ItemExistsInMemory(word.ToLower(), DBUtils.ATTRIBUTE_COL, DBUtils.ATTRIBUTES_TABLE)) {
				attribute = word;
			}
			else if (DBUtils.ItemExistsInMemory (word.ToLower (), DBUtils.OBJECTS_COL, DBUtils.OBJECTS_TABLE)) {
				objectName = word;
			}
		}

		response = DefineObjectAttribute (objectName, attribute);
		return response;
	}

	public string DefineObjectLocation(string objectName){
		int locationId = 0;
		string locationName = "";
		locationId = DBUtils.getId (DBUtils.OBJECTS_LOCATION, DBUtils.OBJECTS_TABLE, DBUtils.OBJECTS_COL, objectName);
		locationName = DBUtils.getValueFromId (locationId, DBUtils.LOCATIONS_ID, DBUtils.LOCATIONS_TABLE, DBUtils.LOCATIONS_COL);
				
		return locationName;
	}

	public string DefinePersonLocation(string personName){
		int locationId = 0;
		string locationName = "";
		locationId = DBUtils.getId (DBUtils.PERSON_LOCATION, DBUtils.PEOPLE_TABLE, DBUtils.PEOPLE_COL, personName);
		locationName = DBUtils.getValueFromId (locationId, DBUtils.LOCATIONS_ID, DBUtils.LOCATIONS_TABLE, DBUtils.LOCATIONS_COL);

		return locationName;
	}

	public string DefineObjectAttribute(string objectName, string attribute){
		int attributeId = 0;
		int objectId = 0;
		List<string> attributeValue = null;
		attributeId = DBUtils.getId (DBUtils.ATTRIBUTE_ID, DBUtils.ATTRIBUTES_TABLE, DBUtils.ATTRIBUTE_COL, attribute);
		objectId = DBUtils.getId(DBUtils.OBJECTS_ID, DBUtils.OBJECTS_TABLE, DBUtils.OBJECTS_COL, objectName);
		attributeValue = DBUtils.getObjectAttribute (objectId, attributeId);

		return attributeValue[0];
	}

	private bool WordIsObject(string word) {
		if (DBUtils.ItemExistsInMemory (word.ToLower (), DBUtils.OBJECTS_COL, DBUtils.OBJECTS_TABLE)) {
			return true;
		}

		return false;
	}

	private bool WordIsAttribute(string word) {
		if(DBUtils.ItemExistsInMemory(word.ToLower(), DBUtils.ATTRIBUTE_COL, DBUtils.ATTRIBUTES_TABLE)) {
			return true;
		}

		return false;
	}

	private bool WordIsLocation(string word) {
		if(DBUtils.ItemExistsInMemory(word.ToLower(), DBUtils.LOCATIONS_COL, DBUtils.LOCATIONS_TABLE)) {
			return true;
		}

		return false;
	}

	private bool WordIsAdjective(string word) {
		if(DBUtils.ItemExistsInMemory(word.ToLower(), DBUtils.ADJECTIVES_COL, DBUtils.ADJECTIVES_TABLE)) {
			return true;
		}

		return false;
	}
}
