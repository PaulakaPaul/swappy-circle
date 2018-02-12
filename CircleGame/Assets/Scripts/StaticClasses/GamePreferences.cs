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

	public const string CircleIndex = "CircleIndex"; 
	public const string DropIndex = "DropIndex";
	public const string BuffIndex = "BuffIndex";

	public static string CoinScore = "CoinScore";

	public static string MusicVolume = "MusicVolume";
	public const string LastMusicVolume = "LastMusicVolume"; // we use this to restore the music when we use the on off button from the gameplay
	public const string ShowTrail = "ShowTrail";

	// NOTE we are going to use integers to represent boolean variables
	// 0 - false, 1 - true

	public static float getMusicState() {
		return PlayerPrefs.GetFloat (GamePreferences.MusicVolume);
	}

	public static void SetMusicState(float volume) {
		if(volume >= 0.0f && volume <= 1.0f) // the audio source volume it's called between 0 and 1
			PlayerPrefs.SetFloat (GamePreferences.MusicVolume, volume);
	}

	public static float getLastMusicState() {
		return PlayerPrefs.GetFloat (GamePreferences.LastMusicVolume);
	}

	public static void SetLastMusicState(float volume) {
		if(volume >= 0.0f && volume <= 1.0f) // the audio source volume it's called between 0 and 1
			PlayerPrefs.SetFloat (GamePreferences.LastMusicVolume, volume);
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

	public static void SetCircleIndex(int index) {
		if(index >= 0 && index < GameManager.instance.getCircleModelsLength())
			PlayerPrefs.SetInt (GamePreferences.CircleIndex, index);
	}

	public static int GetCircleIndex() {
		return PlayerPrefs.GetInt (GamePreferences.CircleIndex);
	}

	public static void SetDropIndex(int index) {
		if (index >= 0 && index < GameManager.instance.getDropModelsLength ())
			PlayerPrefs.SetInt (GamePreferences.DropIndex, index);
	}

	public static int GetDropIndex() {
		return PlayerPrefs.GetInt (GamePreferences.DropIndex);
	}

	public static void SetTrailState(int state) {
		PlayerPrefs.SetInt (GamePreferences.ShowTrail, state);
	}

	public static int GetTrailState() {
		return PlayerPrefs.GetInt (GamePreferences.ShowTrail);
	}
}
