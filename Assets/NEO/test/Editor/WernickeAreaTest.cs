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

	[Test]
	public void WernickeAreaShouldParseObjectFromWhereStatement() {
		string whereStatement = "where is the nightstand";

		string location = wernickeArea.AnalyzeWhereQuery (whereStatement);

		Assert.AreEqual ("bedroom", location);
	}

	[Test]
	public void WernickeAreaShouldReturnNeosLocation() {
		string whereStatement = "where are you";

		string location = wernickeArea.AnalyzeSentence (whereStatement);

		Assert.AreEqual ("main room", location);
	}

	[Test]
	public void WernickeAreaShouldAnalyzeWhereQuery() {
		string whereStatement = "where is the bed";

		string location = wernickeArea.AnalyzeSentence (whereStatement);

		Assert.AreEqual ("bedroom", location);
	}

	[Test]
	public void WernickeAreaShouldAnalyzeWhatQuery() {
		string whatStatement = "what color is the nightstand";

		string color = wernickeArea.AnalyzeSentence (whatStatement);

		Assert.AreEqual ("brown", color);
	}

}
