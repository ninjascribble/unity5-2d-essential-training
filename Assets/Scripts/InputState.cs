using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputState : MonoBehaviour {

	public bool actionButton;
	public float absVelX = 0f;
	public float absVelY = 0f;
	public bool standing;
	public float standingThreshold = 1f;

	public Rigidbody2D body2d;

	void Awake () {
		body2d = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		actionButton = Input.anyKeyDown;
	}

	// Important: All physics calculations should go here
	// instead of in Update
	void FixedUpdate () {
		absVelX = Mathf.Abs (body2d.velocity.x);
		absVelY = Mathf.Abs (body2d.velocity.y);
		standing = absVelY < standingThreshold;
	}
}
