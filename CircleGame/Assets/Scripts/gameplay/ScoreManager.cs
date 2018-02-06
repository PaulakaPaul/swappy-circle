using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {


	public static ScoreManager instance;
	private int score;
	private int lifes;

	public const int TWO_TIMES_COMBO_LIMIT = 4;
	public const int THREE_TIMES_COMBO_LIMIT = 8;

	private ScoreManager() {}


	void Awake() {

		if (instance == null) {
			instance = this;
		} 	
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDisable() {
		gameOverScoreManager (); // save the high score when the scene it's left or we start the game with a life less
	}

	private void counterChecker(int comboCounter,int formAmount) {

		if (GameManager.instance.getDoubleScoreState ())
			formAmount *= 2; // it means that we have the double coins buff activated

		if (comboCounter < TWO_TIMES_COMBO_LIMIT) {
			score += formAmount;
		} else if (comboCounter < THREE_TIMES_COMBO_LIMIT) {
			score += formAmount * 2;
		} else {
			score += formAmount * 3;
		}

		//  show the combo gui if needed
		GameObject.FindGameObjectWithTag ("uimanager").GetComponent<UIManager> ().InstantiateComboUI ();
	}

	public void incrementCube() {
		CircleController.incrementComboCounter ();
		counterChecker (CircleController.getComboCounter(), 3); // gets the comboCounter data from the Blue/Red/Green checkers 
	}

	public void incrementStar() {
		CircleController.incrementComboCounter ();
		counterChecker (CircleController.getComboCounter(), 5); // gets the comboCounter data from the Blue/Red/Green checkers 
	}

	public void incrementDiamond() {
		CircleController.incrementComboCounter ();
		counterChecker (CircleController.getComboCounter(), 8); // gets the comboCounter data from the Blue/Red/Green checkers 
	}

	public void incrementCoins() {

		incrementCoinsLevelLogic ();

		if(GameManager.instance.getDoubleCoinState()) // it means we have the double coin buff activated -> we repeat the coin increment logic
			incrementCoinsLevelLogic(); 
	}

	private void incrementCoinsLevelLogic() {
		if (GamePreferences.GetEasyDifficultyState () == 1) {
			GamePreferences.SetCoinScore(GamePreferences.GetCoinScore() + 1);
		} else if (GamePreferences.GetMediumDifficultyState () == 1) {
			GamePreferences.SetCoinScore(GamePreferences.GetCoinScore() + 2);
		} else if (GamePreferences.GetHardDifficultyState () == 1) {
			GamePreferences.SetCoinScore(GamePreferences.GetCoinScore() + 3);
		}
	}

	public void decrementLife() {
		lifes--;
		GameManager.instance.ScoreChecker (score, lifes);
	}

	public void takeDataFromGameManager(int score, int lifes) {
		this.score = score;
		this.lifes = lifes;
	}
		
		
	public void gameOverScoreManager() {

		if (GamePreferences.GetEasyDifficultyState () == 1) {
			if (GamePreferences.GetEasyDifficultyHighscore () < score) {
				GamePreferences.SetEasyDifficultyHighScore (score);
			}
		} else if (GamePreferences.GetMediumDifficultyState () == 1) {
			if (GamePreferences.GetMediumDifficultyHighscore () < score) {
				GamePreferences.SetMediumDifficultyHighScore (score);
			}
		} else if (GamePreferences.GetHardDifficultyState () == 1) {
			if (GamePreferences.GetHardDifficultyHighscore () < score) {
				GamePreferences.SetHardDifficultyHighScore (score);
			}
		} 
	} 

	public int getScore() {
		return score;
	}

	public int getLifes() {
		return lifes;
	}
}
