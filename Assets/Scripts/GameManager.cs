using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public GameObject playerPrefab;
	public Text continueText;

	private float blinkTime = 0f;
	private GameObject floor;
	private Spawner spawner;
	private GameObject player;
	private TimeManager timeManager;
	private bool gameStarted;
	private bool blink;

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
		continueText.text = "PRESS ANY BUTTON TO START";

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

		if (gameStarted == false) {
			blinkTime++;

			if (blinkTime % 40 == 0) {
				blink = !blink;
			}

			continueText.canvasRenderer.SetAlpha (blink ? 0 : 1);
		}
	}

	void OnPlayerKilled () {
		var destroyScript = player.GetComponent<DestroyOffscreen> ();

		destroyScript.DestroyCallback -= OnPlayerKilled;
		spawner.active = false;
		player.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		timeManager.ManipulateTime (0, 5.5f);
		gameStarted = false;
		continueText.text = "PRESS ANY BUTTON TO RESTART";
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
		continueText.canvasRenderer.SetAlpha (0);
	}
}
