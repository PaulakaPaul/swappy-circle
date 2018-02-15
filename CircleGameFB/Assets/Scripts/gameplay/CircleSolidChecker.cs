using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSolidChecker : MonoBehaviour {

	private List<string> allTagLists = new List<string>();
	[SerializeField]
	private GameObject explosion;

	private bool isHit;
	// Use this for initialization
	void Start () {
		allTagLists.AddRange (FormsSpawner.redTagTypes);
		allTagLists.AddRange (FormsSpawner.greenTagTypes);
		allTagLists.AddRange (FormsSpawner.blueTagTypes);
		allTagLists.Add (FormsSpawner.coinTag);
		allTagLists.AddRange (FormsSpawner.rainDropTags);
		allTagLists.AddRange (FormsSpawner.timeDropTags);
		allTagLists.AddRange (FormsSpawner.unifiedDropTags);

		isHit = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col) {

		if (!FormsSpawner.instance.IsRainDropActivated) { // when this drop it's activated we can't die
			foreach (string type in allTagLists)
				if (col.gameObject.tag.Equals (type)) {
					GameObject exp = Instantiate (explosion, col.gameObject.transform.position, Quaternion.identity) as GameObject;
					//col.gameObject.GetComponent<Animator>().Play("Explosion");
					Destroy (col.gameObject);

					if (!isHit) { //so the objects wont take more than one life per round (if 2 objects are close and both hit the solid part --> 2 lifes, we don't like that)
						ScoreManager.instance.decrementLife ();
						isHit = true;
					}
			
					Destroy (exp, 1f);
				}
		} else if (FormsSpawner.instance.IsRainDropActivated) { // then we just destory the object
			Destroy (col.gameObject);
		}
	}
}
	