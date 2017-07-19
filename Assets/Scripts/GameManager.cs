using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject playerPrefab;

	private GameObject floor;
	private Spawner spawner;
	private GameObject player;

	// Use this for initialization
	void Awake () {
		floor = GameObject.Find ("Foreground");
		spawner = GameObject.Find ("Spawner").GetComponent<Spawner>();
	}

	void Start () {
		var floorHeight = floor.transform.localScale.y;
		var floorPos = floor.transform.position;

		floorPos.x = 0;
		floorPos.y = (Screen.height / PixelPerfectCamera.pixelsToUnits / 2) * -1 + floorHeight / 2;
		floor.transform.position = floorPos;

		spawner.active = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!player || player.active != true) {
			Reset ();
		}
	}

	void Reset () {
 	 	var playerX = 0;
		var playerY = Screen.height / PixelPerfectCamera.pixelsToUnits / 2;
		var playerZ = 0;

		player = GameObjectUtil.Instantiate (playerPrefab, new Vector3 (playerX, playerY, playerZ));

		spawner.active = true;
	}
}
