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
		allTagLists.AddRange (FormsSpawner.getRedTags());
		allTagLists.AddRange (FormsSpawner.getGreenTags());
		allTagLists.AddRange (FormsSpawner.getBlueTags());
		isHit = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col) {

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
	}
}
	