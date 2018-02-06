using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public static readonly string playScene = "level1";
	public static readonly string menuScene = "menu";
	public static readonly string levelsScene = "levels";
	public const string shopScene = "shop";

	private bool gameStartedFromMenu = false ;
	private bool gameReplayed = false;

	private int score, lifes;
	private float timer;


	// items that have to be accesed in any moment of the gameplay
	[SerializeField]
	private GameObject[] circleModels;
	[SerializeField]
	private MultiDimensionalObject[] dropModels;


	public GameObject[] CircleModels {
		get { return circleModels; }
	}

	public MultiDimensionalObject[] DropModels {
		get { return dropModels; } 
	}


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



	private void MySceneLoaderChecker (Scene scene, LoadSceneMode mode) {
		
		if (scene.name.Equals(playScene)) { // if we load the play scene
			if (gameStartedFromMenu) {
				//setting data for the start of the game
				score = 0;
				lifes = 3;
				timer = 120f;

				//making the view
			//	UIManager.instance.resetLifes();

				//seding data to game controllers
				ScoreManager.instance.takeDataFromGameManager (score, lifes);
				CircleController.setTimer (timer);
			} else if(gameReplayed) {

				// making the view
				UIManager.instance.changeLifesUi (lifes);

				// sending data to game controllers
				CircleController.setTimer (timer);
				ScoreManager.instance.takeDataFromGameManager (score, lifes);

			}

			// we send all the data to other classes during the game so  they will process the data
		}

	}


	public void ScoreChecker(int score, int lifes) { // this is called when we replay a game with another life

		if (ScoreManager.instance.getLifes () <= 0) {
			// the gameOver() function it is called from the circleController if this variable it's true
			GameObject.FindWithTag ("circleController").GetComponent<CircleController> ().setGameOverTrue ();
		} else if (ScoreManager.instance.getLifes() > 0) {
			
			gameStartedFromMenu = false;			
			gameReplayed = true;

			this.score = score;
			this.lifes = lifes;

			this.timer = CircleController.getTimer ();


			/*// activate transparent hearts if you have less than 3 lifes
			switch (lifes) {
			case 1:
				UIManager.instance.getAnimht2 ().SetBool ("minusLife", true);
				goto case 2;
			case 2 : 
				UIManager.instance.getAnimht1 ().SetBool ("minusLife", true);
				break;
			} */ 


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
		if (!PlayerPrefs.HasKey ("Game Initialized!")) {
			GamePreferences.SetEasyDifficultyState (1);
			GamePreferences.SetEasyDifficultyHighScore (0);

			GamePreferences.SetMediumDifficultyState (0);
			GamePreferences.SetMediumDifficultyHighScore (0);


			GamePreferences.SetHardDifficultyState (0);
			GamePreferences.SetHardDifficultyHighScore (0);

			GamePreferences.SetCoinScore (0);
			GamePreferences.SetMusicState (0.5f);
			GamePreferences.SetLastMusicState (0.5f); // we use this to go back to the last music state when we use the on/off button

			GamePreferences.SetCircleIndex (0);
			GamePreferences.SetDropIndex (0);

			GamePreferences.SetTrailState (1); // we show the first time the trail

			PlayerPrefs.SetInt ("Game Initialized!", 1); // game is initialized
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

	public int getLifes() {
		return lifes;
	}

	public bool getReplayStatus() {
		return gameReplayed;
	}

	public GameObject getCurrentCircle() {
		return circleModels [GamePreferences.GetCircleIndex ()];
	}

	public int getCircleModelsLength() {
		return circleModels.Length;
	}

	public MultiDimensionalObject getCurrentDrops() {
		return dropModels [GamePreferences.GetDropIndex ()];
	}

	public int getDropModelsLength() {
		return dropModels.Length;
	}

	public bool getDoubleCoinState() {
		return GameSaver.instance.getItemStatus (GameSaver.BUFF, 0); // it's index is 0
	}

	public bool getDoubleScoreState() {
		return GameSaver.instance.getItemStatus (GameSaver.BUFF, 1); // it's index is 1
	}
}
