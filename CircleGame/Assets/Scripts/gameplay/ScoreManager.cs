using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {


	public static ScoreManager instance;
	private int score;
	private int lifes;

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

	private void counterChecker(int comboCounter,int formAmount) {
		if (comboCounter < 4) {
			score += formAmount;
		} else if (comboCounter < 8) {
			score += formAmount * 2;
		} else {
			score += formAmount * 3;
		}
	}

	public void incrementCube() {
		CircleController.incrementComboCounter ();
		counterChecker (CircleController.getComboCounter(), 1); // gets the comboCounter data from the Blue/Red/Green checkers 
	}

	public void incrementStar() {
		CircleController.incrementComboCounter ();
		counterChecker (CircleController.getComboCounter(), 3); // gets the comboCounter data from the Blue/Red/Green checkers 
	}

	public void incrementDiamond() {
		CircleController.incrementComboCounter ();
		counterChecker (CircleController.getComboCounter(), 5); // gets the comboCounter data from the Blue/Red/Green checkers 
	}

	public void incrementCoins() {
		if (GamePreferences.GetEasyDifficultyState () == 1) {
			GamePreferences.SetCoinScore(GamePreferences.GetCoinScore() + 1);
		} else if (GamePreferences.GetMediumDifficultyState () == 1) {
			GamePreferences.SetCoinScore(GamePreferences.GetCoinScore() + 2);
		} else if (GamePreferences.GetHardDifficultyState () == 1) {
			GamePreferences.SetCoinScore(GamePreferences.GetCoinScore() + 3);
		}

		Debug.Log ("coins: " + GamePreferences.GetCoinScore ());
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
