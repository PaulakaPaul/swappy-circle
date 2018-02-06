using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialController : MonoBehaviour {

	[SerializeField]
	private GameObject socialPanel;
	[SerializeField]
	private Animator socialPanelAnim;

	private bool isSocialButtonClicked;

	// Use this for initialization
	void Start () {
		isSocialButtonClicked = false; // it's not clicked by default
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SocialButtonClickListener() {
		if (!isSocialButtonClicked) {
			socialPanel.SetActive (true);
			isSocialButtonClicked = true;
		} else {
			StartCoroutine ("closeSocialPanel");
		}
	
	}


	private IEnumerator closeSocialPanel() {
		socialPanelAnim.Play ("SlideOutSocialPanel");
		yield return new WaitForSeconds (1f);
		socialPanel.SetActive (false);
		isSocialButtonClicked = false;
	}
}
