using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenAreaChecker : MonoBehaviour {


	public const string greenCube = "greenCube";
	public const string greenStar = "greenStar";
	public const string greenDiamond = "greenDiamond";
	public const string greenTimeDrop = "greenTimeDrop";
	public const string greenRainDrop = "greenRainDrop";
	public const string greenUnifiedDrop = "greenUnifiedDrop";

	[SerializeField]
	private GameObject redZero;
	[SerializeField]
	private GameObject blueZero;
	[SerializeField]
	private GameObject greenSpark, coinSpark;



	void OnTriggerEnter2D(Collider2D col) {
		FormCatcher.FormOnTriggerEnter2D (col, greenSpark, coinSpark, greenCube, greenStar, greenDiamond, greenTimeDrop, greenUnifiedDrop, greenRainDrop,'r', 'b', redZero, blueZero, 
			UIManager.instance.getDropTimeSlider(), 
			UIManager.instance.getDropRainSlider(), 
			UIManager.instance.getDropUnifiedSlider()); // static function made in the DataType directory
	}
}
	


