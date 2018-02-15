using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsController : MonoBehaviour {

	[SerializeField]
	private GameObject EasySign, MediumSign, HardSign;
	[SerializeField]
	private Text highScore, coinDetailText;

	private readonly string coinDetailTextFormat = "x{0} coin drop";

	// Use this for initialization
	void Start () {
		InitializeOptionsScene ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void backMenu() {
		SceneFader.instance.fadeIn(GameManager.menuScene);
	}


	private void InitializeOptionsScene() {
		if (GamePreferences.GetEasyDifficultyState () == 1) {
			setCorrectCheckSignAndState ("easy");
		} else if (GamePreferences.GetMediumDifficultyState () == 1) {
			setCorrectCheckSignAndState ("medium");
		} else if (GamePreferences.GetHardDifficultyState () == 1) {
			setCorrectCheckSignAndState ("hard");
		}
	}

	private void setCorrectCheckSignAndState(string state) {
		switch (state) {
		case "easy": 
			EasySign.SetActive (true);
			MediumSign.SetActive (false);
			HardSign.SetActive (false);

			coinDetailText.text = String.Format (coinDetailTextFormat, 1);

			GamePreferences.SetEasyDifficultyState (1);
			GamePreferences.SetMediumDifficultyState (0);
			GamePreferences.SetHardDifficultyState (0);

			highScore.text = GamePreferences.GetEasyDifficultyHighscore ().ToString();
				break;
		case "medium":
			EasySign.SetActive (false);
			MediumSign.SetActive (true);
			HardSign.SetActive (false);

			coinDetailText.text = String.Format (coinDetailTextFormat, 2);

			GamePreferences.SetEasyDifficultyState (0);
			GamePreferences.SetMediumDifficultyState (1);
			GamePreferences.SetHardDifficultyState (0);

			highScore.text = GamePreferences.GetMediumDifficultyHighscore ().ToString();
			break;
		case "hard":
			EasySign.SetActive (false);
			MediumSign.SetActive (false);
			HardSign.SetActive (true);

			coinDetailText.text = String.Format (coinDetailTextFormat, 3);

			GamePreferences.SetEasyDifficultyState (0);
			GamePreferences.SetMediumDifficultyState (0);
			GamePreferences.SetHardDifficultyState (1);

			highScore.text = GamePreferences.GetHardDifficultyHighscore ().ToString();
			break;
		}
	}

	public void easyButtonClick() {
		setCorrectCheckSignAndState ("easy");
	}

	public void mediumButtonClick() {
		setCorrectCheckSignAndState ("medium");
	}

	public void hardButtonClick() {
		setCorrectCheckSignAndState ("hard");
	}
}
