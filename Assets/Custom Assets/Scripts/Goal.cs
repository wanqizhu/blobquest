using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

// script attached to the goal of each level; upon contact you win
public class Goal : MonoBehaviour {
	public float massRequired, maxMass=1;
	public float targetTime=100;  // targetTime should be set so that most players complete the level between x and 2x
	public string nextSceneName;

	private string[] cheatCode;
	private int index;

	void Start() {
		// hackssss
		cheatCode = new string[] { "b", "l", "o", "b", "b" };
		index = 0;    
	}

	void Update() {
		// Check if any key is pressed
		if (Input.anyKeyDown) {
			// Check if the next key in the code is pressed
			if (Input.GetKeyDown(cheatCode[index])) {
				// Add 1 to index to check the next key in the code
				index++;
			}
			// Wrong key entered, we reset code typing
			else {
				index = 0;    
			}
		}

		// If index reaches the length of the cheatCode string, 
		// the entire code was correctly entered
		if (index == cheatCode.Length) {
			// Cheat code successfully inputted!
			// Unlock crazy cheat code stuff

			// win instantly!!
			LevelData.mass = maxMass;
			index = 0;
			win();
		}
	}



	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Player") {
			LevelData.mass = other.gameObject.GetComponent<Rigidbody2D>().mass;
			win ();
		}
	}

	void win() {
		// Level is won, start next one
		Time.timeScale = 0;
		//Do animations

		LevelData.maxMass = maxMass;
		LevelData.targetTime = targetTime;
		if (LevelData.mass < massRequired) {
			LevelData.nextLevel = SceneManager.GetActiveScene ().name;
			LevelData.won = false;
		} else {
			LevelData.nextLevel = nextSceneName;
			LevelData.won = true;
		}
		LevelData.ended = true;
		SceneManager.LoadScene("LevelEnd", LoadSceneMode.Additive);
		SceneManager.SetActiveScene (SceneManager.GetSceneByName("LevelEnd"));
	}
}
