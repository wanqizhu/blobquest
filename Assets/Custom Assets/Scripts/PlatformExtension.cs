using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// obselete; should merge with ButtonTarget/ButtonTrigger
public class PlatformExtension : MonoBehaviour {

	public bool oneTime;
	public Vector3 endScale;

	// only if not oneTime
	public Vector3 startScale;
	public float scaleSpeed;


	private bool triggered = false;
	private Vector3 target;


	void FixedUpdate () {

		// moves left and right
		if (triggered) {
			if (transform.localScale.x < startScale.x) {
				target = endScale;
			}

			if (!oneTime && transform.localScale.x > endScale.x) {
				target = startScale;
			}

			var cur = transform.localScale;
			transform.localScale = Vector3.Lerp (cur, target, scaleSpeed * Time.deltaTime);
		}
	}



	void enable () {
		triggered = true;
//		Debug.Log ("enabled");
	}

	void disable () {
//		Debug.Log ("disabled");
		triggered = false;
	}
}
