using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedAreaChecker : MonoBehaviour {

	public const string redCube = "redCube"; // implicit static
	public const string redStar = "redStar";
	public const string redDiamond = "redDiamond";
	public const string redTimeDrop = "redTimeDrop";
	public const string redRainDrop = "redRainDrop";
	public const string redUnifiedDrop = "redUnifiedDrop";

	[SerializeField]
	private GameObject greenZero;
	[SerializeField]
  	private GameObject blueZero;
	[SerializeField]
	private GameObject redSpark, coinSpark;



	void OnTriggerEnter2D(Collider2D col) {
		FormCatcher.FormOnTriggerEnter2D (col, redSpark, coinSpark, redCube, redStar, redDiamond, redTimeDrop, redUnifiedDrop, redRainDrop, 'b', 'g', blueZero, greenZero, 
			UIManager.instance.getDropTimeSlider(), 
			UIManager.instance.getDropRainSlider(), 
			UIManager.instance.getDropUnifiedSlider()); // static function made in the DataType directory
		} 
		


}


