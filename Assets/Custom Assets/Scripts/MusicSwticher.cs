using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSwticher : MonoBehaviour {
	public AudioClip replace;
	public bool loop = false;
	// Use this for initialization
	void Start () {
		AudioSource s = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<AudioSource> ();
		s.Stop ();
		s.clip = replace;
		s.Play ();
		s.loop = loop;
	}
}
