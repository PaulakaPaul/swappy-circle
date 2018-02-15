using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


[Serializable]
public class SaveData{

	private bool[] circlesUnlocked;
	private bool[] dropsUnlocked;
	private bool[] buffsUnlocked;


	public SaveData(bool[] circlesUnlocked, bool[] dropsUnlocked, bool[] buffsUnlocked) {
		this.circlesUnlocked = circlesUnlocked;
		this.dropsUnlocked = dropsUnlocked;
		this.buffsUnlocked = buffsUnlocked;
	}

	public SaveData() {}


	public bool[] CirclesUnlocked {
		get {
			return circlesUnlocked;
		}

		set {
			circlesUnlocked = value;
		}
	}

	public bool[] DropsUnlocked {
		get {
			return dropsUnlocked;
		}

		set {
			dropsUnlocked = value;
		}
	}

	public bool[] BuffsUnlocked {
		get {
			return buffsUnlocked;
		}

		set {
			buffsUnlocked = value;
		}
	}
}
