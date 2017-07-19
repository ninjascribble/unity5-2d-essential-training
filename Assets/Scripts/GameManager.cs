﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject playerPrefab;

	private GameObject floor;
	private Spawner spawner;
	private GameObject player;
	private TimeManager timeManager;
	private bool gameStarted;

	// Use this for initialization
	void Awake () {
		floor = GameObject.Find ("Foreground");
		spawner = GameObject.Find ("Spawner").GetComponent<Spawner>();
		timeManager = GetComponent<TimeManager> ();
	}

	void Start () {
		var floorHeight = floor.transform.localScale.y;
		var floorPos = floor.transform.position;

		floorPos.x = 0;
		floorPos.y = (Screen.height / PixelPerfectCamera.pixelsToUnits / 2) * -1 + floorHeight / 2;
		floor.transform.position = floorPos;

		spawner.active = false;

		Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameStarted == false && Time.timeScale == 0) {
			if (Input.anyKeyDown) {
				timeManager.ManipulateTime (1f, 1f);
				Reset ();
			}
		}
	}

	void OnPlayerKilled () {
		var destroyScript = player.GetComponent<DestroyOffscreen> ();

		destroyScript.DestroyCallback -= OnPlayerKilled;
		spawner.active = false;
		player.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		timeManager.ManipulateTime (0, 5.5f);
		gameStarted = false;
	}

	void Reset () {
 	 	var playerX = 0;
		var playerY = Screen.height / PixelPerfectCamera.pixelsToUnits / 2 + 100;
		var playerZ = 0;

		player = GameObjectUtil.Instantiate (playerPrefab, new Vector3 (playerX, playerY, playerZ));

		var destroyScript = player.GetComponent<DestroyOffscreen> ();

		destroyScript.DestroyCallback += OnPlayerKilled;
		spawner.active = true;
		gameStarted = true;
	}
}
