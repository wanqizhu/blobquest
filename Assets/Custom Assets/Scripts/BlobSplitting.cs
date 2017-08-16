using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// handles splitting behavior
public class BlobSplitting : MonoBehaviour {

	public GameObject splitBlob; // right now the split blob is an exact copy of the original
	public float split_penalty = 0f; // if we lose mass (flat or percentage) while splitting
	public Vector2 splitForce;

	private Rigidbody2D rig;
	private bool spliting = false;
	private bool about_to_split = false;
	private BlobMovement moveScript;

	// Use this for initialization
	void Start () {
		rig = GetComponent<Rigidbody2D>();
		moveScript = GetComponent<BlobMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!about_to_split && !spliting && rig.mass > moveScript.minMass*2f) { // can't split if not enough mass
			spliting = Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
		}
	}

	void FixedUpdate() {
		if (about_to_split) {
			about_to_split = false;
			spliting = false;
			rig.mass -= split_penalty; // if percentage then divide
			rig.mass /= 2f;

			GameObject split = Instantiate (splitBlob, transform.position + new Vector3(-1, 0) * transform.localScale.x, Quaternion.identity) as GameObject;
			rig.AddForce (splitForce);
			split.GetComponent<Rigidbody2D>().AddForce (-splitForce);
		}

		// we use two variables to create a frame delay so as to not split infinitely 
		// this way, when we give the split command, it'll first set about to split to true,
		// then next frame, it'll actually split (but set about_to_split to false so the new blob won't split too)
		if (spliting) {
			about_to_split = true;
		}


	}
}
