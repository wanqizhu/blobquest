using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// clear player preferences (including leaderboard)
public class ClearPlayerPref : MonoBehaviour {
	private Button b;
	void Start () {
		b = this.gameObject.GetComponent<Button> ();
		b.onClick.AddListener (deletePlayerPref);
	}
	
	void deletePlayerPref(){
		PlayerPrefs.DeleteAll ();
	}
}
