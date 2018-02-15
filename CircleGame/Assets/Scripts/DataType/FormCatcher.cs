using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class FormCatcher {

	public const float UNIFIED_TIME = 5f;
	private static bool stillUnified; // it's here cuz we use this variable only in this function -> we use this to know when we caught a unified drop
	public static bool StillUnified { get { return stillUnified; } set { stillUnified = value; } }

	static FormCatcher() {
		stillUnified = false;
	}

	public static void FormOnTriggerEnter2D(Collider2D col, GameObject sparkObj, GameObject coinSpark, string cubeTag, string starTag, string diamondTag,
		string timeDropTag, string unifiedDropTag, string rainDropTag,
		char c1, char c2, GameObject c1Zero, GameObject c2Zero,
		GameObject dropSliderTime, GameObject dropSliderRain, GameObject dropSliderUnified) { // c1 -> first letter of one of the complementary colour , c2 -> first letter of the other ...
		// the c1 colour comes pachet with the c1Zero colour ( c1, c2 can be o only 'g' , 'r' , 'b')
	
		GameObject spark = null; 
		GameObject coinSparkReference = null;
		bool caught = true; // var to play audioclip only when a form its caught, caught = false if the form it's incorrect

		// increment score or do logic for every dropped object

		if (col.tag == cubeTag) {
			spark = cubeCought (sparkObj, col);
		} else if (col.tag == starTag) {
			spark = starCought (sparkObj, col);
		} else if (col.tag == diamondTag) {
			spark = diamondCought (sparkObj, col);
		} else if (col.tag == FormsSpawner.coinTag) {
			coinSparkReference = Object.Instantiate<GameObject> (coinSpark, col.gameObject.transform.position, Quaternion.identity);
			ScoreManager.instance.incrementCoins ();
		} else if (col.tag == timeDropTag) { 
			if (FormsSpawner.instance.IsTimeDropActivated) // same like at rain drop and unified drop -> so the first cycle won't stop the second cycle when it resets the data from the first cycle
				FormsSpawner.instance.TimeDropStopLowerGravityScale ();
			
			CircleController.addTimeFromTimeDrop (); // add time
			FormsSpawner.instance.TimeDropLowerGravityScale (); // lower rate
			callTimeDropSlider(dropSliderTime); // show the slider
		} else if (col.tag == rainDropTag) {
			if (FormsSpawner.instance.IsRainDropActivated) // if we already have a rain drop cycle activated we have to stop it and only after start the new one
				// otherwise the old cycle will set the isRainDropActivated variable to false and the second cycle will finish faster
				FormsSpawner.instance.StopRainDropSpawn ();
			
			FormsSpawner.instance.rainDropSpawn ();
			callRainDropSlider (dropSliderRain); // show the slider
		} else if (col.tag == unifiedDropTag) {
			
			if (stillUnified) // if we cought a unifiedDrop and after cought another one without finishing the first one we need to reset the on going coroutine
				// otherwise the first coroutine will put the stillUnified variable on false and will finish the second timing too
				Timer.instance.stopTimerForUnifiedDrop();
			
			stillUnified = true;
			Timer.instance.timerForUnifiedDrop ();
			callUnifiedDropSlider (dropSliderUnified); // show slider
		} else { // it means we cought a different colour form
			
			if (stillUnified) { // it mean's that we have a unified drop cycle active and we don't loose points

				GameObject sparkUnified = null;

				var cubes = new HashSet<string> {
					FormsSpawner.redTagTypes [0],
					FormsSpawner.blueTagTypes [0],
					FormsSpawner.greenTagTypes [0]
				};
				cubes.Remove (cubeTag); // we don't need the current cube Tag

				if (cubes.Contains (col.tag))
					sparkUnified = cubeCought (sparkObj, col);

				var stars = new HashSet<string> {
					FormsSpawner.redTagTypes [1],
					FormsSpawner.blueTagTypes [1],
					FormsSpawner.greenTagTypes [1]
				};
				stars.Remove (starTag); // we don't need the current star Tag
				if(stars.Contains(col.tag)) 
					sparkUnified = starCought(sparkObj, col);

				var diamonds = new HashSet<string> {
					FormsSpawner.redTagTypes [2],
					FormsSpawner.blueTagTypes [2],
					FormsSpawner.greenTagTypes [2]
				};
				diamonds.Remove (diamondTag); // we don't need the current diamond Tag
				if (diamonds.Contains (col.tag))
					sparkUnified = diamondCought(sparkObj, col);

				if (sparkUnified != null)
					Object.Destroy (sparkUnified, 1f);

			} else { // we have 0 points -> different colour caught
				// activate lose animation/code
				CircleController.resetCounter ();
				caught = false;

				char colour = col.tag [0];
				GameObject zero = null;
				if (colour == c1) {
					zero = Object.Instantiate<GameObject> (c1Zero, col.gameObject.transform.position, Quaternion.identity);
				} else if (colour == c2) {
					zero = Object.Instantiate<GameObject> (c2Zero, col.gameObject.transform.position, Quaternion.identity);
				}
				if (zero != null)
					Object.Destroy (zero, 1f);
			}
		} 



		// play audio when catch a form
		if(caught)
			AudioSource.PlayClipAtPoint (MusicController.instance.catchFormClip, Vector3.zero, MusicController.instance.VolumeAudioClips);

		if (spark != null) {
			Object.Destroy (spark, 1f);
		}

		if (coinSparkReference != null) {
			Object.Destroy (coinSparkReference, 1f);
		}
		Object.Destroy (col.gameObject, FormsSpawner.getDestroyTime ());
	
	
	}

	private static GameObject cubeCought(GameObject sparkObj, Collider2D col) {
		ScoreManager.instance.incrementCube ();
		return Object.Instantiate<GameObject> (sparkObj, col.gameObject.transform.position, Quaternion.identity);
	}

	private static GameObject starCought(GameObject sparkObj, Collider2D col) {
		ScoreManager.instance.incrementStar ();
		return Object.Instantiate<GameObject> (sparkObj, col.gameObject.transform.position, Quaternion.identity);
	}

	private static GameObject diamondCought(GameObject sparkObj, Collider2D col) {
		ScoreManager.instance.incrementDiamond ();
		return Object.Instantiate<GameObject> (sparkObj, col.gameObject.transform.position, Quaternion.identity);
	}

	private static void callTimeDropSlider(GameObject dropSlider) {
		dropSlider.SetActive (true);
		dropSlider.GetComponent<SpecialDropSlider> ().setupSlider (FormsSpawner.TIME_DROP_DURATION);
	}

	private static void callUnifiedDropSlider(GameObject dropSlider) {
		dropSlider.SetActive (true);
		dropSlider.GetComponent<SpecialDropSlider> ().setupSlider (FormCatcher.UNIFIED_TIME);
	}

	private static void callRainDropSlider(GameObject dropSlider) {
		dropSlider.SetActive (true);
		dropSlider.GetComponent<SpecialDropSlider> ().setupSlider (FormsSpawner.instance.getRainSpawnTime());
	}
	
}
