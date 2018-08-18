using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class DBUtils {

	public static readonly string OBJECTS_TABLE = "OBJECTS";
	public static readonly string OBJECTS_ID = "OBJECT_ID";
	public static readonly string OBJECTS_COL = "OBJECT_NAME";
	public static readonly string OBJECTS_LOCATION = "OBJECT_LOCATION";
	private const string OBJECT_X_POS = "object_x_position";
	private const string OBJECT_Z_POS = "object_z_position";
	public static readonly string ADJECTIVES_TABLE = "ADJECTIVES";
	public static readonly string ADJECTIVES_ID = "ADJECTIVE_ID";
	public static readonly string ADJECTIVES_COL = "ADJECTIVE_NAME";
	public static readonly string CATEGORIES_TABLE = "CATEGORIES";
	public static readonly string CATEGORIES_ID = "CATEGORY_ID";
	public static readonly string CATEGORIES_COL = "CATEGORY_NAME";
	public static readonly string LOCATIONS_TABLE = "LOCATIONS";
	public static readonly string LOCATIONS_ID = "LOCATION_ID";
	public static readonly string LOCATIONS_COL = "LOCATION_NAME";
	public static readonly string LOCATION_X = "LOCATION_X";
	public static readonly string LOCATION_Y = "LOCATION_Y";
	public static readonly string ATTRIBUTES_TABLE = "ATTRIBUTES";
	public static readonly string ATTRIBUTE_ID = "ATTRIBUTE_ID";
	public static readonly string ATTRIBUTE_COL = "ATTRIBUTE_NAME";
	public static readonly string PEOPLE_TABLE = "PEOPLE";
	public static readonly string PEOPLE_ID = "PERSON_ID";
	public static readonly string PEOPLE_COL = "PERSON_NAME";
	public static readonly string PERSON_LOCATION = "PERSON_LOCATION";
	//linking tables
	public static readonly string ADJECTIVE_TYPE_TABLE = "ADJECTIVE_TYPE";
	public static readonly string OBJECT_DESCRIPTION_TABLE = "OBJECT_DESCRIPTION";
	public static readonly string OBJECT_DESCRIPTION_ATT_VALUE = "ATTRIBUTE_VALUE";
	public static readonly string OBJECT_DESC_OBJECT_ID = "OBJECT_ID";
	public static readonly string OBJECT_DESC_ATTRIBUTE_ID = "ATTRIBUTE_ID";
	public static readonly string OBJECT_CATEGORIES_TABLE = "OBJECT_CATEGORIES";
	public static readonly int X_POSITION_ATT_ID = 5;
	public static readonly int Y_POSITION_ATT_ID = 6;
	public static readonly int LOCATION_ATT_ID = 7;

	public static readonly string multiple_word_matcher = " %";
	// string for connecting to DB
	public static readonly string connectionString = "URI=file:" + Application.dataPath + "/neo_brain.db";


	public static int getId(string id_name, string table_name, string col_name, string filter, IDbCommand dbCommand){
		dbCommand.CommandText = "SELECT "+id_name+" FROM "+table_name+" WHERE "+col_name+" = @filter";
		dbCommand.Parameters.Clear ();
		dbCommand.Parameters.Add (new SqliteParameter("@filter", filter));
		return readId (dbCommand);
	}

	public static int readId(IDbCommand dbCommand){
		int id = -1;
		using (IDataReader reader = dbCommand.ExecuteReader ()) {

			while (reader.Read ()) {
				//				Debug.Log ("id: " + reader.GetInt32 (0));
				id = reader.GetInt32 (0);
			}
			reader.Close ();
			return  id;
		}

	}

	public static Dictionary<string, string> getStringPairings(IDbCommand dbCommand) {
		Dictionary<string, string> pairings = new Dictionary<string, string> ();
		using (IDataReader reader = dbCommand.ExecuteReader ()) {

			while (reader.Read ()) {
				//Debug.Log ("id: " + reader.GetInt32 (0));
				pairings.Add(reader.GetString(0), reader.GetString(1));
			}
			reader.Close ();
			return  pairings;
		}
	}

	public static string getValueFromId(int id, string id_col, string table_name, string col_name, IDbCommand dbCommand){
		dbCommand.CommandText = "SELECT "+col_name+" FROM "+table_name+" WHERE "+id_col+" = @id";
		dbCommand.Parameters.Clear ();
		dbCommand.Parameters.Add (new SqliteParameter("@id", id));
		List<string> results = readStringResults (dbCommand);
		return results[0];
	}

	public static List<string> readStringResults(IDbCommand dbCommand){
		List<string> results = new List<string> ();
		using (IDataReader reader = dbCommand.ExecuteReader ()) {

			while (reader.Read ()) {
				//Debug.Log ("id: " + reader.GetInt32 (0));
				results.Add(reader.GetString(0));
			}
			reader.Close ();
			return  results;
		}
	}

	public static List<string> getObjectAttribute(int objectId, int attributeId, IDbCommand dbCommand) {
		dbCommand.CommandText = "SELECT "+OBJECT_DESCRIPTION_ATT_VALUE+
			" FROM "+OBJECT_DESCRIPTION_TABLE+
			" WHERE "+OBJECT_DESC_ATTRIBUTE_ID+" = @at_id AND " +OBJECT_DESC_OBJECT_ID+" = @ob_id";
		dbCommand.Parameters.Clear ();
		dbCommand.Parameters.Add (new SqliteParameter("@at_id", attributeId));
		dbCommand.Parameters.Add (new SqliteParameter("@ob_id", objectId));
		List<string> results = readStringResults (dbCommand);

		return results;
	}

	public static Vector2 getLocationCoordinates(string location, IDbCommand dbCommand) {
		dbCommand.CommandText = "SELECT " + LOCATION_X + ", " + LOCATION_Y +
		" FROM " + LOCATIONS_TABLE +
			" WHERE " + LOCATIONS_COL + " = @location OR " + LOCATIONS_COL + " like @location_with_space";
		dbCommand.Parameters.Clear ();
		dbCommand.Parameters.Add (new SqliteParameter("@location", location));
		dbCommand.Parameters.Add (new SqliteParameter("@location_with_space", location + multiple_word_matcher));
		return readLocationCoordinates (dbCommand);
	}

	public static bool ItemExistsInMemory(string item, string colName, string table) {
		List<string> results = null;
		bool resultsFound = false;

		using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
			dbConnection.Open ();

			using (IDbCommand dbCommand = dbConnection.CreateCommand ()) {
				string command = "SELECT " + colName +
				                 " FROM " + table +
				                 " WHERE " + colName + " = @item OR " + colName + " LIKE @item_with_space";
				dbCommand.Parameters.Add (new SqliteParameter ("@item", item));
				// in this case, match any item that starts with the passed in word
				dbCommand.Parameters.Add (new SqliteParameter ("@item_with_space", item + multiple_word_matcher));

				dbCommand.CommandText = command;
				results = DBUtils.readStringResults (dbCommand);
				if (results.Count != 0) {
					resultsFound = true;
				}
			}
			dbConnection.Close ();
		}

		return resultsFound;
	}

	private static List<int> ReadIntResults(IDbCommand dbCommand){
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

	public static Vector2 readLocationCoordinates(IDbCommand dbCommand) {
		int x = 0;
		int y = 0;

		using (IDataReader reader = dbCommand.ExecuteReader ()) {

			while (reader.Read ()) {
				//Debug.Log ("id: " + reader.GetInt32 (0));
				x = reader.GetInt32(0);
				y = reader.GetInt32 (1);
			}
			reader.Close ();
			return  new Vector2(x,y);
		}
	}

	public static Vector2 getObjectCoordinates(string objName, IDbCommand dbCommand) {
		dbCommand.CommandText = "SELECT " + OBJECT_X_POS + ", " + OBJECT_Z_POS +
			" FROM " + OBJECTS_TABLE +
			" WHERE " + OBJECTS_COL + " = @objName";
		dbCommand.Parameters.Clear ();
		dbCommand.Parameters.Add (new SqliteParameter("@objName", objName));
		return readLocationCoordinates (dbCommand);
	}
}
