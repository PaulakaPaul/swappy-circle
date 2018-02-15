using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameSaver : MonoBehaviour {

	public static GameSaver instance;

	public const string CIRCLE = "circle", DROP = "drop", BUFF = "buff";

	private bool[] circlesUnlocked;
	private bool[] dropsUnlocked;
	private bool[] buffsUnlocked;

	private readonly string FILE_NAME = "swappyCircleGameData.dat";
	private SaveData saveData;

	void Awake() {

		Debug.Log (Application.persistentDataPath);

		if (instance == null)
			instance = this;


		LoadGameData (); // it has to be in the Awake() function cuz other scripts need this data in the start function
		initializeGame ();
	}

	// Use this for initialization
	void Start () {
		


		/*
		Debug.Log ("currentCircleIndex: " + GamePreferences.GetCircleIndex ());
		for (int i = 0; i < ShopConstants.NUMBER_CIRCLE_MODELS; i++)
			Debug.Log (i + ": " + circlesUnlocked [i]); */
	}


	void OnDisable() { //data saved when this object it's destroyed, until then we keep everything dynamically
		SaveGameData ();
		Debug.Log ("SaveGameData");
	}


	private void initializeGame() {

		if (saveData == null) {

			circlesUnlocked = new bool[ShopConstants.NUMBER_CIRCLE_MODELS];
			dropsUnlocked = new bool[ShopConstants.NUMBER_DROP_MODELS];
			buffsUnlocked = new bool[ShopConstants.NUMBER_BUFF_MODELS];


			// initialize var in arrays
			circlesUnlocked [0] = true; // the first model is unlocked by default
			dropsUnlocked [0] = true; // the first model is unlocked by default

			for (int i = 1; i < ShopConstants.NUMBER_CIRCLE_MODELS; i++)
				circlesUnlocked [i] = false;

			for (int i = 1; i < ShopConstants.NUMBER_DROP_MODELS; i++)
				dropsUnlocked [i] = false;

			for (int i = 0; i < ShopConstants.NUMBER_BUFF_MODELS; i++)
				buffsUnlocked [i] = false;

			// creating the saveData object
			saveData = new SaveData (circlesUnlocked, dropsUnlocked, buffsUnlocked); 

			SaveGameData ();
			LoadGameData ();
		}

	}


	public void SaveGameData() {

		using (FileStream file = File.Create(Application.persistentDataPath + "/" + FILE_NAME)) // using -> try with resources
			 {
			
			BinaryFormatter bf = new BinaryFormatter ();

			if (saveData != null) {
			
				saveData.CirclesUnlocked = circlesUnlocked;
				saveData.DropsUnlocked = dropsUnlocked;
				saveData.BuffsUnlocked = buffsUnlocked;

				bf.Serialize (file, saveData);
			}

		}
	}


	public void LoadGameData() {

		if (File.Exists (Application.persistentDataPath + "/" + FILE_NAME)) { // this this test for the first time we open the game -> we don't have the file

			try {
	
				using (FileStream file = File.Open (Application.persistentDataPath + "/" + FILE_NAME, FileMode.Open)) { // using -> try with resources 

					BinaryFormatter bf = new BinaryFormatter ();

					saveData = (SaveData)bf.Deserialize (file);

					if (saveData != null) {
						this.circlesUnlocked = saveData.CirclesUnlocked;
						this.dropsUnlocked = saveData.DropsUnlocked;
						this.buffsUnlocked = saveData.BuffsUnlocked;


						// if the arrays changed in the ShopConstants, when we grab the old version from the file we recreate the array with it's new form
						if (circlesUnlocked.Length != ShopConstants.NUMBER_CIRCLE_MODELS) {
							circlesUnlocked = validateData (circlesUnlocked, ShopConstants.NUMBER_CIRCLE_MODELS);
						}
						
						if (dropsUnlocked.Length != ShopConstants.NUMBER_DROP_MODELS) {
							dropsUnlocked = validateData (dropsUnlocked, ShopConstants.NUMBER_DROP_MODELS);
						}

						if (buffsUnlocked.Length != ShopConstants.NUMBER_BUFF_MODELS) {
							buffsUnlocked = validateData (buffsUnlocked, ShopConstants.NUMBER_BUFF_MODELS);
						}

					}

				}

			}  catch (FileNotFoundException) {
				saveData = null; // if the file is not yet created on the disk 
			}
		} else {
			saveData = null;
		}
	}


	private bool[] validateData(bool[] data, int constantIndex) {
		int index;

		if(constantIndex > data.Length) // take the lower int as index
			index = data.Length;
		else 
			index = constantIndex;

		bool[] aux = new bool[constantIndex]; // copy the common data in a new array

		for(int i = 0 ; i < index; i++)
			aux[i] = data[i];

		for(int i = index ; i < constantIndex ; i++) // if index = NUMBER_CIRCLE_MODELS this for wont do anything otherwise it will add false to the extra fields
			aux[i] = false;

		return aux;
	}

	public void unlockItem(string type, int itemIndex) {

		switch (type) {
		case CIRCLE:
			if(itemIndex < ShopConstants.NUMBER_CIRCLE_MODELS)
				circlesUnlocked [itemIndex] = true;
			break;
		case DROP:
			if(itemIndex < ShopConstants.NUMBER_DROP_MODELS)
				dropsUnlocked [itemIndex] = true;
			break;
		case BUFF:
			if(itemIndex < ShopConstants.NUMBER_BUFF_MODELS)
				buffsUnlocked [itemIndex] = true;
			break;
		}
	}

	public bool getItemStatus(string type, int itemIndex) {
	
		switch (type) {
		case CIRCLE:
			if (itemIndex < ShopConstants.NUMBER_CIRCLE_MODELS)
				return circlesUnlocked [itemIndex];
			break;
		case DROP:
			if (itemIndex < ShopConstants.NUMBER_DROP_MODELS)
				return dropsUnlocked [itemIndex];
			break;
		case BUFF:
			if (itemIndex < ShopConstants.NUMBER_BUFF_MODELS)
				return buffsUnlocked [itemIndex];
			break;
		}
		
		throw new NotFoundException ("The item has not been found");
	}


}
