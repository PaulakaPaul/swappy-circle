using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour {

	public static SettingsController instance;

	public const string MUSIC_BUTTON_NAME = "MusicButton", DETAIL_BUTTON_NAME = "DetailsButton", TRAIL_BUTTON_NAME = "TrailButton";
	public const string detailsText = "Main rules: \n - CATCH ( the forms)\n - ROTATE (the circle)\n - SURVIVE ( lifes and heart)\n\nScore:\n - cubes : 3 points\n - stars : 5 points\n - diamonds : 8 points\n\nCombos (catching multiple forms without \nmaking a mistake):\n - x2  from 5 to 8 elements\n - x3 from 9 to how make you can handle it\n\n  You can also personalize your game as you want \nfrom the shop with new circles, drops and buffs.\n\n   Hope that this description helped you !\n   For more information or bug reports please mail\nme anytime at p.e.iusztin.gmail@gmail.com.";

	[SerializeField]
	private GameObject trailObject, musicObject, detailsObject, mainPanel, buttonsAndTitle;
	private GameObject currentObjectActive;
	[SerializeField]
	private Button backButton, settingsButtonTop, settingsButtonMainMenu;
	[SerializeField]
	private Text scrollViewText;
	[SerializeField]
	private Animator panelAnim;
	[SerializeField]
	private Slider musicSlider;

	private bool backButtonListnerAdded; // to add the listener only once
	private bool exitMainPanel; // we use the same back button to exit from the main panel or to go back to the buttons
	private bool isSettingsMenuClosed;

	void Awake() {

		Debug.Log (panelAnim);

		if (instance == null)
			instance = this;
	}

	// Use this for initialization
	void Start () {
		backButtonListnerAdded = false;
		exitMainPanel = true; // we firstly exit the main panel
		isSettingsMenuClosed = true; // when the start function runs the setting menu is cloed
		backButton.onClick.AddListener( () => StartCoroutine("exitMainPanelRoutine") ); // we don't have yet the current object to make the whole logic
	}

	void OnEnable() {
		SceneManager.sceneLoaded += MyOnSceneLoaded;
	}

	void OnDisable() {
		SceneManager.sceneLoaded -= MyOnSceneLoaded;
	}


	private void MyOnSceneLoaded(Scene scene, LoadSceneMode mode) {
	
		// if in the menu scene we use a different settings button
		if(scene.name.Equals(GameManager.menuScene)) {
			settingsButtonTop.gameObject.SetActive(false);
			settingsButtonMainMenu.gameObject.SetActive(true);
		} else { // we activate the top settings button
			settingsButtonTop.gameObject.SetActive(true);
			settingsButtonMainMenu.gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//for the settings button
	public void settingsButtonListener() {
		mainPanel.SetActive (true);
		isSettingsMenuClosed = false;
		Time.timeScale = 0f; // pausing the game when you want to use the settings
	}


	//for the panel buttons
	public void buttonClickedListener() {

		string buttonName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name; // gets the current clicked object name

		// getting the current object
		switch (buttonName) {
		case MUSIC_BUTTON_NAME:
			currentObjectActive = musicObject;
			MusicObjectLogic ();
			break;
		case DETAIL_BUTTON_NAME:
			currentObjectActive = detailsObject;
			DetailsObjectLogic();
			break;
		case TRAIL_BUTTON_NAME:
			currentObjectActive = trailObject;
			TrailObjectLogic ();
			break;
		}


		//making the view switch
		currentObjectActive.SetActive (true);
		buttonsAndTitle.SetActive (false);

		// preparing the back button

		if (!backButtonListnerAdded) {
			backButton.onClick.RemoveAllListeners(); // clearing the first listener added in the Start() method
			backButton.onClick.AddListener (() => {
				if(exitMainPanel) {
					StartCoroutine("exitMainPanelRoutine"); // coroutine to let the animator to play it's fade out anim for the panel
				} else {
				// making back the view switch
				buttonsAndTitle.SetActive (true);
				currentObjectActive.SetActive (false);
					exitMainPanel = true;
				}


			});

			backButtonListnerAdded = true; // we add the listener only once
		}

		exitMainPanel = false; // if this button is clicked it means we won't exit the main panel

	}

	private IEnumerator exitMainPanelRoutine() {
		panelAnim.Play("SlideOutSettingsPanel");
		Time.timeScale = 1f; //unpausing the game
		yield return new WaitForSeconds (1f);
		mainPanel.SetActive(false);
		isSettingsMenuClosed = true;

	}



	private void TrailObjectLogic() {

		Toggle toggleTrail = currentObjectActive.transform.Find ("ToggleTrail").gameObject.GetComponent<Toggle> ();
		if (toggleTrail != null) {
		
			// making the ui
			if (GamePreferences.GetTrailState () == 1)
				toggleTrail.isOn = true;
			else
				toggleTrail.isOn = false;


			// setting the listener
			toggleTrail.onValueChanged.AddListener ((bool value) => {
			
				if (value) {
					GamePreferences.SetTrailState (1);
				} else {
					GamePreferences.SetTrailState (0);
				}
			
			});
		
		}
	}

	private void MusicObjectLogic() {
		musicSlider.value = GamePreferences.getMusicState ();
		// look at the sliderListerner for further logic
	}

	public void sliderListener(float volume) { // listner added in the inspector
		MusicController.instance.ChangeBackgroundVolume(volume);
	}

	private void DetailsObjectLogic() {
		scrollViewText.text = detailsText;
	}

	public bool getSettingsPanelState() {
		return isSettingsMenuClosed;
	}


}
