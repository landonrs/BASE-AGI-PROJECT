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

	public string AnalyzeWhereQuery(string sentence){
		string[] words = sentence.Split ();
		foreach (string word in words){

		}
		return null;
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
}
