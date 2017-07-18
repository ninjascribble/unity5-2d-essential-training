using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffscreen : MonoBehaviour {

	public float offset = 16f;

	private bool offscreen;
	private float offscreenX = 0;
	private Rigidbody2D body2d;

	void Awake () {
		body2d = GetComponent<Rigidbody2D> ();
	}

	// Use this for initialization
	void Start () {
		// Get the actual pixels of the screen based on current resolution
		offscreenX = (Screen.width / PixelPerfectCamera.pixelsToUnits) / 2 + offset;
	}
	
	// Update is called once per frame
	void Update () {
		var posX = transform.position.x;
		var dirX = body2d.velocity.x;

		if (Mathf.Abs (posX) > offscreenX) {

			// Offscreen to the left
			if (dirX < 0 && posX < offscreenX * -1) {
				offscreen = true;
			}

			// Offscreen to the right
			else if (dirX > 0 && posX > offscreenX) {
				offscreen = true;
			}

			// Onscreen
			else {
				offscreen = false;
			}

			if (offscreen == true) {
				OnOutOfBounds ();
			}
		}
	}

	void OnOutOfBounds () {
		offscreen = false;
		GameObjectUtil.Destroy (gameObject);
	}
}
