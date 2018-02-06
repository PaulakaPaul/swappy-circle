using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBrain : MonoBehaviour {

	[SerializeField]
	private GameObject scoreBuffImage, coinBuffImage; // images for the buffs

	public static PanelBrain instance;


	void Awake() {

		if (instance == null)
			instance = this;
	}

	// Use this for initialization
	void Start () {

		//initialScoreBuffImagePosition = scoreBuffImage.transform.position;
		//initialCoinBuffImagePosition = coinBuffImage.transform.position;

		EnableBuffPictures ();
	}

	// Update is called once per frame
	void Update () {
		
	}


	public void EnableBuffPictures() {

		if (GameManager.instance.getDoubleCoinState ()) { // if we have the buff we enable the image 
			coinBuffImage.SetActive (true);
		} else {
			// if we don't have the coin buff we switch the images -> in this case even if we buy at the same runtime the coin buff the images wont overlap
			if (GameManager.instance.getDoubleScoreState ()) { // we make this only if we have what to show
				Vector3 aux1 = scoreBuffImage.transform.position;
				Vector3 aux2 = coinBuffImage.transform.position;
				scoreBuffImage.transform.position = aux2;
				coinBuffImage.transform.position = aux1;
			}
		}

		if (GameManager.instance.getDoubleScoreState ()) {
			scoreBuffImage.SetActive (true);
		} 

	}
}
