using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class WernickeAreaTest {

	WernickeArea wernickeArea;

	[SetUp]
	public void setupWernickeArea(){
		wernickeArea = WernickeArea.getInstance ();
	}

	[Test]
	public void WernickeAreaTestSimplePasses() {
		// Use the Assert class to test conditions.
		Assert.IsTrue(true);
	}

	[Test]
	public void WernickeAreaShouldReturnCorrectObjectLocation() {
		string objectName = "oven";

		string location = wernickeArea.DefineObjectLocation (objectName);

		Assert.AreEqual ("kitchen", location);
	}


}
