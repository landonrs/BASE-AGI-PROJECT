using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class NeoMemoryTest {

	private NeoMemory neoMemory;
	private ObjectData apple;

	[SetUp]
	public void SetupNeoMemory(){
		neoMemory = NeoMemory.getInstance ();
		neoMemory.setconnectionString("URI=file:" + Application.dataPath + "/test/neo_brain_test.db");
		apple = new ObjectData("apple", "red", new List<string>(new string[]{"fruit", "food"}), 200, 200);
	}


	[Test]
	public void NeoMemoryTestSimplePasses() {
		neoMemory.memorizeObject(apple, "kitchen");
		Assert.AreEqual(neoMemory.getObjects(), new List<string>(new string[]{"apple"}));
	}

	[TearDown]
	public void cleanUpDB(){
		List<string> tables = neoMemory.getTableNames();
		foreach (string table in tables){
			if (table != DBUtils.ATTRIBUTES_TABLE) {
				neoMemory.runCommand ("DELETE FROM " + table);
			}
		}
	}
}
