using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class WernickeAreaTest {

	WernickeArea wernickeArea;
	private Vector2 kitchenLocation = new Vector2(17,5);
	private Vector2 bedroomLocation = new Vector2(17,-6);
	private Vector2 mainRoomLocation = new Vector2(-6, 0);

	[SetUp]
	public void setupWernickeArea(){
		wernickeArea = WernickeArea.getInstance ();
		wernickeArea.setConnectionString("URI=file:" + Application.dataPath + "/NEO/test/Editor/wernicke_area_test_db.db");
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

//	[Test]
//	public void WernickeAreaShouldReturnNeosLocation() {
//		string whereStatement = "where are you";
//
//		string location = wernickeArea.AnalyzeSentence (whereStatement);
//
//		Assert.AreEqual ("main room", location);
//	}

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

	[Test]
	public void WernickeAreaShouldReturnLocationOfRoom() {
		string goToStatement = "go to the bedroom";
		Vector2 targetLocation  = wernickeArea.getTargetLocation (goToStatement);
		Assert.AreEqual (bedroomLocation, targetLocation);
	}

	[Test]
	public void WernickeAreaShouldReturnLocationOfRoomNameWithMultipleWords() {
		string goToStatement = "go to the main room";
		Vector2 targetLocation  = wernickeArea.getTargetLocation (goToStatement);
		Assert.AreEqual (mainRoomLocation, targetLocation);
	}

}
