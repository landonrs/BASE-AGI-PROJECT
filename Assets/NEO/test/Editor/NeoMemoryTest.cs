﻿using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Data;

public class NeoMemoryTest {

	private NeoMemory neoMemory;
	private ObjectData apple;
	private string TEST_CONNECTION_STRING;

	[SetUp]
	public void SetupNeoMemory(){
		neoMemory = NeoMemory.getInstance ();
		TEST_CONNECTION_STRING = "URI=file:" + Application.dataPath + "/test/neo_brain_test.db";
		neoMemory.setconnectionString (TEST_CONNECTION_STRING);
		DBUtils.setConnectionString(TEST_CONNECTION_STRING);
		apple = new ObjectData("apple", "red", new List<string>(new string[]{"fruit", "food"}), 200, 500);
	}


	[Test]
	public void NeoMemoryTestSimplePasses() {
		neoMemory.memorizeObject(apple, "kitchen");
		Assert.AreEqual(neoMemory.getObjects(), new List<string>(new string[]{"apple"}));
	}

	[Test]
	public void NeoMemoryUpdatesPersonLocation() {
		LocationData data = new LocationData ("kitchen", 200, 200);
		neoMemory.MemorizeLocation (data);
		neoMemory.UpdatePersonLocation ("you", "kitchen");

		Assert.AreEqual(1, DBUtils.getId(DBUtils.PERSON_LOCATION, DBUtils.PEOPLE_TABLE, DBUtils.PEOPLE_COL, "you"));
	}


	[Test]
	public void NeoMemoryMemorizesObjectXAndZPositions() {
		neoMemory.memorizeObject(apple, "kitchen");

		Assert.AreEqual (DBUtils.getObjectCoordinates ("apple")[0], 200);
	}



	[TearDown]
	public void cleanUpDB(){
		List<string> tables = neoMemory.getTableNames();
		foreach (string table in tables){
			if (table.ToUpper() != DBUtils.ATTRIBUTES_TABLE && table.ToUpper() != DBUtils.PEOPLE_TABLE) {
				neoMemory.runCommand ("DELETE FROM " + table);
			}
		}
	}
}
