using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HintDisplay : MonoBehaviour {
	public GameObject showOnClick;
	public string hintTextString;
	public Text hintText;
	public GameObject player;
	// some sort of trigger condition
	public enum condition {timeIdle, timePlaying, numDeath};
	public condition triggerCondition;
	public float timeIdleLimit;
	public float timePlayingLimit;
	public float numDeathLimit;

	private BlobMovement moveScript;

	// Use this for initialization
	void Start () {
		hintText.text = hintTextString;
		showOnClick.SetActive(false);
		moveScript = player.GetComponent<BlobMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!string.IsNullOrEmpty(hintText.text)) {
			if (triggerCondition == condition.timeIdle) {
				if (moveScript.timeIdle > timeIdleLimit) { // condition
					showOnClick.SetActive(true);
				}

			} else if (triggerCondition == condition.timePlaying) {
				if (LevelData.timePlaying > timePlayingLimit) { // condition
					showOnClick.SetActive(true);
				}

			} else if (triggerCondition == condition.numDeath) {
				if (LevelData.numDeath >= numDeathLimit) { // condition
					showOnClick.SetActive(true);
				}
			}
		}
	}
}
