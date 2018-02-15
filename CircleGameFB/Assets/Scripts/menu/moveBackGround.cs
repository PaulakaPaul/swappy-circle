using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBackGround : MonoBehaviour {


	[SerializeField]
	private float speed;
	private Vector2 offset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		moveBackground ();

	}

	private void moveBackground() {
		offset = new Vector2 (0, Time.time * speed);
		GetComponent<Renderer> ().material.SetTextureOffset ("_MainTex", offset);
	}
}
