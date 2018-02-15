using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Facebook.Unity;

public class SocialController : MonoBehaviour {

	[SerializeField]
	private GameObject socialPanel, todoLeaderboardPanel;
	[SerializeField]
	private Animator socialPanelAnim;

	private bool isSocialButtonClicked;


	public const string FB_SHARE_TITLE = "Swappy Circle";


	void Awake (){
		InitializeFB ();
	}

	private void InitializeFB() {
		if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			FB.Init(InitCallback, OnHideUnity);
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp();
		}
	}

	// func for FB.Init()
	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp();
			// Continue with Facebook SDK
			// ...
		} else {
			Debug.Log("Failed to Initialize the Facebook SDK");
		}
	}

	// func for FB.Init()
	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}

	// Use this for initialization
	void Start () {
		isSocialButtonClicked = false; // it's not clicked by default
	}

	// Update is called once per frame
	void Update () {

	}

	//fb button listener
	public void ShareFBListener() {


		string fb_share_description = "Best kind of game, easy to learn and hard to master.\nEasy highscore: " + GamePreferences.GetEasyDifficultyHighscore () +
		                              "\nMedium highscore: " + GamePreferences.GetMediumDifficultyHighscore () + "\nHard highscore: " + GamePreferences.GetHardDifficultyHighscore ();

		FB.ShareLink(
			contentURL: new Uri("https://1drv.ms/u/s!AucxQZ6OBPB75QtSFuwFbaUHUb-a"),
			contentTitle: FB_SHARE_TITLE,
			contentDescription: fb_share_description,
			callback: ShareCallback
		);


		// the contentTitle and contentDescription are not working, sdk broken
	}

	//leaderboards button listener
	public void LeaderboardsListener() {
		//TODO implement leaderboards...
		StartCoroutine ("ShowFailedToLoadAdText");
	}

	// useful for debugging
	private void ShareCallback (IShareResult result) {
		if (result.Cancelled || !String.IsNullOrEmpty(result.Error)) {
			Debug.Log("ShareLink Error: "+result.Error);
		} else if (!String.IsNullOrEmpty(result.PostId)) {
			// Print post identifier of the shared content
			Debug.Log(result.PostId);
		} else {
			// Share succeeded without postID
			Debug.Log("ShareLink success!");
		}
	}

	//main button listener -> opens/closes the panel
	public void SocialButtonClickListener() {
		if (!AdManager.instance.AdOpened && SettingsController.instance.IsSettingsMenuClosed) { // continue if no ads are opened
			if (!isSocialButtonClicked) {
				socialPanel.SetActive (true);
				isSocialButtonClicked = true;
			} else {
				StartCoroutine ("closeSocialPanel");
			}
		}
	}
		
	// enumerator for slide out anim
	private IEnumerator closeSocialPanel() {
		socialPanelAnim.Play ("SlideOutSocialPanel");
		yield return new WaitForSeconds (1f);
		socialPanel.SetActive (false);
		isSocialButtonClicked = false;
	}

	private IEnumerator ShowFailedToLoadAdText() {
		todoLeaderboardPanel.SetActive (true);
		yield return new WaitForSeconds (1f);
		todoLeaderboardPanel.SetActive (false);
	} 
}
