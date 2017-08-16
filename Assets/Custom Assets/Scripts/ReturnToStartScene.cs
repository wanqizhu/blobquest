using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// "Return to Main Menu" button on pause screen/end-of-game screen
public class ReturnToStartScene : MonoBehaviour {
	private Button b;

	void Start () {
		b = this.gameObject.GetComponent<Button> ();
		b.onClick.AddListener (returnToStart);
	}
	
	void returnToStart () {
		Time.timeScale = 1;
		LevelData.paused = false;
		if (SceneManager.GetSceneByName ("Pause").IsValid()) {
			SceneManager.UnloadSceneAsync ("Pause");
		}
		SceneManager.LoadScene ("Start Menu", LoadSceneMode.Single);
	}
}
