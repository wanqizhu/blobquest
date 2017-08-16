using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
//
using UnityEngine.SceneManagement;

public static class LevelData {
	public static float mass, maxMass;
	public static string nextLevel;
	public static int currentLevel=1;
	public static bool won, paused=false, ended=false;
	public static float timePlaying=0f, targetTime;
	public static int numDeath=0;
	public static string playerName="player1";
	public static float score;
	public static float[] bestScores = new float[15];
}
