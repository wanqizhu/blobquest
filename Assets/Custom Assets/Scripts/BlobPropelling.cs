using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this handles propelling behavior
public class BlobPropelling : MonoBehaviour {

	public float    fireForce;
	public GameObject propellentBlob; // the object being shot out
	public float propellentMass;

	private bool     mousePressed;
	private Rigidbody2D rig;
	private Vector3    heading;
	private float    distance;
	private Vector3    direction;
	private BlobMovement moveScript;


	void Start () {
		rig = GetComponent<Rigidbody2D>();
		moveScript = GetComponent<BlobMovement> ();
	}
	
	// Update is called once per frame
	void Update () {
		// mousePressed will trigger this if we hold the mouse key down, so we'll shoot continuously rather than once-per-click
		if ((Input.GetMouseButtonDown(0) || mousePressed) && !LevelData.paused && !LevelData.ended){

			mousePressed = true;

			heading = Input.mousePosition;
			heading = Camera.main.ScreenToWorldPoint(heading);
			heading -=  transform.position;
			heading.z = 0f;
			distance = heading.magnitude;
			direction = heading / distance; // this is a unit vector pointing in the direction of the mouse

			Vector2 jumpVec = new Vector2(direction.x,direction.y) * fireForce * Mathf.Pow(rig.mass, 0.9f);

			// fire in the right direction; if we're facing left, then we need to negate transform.localscale.x (see Flip script in BlobMovement)
			var scale = moveScript.m_FacingRight ? transform.localScale.x : -transform.localScale.x;

			GameObject propellent = Instantiate(propellentBlob, transform.position + direction * scale, Quaternion.identity) as GameObject;
			Rigidbody2D propellentBody = propellent.GetComponent<Rigidbody2D> ();

			// shoot out the mass
			propellentBody.mass = propellentMass;
			rig.mass -= propellentBody.mass;

			rig.AddForce (-jumpVec);
			// scale down the force for the propellent since it has MUCH lower mass (so it doesn't fly off screen)
			propellent.GetComponent<Rigidbody2D>().AddForce(jumpVec / Mathf.Pow(rig.mass,0.9f)*.175f);//Mathf.Pow(propellentMass, 1f/2f)
		

		}

		if (Input.GetMouseButtonUp(0)){
			mousePressed = false;
		}
	}
}
