using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedAreaChecker : MonoBehaviour {

	public const string redCube = "redCube"; // implicit static
	public const string redStar = "redStar";
	public const string redDiamond = "redDiamond";
	[SerializeField]
	private GameObject greenZero;
	[SerializeField]
  	private GameObject blueZero;
	[SerializeField]
	private GameObject redSpark, coinSpark;

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
		case redCube:
			spark = Instantiate<GameObject> (redSpark, col.gameObject.transform.position, Quaternion.identity);
			ScoreManager.instance.incrementDiamond ();
			break;
		case redStar:
			spark = Instantiate<GameObject> (redSpark, col.gameObject.transform.position, Quaternion.identity);
			ScoreManager.instance.incrementDiamond ();
			break;
		case redDiamond:
			spark = Instantiate<GameObject> (redSpark, col.gameObject.transform.position, Quaternion.identity);
			ScoreManager.instance.incrementDiamond ();
			break;
		case FormsSpawner.coinTag:
			coinSparkReference = Instantiate<GameObject> (coinSpark, col.gameObject.transform.position, Quaternion.identity);
			ScoreManager.instance.incrementCoins ();
			break;
		default:
			// activate lose animation/code

			CircleController.resetCounter();

			char colour = col.gameObject.tag[0];
			GameObject zero = null;
			if (colour.Equals ('g')) {
				zero = Instantiate (greenZero, col.gameObject.transform.position, Quaternion.identity) as GameObject;
			} else if(colour.Equals('b')) {
				zero =	Instantiate (blueZero, col.gameObject.transform.position, Quaternion.identity) as GameObject;
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


