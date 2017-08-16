using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// generic script for triggering a button
public class ButtonTriggers : MonoBehaviour {

	public string targetScript;
	public string exitTargetScript;
	public float minTriggerMass; // mass required in contact to trigger the button

	private float totalMass = 0;
	private ContactPoint2D[] collisions = new ContactPoint2D[1000];
	private bool activated = false;

	void FixedUpdate() {
		totalMass = 0;
		var num = GetComponent<Rigidbody2D>().GetContacts (collisions);

		// total mass of all objects in contact
		for (int i = 0; i < num; i++) {
			if (collisions [i].collider.tag == "Player" || collisions [i].collider.tag == "BlobMass") {
				totalMass += collisions [i].rigidbody.mass;
			}
		}

		// reached minimum trigger mass --> send a message to all children to call their triggerScript
		if(!activated && totalMass >= minTriggerMass) {
			gameObject.BroadcastMessage(targetScript);
			activated = true;
		}

		// de-trigger script, if any
		if(activated && totalMass < minTriggerMass) {
			gameObject.BroadcastMessage(exitTargetScript);
			activated = false;
		}
	}
}