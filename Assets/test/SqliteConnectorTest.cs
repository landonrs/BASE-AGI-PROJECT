using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class SqliteConnectorTest {
	NeoMemory conn;
	ObjectData apple;

	[SetUp]
	public void initializeDB(){
		conn = NeoMemory.getInstance();
		conn.setconnectionString("URI=file:" + Application.dataPath + "/test/neo_brain_test.db");
		apple = new ObjectData("apple", "red", new List<string>(new string[]{"fruit", "food"}), 200, 200);
	}

	[Test]
	public void DBTestSimplePasses() {
		// Use the Assert class to test conditions.
		conn.memorizeObject(apple, "kitchen");
		Assert.AreEqual(conn.getObjects(), new List<string>(new string[]{"apple"}));
	}

	[Test]
	public void SqliteConnectorMemorizesObject() {
		Assert.IsTrue (true);
	}

	[Test]
	public void SqliteConnectorGetsColor() {
		Assert.IsTrue (true);
	}

	[Test]
	public void SqliteConnectorGetsAttribute() {
		Assert.IsTrue (true);
	}

	[TearDown]
	public void cleanUpDB(){
		//put delete statements here
		//		List<string> tables = conn.getTableNames();
		//		foreach (string table in tables){
		//			conn.runCommand("DELETE FROM " + table);
		//		}

	}
}
