using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this script renders the blob/leaderboard/stats upon level completion
public class EndOfLevelRendering : MonoBehaviour {

	public Vector3 movement;
	public GameObject blob;
	public Text titleText;
	public Text infoText;
	public Text scoreboardText;

	[HideInInspector] public string[] scores;

	private float dest;
	private Vector3 startPos;




	void Start () {
		scores = new string[10];
		ComputeScore (); // find out what score we got on this level

		// if we've played this level before, update the best score of this current run (for end-of-game leaderboard purposes)
		LevelData.bestScores[LevelData.currentLevel] = Mathf.Max (LevelData.score, LevelData.bestScores [LevelData.currentLevel]);

		AddScore(LevelData.playerName, LevelData.score);

		scoreboardText.text = "High Score\n\n" + string.Join("\n", scores);

		startPos = blob.transform.position;
		dest = (240f) * (LevelData.mass / LevelData.maxMass);
		if (dest > 240) { // if we had more mass than the level's target maxMass, we should just see the full blob
			dest = 240;
		}
		infoText.text = "Mass: " + LevelData.mass.ToString()
			+ "\nTime: " + LevelData.timePlaying.ToString()
			+ "\nNumber of Tries: " + (1 + LevelData.numDeath).ToString()
			+ "\n\nFinal score: " + LevelData.score.ToString();
		titleText.text = "Level " + LevelData.currentLevel.ToString() + " Complete!";
	}


	void Update () {
		if (transform.localPosition.y < -245 + dest) {
			transform.Translate (movement);
			blob.transform.position = startPos;
		}
	}

	void ComputeScore () {
		LevelData.score = LevelData.mass / LevelData.maxMass * 100f
			* Mathf.Pow(LevelData.targetTime / LevelData.timePlaying, 0.2f) // use 0.2 pow to smooth out scoring so it's always around 2 digits
			/ Mathf.Sqrt (LevelData.numDeath + 1);
	}

	// this adds the current score to player preferences and updates the top-10 leaderboard
	// the key is LevelData.currentLevel + "_" + i + "HScore"
	public void AddScore(string name, float score){
		float newScore;
		string newName;
		float oldScore;
		string oldName;
		newScore = score;
		newName = name;
		for(int i=0;i<10;i++){
			if(PlayerPrefs.HasKey(LevelData.currentLevel + "_" + i + "HScore")){
				if(PlayerPrefs.GetFloat(LevelData.currentLevel + "_" + i + "HScore")<newScore){ 
					// new score is higher than the stored score
					oldScore = PlayerPrefs.GetFloat(LevelData.currentLevel + "_" + i + "HScore");
					oldName = PlayerPrefs.GetString(LevelData.currentLevel + "_" + i + "HScoreName");
					PlayerPrefs.SetFloat(LevelData.currentLevel + "_" + i + "HScore",newScore);
					PlayerPrefs.SetString(LevelData.currentLevel + "_" + i + "HScoreName",newName);
					newScore = oldScore;
					newName = oldName;
				}
			}else{
				PlayerPrefs.SetFloat(LevelData.currentLevel + "_" + i + "HScore",newScore);
				PlayerPrefs.SetString(LevelData.currentLevel + "_" + i + "HScoreName",newName);
				newScore = 0;
				newName = "";
			}

			// this array is used for displaying the leaderboard text field
			scores[i] = (PlayerPrefs.GetString (LevelData.currentLevel + "_" + i + "HScoreName") + "      " + PlayerPrefs.GetFloat (LevelData.currentLevel + "_" + i + "HScore").ToString("F3"));
		}
	}
}
