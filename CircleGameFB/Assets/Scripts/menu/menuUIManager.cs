using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuUIManager : MonoBehaviour {


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void play() {

		if (SettingsController.instance.getSettingsPanelState ()) { // go to another scene only of the settings panel is closed
			GameManager.instance.SetGameStartedFromMenuTrue ();
			//SceneManager.LoadScene (GameManager.playScene);
			SceneFader.instance.fadeIn (GameManager.playScene);
		}
	}

	public void exit() {
		if (SettingsController.instance.getSettingsPanelState ())  // go to another scene only of the settings panel is closed
			Application.Quit ();
	}

	public void levels() {
		if (SettingsController.instance.getSettingsPanelState ()) // go to another scene only of the settings panel is closed
			//SceneManager.LoadScene (GameManager.levelsScene);
			SceneFader.instance.fadeIn(GameManager.levelsScene);
	}

	public void shop() {
		if (SettingsController.instance.getSettingsPanelState ()) // go to another scene only of the settings panel is closed
			SceneFader.instance.fadeIn (GameManager.shopScene);
	}
		
}
