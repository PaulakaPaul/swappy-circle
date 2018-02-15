using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormsSpawner : MonoBehaviour {


	public static FormsSpawner instance;

	[SerializeField]
	private GameObject coin;
	[SerializeField]
	private GameObject[] unifiedDrops, rainDrops, timeDrops;

	private GameObject[] cubeModels;
	private GameObject[] starModels;
	private GameObject[] diamondModels;

	[SerializeField]
	private float minPos;   // var to keep the spawn x domain                    
	[SerializeField]
	private float maxPos;   // var to keep the spawn x domain                   

	private readonly int maxColours = 3;      
	private float gravityScale;
	private float waitStrikeTime; // time beetween spawing forms in a ture 
	private int numberFormsSpawning; // number of forms spawn per ture 
	private float elapseTime; // time beetween tures

	//special drops variables
	private readonly int rainDropRate = 10; // everything related to the drops(except gravity scale) happens 10 times faster
	private bool isRainDropActivated; // we know when we caught a rainDrop
	public bool IsRainDropActivated  { get { return isRainDropActivated; } }
	private bool isTimeDropActivated;
	public bool IsTimeDropActivated { get { return isTimeDropActivated; } } // logic var
	public const float RAIN_DROP_FINAL_EASY_DURATION = 2.5f;
	private float rainDropFinalDurationScaled; // this duration will be scaled for every level relative to the EASY_DURATION and gravityscale
	private float rainDropFinalDurationScaledWhileTimeDropActivated; // we need another scaled duration when we loweer the gravity when we catch a time drop
	public const float TIME_DROP_DURATION = 5f; // lower gravity scale duration
	public const float LOWER_GRAVITY_SCALE_RATE = 0.15f; // this is the rate we lower the gravity scale when we catch a time drop
	public const float EASY_GRAVITY_SCALE = 0.10f;
	private float addToSmoothTheRainDropDuration;

	private static readonly float destroyTime = 0.08f; // destroy time for the forms




	//tags for all the items
	public const string coinTag = "coin";
	public static readonly List<string> unifiedDropTags = new List<string> { "blueUnifiedDrop", "greenUnifiedDrop", "redUnifiedDrop" };
	public static readonly List<string> rainDropTags = new List<string> { "blueRainDrop", "greenRainDrop", "redRainDrop"} ;
	public static readonly List<string> timeDropTags = new List<string> {"blueTimeDrop", "greenTimeDrop", "redTimeDrop"} ;
	public static readonly List<string> redTagTypes = new List<string>  { "redCube", "redStar", "redDiamond"};
	public static readonly List<string> greenTagTypes = new List<string> {"greenCube", "greenStar", "greenDiamond"};
	public static readonly List<string> blueTagTypes = new List<string> {"blueCube", "blueStar", "blueDiamond"};


	void Awake(){
		if (instance == null)
			instance = this;
		
		levelTimeSpawnSelect ();
		grabModels ();
	}
	// Use this for initialization
	void Start () {
		isRainDropActivated = false; // isn't by default
		isTimeDropActivated = false; // isn't by default
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
			waitStrikeTime = 0.80f; // time beetween spawing forms in a ture 
			numberFormsSpawning = 3; // number of forms spawn per ture 
			elapseTime = 0.60f; // time beetween tures
			gravityScale = EASY_GRAVITY_SCALE; // how fast the drops are dropping...
			addToSmoothTheRainDropDuration = 0f;

		} else if (GamePreferences.GetMediumDifficultyState () == 1) {

			waitStrikeTime = 0.75f; // time beetween spawing forms in a ture 
			numberFormsSpawning = 4; // number of forms spawn per ture 
			elapseTime = 0.55f; // time beetween tures
			gravityScale = 0.20f; // how fast the drops are dropping...
			addToSmoothTheRainDropDuration = 1f;

		} else if (GamePreferences.GetHardDifficultyState () == 1) {

			waitStrikeTime = 0.67f; // time beetween spawing forms in a ture 
			numberFormsSpawning = 5; // number of forms spawn per ture 
			elapseTime = 0.45f; // time beetween tures
			gravityScale = 0.26f; // how fast the drops are dropping...
			addToSmoothTheRainDropDuration = 1.5f;
		}
	
		// easy 3 rule, but inversly proportional
		rainDropFinalDurationScaled = (RAIN_DROP_FINAL_EASY_DURATION * EASY_GRAVITY_SCALE) / gravityScale + addToSmoothTheRainDropDuration; // we need to scale this cuz if the gravity scale is higher the forms drop faster
		rainDropFinalDurationScaledWhileTimeDropActivated = (gravityScale * rainDropFinalDurationScaled) / (gravityScale - LOWER_GRAVITY_SCALE_RATE); 

	}

	private void grabModels() {

		MultiDimensionalObject currentDrop = GameManager.instance.getCurrentDrops ();

		cubeModels = new GameObject[maxColours];
		starModels = new GameObject[maxColours];
		diamondModels = new GameObject[maxColours];

		for (int i = 0; i < maxColours; i++) {
			cubeModels [i] = currentDrop.objects [i]; // we store the cubes from 0 to 2
			cubeModels[i].GetComponent<Rigidbody2D>().gravityScale = gravityScale;

			starModels [i] = currentDrop.objects[ i + 3]; // we store the stars from 3 to 5
			starModels[i].GetComponent<Rigidbody2D>().gravityScale = gravityScale;

			diamondModels[i] = currentDrop.objects[i + 6]; // we store the diamons from 6 to 8
			diamondModels[i].GetComponent<Rigidbody2D>().gravityScale = gravityScale;

			// settings gravity scale for all the models even special drops
			rainDrops[i].GetComponent<Rigidbody2D> ().gravityScale = gravityScale;
			timeDrops[i].GetComponent<Rigidbody2D> ().gravityScale = gravityScale;
			unifiedDrops[i].GetComponent<Rigidbody2D> ().gravityScale = gravityScale;
		}

		// settings gravity scale for all the models even special drops
		coin.GetComponent<Rigidbody2D> ().gravityScale = gravityScale;

	}

	private IEnumerator spawnForms() {


		GameObject form;


		for (int i = 0; i < numberFormsSpawning; i++) { 

			// random stuff
			float rnd = Random.Range (0, 100);
			float rndColor = Random.Range (0, maxColours);
			float position = Random.Range (minPos, maxPos); // x position for the drop ( y is the FormsSpawners transform.position.y)

			Vector3 formPos = new Vector3 (position, transform.position.y, 0);
			int rndColorInt = (int)rndColor;

			if (rnd < 5) { //5%
				//spawn rain drop
				form = Instantiate (rainDrops[rndColorInt], formPos, Quaternion.identity) as GameObject;
			} else if (rnd < 10) { // 5%
				// spawn unified drop
				form = Instantiate (unifiedDrops[rndColorInt], formPos, Quaternion.identity) as GameObject;
			} else if (rnd < 15) { // 5%
				// spawn time drop
				form = Instantiate (timeDrops[rndColorInt], formPos, Quaternion.identity) as GameObject;
			} else if (rnd < 30) { // 15%
				// spawn coin
						form = Instantiate(coin, formPos, Quaternion.identity) as GameObject;
			} else if (rnd < 65) { // 35%
				// spawn cubes	
						form = Instantiate (cubeModels [rndColorInt], formPos, Quaternion.identity) as GameObject;
			} else if (rnd < 85) { // 20%
				// spawn stars
						form = Instantiate (starModels [rndColorInt], formPos, Quaternion.identity) as GameObject;
			} else if (rnd < 100) { // 15%
				// spawn diamonds
						form = Instantiate (diamondModels [rndColorInt], formPos, Quaternion.identity) as GameObject;
			}
				
			yield return new WaitForSeconds (waitStrikeTime);
		}

			
	}

	public void rainDropSpawn() {
		StartCoroutine("rainDropSpawnEnum");
	}

	public void StopRainDropSpawn() {
		StopCoroutine ("rainDropSpawnEnum");
	}

	private IEnumerator rainDropSpawnEnum() { // activated when we catch a rainDrop drop

		GameObject form;

		int numberFormsRainSpawning = numberFormsSpawning * rainDropRate;
		float waitStrikeTimeRainDrop = waitStrikeTime / rainDropRate;
		isRainDropActivated = true; // we need this to notify the solid checker that the rain drop is activated ( we can't die when this is activated)


		for (int i = 0; i < numberFormsRainSpawning; i++) { 

			// random stuff
			float rnd = Random.Range (0, 100);
			float rndColor = Random.Range (0, maxColours);
			float position = Random.Range (minPos, maxPos); // x position for the drop ( y is the FormsSpawners transform.position.y)

			Vector3 formPos = new Vector3 (position, transform.position.y, 0);
			int rndColorInt = (int)rndColor;

			 if (rnd < 40) { // 40%
				// spawn cubes	
				form = Instantiate (cubeModels [rndColorInt], formPos, Quaternion.identity) as GameObject;
			} else if (rnd < 80) { // 40%
				// spawn stars
				form = Instantiate (starModels [rndColorInt], formPos, Quaternion.identity) as GameObject;
			} else if (rnd < 100) { // 20%
				// spawn diamonds
				form = Instantiate (diamondModels [rndColorInt], formPos, Quaternion.identity) as GameObject;
			}
			yield return new WaitForSeconds (waitStrikeTimeRainDrop);
		}
			
		if (coin.GetComponent<Rigidbody2D> ().gravityScale == gravityScale - LOWER_GRAVITY_SCALE_RATE) { // lower gravity scale -> higher time -> we need to change it  
			yield return new WaitForSeconds(rainDropFinalDurationScaledWhileTimeDropActivated);
		} else { // the normal drop rate
			yield return new WaitForSeconds (rainDropFinalDurationScaled);
		}

		isRainDropActivated = false;
	}

	public static float getDestroyTime() {
		return destroyTime;
	}

	public void TimeDropLowerGravityScale() {
		StartCoroutine ("catchTimeDropEnum");
	}

	public void TimeDropStopLowerGravityScale() {
		StopCoroutine ("catchTimeDropEnum");
	}

	private IEnumerator catchTimeDropEnum() {
		isTimeDropActivated = true;
		changeGravityScale (gravityScale - LOWER_GRAVITY_SCALE_RATE);
		yield return new WaitForSeconds (TIME_DROP_DURATION);
		changeGravityScale (gravityScale);
		isTimeDropActivated = false;
	}

	private void changeGravityScale(float gravityScale) {

		coin.GetComponent<Rigidbody2D> ().gravityScale = gravityScale;
		for (int i = 0; i < maxColours; i++) {
			cubeModels [i].GetComponent<Rigidbody2D> ().gravityScale = gravityScale;
			starModels [i].GetComponent<Rigidbody2D> ().gravityScale = gravityScale;
			diamondModels [i].GetComponent<Rigidbody2D> ().gravityScale = gravityScale;

			timeDrops [i].GetComponent<Rigidbody2D> ().gravityScale = gravityScale;
			rainDrops [i].GetComponent<Rigidbody2D> ().gravityScale = gravityScale;
			unifiedDrops [i].GetComponent<Rigidbody2D> ().gravityScale = gravityScale;
		}
	}

	public float getRainSpawnTime() {

		if (coin.GetComponent<Rigidbody2D> ().gravityScale == gravityScale - LOWER_GRAVITY_SCALE_RATE) { // lower gravity scale -> higher time -> we need to change it  
			return  numberFormsSpawning * rainDropRate * (waitStrikeTime / rainDropRate) + rainDropFinalDurationScaledWhileTimeDropActivated;
		} else { // the normal drop rate
			return numberFormsSpawning * rainDropRate * (waitStrikeTime / rainDropRate) +  (rainDropFinalDurationScaled);
		}
	}

}
