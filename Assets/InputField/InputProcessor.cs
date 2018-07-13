using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputProcessor : MonoBehaviour {

	private static InputField inputField;
	private WernickeArea wernickeArea;

	// Use this for initialization
	void Start () {
		inputField = GetComponent<InputField>();
		wernickeArea = WernickeArea.getInstance ();

	}
	
	// Update is called once per frame
	void Update () {
		if(inputField.text != "" && Input.GetKey(KeyCode.Return)) {
			string response = wernickeArea.AnalyzeSentence (inputField.text);
			Debug.Log (response);
			inputField.text = "";
		}
	}

	public static bool isInputFocused(){
		if (inputField.isFocused)
			return true;
		else
			return false;
	}
}
