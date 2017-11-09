using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GamePreferences {

	public static string EasyDifficultyState = "EasyDifficultyState";
	public static string MediumDifficultyState = "MediumDifficultyState";
	public static string HardDifficultyState = "HardDifficultyState";


	public static string EasyDifficultyHighscore = "EasyDifficultyHighscore";
	public static string MediumDifficultyHighscore = "MediumDifficultyHighscore";
	public static string HardDifficultyHighscore = "HardDifficultyHighscore";

	public static string CoinScore = "CoinScore";

	public static string IsMusicOn = "IsMusicOn";

	// NOTE we are going to use integers to represent boolean variables
	// 0 - false, 1 - true

	public static int getMusicState() {
		return PlayerPrefs.GetInt (GamePreferences.IsMusicOn);
	}

	public static void SetMusicState(int difficulty) {
		PlayerPrefs.SetInt (GamePreferences.IsMusicOn, difficulty);
	}

	public static void SetEasyDifficultyState(int difficulty) {
		PlayerPrefs.SetInt (GamePreferences.EasyDifficultyState, difficulty);
	}

	public static int GetEasyDifficultyState() {
		return PlayerPrefs.GetInt (GamePreferences.EasyDifficultyState);
	}

	public static void SetMediumDifficultyState(int difficulty) {
		PlayerPrefs.SetInt (GamePreferences.MediumDifficultyState, difficulty);
	}

	public static int GetMediumDifficultyState() {
		return PlayerPrefs.GetInt (GamePreferences.MediumDifficultyState);
	}

	public static void SetHardDifficultyState(int difficulty) {
		PlayerPrefs.SetInt (GamePreferences.HardDifficultyState, difficulty);
	}

	public static int GetHardDifficultyState() {
		return PlayerPrefs.GetInt (GamePreferences.HardDifficultyState);
	}

	public static int GetEasyDifficultyHighscore() {
		return PlayerPrefs.GetInt (GamePreferences.EasyDifficultyHighscore);
	}

	public static int GetMediumDifficultyHighscore() {
		return PlayerPrefs.GetInt (GamePreferences.MediumDifficultyHighscore);
	}

	public static int GetHardDifficultyHighscore() {
		return PlayerPrefs.GetInt (GamePreferences.HardDifficultyHighscore);
	}

	public static void SetEasyDifficultyHighScore(int score) {
		PlayerPrefs.SetInt (GamePreferences.EasyDifficultyHighscore, score);
	}

	public static void SetMediumDifficultyHighScore(int score) {
		PlayerPrefs.SetInt (GamePreferences.MediumDifficultyHighscore, score);
	}

	public static void SetHardDifficultyHighScore(int score) {
		PlayerPrefs.SetInt (GamePreferences.HardDifficultyHighscore, score);
	}

	public static void SetCoinScore(int score) {
		PlayerPrefs.SetInt (GamePreferences.CoinScore, score);
	}
		
	public static int GetCoinScore() {
		return PlayerPrefs.GetInt (GamePreferences.CoinScore);
	}

}
