// TODO
// https://docs.google.com/document/d/1tcLtuvkVRxAt3LxA-EXnwpHBusn1icZ45k4qyrso71U/edit

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


// this is the base script controlling blob movement and generic behaviors
public class BlobMovement : MonoBehaviour {

    Animator animator;

	public float maxSpeed;
	public float minMass;
	public float jumpForce;
	public float baseMass;
	public float accelerationTimeAirborne;
	public float accelerationTimeGround;
	public int jumpGracePeriod = 5;


    // sorry for the clutter, but i didn't know how else to do this
    public Sprite img1, img2, img3, img4, img5;

	private Rigidbody2D rig;
	private bool jumping = false;
	private int canJump = 0;
	private float jumpAngle = Mathf.PI/2; // current jump angle of the most recent contacted wall
	private bool isDead = false;
	[HideInInspector] public float timeIdle = 0;
	[HideInInspector] public bool m_FacingRight = true;
    private bool animatingJump;
	private int facing = 1;
    const int STATE_IDLE = 0;
    const int STATE_MOVE = 1;
    const int STATE_SLEEP = 2;

	float velocityXSmoothing;

	private List<customDataWithColliders> _touchingColliders = new List<customDataWithColliders>();



	void Start () {
		rig = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animatingJump = false;
	}
	

	void Update () {
		if (!jumping) {
			jumping = Input.GetButtonDown ("Jump") || Input.GetButtonDown ("Vertical");
		}
	}

