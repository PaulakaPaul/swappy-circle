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
		GameManager.instance.SetGameStartedFromMenuTrue ();
		//SceneManager.LoadScene (GameManager.playScene);
		SceneFader.instance.fadeIn(GameManager.playScene);
	}

	public void exit() {
		Application.Quit ();
	}

	public void levels() {
		//SceneManager.LoadScene (GameManager.levelsScene);
		SceneFader.instance.fadeIn(GameManager.levelsScene);
	}
		
}
