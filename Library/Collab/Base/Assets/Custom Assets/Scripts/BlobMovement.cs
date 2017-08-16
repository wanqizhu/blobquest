using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlobMovement : MonoBehaviour {

	public float maxSpeed;
	public float minMass;
	public float jumpForce;
	public float baseMass;
	public float accelerationTimeAirborne;
	public float accelerationTimeGround;


	private Rigidbody2D rig;
	private bool jumping = false;
	private bool canJump = true;
	private float jumpAngle = Mathf.PI/2;
	private static int numAlive = 0;
	float velocityXSmoothing;

	private List<customDataWithColliders> _touchingColliders = new List<customDataWithColliders>();




	// Use this for initialization
	void Start () {
		rig = GetComponent<Rigidbody2D>();
		numAlive += 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (!jumping) {
			jumping = Input.GetButtonDown ("Jump") || Input.GetButtonDown ("Vertical");
		}
	}

	void FixedUpdate () {
		float dir = Input.GetAxisRaw ("Horizontal");  // dir goes from 0 to +/- 1 as we press and hold down the key
		float newXVelocity = dir == 0 ? rig.velocity.x : dir * maxSpeed / Mathf.Pow(rig.mass/baseMass, 0.3f);

		float velocityX = Mathf.SmoothDamp (rig.velocity.x, newXVelocity, ref velocityXSmoothing, canJump ? accelerationTimeGround : accelerationTimeAirborne);

		rig.velocity = new Vector2 (velocityX, rig.velocity.y);

		canJump = _touchingColliders.Count > 0; // only set canJump to false after exiting all collisions

		if (jumping && canJump) {
			//if(rig.velocity.y < 0){
			rig.velocity = new Vector2 (rig.velocity.x, 0);
			//}

			// jump at an angle, halfway between vertical and the normal of the wall
			// the exponent controls how sensitive the jump strength is in relation to the mass; this makes it jump slightly higher as the mass gets lighter

			jumpAngle = _touchingColliders.Last()._jumpAngle;
			rig.AddForce (jumpForce*Mathf.Pow(rig.mass/baseMass, 0.9f) / Mathf.Sin(jumpAngle) * new Vector2 (Mathf.Cos(jumpAngle), Mathf.Sin(jumpAngle))); 
			//canJump = false;
		}
		jumping = false;

		if (rig.mass < minMass) {
			Die();
		}

		transform.localScale = Mathf.Pow(rig.mass/baseMass, 1f/3f) * Vector2.one; // if we have penalty, then we should scale the size appropriately
		// use cube root so the ball is more visible at small mass
	}

	public void Die(){
		numAlive -= 1;
		if (numAlive == 0) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name, LoadSceneMode.Single);
		} else {
			Destroy (this.gameObject);
		}
//		Debug.Log (GameObject.FindGameObjectsWithTag ("Player").Length);
//		if (GameObject.FindGameObjectsWithTag ("Player").Length == 1) {
//			SceneManager.LoadScene (SceneManager.GetActiveScene ().name, LoadSceneMode.Single);
//		} else {
//			Destroy(this.gameObject);  // death animation?
//		}
	}

	void OnCollisionEnter2D(Collision2D otherObj) {


		if (otherObj.gameObject.tag == "Surface") {
			Collider2D collider = otherObj.collider;

			Vector3 contactPoint = otherObj.contacts[0].point;
			Vector2 collisionNormal = otherObj.contacts[0].normal;
			float normAngle = Mathf.Atan (Mathf.Abs(collisionNormal.y / collisionNormal.x));

			if (Mathf.Abs (collisionNormal.x) > Mathf.Abs (collisionNormal.y)) {
				// horizontal collision
			} else {
				//vertical collision

				// if we want to be able to jump off ceilings, then we need to model it similar to jumping off left/right walls
			}

			if (collisionNormal.x > 0) {
				// colliding from the right
				jumpAngle = Mathf.Abs (Mathf.PI * 0.5f + normAngle) / 2f;
			} else { // normal angle is between pi/2 and pi, so we need to take average slighly differently since Tan(abs(y/x)) gives 180-desired_angle
				jumpAngle = Mathf.Abs (Mathf.PI * 1.5f - normAngle) / 2f;
			}


			_touchingColliders.Add(new customDataWithColliders(collider, jumpAngle));

		}


	}


	void OnCollisionExit2D(Collision2D otherObj){
		if (otherObj.gameObject.tag == "Surface") {
			_touchingColliders.Remove(new customDataWithColliders(otherObj.collider, jumpAngle));
			//Debug.Log (_touchingColliders.Last());
			//Debug.Log (_touchingColliders.Count ());
		}

	}

	public struct customDataWithColliders : IEquatable<customDataWithColliders> {
		public Collider2D _collider;
		public float _jumpAngle;

		public customDataWithColliders(Collider2D _collider, float _jumpAngle) {
			this._collider = _collider;
			this._jumpAngle = _jumpAngle;
		}

		// this might not be needed but might as well
		public override bool Equals(System.Object obj) {
			return ((customDataWithColliders)obj)._collider == this._collider;
		}

		public bool Equals(customDataWithColliders obj) {
			return ((customDataWithColliders)obj)._collider == this._collider;
		}

		public override int GetHashCode ()
		{
			return this._collider.GetHashCode ();
		}
	}
}
