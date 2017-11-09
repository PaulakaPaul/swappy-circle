using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour {


	public static SceneFader instance;
	[SerializeField]
	private GameObject canvas;
	[SerializeField]
	private Animator anim;

	void Awake() {
	
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	
	}

	public void fadeIn(string level) {

		StartCoroutine (fadeInScene (level));
	} 

	private void fadeOut() {
		StartCoroutine ("fadeOutScene");
	}

	private IEnumerator fadeInScene(string level) {

		canvas.SetActive (true);
		anim.Play ("fadeIn");
		//yield return StartCoroutine( MyCoroutine.WaitForRealSeconds(0.4f));
		yield return new WaitForSecondsRealtime(0.7f);
		SceneManager.LoadScene (level);
		fadeOut ();
	}

	private IEnumerator fadeOutScene() {

		anim.Play ("fadeOut");
		//yield return StartCoroutine( MyCoroutine.WaitForRealSeconds(0.5f));
		yield return new WaitForSecondsRealtime(1f);
		canvas.SetActive (false);
	}


}
