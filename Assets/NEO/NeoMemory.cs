using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;


// This script gives an example of how to access a sqlite db in a unity script
public class NeoMemory {
	private const string OBJECTS_TABLE = "OBJECTS";
	private const string OBJECTS_ID = "OBJECT_ID";
	private const string OBJECTS_COL = "OBJECT_NAME";
	private const string OBJECTS_LOCATION = "OBJECT_LOCATION";
	private const string ADJECTIVES_TABLE = "ADJECTIVES";
	private const string ADJECTIVES_ID = "ADJECTIVE_ID";
	private const string ADJECTIVES_COL = "ADJECTIVE_NAME";
	private const string CATEGORIES_TABLE = "CATEGORIES";
	private const string CATEGORIES_ID = "CATEGORY_ID";
	private const string CATEGORIES_COL = "CATEGORY_NAME";
	private const string LOCATIONS_TABLE = "LOCATIONS";
	private const string LOCATIONS_ID = "LOCATION_ID";
	private const string LOCATIONS_COL = "LOCATION_NAME";
	private const string ATTRIBUTES_TABLE = "ATTRIBUTES";
	private const string ATTRIBUTE_ID = "ATTRIBUTE_ID";
	private const string ATTRIBUTE_COL = "ATTRIBUTE_NAME";
	private const string ATTRIBUTE_VALUE_COL = "ATTRIBUTE_VALUE";
	private const string OBJECT_X_POS = "object_x_position";
	private const string OBJECT_Z_POS = "object_z_position";
	//linking tables
	private const string ADJECTIVE_TYPE_TABLE = "ADJECTIVE_TYPE";
	private const string OBJECT_DESCRIPTION_TABLE = "OBJECT_DESCRIPTION";
	private const string OBJECT_CATEGORIES_TABLE = "OBJECT_CATEGORIES";
	private const int X_POSITION_ATT_ID = 5;
	private const int Y_POSITION_ATT_ID = 6;
	private const int LOCATION_ATT_ID = 7;

	private string connectionString;
	private static NeoMemory instance;

	public void setconnectionString(string connString){
		this.connectionString = connString;
	}

	public static NeoMemory getInstance(){
		if (instance == null) {
			instance = new NeoMemory ();
		}
		return instance;
	}

	private NeoMemory(){
		connectionString = "URI=file:" + Application.dataPath + "/neo_brain.db";
	}

	// Use this for initialization
	void Awake () {
		
	}

	// Update is called once per frame
	void Update () {

	}

	// retrieve object from DB using select statement
	public List<string> getObjects(){
		List<string> objNames = new List<string>();
		using (IDbConnection dbConnection = new SqliteConnection (connectionString)) {
			dbConnection.Open ();

			using (IDbCommand dbCommand = dbConnection.CreateCommand ()) {

				string sqlQuery = "SELECT * FROM OBJECTS";

				dbCommand.CommandText = sqlQuery;

				using (IDataReader reader = dbCommand.ExecuteReader ()) {

					while (reader.Read ()) {
						objNames.Add (reader.GetString (1));
					}
					dbConnection.Close ();
					reader.Close ();
				}
			}
		}
		return objNames;

	}

