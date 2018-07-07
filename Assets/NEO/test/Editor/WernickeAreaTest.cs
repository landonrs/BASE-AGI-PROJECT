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
	}

	[Test]
	public void WernickeAreaShouldReturnCorrectObjectLocation() {
		string sentence = "Where is the oven?";

		string location = wernickeArea.DefineObjectLocation (sentence);

		Assert.AreEqual ("kitchen", location);
	}


}
