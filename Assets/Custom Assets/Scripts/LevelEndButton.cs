using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

// what happens when you click the "Next Level" button at end-of-level
public class LevelEndButton : MonoBehaviour {
	private Text t;
	private Button b;

	void Start () {
		b = this.gameObject.GetComponent<Button> ();
		t = this.gameObject.GetComponentInChildren<Text> ();
		if (LevelData.won) {
			t.text = "Next Level";
		} else {
			t.text = "Retry"; // we should make this more robust
		}

		b.onClick.AddListener (buttonClick);
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Return)) {
			buttonClick ();
		}
	}

	void buttonClick(){
		Time.timeScale = 1;
		// reset LevelData
		LevelData.ended = false;
		LevelData.timePlaying = 0;
		LevelData.numDeath = 0;
		LevelData.currentLevel += 1; // there are some bugs with the last level
//		int.TryParse(Regex.Match (LevelData.nextLevel, @"\d+$").Value, out LevelData.currentLevel);  // require levels to be named with a number at the end
//		if (LevelData.currentLevel == 0) {LevelData.currentLevel = 15;} // set manually to maxLevel + 1
//		LevelData.currentLevel -= 1;
		SceneManager.LoadScene(LevelData.nextLevel, LoadSceneMode.Single);
	}
}
