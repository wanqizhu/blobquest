using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour {
	public InputField nameText;
	public InputField level;
	public int maxLevel;
	private Button b;
	// Use this for initialization
	void Start () {
		
		if (level != null) {
			level.text = LevelData.currentLevel.ToString ();
		}
		if (nameText != null) {
			nameText.text = LevelData.playerName;
		}
		LevelData.bestScores = new float[15]; // reset best scores

		b = this.gameObject.GetComponent<Button> ();
		b.onClick.AddListener (playGame);
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Return)) {
			playGame ();
		}
	}

	void playGame(){
//		Debug.Log (level.text);
//		Debug.Log (name.text);

		if (level != null) {
			int.TryParse (level.text, out LevelData.currentLevel);
			if (LevelData.currentLevel < 1 || LevelData.currentLevel > maxLevel) {
				LevelData.currentLevel = 1;
			}
		} else {
			LevelData.currentLevel = 1;
		}
		if (nameText != null) {
			LevelData.playerName = nameText.text;
		}
		try {
			SceneManager.LoadScene ("Level " + LevelData.currentLevel.ToString(), LoadSceneMode.Single);
		} catch (Exception e) {
			Debug.Log (e);
			SceneManager.LoadScene ("Level 1", LoadSceneMode.Single);

		}
	}
}
