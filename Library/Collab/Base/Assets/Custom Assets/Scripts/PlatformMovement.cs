using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour {

	public float leftXthreshhold;
	public float rightXthreshhold;
	public Vector3 movement;

	private bool triggered = false;
	private float dir = 1;

	

	void FixedUpdate () {

		// moves left and right
		if (triggered) {
			if (dir < 0) {
				if (transform.position.x > rightXthreshhold) {
					dir *= -1;
				}
			} else if (dir > 0) {
				if (transform.position.x < leftXthreshhold) {
					dir *= -1;
				}
			}

			transform.Translate (dir * movement * Time.deltaTime);
		}
	}

	private void OnCollisionStay2D(Collision2D coll)
	{
		if (triggered) {
			if ((coll.collider.tag == "Player" || coll.collider.tag == "BlobMass") && coll.gameObject.transform.parent == null) {

				// http://answers.unity3d.com/questions/147816/how-to-avoid-scaling-heritage-when-parenting.html
				var emptyObject = new GameObject(); // so that the ball maintains its localscale
				emptyObject.transform.parent = transform;
				coll.gameObject.transform.parent = emptyObject.transform;
			}
		}
	}

	private void OnCollisionExit2D(Collision2D coll)
	{
		if (coll.collider.tag == "Player" || coll.collider.tag == "BlobMass") {
			var tmp = coll.gameObject.transform.parent;

			coll.gameObject.transform.parent = null;
			if (tmp != null) {
				Destroy (tmp.gameObject);
			}
			if (triggered) {
				coll.gameObject.GetComponent<Rigidbody2D> ().velocity += new Vector2 (dir * movement.x, dir * movement.y);
			}
		}
				
	}


	void enable () {
		triggered = true;
		Debug.Log ("enabled");
	}

	void disable () {
		Debug.Log ("disabled");
		triggered = false;
	}
}
