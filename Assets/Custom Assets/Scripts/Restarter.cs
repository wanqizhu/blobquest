using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// script for an object that restarts the level upon contact
namespace UnityStandardAssets._2D
{
    public class Restarter : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
			if (other.tag == "Player") {
				other.gameObject.GetComponent<BlobMovement> ().Die ();
			} else if (other.tag == "BlobMass") {
				Destroy (other.gameObject);
			} else if (other.tag == "Goal" && !SceneManager.GetSceneByName("LevelFailed").isLoaded) {
				Time.timeScale = 0;
				LevelData.paused = true;
				SceneManager.LoadScene ("LevelFailed", LoadSceneMode.Additive);
			}
        }
    }
}