	void FixedUpdate () {

        // handle animation here
        if (Input.GetKey("left") || Input.GetKey("right"))
        {
            animator.SetInteger("state", STATE_MOVE);
        }
        else if (timeIdle >= 5)
        {
            animator.SetInteger("state", STATE_SLEEP);
        }
        else
        {
            animator.SetInteger("state", STATE_IDLE);
        }

        // JUMPING
        if (jumping) {
            animator.enabled = false;
            animatingJump = true;
        }
        if (animatingJump) // different stages of animation
        {
            if (rig.velocity.y > 4)
            {
                GetComponent<SpriteRenderer>().sprite = img1;
            }
            else if (rig.velocity.y >= 1)
            {
                GetComponent<SpriteRenderer>().sprite = img2;
            }
            else if (rig.velocity.y < 1 && rig.velocity.y > -1)
            {
                GetComponent<SpriteRenderer>().sprite = img3;
            }
            else if (rig.velocity.y < -1 && rig.velocity.y > -4)
            {
                GetComponent<SpriteRenderer>().sprite = img4;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = img5;
            }
        }
        else {
            animator.enabled = true;
        }

        LevelData.timePlaying += Time.deltaTime;


		// reset grace period if we're still in contact with a wall
		if (canJump != -1 && _touchingColliders.Count > 0) {
			canJump = jumpGracePeriod;
		}

		if (jumping && canJump > 0) {
//			if(rig.velocity.y < 0) { // if we're sliding down next to a wall, then reset the y-velocity before we jump; if we're somehow accelerating up, we should keep our speed
				rig.velocity = new Vector2 (rig.velocity.x, 0);
//			}

			// jump at an angle, halfway between vertical and the normal of the wall
			// the exponent controls how sensitive the jump strength is in relation to the mass; this makes it jump slightly higher as the mass gets lighter


			if (_touchingColliders.Count <= 1) { // exiting the last collider; this has a bug if we're stuck between two walls and jumping doesn't move a single pixel
				canJump = -1; // is_currently_jumping midair
			}

			rig.AddForce (jumpForce*Mathf.Pow(rig.mass/baseMass, 0.9f) / Mathf.Sin(jumpAngle) * new Vector2 (Mathf.Cos(jumpAngle), Mathf.Sin(jumpAngle))); 

		}
		jumping = false;
		if (canJump > 0) {
			canJump -= 1; // count down the grace period
		}


		float dir = Input.GetAxisRaw ("Horizontal");
		float newXVelocity;
		if (dir == 0) {
			newXVelocity = rig.velocity.x;
		} else {
			// this is a bit iffy
			// should take another look at some point
			if (Mathf.Abs (rig.velocity.x) >= maxSpeed) {
				if (rig.velocity.x / dir > 0) { // if moving past maxSpeed and accelerating in the same direction, do nothing
					newXVelocity = rig.velocity.x;
				} else { // decelerate
					newXVelocity = rig.velocity.x + dir * maxSpeed / Mathf.Pow (rig.mass / baseMass, 0.3f);
				}
			} else { // below maxSpeed, so accelerate to max speed
				newXVelocity = dir * maxSpeed / Mathf.Pow (rig.mass / baseMass, 0.3f);
			}
		}

		// If the input is moving the player right and the player is facing left...
		if (dir > 0 && !m_FacingRight)
		{
			// ... flip the player.
			Flip();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (dir < 0 && m_FacingRight)
		{
			// ... flip the player.
			Flip();
		}

		// slowly change the current x-velocity to the desired velocity to give the feeling of smooth acceleration as we move
		float velocityX = Mathf.SmoothDamp (rig.velocity.x, newXVelocity, ref velocityXSmoothing, canJump > 0 ? accelerationTimeGround : accelerationTimeAirborne);

		rig.velocity = new Vector2 (velocityX, rig.velocity.y);

		if (rig.velocity == Vector2.zero) {
			timeIdle += Time.deltaTime;
		} else {
			timeIdle = 0;
		}

		// lost too much mass; RIP
		if (rig.mass < minMass) {
			Die();
		}

		// scale the blob based on its current mass
		transform.localScale = Mathf.Pow(rig.mass/baseMass, 1f/3f) * new Vector2(facing, 1); // if we have penalty, then we should scale the size appropriately
                                                                                  // use cube root so the ball is more visible at small mass
	}


	public void Die() {
		if (!isDead) {
			isDead = true;
			StartCoroutine (die_coroutine ());
		}
	}

	IEnumerator die_coroutine(){
		rig.position = new Vector2 (-200, -200); // move off screen to exit all collisions
		yield return new WaitForSeconds(0.5F); // allow enough time to trigger OnCollisionExit
		//Debug.Log ("dieded2");
		Destroy (this.gameObject);
		// death animation?
	}


	void OnCollisionEnter2D(Collision2D otherObj) {

		if (otherObj.gameObject.tag == "Surface") {
			// entering collision with a wall

            animatingJump = false;
			Collider2D collider = otherObj.collider;

			Vector3 contactPoint = otherObj.contacts[0].point;
			Vector2 collisionNormal = otherObj.contacts[0].normal;
			float normAngle = Mathf.Atan (Mathf.Abs(collisionNormal.y / collisionNormal.x));

			if (collisionNormal == new Vector2 (0, -1)) {
				//Debug.Log ("vertical");
				return; // disallow vertical jump
			}

//			if (Mathf.Abs (collisionNormal.x) > Mathf.Abs (collisionNormal.y)) {
//				// horizontal collision
//			} else {
//				//vertical collision
//
//				// if we want to be able to jump off ceilings, then we need to model it similar to jumping off left/right walls
//			}

			if (collisionNormal.x > 0) {
				// colliding from the right
				jumpAngle = Mathf.Abs (Mathf.PI * 0.5f + normAngle) / 2f;
			} else { // normal angle is between pi/2 and pi, so we need to take average slighly differently since Tan(abs(y/x)) gives 180-desired_angle
				jumpAngle = Mathf.Abs (Mathf.PI * 1.5f - normAngle) / 2f;
			}

			canJump = jumpGracePeriod;

			_touchingColliders.Add(new customDataWithColliders(collider, jumpAngle));

		}
	}


	void OnCollisionExit2D(Collision2D otherObj){
		if (otherObj.gameObject.tag == "Surface") {
			// update which walls we're still in contact with
			if (_touchingColliders.Count > 0) {
				jumpAngle = _touchingColliders.Last()._jumpAngle; // this is so that if we're exiting the last collision but still in our grace period, we use the last jump angle
				_touchingColliders.Remove(new customDataWithColliders(otherObj.collider, jumpAngle));
				if (_touchingColliders.Count > 0) {
					jumpAngle = _touchingColliders.Last ()._jumpAngle;
				}
			}
		}

	}

	// stores jump angle corresponding to each collider
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

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		facing *= -1;
	}
}
