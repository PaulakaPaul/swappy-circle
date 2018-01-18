using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MiddleStates { Blue, Red, Green };

public class CircleController : MonoBehaviour {

	[SerializeField]
	private GameObject[] circleModels;
	[SerializeField]
	private Vector3 circlePosition;

	private static MiddleStates currentMiddleState = MiddleStates.Red;

	private GameObject currentCircle;
	private static int currentCirleIndex = 0;
	private bool gameStart;
	public static bool gameOver;
	private static float timer;
	private float screenWidthReference;

	private static int comboCounter;


	void Awake() {

		currentCircle = circleModels[currentCirleIndex]; //array with all the circle models

	}

	// Use this for initialization
	void Start () {
		currentCircle.transform.position = circlePosition;
		screenWidthReference = Screen.width / 2;
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

		if(!gameStart) {
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
				
		
		
		#elif UNITY_STANDALONE_WIN
			
			if (!gameStart) {
				float h = Input.GetAxis ("Horizontal");
				if (h != 0) {
					gameStart = true;
					GameObject.FindWithTag ("formSpawner").GetComponent<FormsSpawner> ().startSpawningForms ();
				}
			}




			if (gameStart && !gameOver) {
				if (Input.GetKeyDown (KeyCode.RightArrow)) {
					currentCircle.transform.Rotate (new Vector3 (0, 0, -60));
					currentMiddleState++;
				} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
					currentCircle.transform.Rotate (new Vector3 (0, 0, 60));
					currentMiddleState--;
				}

				timer -= Time.deltaTime;
			}

		#endif

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

	public static void resetCounter() {
		comboCounter = 0;
	}

	public static int getComboCounter() {
		return comboCounter;
	}
}
