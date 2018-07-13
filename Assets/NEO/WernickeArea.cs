using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;

public class WernickeArea {

	//private static NeoMemory memory;
	private static WernickeArea instance;
	private string connectionString;

	private WernickeArea(){ connectionString = "URI=file:" + Application.dataPath + "/neo_brain.db";}

	public static WernickeArea getInstance(){
		if (instance == null) {
			instance = new WernickeArea ();
			//memory = NeoMemory.getInstance ();
		}
		return instance;

	}

	public string AnalyzeSentence(string sentence) {
		string response = "";
		string verb = "";
		string[] words = sentence.Split ();
		foreach (string word in words) {
			if (word.ToLower ().Equals ("where")) {
				response = AnalyzeWhereQuery (sentence);
				return response;
			}
			if (word.ToLower ().Equals ("what")) {
				response = AnalyzeWhatQuery (sentence);
				return response;
			}
		}

		return null;
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
		using (IDbConnection dbConnection = new SqliteConnection (connectionString)) {
			dbConnection.Open ();

			using (IDbCommand dbCommand = dbConnection.CreateCommand ()) {
				locationId = DBUtils.getId (DBUtils.OBJECTS_LOCATION, DBUtils.OBJECTS_TABLE, DBUtils.OBJECTS_COL, objectName, dbCommand);
				locationName = DBUtils.getValueFromId (locationId, DBUtils.LOCATIONS_ID, DBUtils.LOCATIONS_TABLE, DBUtils.LOCATIONS_COL, dbCommand);
			}
			dbConnection.Close ();
		}
				
		return locationName;
	}

	public string DefinePersonLocation(string personName){
		int locationId = 0;
		string locationName = "";
		using (IDbConnection dbConnection = new SqliteConnection (connectionString)) {
			dbConnection.Open ();

			using (IDbCommand dbCommand = dbConnection.CreateCommand ()) {
				locationId = DBUtils.getId (DBUtils.PERSON_LOCATION, DBUtils.PEOPLE_TABLE, DBUtils.PEOPLE_COL, personName, dbCommand);
				locationName = DBUtils.getValueFromId (locationId, DBUtils.LOCATIONS_ID, DBUtils.LOCATIONS_TABLE, DBUtils.LOCATIONS_COL, dbCommand);
			}
			dbConnection.Close ();
		}

		return locationName;
	}

	public string DefineObjectAttribute(string objectName, string attribute){
		int attributeId = 0;
		int objectId = 0;
		List<string> attributeValue = null;
		using (IDbConnection dbConnection = new SqliteConnection (connectionString)) {
			dbConnection.Open ();

			using (IDbCommand dbCommand = dbConnection.CreateCommand ()) {
				attributeId = DBUtils.getId (DBUtils.ATTRIBUTE_ID, DBUtils.ATTRIBUTES_TABLE, DBUtils.ATTRIBUTE_COL, attribute, dbCommand);
				objectId = DBUtils.getId(DBUtils.OBJECTS_ID, DBUtils.OBJECTS_TABLE, DBUtils.OBJECTS_COL, objectName, dbCommand);
				attributeValue = DBUtils.getObjectAttribute (objectId, attributeId, dbCommand);
			}
			dbConnection.Close ();
		}

		return attributeValue[0];
	}
}
