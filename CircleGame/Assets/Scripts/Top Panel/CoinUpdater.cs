using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinUpdater : MonoBehaviour {

	public static CoinUpdater instance;

	[SerializeField]
	private Text coinText;


	void Awake() {

		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		coinText.text = GamePreferences.GetCoinScore ().ToString ();
	}
}
