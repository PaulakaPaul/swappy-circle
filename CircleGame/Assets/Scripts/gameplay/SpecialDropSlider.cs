using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialDropSlider : MonoBehaviour {

	private Slider specialDropSlider;
	private float timer = 1000f; // so it wont deactivate it's self until the setupSlider function it's called

	void Awake() {
		
		specialDropSlider = gameObject.GetComponent<Slider> ();
	}

	void Update() {

		// updating the slider
		if (timer > 0) {
			timer -= Time.deltaTime;
			specialDropSlider.value = timer;
		} else { // when the time it's finished the object it's deactivated
			gameObject.SetActive (false);
		}

	}
		
	public void setupSlider(float maxValue) {
		specialDropSlider.maxValue = maxValue;
		specialDropSlider.minValue = 0f;

		specialDropSlider.value = maxValue;
		timer = maxValue;
	}
}
