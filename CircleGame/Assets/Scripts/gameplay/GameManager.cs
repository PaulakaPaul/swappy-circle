using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public static string playScene = "level1";
	public static string menuScene = "menu";
	public static string levelsScene = "levels";

	private bool gameStartedFromMenu = false ;
	private bool gameReplayed = false;

	private int score, lifes;
	private float timer;


	private GameManager() {}

	void Awake() {

	
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}


	}

	void OnEnable() {
		SceneManager.sceneLoaded += MySceneLoaderChecker;
	}

	void OnDisable() {
		SceneManager.sceneLoaded -= MySceneLoaderChecker;
	}



	void MySceneLoaderChecker (Scene scene, LoadSceneMode mode) {

		if (scene.name.Equals(playScene)) {
			if (gameStartedFromMenu) {
				score = 0;
				lifes = 3;
				timer = 60f;
				ScoreManager.instance.takeDataFromGameManager (score, lifes);
				CircleController.setTimer (timer);
			} else if(gameReplayed) {

				// activate transparent hearts if you have less than 3 lifes
				switch (lifes) {
					case 1:
					UIManager.instance.getAnimht2 ().SetBool ("minusLife", true);
					goto case 2;
					case 2 : 
					UIManager.instance.getAnimht1 ().SetBool ("minusLife", true);
					break;
				}


				CircleController.setTimer (timer);
				ScoreManager.instance.takeDataFromGameManager (score, lifes);

			}

			// we send all the data to other classes during the game so it they can process the data

		}
	}


	public void ScoreChecker(int score, int lifes) {

		if (ScoreManager.instance.getLifes () <= 0) {
			// the gameOver() function it is called from the circleController if this variable it's true
			GameObject.FindWithTag ("circleController").GetComponent<CircleController> ().setGameOverTrue ();
		} else if (ScoreManager.instance.getLifes() > 0) {
			
			gameStartedFromMenu = false;			
			gameReplayed = true;

			this.score = score;
			this.lifes = lifes;

			this.timer = CircleController.getTimer ();

			// Reload GAME
			UIManager.instance.replayWithAnotherLife();

			// we take all the data from the other classes when the game is over or it will be restarted so it can be perpetuated to a new life change in the game
		}
	

	}

	// Use this for initialization
	void Start () {
		InitializeVariables ();
	}
		


	private void InitializeVariables() {
		if (!PlayerPrefs.HasKey ("Game Initialized")) {
			GamePreferences.SetEasyDifficultyState (0);
			GamePreferences.SetEasyDifficultyHighScore (0);

			GamePreferences.SetMediumDifficultyState (1);
			GamePreferences.SetMediumDifficultyHighScore (0);


			GamePreferences.SetHardDifficultyState (0);
			GamePreferences.SetHardDifficultyHighScore (0);

			GamePreferences.SetCoinScore (0);

			PlayerPrefs.SetInt ("Game Initialized", 1); // game is initialized
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void gameStarts() {
		
	}

	public void gameOver() {
		GameObject.FindWithTag ("formSpawner").GetComponent<FormsSpawner> ().stopSpawningForms ();
		ScoreManager.instance.gameOverScoreManager ();
		UIManager.instance.gameOverUIManager ();
	}

	public void SetGameStartedFromMenuTrue() {
			gameStartedFromMenu = true;
			gameReplayed = false;
	}




}
