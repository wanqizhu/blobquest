using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityStandardAssets._2D
{
    public class Salt : MonoBehaviour
    {
		public float saltiness; // how much mass the blob loses per frame of contact


        private void OnTriggerStay2D(Collider2D coll)
        {
			
			if (coll.tag == "Player")
            {
				coll.gameObject.GetComponent<Rigidbody2D> ().mass -= saltiness;
			}else if(coll.tag == "BlobMass"){ // kills propellent instantly
				Destroy(coll.gameObject);
			}
        }
    }
}
