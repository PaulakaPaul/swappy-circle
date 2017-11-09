using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenAreaChecker : MonoBehaviour {


	public const string greenCube = "greenCube";
	public const string greenStar = "greenStar";
	public const string greenDiamond = "greenDiamond";
	[SerializeField]
	private GameObject redZero;
	[SerializeField]
	private GameObject blueZero;
	[SerializeField]
	private GameObject greenSpark, coinSpark;

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
		case greenCube:
			spark = Instantiate<GameObject> (greenSpark, col.gameObject.transform.position, Quaternion.identity);
			ScoreManager.instance.incrementDiamond ();
			break;
		case greenStar:
			spark = Instantiate<GameObject> (greenSpark, col.gameObject.transform.position, Quaternion.identity);
			ScoreManager.instance.incrementDiamond ();
			break;
		case greenDiamond:
			spark = Instantiate<GameObject> (greenSpark, col.gameObject.transform.position, Quaternion.identity);
			ScoreManager.instance.incrementDiamond ();
			break;
		case FormsSpawner.coinTag:
			coinSparkReference = Instantiate<GameObject> (coinSpark, col.gameObject.transform.position, Quaternion.identity);
			ScoreManager.instance.incrementCoins ();
			break;
		default:
			//activate lose animation/code
			CircleController.resetCounter();

			char colour = col.tag[0];
			GameObject zero = null;

			if (colour == 'b') {
				zero = Instantiate (blueZero, col.gameObject.transform.position, Quaternion.identity) as GameObject;
			} else if (colour == 'r') {
				zero =	Instantiate (redZero, col.gameObject.transform.position, Quaternion.identity) as GameObject;
			}

			if( zero != null)
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
	


