using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyCoroutine  {

	public static IEnumerator WaitForRealSeconds(float time) {
	
		float start = Time.realtimeSinceStartup; // the time since the game started , relative to the beggining of the game , indipendent with the Time.timescale = 0f;

		while (Time.realtimeSinceStartup < (start + time)) {
			yield return null;
		}
	}


}
