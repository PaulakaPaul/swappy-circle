using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {
	// timer for non monobehaviour classes

	public static Timer instance;

	void Awake() {
		if (instance == null)
			instance = this;
	}


	public void timerForUnifiedDrop() {
		StartCoroutine ("timerEnumUnifiedDrop", FormCatcher.UNIFIED_TIME);
	}

	public void stopTimerForUnifiedDrop() {
		StopCoroutine ("timerEnumUnifiedDrop");
	}
	
	private  IEnumerator timerEnumUnifiedDrop(float time) {
		yield return new WaitForSecondsRealtime (time);
		FormCatcher.StillUnified = false;
	}
}