	// save the encountered object and all its related metadata to the DB
	public void memorizeObject(ObjectData objMeta, string objLocation) {
		string objName = objMeta.objectName.ToLower();
		string colorName = objMeta.color.ToLower();
		List<string> categories = objMeta.categories;
		objLocation = objLocation.ToLower ();
		using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
			dbConnection.Open ();

			using (IDbCommand dbCommand = dbConnection.CreateCommand ()) {
				// make sure we haven't learned about this particular object before
				if(objectExistsInMemory (objMeta.objectName, dbCommand) == -1)
					{
					// memorize the object and its location
					int location_id = DBUtils.getId (LOCATIONS_ID, LOCATIONS_TABLE, LOCATIONS_COL, objLocation);
					string sqlQuery = "INSERT into OBJECTS("+OBJECTS_COL+", " +OBJECTS_LOCATION+", " + OBJECT_X_POS +", " + OBJECT_Z_POS + ")"
						+ "values(@name, @location_id, @x_pos, @z_pos)";
					dbCommand.Parameters.Add (new SqliteParameter("@name", objName));
					dbCommand.Parameters.Add (new SqliteParameter("@location_id", location_id));
					dbCommand.Parameters.Add (new SqliteParameter("@x_pos", objMeta.x_pos));
					dbCommand.Parameters.Add (new SqliteParameter("@z_pos", objMeta.z_pos));
					dbCommand.CommandText = sqlQuery;
					dbCommand.ExecuteScalar();
					//Debug.Log ("inserted object name into DB");

					// memorize the word of the object's color
					sqlQuery = "INSERT OR IGNORE INTO ADJECTIVES ("+ADJECTIVES_COL+") VALUES (@adj_name)";
					dbCommand.Parameters.Add (new SqliteParameter("@adj_name", colorName));
					dbCommand.CommandText = sqlQuery;
					dbCommand.ExecuteScalar();
					//Debug.Log ("inserted adjective name into DB");

					// get the primary key of the object
					sqlQuery = "SELECT OBJECT_ID FROM OBJECTS WHERE OBJECT_NAME = @filter";
					dbCommand.Parameters.Add(new SqliteParameter("@filter", objName));
					dbCommand.CommandText = sqlQuery;
					int objId = DBUtils.readId (dbCommand);

					AttachColorToObject (colorName, objId, dbCommand);

					// insert the categories
					foreach (string category in categories) {
						sqlQuery = "INSERT OR IGNORE INTO CATEGORIES (category_name) VALUES (@cat_name)";
						dbCommand.Parameters.Add (new SqliteParameter ("@cat_name", category));
						dbCommand.CommandText = sqlQuery;
						dbCommand.ExecuteScalar ();

						//get category id
						int cat_id = DBUtils.getId (CATEGORIES_ID, CATEGORIES_TABLE, CATEGORIES_COL, category);
						//Debug.Log ("cat_id: " + cat_id);

						//associate the object with the category
						sqlQuery = "INSERT OR IGNORE INTO OBJECT_CATEGORIES (object_id, category_id) VALUES (@obj_id, @cat_id)";
						dbCommand.Parameters.Add (new SqliteParameter ("@cat_id", cat_id));
						dbCommand.Parameters.Add (new SqliteParameter ("@obj_id", objId));
						dbCommand.CommandText = sqlQuery;
						dbCommand.ExecuteScalar ();
					}

					//Debug.Log ("object memorized");
				}

				dbConnection.Close ();
			}

		}
	}

	private void AttachColorToObject(string colorName, int objId, IDbCommand dbCommand) {
		// get the primary key of the color attribute
		int att_id = DBUtils.getId (ATTRIBUTE_ID, ATTRIBUTES_TABLE, ATTRIBUTE_COL, "color");
		//Debug.Log ("att_id: " + att_id);

		// get the primary key of the color
		int adj_id = DBUtils.getId(ADJECTIVES_ID, ADJECTIVES_TABLE, ADJECTIVES_COL, colorName);
		//Debug.Log ("adj_id: " + adj_id);

		// associate the name of the color to the attribute "color"
		//TODO prevent repeating values from being saved.
		string sqlQuery = "INSERT OR IGNORE INTO " + ADJECTIVE_TYPE_TABLE + " (adjective_id, attribute_id) VALUES (@adj_id, @att_id)";
		dbCommand.Parameters.Add (new SqliteParameter("@adj_id", adj_id));
		dbCommand.Parameters.Add (new SqliteParameter("@att_id", att_id));
		dbCommand.CommandText = sqlQuery;
		dbCommand.ExecuteScalar();

		// associate the color with the object
		sqlQuery = "INSERT INTO " + OBJECT_DESCRIPTION_TABLE + " (attribute_id, object_id, attribute_value) VALUES (@att_id, @obj_id, @adj_name)";
		dbCommand.Parameters.Add (new SqliteParameter("@obj_id", objId));
		dbCommand.Parameters.Add (new SqliteParameter("@adj_name", colorName));
		dbCommand.CommandText = sqlQuery;
		dbCommand.ExecuteScalar();
	}
		

	private List<int> ReadIntResults(IDbCommand dbCommand){
		List<int> results = new List<int> ();
		using (IDataReader reader = dbCommand.ExecuteReader ()) {

			while (reader.Read ()) {
				//Debug.Log ("id: " + reader.GetInt32 (0));
				results.Add(reader.GetInt32(0));
			}
			reader.Close ();
			return  results;
		}
	}


	private int objectExistsInMemory(string name, IDbCommand dbCommand){
		dbCommand.CommandText = 
			"SELECT " + OBJECTS_ID + 
			" FROM OBJECTS WHERE OBJECT_NAME = @obj_name"; 
		dbCommand.Parameters.Add (new SqliteParameter ("@obj_name", name));
		int result = DBUtils.readId (dbCommand);
		if (result != -1) {
			Debug.Log ("result found in DB: " + result);
		}

		return result;
	}

	public List<string> getTableNames(){
		List<string> tables = null;
		using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
			dbConnection.Open ();

			using (IDbCommand dbCommand = dbConnection.CreateCommand ()) {
				string sqlQuery = "SELECT name FROM sqlite_master WHERE type = 'table' ORDER BY 1";
				dbCommand.CommandText = sqlQuery;
				tables = DBUtils.readStringResults (dbCommand);

			}
			dbConnection.Close ();
			return tables;
		}

	}

	// for updates and deletions
	public void runCommand(string sql) {
		using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
			dbConnection.Open ();

			using (IDbCommand dbCommand = dbConnection.CreateCommand ()) {
				dbCommand.CommandText = sql;
				dbCommand.ExecuteNonQuery ();

			}
			dbConnection.Close ();
		}
	}

	public bool ItemExistsInMemory(string item, string colName, string table) {
		List<string> results = null;
		List<int> idResults = null;
		int valueID = -1;
		bool resultsFound = false;

		using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
			dbConnection.Open ();

			using (IDbCommand dbCommand = dbConnection.CreateCommand ()) {
				string command = "SELECT " + colName + " FROM " + table + " WHERE " + colName + " = @item";
				// if the item is a numeric id, pass it in as the parameter
				if (int.TryParse (item, out valueID)) {
					dbCommand.Parameters.Add(new SqliteParameter("@item", valueID));
				} else {
					dbCommand.Parameters.Add(new SqliteParameter("@item", item));
				}

				dbCommand.CommandText = command;
				if (valueID == 0) {
					results = DBUtils.readStringResults (dbCommand);
					if (results.Count != 0) {
						resultsFound = true;
					}
				} else {
					idResults = ReadIntResults (dbCommand);
					if (idResults.Count != 0) {
						resultsFound = true;
					}

				}
			}
			dbConnection.Close ();
		}

		return resultsFound;
	}

	public List<string> getObjectCategories(string objName){
		List<string> categoryNames = new List<string>();
		using (IDbConnection dbConnection = new SqliteConnection (connectionString)) {
			dbConnection.Open ();

			using (IDbCommand dbCommand = dbConnection.CreateCommand ()) {

				//first get the id of the object
				int objId = objectExistsInMemory(objName, dbCommand);
				if (objId == -1) {
					return null;
				}

				string sqlQuery = "SELECT " + CATEGORIES_COL + " FROM " + CATEGORIES_TABLE + " c JOIN OBJECT_CATEGORIES oc ON c." + CATEGORIES_ID +
					" = oc." + CATEGORIES_ID + " JOIN " + OBJECTS_TABLE + " o ON oc." + OBJECTS_ID + " = o." + OBJECTS_ID + " WHERE o." + OBJECTS_ID + " = @objId";
				dbCommand.Parameters.Add(new SqliteParameter("@objId", objId));
				dbCommand.CommandText = sqlQuery;

				categoryNames = DBUtils.readStringResults (dbCommand);
			}
			dbConnection.Close ();
		}
		return categoryNames;	
	}

	public Dictionary<string,string> getObjectDescription(string objName) {
		Dictionary<string, string> objectDescription = null;
		using (IDbConnection dbConnection = new SqliteConnection (connectionString)) {
			dbConnection.Open ();

			using (IDbCommand dbCommand = dbConnection.CreateCommand ()) {

				//first get the id of the object
				int objId = objectExistsInMemory (objName, dbCommand);
				if (objId == -1) {
					return null;
				}

				string sqlQuery = "SELECT " + ATTRIBUTE_COL + ", " + ATTRIBUTE_VALUE_COL +
					" FROM " + ATTRIBUTES_TABLE + " att JOIN " + OBJECT_DESCRIPTION_TABLE + " od ON att." + ATTRIBUTE_ID + " = od." + ATTRIBUTE_ID +
					" JOIN " + OBJECTS_TABLE + " o ON od." + OBJECTS_ID + " = o." + OBJECTS_ID + " WHERE o." + OBJECTS_ID + " = @objId";
				dbCommand.Parameters.Add(new SqliteParameter("@objId", objId));
				dbCommand.CommandText = sqlQuery;
				objectDescription = DBUtils.getStringPairings (dbCommand);
			}
			dbConnection.Close ();
			return objectDescription;
		}
	}
		
	public void MemorizeLocation(LocationData data){
		string locName = data.locationName.ToLower ();
	
		using (IDbConnection dbConnection = new SqliteConnection (connectionString)) {
			dbConnection.Open ();

			using (IDbCommand dbCommand = dbConnection.CreateCommand ()) {

				if (!ItemExistsInMemory(locName, LOCATIONS_COL, LOCATIONS_TABLE)) {

					string sql = "INSERT INTO LOCATIONS (LOCATION_NAME, LOCATION_X, LOCATION_Y) VALUES (@name, @x_center, @y_center)";
					dbCommand.Parameters.Add (new SqliteParameter ("@name", data.locationName));
					dbCommand.Parameters.Add (new SqliteParameter ("@x_center", data.centerX));
					dbCommand.Parameters.Add (new SqliteParameter ("@y_center", data.centerZ));
					dbCommand.CommandText = sql;
					dbCommand.ExecuteScalar ();
				}
			}
		}
	}

	public void UpdatePersonLocation(string personName, string location) {
		location = location.ToLower ();
		personName = personName.ToLower ();
		using (IDbConnection dbConnection = new SqliteConnection (connectionString)) {
			dbConnection.Open ();

			using (IDbCommand dbCommand = dbConnection.CreateCommand ()) {
				int location_id = DBUtils.getId (LOCATIONS_ID, LOCATIONS_TABLE, LOCATIONS_COL, location);
				string sql = "UPDATE " + DBUtils.PEOPLE_TABLE + " SET " + DBUtils.PERSON_LOCATION + " = @location_id " +
				             "WHERE " + DBUtils.PEOPLE_COL + " = @person_name";
				dbCommand.Parameters.Add (new SqliteParameter ("@location_id", location_id));
				dbCommand.Parameters.Add (new SqliteParameter ("@person_name", personName));
				dbCommand.CommandText = sql;
				dbCommand.ExecuteScalar ();
			}
			dbConnection.Close ();
		}
	}

	public List<string> GetLocations(){
		List<string> locNames = new List<string>();
		using (IDbConnection dbConnection = new SqliteConnection (connectionString)) {
			dbConnection.Open ();

			using (IDbCommand dbCommand = dbConnection.CreateCommand ()) {

				string sqlQuery = "SELECT " + LOCATIONS_COL +  " FROM " + LOCATIONS_TABLE;

				dbCommand.CommandText = sqlQuery;

				using (IDataReader reader = dbCommand.ExecuteReader ()) {

					while (reader.Read ()) {
						locNames.Add (reader.GetString (0));
					}
					dbConnection.Close ();
					reader.Close ();
				}
			}
		}
		return locNames;

	}
		
			
}

