using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueAreaChecker : MonoBehaviour {


	public const string blueCube = "blueCube";
	public const string blueStar = "blueStar";
	public const string blueDiamond = "blueDiamond";
	public const string blueTimeDrop = "blueTimeDrop";
	public const string blueRainDrop = "blueRainDrop";
	public const string blueUnifiedDrop = "blueUnifiedDrop";

	[SerializeField]
	private GameObject greenZero;
	[SerializeField]
	private GameObject redZero;
	[SerializeField]
	private GameObject blueSpark, coinSpark;


	void OnTriggerEnter2D(Collider2D col) {
		FormCatcher.FormOnTriggerEnter2D (col, blueSpark, coinSpark, blueCube, blueStar, blueDiamond, blueTimeDrop, blueUnifiedDrop, blueRainDrop, 'r', 'g', redZero, greenZero, 
			UIManager.instance.getDropTimeSlider(), 
			UIManager.instance.getDropRainSlider(), 
			UIManager.instance.getDropUnifiedSlider()); // static function made in the DataType directory
	}
}



