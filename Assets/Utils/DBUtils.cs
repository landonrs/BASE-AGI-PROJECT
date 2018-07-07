using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
