using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is the generic button script attached to an object
//  when the button is triggered, we'll move this object along a linear path
//  the object should be a subchild of the button
public class ButtonTarget : MonoBehaviour {

	public bool returnToOriginalPosition = false; // whether the platform should reset to its original position when the button is de-pressed
	public bool bounce = false; // whether the platform should continue to bounce back and forth while the button is pressed (as opposed to only go until it reaches the end of its path)
	public float speed;
	public Vector3 initial; // initial position
	public Vector3 target; // target position

	private bool triggered = false;
	private int dir = 1;

	void Start () {
		// force them to be slightly different so math works xDxDxD
		if (target.x == initial.x) {
			target.x += 0.000001f;
		}

		if (target.y == initial.y) {
			target.y += 0.000001f;
		}
	}


	// Use this for initialization
	void FixedUpdate () {
		if (triggered) {
			// figure out whether the object is currently before initial, between initial and target, or past target
			var dirX = transform.position.x == target.x ? 1f : (transform.position.x - initial.x) / (transform.position.x - target.x);

			if (dir > 0) {
				if (dirX >= 1) {
					dir *= -1;
				}
				// make it move slightly past target so that dirX and dirY do not have divide-by-zero errors
				transform.position = Vector3.MoveTowards (transform.position, 1.0001f*target + (-0.0001f)*initial, speed * Time.deltaTime);
			} else if (dir < 0 && bounce) {
				if (dirX >= 0 && dirX < 1) {
					dir *= -1;
				}

				transform.position = Vector3.MoveTowards (transform.position, 1.0001f*initial + (-0.0001f)*target, speed * Time.deltaTime);
			}

		} else if (returnToOriginalPosition) {
			transform.position = Vector3.MoveTowards (transform.position, 1.0001f*initial + (-0.0001f)*target, speed * Time.deltaTime);
		}

	}

	void enable () {
		triggered = true;
		dir = 1;
	}

	void disable () {
		triggered = false;
	}
}
