using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MiddleStates { Blue, Red, Green };

public class CircleController : MonoBehaviour {


	[SerializeField]
	private Vector3 circlePosition;

	private static MiddleStates currentMiddleState = MiddleStates.Red;

	private GameObject currentCircle;
	private bool gameStart;
	public bool GameStart{ get { return gameStart; } }
	public static bool gameOver;
	private static float timer;
	private float screenWidthReference;
	private float minVerticalHeightReference;
	private float maxVerticalHeightReference;  // don't want to be able to touch the top and bottom
	// extremes of the screen

	private static int comboCounter;


	void Awake() {

		currentCircle = Instantiate (GameManager.instance.getCurrentCircle (), 
			circlePosition, Quaternion.identity) as GameObject; //array with all the circle models

	}

	// Use this for initialization
	void Start () {
		//currentCircle.transform.position = circlePosition;

		screenWidthReference = Screen.width / 2;
		minVerticalHeightReference = Screen.height * 0.1f;
		maxVerticalHeightReference = Screen.height * 0.9f;
		gameStart = false;
		gameOver = false;
		comboCounter = 0;

	}

	public static void setTimer(float timerGameManager) {
		timer = timerGameManager;
	}

	public static float getTimer() {
		return timer;
	}
	
	// Update is called once per frame
	void Update () {

		if (timer <= 0) {
			gameOver = true;
		}

		if (gameOver) {
			GameManager.instance.gameOver ();
		}

		#if UNITY_ANDROID

		if(!gameStart && !UIManager.instance.ShowTrailStarted) {
			if( Input.touchCount > 0) {
				gameStart = true;
					GameObject.FindWithTag("formSpawner").GetComponent<FormsSpawner>().startSpawningForms();
			}

		}

		if(gameStart && !gameOver && Time.timeScale == 1.0f) { // if the game is paused the circle cannot move
			if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began 
				&& Input.GetTouch(0).position.y >= minVerticalHeightReference && Input.GetTouch(0).position.y <= maxVerticalHeightReference) { // don't want to be able to touch the top and bottom
				// extremes of the screen
				
				if( Input.GetTouch(0).position.x < screenWidthReference ) {
					currentCircle.transform.Rotate(new Vector3(0, 0, 60));
					currentMiddleState++;
			} else if ( Input.GetTouch(0).position.x > screenWidthReference) {
					currentCircle.transform.Rotate(new Vector3(0, 0, -60));
					currentMiddleState--;
			}
		
			}
		timer -= Time.deltaTime;
		}
				
		
		
		#elif UNITY_STANDALONE_WIN
			
		if (!gameStart && !UIManager.instance.ShowTrailStarted) { // if the game did not start and if the trail finished we test this -> so the game can start
				float h = Input.GetAxis ("Horizontal");
				if (h != 0) {
					gameStart = true;
					GameObject.FindWithTag ("formSpawner").GetComponent<FormsSpawner> ().startSpawningForms ();
				}
			}




		if (gameStart && !gameOver && Time.timeScale == 1.0f) { // if the game is paused the circle cannot move
				if (Input.GetKeyDown (KeyCode.RightArrow)) {
					currentCircle.transform.Rotate (new Vector3 (0, 0, -60));
					currentMiddleState++;
				} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
					currentCircle.transform.Rotate (new Vector3 (0, 0, 60));
					currentMiddleState--;
				}

				timer -= Time.deltaTime;
			}


		#elif UNITY_IOS

		if(!gameStart && !UIManager.instance.ShowTrailStarted) {
		if( Input.touchCount > 0) {
		gameStart = true;
		GameObject.FindWithTag("formSpawner").GetComponent<FormsSpawner>().startSpawningForms();
		}

		}

		if(gameStart && !gameOver) {
		if( Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {

		if( Input.GetTouch(0).position.x < screenWidthReference ) {
		currentCircle.transform.Rotate(new Vector3(0, 0, 60));
		currentMiddleState++;
		} else if ( Input.GetTouch(0).position.x > screenWidthReference) {
		currentCircle.transform.Rotate(new Vector3(0, 0, -60));
		currentMiddleState--;
		}

		}
		timer -= Time.deltaTime;
		}



		#endif

		if (gameStart) {
			UIManager.instance. SetActivatePointer (false); // deactive the ui pointer when the game starts
		}

	}

	public static MiddleStates getCurrentMiddleState() {
		return currentMiddleState;
	}

	public bool getGameOverState() {
		return gameOver;
	}

	public void setGameOverTrue() {
		gameOver = true;
	}

	public static void incrementComboCounter() {
		comboCounter++;
	}

	public static void addTimeFromTimeDrop() {
		timer += 5f;
	}

	public static void resetCounter() {
		comboCounter = 0;
	}

	public static int getComboCounter() {
		return comboCounter;
	}
		
}
