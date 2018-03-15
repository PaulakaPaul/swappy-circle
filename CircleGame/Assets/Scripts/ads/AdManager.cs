using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;


public class AdManager : MonoBehaviour
{

	[SerializeField]
	private GameObject adsPanel;

	public static AdManager instance;

	string appID = "ca-app-pub-5218899435308484~7951254202";

	private InterstitialAd interstitialAd;
	private RewardBasedVideoAd rewardBasedAd;

	private bool adOpened;

	public bool AdOpened { get { return adOpened; } }


	void Awake ()
	{
		if (instance == null)
			instance = this;

		adOpened = false; // we don t show any ad by default
	}

	void Start ()
	{
		MobileAds.Initialize (appID);
		rewardBasedAd = RewardBasedVideoAd.Instance; // singleton
		rewardBasedAd.OnAdRewarded += HandleRewardBasedVideoRewarded;
		rewardBasedAd.OnAdOpening += HandleOnAdLoaded;
		rewardBasedAd.OnAdClosed += HandleOnAdClosed;
		rewardBasedAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;


		// request in the beggining and initialize for the first time the ads references
		RequestInterstitial ();
		RequestRewardedVideo ();
	}

	void OnDisable ()
	{
		interstitialAd.Destroy (); // only the interstitial has to be destroyed, the reward ad it's of a singleton
	}

	void Update() {
		//Debug.Log (adOpened);
	}

	private void RequestInterstitial ()
	{

		#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-5218899435308484/3648336676";
		#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-3940256099942544/4411468910";
		#else
		string adUnitId = "unexpected_platform";
		#endif

		// Initialize an InterstitialAd.
		interstitialAd = new InterstitialAd (adUnitId);

		//add events
		interstitialAd.OnAdOpening += HandleOnAdLoaded;
		interstitialAd.OnAdClosed += HandleOnAdClosed;
		interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder ()
		.TagForChildDirectedTreatment (true)
		.Build ();
		// Load the interstitial with the request.
		interstitialAd.LoadAd (request);

	}

	private void RequestRewardedVideo ()
	{

		#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-5218899435308484/4409961414";
		#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-3940256099942544/1712485313";
		#else
		string adUnitId = "unexpected_platform";
		#endif


		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder ()
		.Build ();
		// Load the rewarded video ad with the request.
		this.rewardBasedAd.LoadAd (request, adUnitId);

	}

	public void RequestAllAds ()
	{

		if (interstitialAd != null) {
			if (!interstitialAd.IsLoaded ()) { // load only if it isn't loaded
				RequestInterstitial ();
			}
		}

		if (rewardBasedAd != null && !rewardBasedAd.IsLoaded ()) { // load only if it isn't loaded
			RequestRewardedVideo ();
		}

	}

	public void ShowInterstitial ()
	{

		if (interstitialAd != null)
		if (interstitialAd.IsLoaded ())
			interstitialAd.Show ();
		else
			RequestInterstitial ();
	}

	public void ShowRewardAd ()
	{

		if (rewardBasedAd.IsLoaded ())
			rewardBasedAd.Show ();
		else
			RequestRewardedVideo ();
	}

	public void HandleRewardBasedVideoRewarded (object sender, Reward args)
	{
		int coinAmount = (int)args.Amount;
		GamePreferences.SetCoinScore (GamePreferences.GetCoinScore () + coinAmount);
	}

	public bool areAllAdsLoaded() {
		return rewardBasedAd.IsLoaded () && interstitialAd.IsLoaded ();
	}

	public void ShowUIAd ()
	{

		if (areAllAdsLoaded ()) { // continue ad logic only if all the ads are loaded
			GamePreferences.DecrementAdsCounter ();
			adOpened = true;
			float rnd = UnityEngine.Random.Range (0, 100);

			if (rnd < 70) {// show rewardAd Panel
				adsPanel.SetActive (true);
			} else {// show interstitial Ad
				ShowInterstitial ();

			} 
		} else { // request again
			RequestAllAds ();
		}
	}

	public void YesButtonShowUIAd ()
	{
		adOpened = false;
		ShowRewardAd ();
		adsPanel.SetActive (false);
	}

	public void NoButtonShowUIAd ()
	{
		adOpened = false;
		adsPanel.SetActive (false);
	}

	private void HandleOnAdLoaded (object sender, EventArgs args)
	{
		Time.timeScale = 0f;
	}

	private void HandleOnAdClosed (object sender, EventArgs args)
	{
		adOpened = false;
		Time.timeScale = 1f;
	}

	private void HandleOnAdFailedToLoad (object sender, AdFailedToLoadEventArgs args)
	{
		adOpened = false; // in case the ad failed to load but the ad panel showed -> adOpened = true, we need to reset the var to false so we can press the buttons
		Debug.Log ("FAILED");
	}



}
