using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Experience_01 : MonoBehaviour {

	public InputField mainInputField;

	// Use this for initialization
	void Start () {

		mainInputField.text = "Enter Text Here...";
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown("return")) {
			Debug.Log(mainInputField.text);
		}
	}
}
