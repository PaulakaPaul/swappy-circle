using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	[SerializeField]
	private Text scoreText;
	[SerializeField]
	private Text timerText;
	[SerializeField]
	private GameObject gameOverPanel, pausePanel;
	[SerializeField]
	private Text gameoverScoreText;
	[SerializeField]
	private GameObject twoTimesCombo, threeTimesCombo;


	[SerializeField]
	private GameObject heart1, heart2;


	private Animator animht1, animht2; 


	public static UIManager instance;
	private CircleController circleControllerReference;
	private readonly float gameOverPanelRaiseTime = 0.5f;


	private UIManager() {}

	void Awake() {
		
		if (instance == null) {
			instance = this;
		} 

		circleControllerReference = GameObject.FindWithTag ("circleController").GetComponent<CircleController> ();

		twoTimesCombo.SetActive (false);
		threeTimesCombo.SetActive (false);

		animht1 = heart1.GetComponent<Animator> ();
		animht2 = heart2.GetComponent<Animator> ();

		// activate transparent hearts if you have less than 3 lifes
		switch (GameManager.instance.getLifes()) {

		case 1:
			//heart2.GetComponent<Image> ().SetTransparency (0f);
			heart2.SetActive(false);
			goto case 2;
		case 2: 
			//heart1.GetComponent<Image> ().SetTransparency (0f);
			heart1.SetActive(false);
			break;
		}
	}



	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		heart1.GetComponent<Image> ().SetTransparency (0.5f);

		if (!circleControllerReference.getGameOverState ()) {
			
			scoreText.text = ScoreManager.instance.getScore ().ToString();
			float t = CircleController.getTimer ();
			timerText.text = t > 0 ? (t >= 10 ? ((int)t).ToString () : "0" + (int) t ) : "00";

			InstantiateComboUI ();
		}
	}

	private void gameOverPanelCall() {
		gameOverPanel.SetActive (true);
	}

	public void gameOverUIManager() {
		
		gameoverScoreText.text = "";

		if (GamePreferences.GetEasyDifficultyState () == 1) {
			gameoverScoreText.text = GamePreferences.GetEasyDifficultyHighscore ().ToString();
		} else if (GamePreferences.GetMediumDifficultyState () == 1) {
			gameoverScoreText.text = GamePreferences.GetMediumDifficultyHighscore ().ToString();
		} else if (GamePreferences.GetHardDifficultyState () == 1) {
			gameoverScoreText.text = GamePreferences.GetHardDifficultyHighscore ().ToString();
		}
		Invoke ("gameOverPanelCall", gameOverPanelRaiseTime);
	}


	// 3 button functions
	public void replay() {
		GameManager.instance.SetGameStartedFromMenuTrue ();
		reloadGamePlay ();
	}

	public void replayWithAnotherLife() {
		Invoke ("reloadGamePlay", gameOverPanelRaiseTime + 0.2f);
	}

	public void menu() {
		SceneManager.LoadScene (GameManager.menuScene);
	}


	private void reloadGamePlay() {
		//SceneManager.LoadScene (GameManager.playScene);
		SceneFader.instance.fadeIn(GameManager.playScene);
	}

	public void pauseGame() {
		pausePanel.SetActive (true);
		Time.timeScale = 0 ; 
	}
		

	public void resumeGame() {
		Time.timeScale = 1;
		pausePanel.SetActive (false);
	}

	public void menuPausePanel() {
		Time.timeScale = 1;
		//SceneManager.LoadScene (GameManager.menuScene);
		SceneFader.instance.fadeIn(GameManager.menuScene);
	}

	private void InstantiateComboUI() {
		int c = CircleController.getComboCounter ();

		if (c == 4) {
			twoTimesCombo.SetActive (true);
			StartCoroutine(deactivateObjectAfterAnAmountOfTime (twoTimesCombo, 1.5f));
		} else if (c == 8) {
			threeTimesCombo.SetActive (true);
			StartCoroutine(deactivateObjectAfterAnAmountOfTime (threeTimesCombo, 1.5f));
		}

	}

	private IEnumerator deactivateObjectAfterAnAmountOfTime(GameObject o, float deleteTime) {
		yield return new WaitForSeconds (deleteTime);
		o.SetActive (false);
	}

	public Animator getAnimht1() {
		return animht1;
	}

	public Animator getAnimht2() {
		return animht2;
	}


}
