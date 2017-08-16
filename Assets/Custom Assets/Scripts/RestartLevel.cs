using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// "Restart" button at pause screen
public class RestartLevel : MonoBehaviour {
	private Button b;
	// Use this for initialization
	void Start () {
		b = this.gameObject.GetComponent<Button> ();
		b.onClick.AddListener (restartLevel);
	}

	void restartLevel(){
		Time.timeScale = 1;
		LevelData.paused = false;
		LevelData.numDeath += 1;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name, LoadSceneMode.Single);
	}
}
