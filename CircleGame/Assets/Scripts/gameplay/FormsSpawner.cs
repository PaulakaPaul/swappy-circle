using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormsSpawner : MonoBehaviour {

	[SerializeField]
	private GameObject coin;
	[SerializeField]
	private GameObject[] cubeModels;
	[SerializeField]
	private GameObject[] starModels;
	[SerializeField]
	private GameObject[] diamondModels;
	[SerializeField]
	private float minPos;                       
	[SerializeField]
	private float maxPos;                       

	private readonly int maxColours = 3;      
	private float waitStrikeTime; // time beetween spawing forms in a ture 
	private int numberFormsSpawning; // number of forms spawn per ture 
	private float elapseTime; // time beetween tures


	private static readonly float destroyTime = 0.08f;
	public const string coinTag = "coin";
	private static readonly List<string> redTagTypes = new List<string>  { "redCube", "redStar", "redDiamond", coinTag};
	private static readonly List<string> greenTagTypes = new List<string> {"greenCube", "greenStar", "greenDiamond", coinTag };
	private static readonly List<string> blueTagTypes = new List<string> {"blueCube", "blueStar", "blueDiamond", coinTag };


	void Awake(){
		levelTimeSpawnSelect ();
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}


	public void startSpawningForms() {
		InvokeRepeating ("spawningFormGroup", 0.1f, waitStrikeTime * numberFormsSpawning + elapseTime);
	}

	private void spawningFormGroup() {
		StartCoroutine ("spawnForms");
	}

	public void stopSpawningForms() {
		StopCoroutine ("spawnForms");
		CancelInvoke ("spawningFormGroup");
	}


	private void levelTimeSpawnSelect() {
	
		if (GamePreferences.GetEasyDifficultyState () == 1) {
			waitStrikeTime = 0.7f; // time beetween spawing forms in a ture 
			numberFormsSpawning = 3; // number of forms spawn per ture 
			elapseTime = 0.5f; // time beetween tures
			
		} else if (GamePreferences.GetMediumDifficultyState () == 1) {

			waitStrikeTime = 0.6f; // time beetween spawing forms in a ture 
			numberFormsSpawning = 4; // number of forms spawn per ture 
			elapseTime = 0.3f; // time beetween tures
		
		} else if (GamePreferences.GetHardDifficultyState () == 1) {

			waitStrikeTime = 0.5f; // time beetween spawing forms in a ture 
			numberFormsSpawning = 6; // number of forms spawn per ture 
			elapseTime = 0.3f; // time beetween tures
		
		}
	
	}

	private IEnumerator spawnForms() {


		GameObject form;


		for (int i = 0; i < numberFormsSpawning; i++) { 

			// random stuff
			float rnd = Random.Range (0, 19);
			float rndColor = Random.Range (0, maxColours);
			float position = Random.Range (minPos, maxPos);

			int rndColorInt = (int)rndColor;

			if (rnd == 0) {
				// spawn coin
				form = Instantiate(coin, new Vector3(position, transform.position.y, 0), Quaternion.identity) as GameObject;
			} else if (rnd <= 10) {
				// spawn cubes	
				form = Instantiate (cubeModels [rndColorInt], new Vector3 (position, transform.position.y, 0), Quaternion.identity) as GameObject;
			} else if (rnd <= 15) {
				// spawn stars
				form = Instantiate (starModels [rndColorInt], new Vector3 (position, transform.position.y, 0), Quaternion.identity) as GameObject;
			} else if (rnd <= 18) {
				// spawn diamonds
				form = Instantiate (diamondModels [rndColorInt], new Vector3 (position, transform.position.y, 0), Quaternion.identity) as GameObject;
			}
			yield return new WaitForSeconds (waitStrikeTime);
		}
	
	}

	public static float getDestroyTime() {
		return destroyTime;
	}

	public static List<string> getRedTags() {
		return redTagTypes;
	}

	public static List<string> getGreenTags() {
		return greenTagTypes;
	}

	public static List<string> getBlueTags() {
		return blueTagTypes;
	}
}
