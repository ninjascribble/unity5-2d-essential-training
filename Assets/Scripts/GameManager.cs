using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public GameObject playerPrefab;
	public Text continueText;
	public Text scoreText;

	private float blinkTime = 0f;
	private float timeElapsed = 0f;
	private float bestTime = 0f;
	private GameObject floor;
	private Spawner spawner;
	private GameObject player;
	private TimeManager timeManager;
	private bool gameStarted;
	private bool blink;
	private bool beatBestTime;

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

		bestTime = PlayerPrefs.GetFloat ("BestTime");
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

			var textColor = beatBestTime ? "#FF0" : "#FFF";

			continueText.canvasRenderer.SetAlpha (blink ? 0 : 1);
			scoreText.text = "TIME: " + formatTime (timeElapsed) + "\n<color=" + textColor + ">BEST: " + formatTime (bestTime) + "</color>";
		} else {
			timeElapsed += Time.deltaTime;
			scoreText.text = "TIME: " + formatTime (timeElapsed);
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

		if (timeElapsed > bestTime) {
			bestTime = timeElapsed;
			PlayerPrefs.SetFloat ("BestTime", bestTime);
			beatBestTime = true;
		}
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
		timeElapsed = 0f;
		beatBestTime = false;
	}

	string formatTime (float value) {
		int m = (int) value / 60;
		int s = (int) value % 60;
		return string.Format("{0:00}:{1:00}", m, s);
	}
}
