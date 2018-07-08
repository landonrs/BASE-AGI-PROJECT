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
	public static readonly string ADJECTIVES_TABLE = "ADJECTIVES";
	public static readonly string ADJECTIVES_ID = "ADJECTIVE_ID";
	public static readonly string ADJECTIVES_COL = "ADJECTIVE_NAME";
	public static readonly string CATEGORIES_TABLE = "CATEGORIES";
	public static readonly string CATEGORIES_ID = "CATEGORY_ID";
	public static readonly string CATEGORIES_COL = "CATEGORY_NAME";
	public static readonly string LOCATIONS_TABLE = "LOCATIONS";
	public static readonly string LOCATIONS_ID = "LOCATION_ID";
	public static readonly string LOCATIONS_COL = "LOCATION_NAME";
	public static readonly string ATTRIBUTES_TABLE = "ATTRIBUTES";
	public static readonly string ATTRIBUTE_ID = "ATTRIBUTE_ID";
	public static readonly string ATTRIBUTE_COL = "ATTRIBUTE_NAME";
	//linking tables
	public static readonly string ADJECTIVE_TYPE_TABLE = "ADJECTIVE_TYPE";
	public static readonly string OBJECT_DESCRIPTION_TABLE = "OBJECT_DESCRIPTION";
	public static readonly string OBJECT_CATEGORIES_TABLE = "OBJECT_CATEGORIES";
	public static readonly int X_POSITION_ATT_ID = 5;
	public static readonly int Y_POSITION_ATT_ID = 6;
	public static readonly int LOCATION_ATT_ID = 7;

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
}
