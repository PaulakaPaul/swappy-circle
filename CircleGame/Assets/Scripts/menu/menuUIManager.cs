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

		if (SettingsController.instance.getSettingsPanelState ()
			&& !AdManager.instance.AdOpened) { // go to another scene only of the settings panel is closed and no ads are opened
			GameManager.instance.SetGameStartedFromMenuTrue ();
			//SceneManager.LoadScene (GameManager.playScene);
			SceneFader.instance.fadeIn (GameManager.playScene);
		}
	}

	public void exit() {
		if (SettingsController.instance.getSettingsPanelState ()
			&& !AdManager.instance.AdOpened)  // go to another scene only of the settings panel is closed and no ads are opened
			Application.Quit ();
	}

	public void levels() {
			if (SettingsController.instance.getSettingsPanelState ()
				&& !AdManager.instance.AdOpened)  // go to another scene only of the settings panel is closed and no ads are opened
			SceneFader.instance.fadeIn(GameManager.levelsScene);
	}

	public void shop() {
				if (SettingsController.instance.getSettingsPanelState ()
					&& !AdManager.instance.AdOpened)  // go to another scene only of the settings panel is closed and no ads are opened
			SceneFader.instance.fadeIn (GameManager.shopScene);
	}
		
}
