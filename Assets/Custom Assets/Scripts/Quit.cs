using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// "Quit" button on start-of-game screen
public class Quit : MonoBehaviour {
	private Button b;

	void Start () {
		b = this.gameObject.GetComponent<Button> ();
		b.onClick.AddListener (quitGame);
	}

	void quitGame(){
		Application.Quit ();
	}
}
