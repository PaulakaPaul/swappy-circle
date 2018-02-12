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
	private Text gameoverScoreText, pauseScoreText;
	[SerializeField]
	private GameObject twoTimesCombo, threeTimesCombo;
	[SerializeField]
	private GameObject dropSliderTime, dropSliderRain, dropSliderUnified;


	[SerializeField]
	private GameObject heart1, heart2;
	[SerializeField]
	private GameObject pointer, trail; // helping ui for the player
	private bool showTrailStarted;// we control the trail logic with this var
	public bool ShowTrailStarted {
		get { return showTrailStarted; }
	}

	private Animator animht1, animht2; 
	[SerializeField]
	private Button showDataButton, rightArrowShowData, leftArrowShowData; //button whichs shows the current forms
	[SerializeField]
	private Sprite timeDropSprite, unifiedDropSprite, rainDropSprite;

	public static UIManager instance;
	private CircleController circleControllerReference;
	private readonly float gameOverPanelRaiseTime = 0.5f;


	public const string CUBE_DESCRIPTION = "3 points";
	public const string STAR_DESCRIPTION = "5 points";
	public const string DIAMOND_DESCRIPTION = "8 points";
	public const string TIME_DROP_DESCRIPTION = " +5 time & delay";
	public const string RAIN_DROP_DESCRIPTION = "let it rain";
	public const string UNIFIED_DROP_DESCRIPTION = "just catch it";


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


	}



	// Use this for initialization
	void Start () {

		if (GamePreferences.GetTrailState () == 1 && !GameManager.instance.getReplayStatus() && !AdManager.instance.AdOpened) // we need this to know when the trail animation and logic it's finished (maybe the user wants to see the trail
			// next time so we need an extra variable) -> this var will always be false after the buttons at the end of the anim are pressed ( we show it if the trail state it's true and the game it's not  
			// replayed with another life
			showTrailStarted = true;
		else {
			showTrailStarted = false;
			pointer.SetActive (true); // we can skip to the pointer part
		}

		if (showTrailStarted) // if everything it's ok we show the trail
			ShowTrail ();
	}
	
	// Update is called once per frame
	void Update () {



		if (!circleControllerReference.getGameOverState ()) {
			
			scoreText.text = ScoreManager.instance.getScore ().ToString();
			float t = CircleController.getTimer ();
			timerText.text = t > 0 ? (t >= 10 ? ((int)t).ToString () : "0" + (int) t ) : "00";
		}
	}

	private void gameOverPanelCall() {
		gameOverPanel.SetActive (true);
	}

	public void gameOverUIManager() {
		
		gameoverScoreText.text = GetHighScoreForCurrentLevel();
		Invoke ("gameOverPanelCall", gameOverPanelRaiseTime);
	}

	private string GetHighScoreForCurrentLevel() {
		if (GamePreferences.GetEasyDifficultyState () == 1) {
			return GamePreferences.GetEasyDifficultyHighscore ().ToString();
		} else if (GamePreferences.GetMediumDifficultyState () == 1) {
			return GamePreferences.GetMediumDifficultyHighscore ().ToString();
		} else if (GamePreferences.GetHardDifficultyState () == 1) {
			return GamePreferences.GetHardDifficultyHighscore ().ToString();
		}

		return "";
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
		pauseScoreText.text = GetHighScoreForCurrentLevel ();
		pausePanel.SetActive (true);
		if(pointer.activeSelf) // if it's active deactivate the pointer
			pointer.SetActive (false);
		Time.timeScale = 0 ; 
	}

	public void resetLifes() {
		heart1.SetActive (true);
		heart2.SetActive (true);
	}

	public void changeLifesUi(int lifes) {
		// makes ui hearts go away for every life lost
		switch (lifes) {
		case 1:
			heart2.SetActive (false);
			goto case 2;
		case 2: 
			heart1.SetActive(false);
			break;
		}
	}
		

	public void resumeGame() {
		Time.timeScale = 1;

		if (!circleControllerReference.GameStart) // if the game did not started we reactivate the pointer
			pointer.SetActive (true);

		pausePanel.SetActive (false);
	}

	public void menuPausePanel() {
		Time.timeScale = 1;
		//SceneManager.LoadScene (GameManager.menuScene);
		SceneFader.instance.fadeIn(GameManager.menuScene);
	}

	public void InstantiateComboUI() {
		int c = CircleController.getComboCounter ();

		if (c == ScoreManager.TWO_TIMES_COMBO_LIMIT + 1) {
			twoTimesCombo.SetActive (true);
			StartCoroutine(deactivateObjectAfterAnAmountOfTime (twoTimesCombo, 1.5f));
		} else if (c == ScoreManager.THREE_TIMES_COMBO_LIMIT + 1) {
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


	public void SetActivatePointer(bool active) {
		pointer.SetActive (active);
	}

	private void ShowTrail() {
		// starting the animation
		StartCoroutine ("ShowTrailEnum");

		// getting the buttons from the fourth step
		GameObject fourthStep = trail.transform.Find ("fourthStep").gameObject;
		Button YesButton = fourthStep.transform.Find ("YesButton").gameObject.GetComponent<Button> ();
		Button NoButton = fourthStep.transform.Find ("NoButton").gameObject.GetComponent<Button> ();

		// add listeners to the fourth step buttons
		YesButton.onClick.AddListener (() => { trail.SetActive (false);
			showTrailStarted = false; // the other script's can start their activity only after this button it's pressed
			pointer.SetActive (true); // show the pointer
		});// the showTrail variable it's already on 1 so it is enough to clear the ui
		NoButton.onClick.AddListener (() => { GamePreferences.SetTrailState(0); // change the state of the trail logic
			trail.SetActive (false); // clear the 
			showTrailStarted = false; // the other script's can start their activity only after this button it's pressed
			pointer.SetActive (true); // show the pointer
		});
	}


	private IEnumerator ShowTrailEnum() {
	
		GameObject firstStep = trail.transform.Find ("firstStep").gameObject;
		GameObject secondStep = trail.transform.Find ("secondStep").gameObject;
		GameObject thirdStep = trail.transform.Find ("thirdStep").gameObject;
		GameObject fourthStep = trail.transform.Find ("fourthStep").gameObject;
	
		firstStep.SetActive (true);
		yield return new WaitForSeconds (2f);
		firstStep.SetActive (false);
		secondStep.SetActive (true);
		yield return new WaitForSeconds (2.5f);
		secondStep.SetActive (false);
		thirdStep.SetActive (true);
		yield return new WaitForSeconds (2f);
		thirdStep.SetActive (false);
		fourthStep.SetActive (true);
	}


	public void ShowDataListener() { // listener for the button whichs shows the current forms

		Time.timeScale = 0f; // pause the game
		populateShowDataUIWithModels();
		showDataButton.transform.Find ("DataPanel").gameObject.SetActive (true);
	}

	private void populateShowDataUIWithModels() {
		// getting current drops -> sprites
		MultiDimensionalObject currentDrops = GameManager.instance.getCurrentDrops ();
		changeShowDataUI (currentDrops.objects [0].GetComponent<SpriteRenderer> ().sprite,
			currentDrops.objects [4].GetComponent<SpriteRenderer> ().sprite,
			currentDrops.objects [8].GetComponent<SpriteRenderer> ().sprite,
			CUBE_DESCRIPTION, STAR_DESCRIPTION, DIAMOND_DESCRIPTION);
	}

	public void rightArrowShowDataListener() {
		changeShowDataUI (timeDropSprite, rainDropSprite, unifiedDropSprite, TIME_DROP_DESCRIPTION, RAIN_DROP_DESCRIPTION, UNIFIED_DROP_DESCRIPTION);
		rightArrowShowData.gameObject.SetActive (false);
		leftArrowShowData.gameObject.SetActive (true);
	}

	public void leftArrowShowDataListnere() {
		populateShowDataUIWithModels ();
		leftArrowShowData.gameObject.SetActive (false);
		rightArrowShowData.gameObject.SetActive (true);
	}

	private void changeShowDataUI(Sprite sprite1, Sprite sprite2, Sprite sprite3, string string1, string string2, string string3) {
		
		// references to the images
		Image image1 = showDataButton.transform.Find ("DataPanel/Item1/drop").gameObject.GetComponent<Image> ();
		Image image2 = showDataButton.transform.Find ("DataPanel/Item2/drop").gameObject.GetComponent<Image> ();
		Image image3 = showDataButton.transform.Find ("DataPanel/Item3/drop").gameObject.GetComponent<Image> ();

		// setting up the sprites ( one of every model)
		image1.sprite = sprite1;
		image2.sprite = sprite2;
		image3.sprite = sprite3;

		//same for text
		Text text1 = showDataButton.transform.Find ("DataPanel/Item1/description").gameObject.GetComponent<Text> ();
		Text text2 = showDataButton.transform.Find ("DataPanel/Item2/description").gameObject.GetComponent<Text> ();
		Text text3 = showDataButton.transform.Find ("DataPanel/Item3/description").gameObject.GetComponent<Text> ();

		text1.text = string1;
		text2.text = string2;
		text3.text = string3;
	}

	public void BackButtonShowDataListener() {
		showDataButton.transform.Find ("DataPanel").gameObject.SetActive (false);
		Time.timeScale = 1f; // set time to default
	}
		
	public GameObject getDropTimeSlider() {
		return dropSliderTime;
	}

	public GameObject getDropUnifiedSlider() {
		return dropSliderUnified;
	}

	public GameObject getDropRainSlider() {
		return dropSliderRain;
	}
}
