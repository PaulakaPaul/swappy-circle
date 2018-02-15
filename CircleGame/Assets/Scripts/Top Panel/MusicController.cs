using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour { // in the Top Panel object

	public static MusicController instance;

	//bg clip
	[SerializeField]
	private AudioSource bgAudioSource;

	[SerializeField]
	private Sprite musicOn, musicOff;

	private Button musicButton;

	// clips ( public to be played from everywhere in the game)
	public AudioClip catchFormClip;

	public float VolumeAudioClips { get ; set ; } // var with which we will control the volume from all the game clips ( range from 0 to 1 as the bgAudioSource range and volumeSlider range)


	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
	}
	

	void Start () {
		// initialize game volume
		ChangeBackgroundVolume(GamePreferences.getMusicState ());
		VolumeAudioClips = GamePreferences.getMusicState ();
	}

	void OnEnable() {
		SceneManager.sceneLoaded += MySceneLoader;
	}

	void OnDisable() {
		SceneManager.sceneLoaded -= MySceneLoader;
	}

	private void MySceneLoader(Scene scene, LoadSceneMode mode) {
	
		// activate the music button
		if (scene.name.Equals (GameManager.playScene)) {
			// find the music button from the scene and add the listener to it
			musicButton = GameObject.FindGameObjectWithTag ("musicButton").GetComponent<Button> ();
			musicButton.gameObject.SetActive (true);
			ChangeBackgroundVolume (GamePreferences.getMusicState ()); // it will setup the on/off state of the Audio Source and also the sprite of the button
			musicButton.onClick.AddListener (OnOffBackgroundMusicListener);
		} else {
			musicButton = null; // we need the reference just in the gameplay scene
		}
	
	}

	private void SetupMusicButtonSprite() {

		if (musicButton != null) { // we do this only in the gameplay scene
			if (bgAudioSource.isPlaying)
				musicButton.gameObject.GetComponent<Image> ().sprite = musicOn;
			else if (!bgAudioSource.isPlaying)
				musicButton.gameObject.GetComponent<Image> ().sprite = musicOff;
		}
	}

	public void OnOffBackgroundMusicListener() { // listener for the game music button

		if (bgAudioSource.isPlaying) {
			bgAudioSource.Stop ();
			// saving 
			GamePreferences.SetLastMusicState (GamePreferences.getMusicState ()); // storing this to give back the default music state
			GamePreferences.SetMusicState (0f);
			// setup the audiosource
			bgAudioSource.volume = 0f;
			VolumeAudioClips = 0f;
		} else if (!bgAudioSource.isPlaying) {
			bgAudioSource.Play ();
			// saving
			GamePreferences.SetMusicState (GamePreferences.getLastMusicState());
			// setup the audiosource
			bgAudioSource.volume = GamePreferences.getLastMusicState();
			VolumeAudioClips = GamePreferences.getLastMusicState ();
		}
		
		SetupMusicButtonSprite ();
	
	}

	public void ChangeBackgroundVolume(float volume) {

		//change the volume
		bgAudioSource.volume = volume;
		// give the same volume to the audio clips
		VolumeAudioClips = volume;
		// save the data
		GamePreferences.SetMusicState (volume);

		// playing or stopping the clip for efficiency and OnOffBackgroundMusicListener logic
		if (volume > 0) {
			if (!bgAudioSource.isPlaying) 
				bgAudioSource.Play ();
		} else {
			if (bgAudioSource.isPlaying)
				bgAudioSource.Stop ();
		}

		SetupMusicButtonSprite ();
	}
		
}
