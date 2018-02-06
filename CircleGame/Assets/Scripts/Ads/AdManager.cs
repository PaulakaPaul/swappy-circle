using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour {


	public static AdManager instance;

	string appID = "ca-app-pub-5218899435308484~7951254202";

	private InterstitialAd interstitialAd;
	private RewardBasedVideoAd rewardBasedAd;

	void Start() {
		MobileAds.Initialize (appID);
		rewardBasedAd = RewardBasedVideoAd.Instance; // singleton
		rewardBasedAd.OnAdRewarded += HandleRewardBasedVideoRewarded;


		RequestInterstitial ();
		RequestRewardedVideo ();
	}

	void OnDisable() {
		interstitialAd.Destroy (); // only the interstitial has to be destroyed, the reward ad it's of a singleton
	}


	private void RequestInterstitial(){
		#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-5218899435308484/3648336676";
		#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-3940256099942544/4411468910";
		#else
		string adUnitId = "unexpected_platform";
		#endif

		// Initialize an InterstitialAd.
		interstitialAd = new InterstitialAd(adUnitId);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder()
		.AddTestDevice("37C56F39A25E4159")
		.Build();
		// Load the interstitial with the request.
		interstitialAd.LoadAd(request);
	}

	private void RequestRewardedVideo() {
		#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-5218899435308484/4475877778";
		#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-3940256099942544/1712485313";
		#else
		string adUnitId = "unexpected_platform";
		#endif

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder()
		.AddTestDevice("37C56F39A25E4159")
		.Build();
		// Load the rewarded video ad with the request.
		this.rewardBasedAd.LoadAd(request, adUnitId);
		}

	public void ShowInterstitial() {

		if (interstitialAd != null)
			if (interstitialAd.IsLoaded ())
				interstitialAd.Show ();
	}	

	public void ShowRewardAd() {

		if (rewardBasedAd.IsLoaded ())
			rewardBasedAd.Show ();
	}

	public void HandleRewardBasedVideoRewarded(object sender, Reward args) {
		int coinAmount = (int) args.Amount;
		GamePreferences.SetCoinScore (GamePreferences.GetCoinScore () + coinAmount);
	}
			
}
