using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMove : MonoBehaviour {
	private bool triggered = false;
	public float bottomthreshhold;
	public float topthreshhold;
	public Vector3 movement;

	void FixedUpdate () {
		if (triggered) {
			if (transform.position.y < topthreshhold) {
				transform.Translate (movement * Time.deltaTime);
			}
		} else {
			if (transform.position.y > bottomthreshhold) {
				transform.Translate (-1 * movement * Time.deltaTime);
			}
		}
	}
	void enable(){
		triggered = true;
	}

	void disable(){
		triggered = false;
	}
}
