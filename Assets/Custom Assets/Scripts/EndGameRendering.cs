using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameRendering : MonoBehaviour {
	
	public Text scoreboard;

	private EndOfLevelRendering scoreScript;


	void Start () {
		scoreScript = GetComponent<EndOfLevelRendering>(); // use the score/leaderboard functions there

		float totalScore = 0; // total score of all levels completed on this playthrough
		foreach (float score in LevelData.bestScores) {
			totalScore += score;
		}

		LevelData.currentLevel = -2; // use -2 to indicate that this is for total-score, rather than score of any particular level

		scoreScript.scores = new string[10];
		scoreScript.AddScore (LevelData.playerName, totalScore); // update the final leaderboard
		scoreboard.text = "Your final score: " + totalScore.ToString () + "\n\nLeaderboard\n\n" + string.Join("\n", scoreScript.scores);
	}
}
