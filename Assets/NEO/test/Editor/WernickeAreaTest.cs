﻿using UnityEngine;
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

	[Test]
	public void WernickeAreaShouldParseObjectFromWhereStatement() {
		string whereStatement = "where is the nightstand";

		string location = wernickeArea.AnalyzeWhereQuery (whereStatement);

		Assert.AreEqual ("bedroom", location);
	}

	[Test]
	public void WernickeAreaShouldAnalyzeWhereQuesry() {
		string whereStatement = "where is the nightstand";

		string location = wernickeArea.AnalyzeSentence (whereStatement);

		Assert.AreEqual ("bedroom", location);
	}
}
