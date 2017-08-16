using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// "Resume" button on pause screen
public class Resume : MonoBehaviour {
	private Button b;

	void Start () {
		b = this.gameObject.GetComponent<Button> ();
		b.onClick.AddListener (resumeClick);
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Time.timeScale = 1;
			LevelData.paused = false;
			SceneManager.UnloadSceneAsync ("Pause");
		}
	}

	void resumeClick(){
		Time.timeScale = 1;
		LevelData.paused = false;
		SceneManager.UnloadSceneAsync ("Pause");
	}
}
