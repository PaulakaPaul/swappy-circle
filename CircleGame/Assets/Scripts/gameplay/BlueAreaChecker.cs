using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueAreaChecker : MonoBehaviour {


	public const string blueCube = "blueCube";
	public const string blueStar = "blueStar";
	public const string blueDiamond = "blueDiamond";
	[SerializeField]
	private GameObject greenZero;
	[SerializeField]
	private GameObject redZero;
	[SerializeField]
	private GameObject blueSpark, coinSpark;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col) {

		GameObject spark = null; 
		GameObject coinSparkReference = null;

		// increment score
		switch (col.tag) {
		case blueCube:
			spark = Instantiate<GameObject> (blueSpark, col.gameObject.transform.position, Quaternion.identity);
			ScoreManager.instance.incrementCube ();
			break;
		case blueStar:
			spark = Instantiate<GameObject> (blueSpark, col.gameObject.transform.position, Quaternion.identity);
			ScoreManager.instance.incrementStar ();
			break;
		case blueDiamond:
			spark = Instantiate<GameObject> (blueSpark, col.gameObject.transform.position, Quaternion.identity);
			ScoreManager.instance.incrementDiamond ();
			break;
		case FormsSpawner.coinTag:
			coinSparkReference = Instantiate<GameObject> (coinSpark, col.gameObject.transform.position, Quaternion.identity);
			ScoreManager.instance.incrementCoins ();
			break;
		default:
			// activate lose animation/code
			CircleController.resetCounter();

			char colour = col.tag [0];
			GameObject zero = null;
			if (colour == 'r') {
				zero = Instantiate<GameObject> (redZero, col.gameObject.transform.position, Quaternion.identity);
			} else if (colour == 'g') {
				zero = Instantiate<GameObject> (greenZero, col.gameObject.transform.position, Quaternion.identity);
			}
			if (zero != null)
				Destroy (zero, 1f);
			break;
			}

		if (spark != null) {
			Destroy (spark, 1f);
		}

		if (coinSparkReference != null) {
			Destroy (coinSparkReference, 1f);
		}
		Destroy (col.gameObject, FormsSpawner.getDestroyTime ());
	}


}



