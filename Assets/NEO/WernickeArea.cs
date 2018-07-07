using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;

public class WernickeArea {

	private static NeoMemory memory;
	private static WernickeArea instance;
	private string connectionString;

	private WernickeArea(){ connectionString = "URI=file:" + Application.dataPath + "/neo_brain.db";}

	public static WernickeArea getInstance(){
		if (instance == null) {
			instance = new WernickeArea ();
			memory = NeoMemory.getInstance ();
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
		using (IDbConnection dbConnection = new SqliteConnection (connectionString)) {
			dbConnection.Open ();

			using (IDbCommand dbCommand = dbConnection.CreateCommand ()) {
				string sql = "SELECT " + DBUtils.OBJECTS_LOCATION +
				" FROM " + DBUtils.OBJECTS_TABLE +
				" WHERE " + DBUtils.OBJECTS_COL + " = @name";
			}
		}
				
		return null;
	}
}
