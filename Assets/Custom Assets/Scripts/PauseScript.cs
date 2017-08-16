using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// script to enable pausing functionality; press "Esc" to pause during a level
public class PauseScript : MonoBehaviour {


	void Start () {
		GetComponent<AudioSource> ().loop = true;
	}
	

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) && !LevelData.paused && !LevelData.ended) {
			Time.timeScale = 0; // stop all physics
			LevelData.paused = true;
			SceneManager.LoadScene ("Pause",LoadSceneMode.Additive);
			SceneManager.SetActiveScene (SceneManager.GetSceneByName("Pause"));
		}

		if (GameObject.FindGameObjectsWithTag ("Player").Length == 0 && !SceneManager.GetSceneByName("LevelFailed").isLoaded) {
			// oops all blobs have died
			Time.timeScale = 0;
			LevelData.paused = true;
			SceneManager.LoadScene ("LevelFailed", LoadSceneMode.Additive);
		};
	}
}
