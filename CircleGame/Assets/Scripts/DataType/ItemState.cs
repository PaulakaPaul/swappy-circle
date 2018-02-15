using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemState {

	private Sprite picture;
	private string blockedDescription;
	private int scoreToUnblock;
	private string levelToUnblock;
	private int coinsToUnlock;

	public ItemState(Sprite picture, string blockedDescription, int scoreToUnblock, string levelToUnblock, int coinsToUnlock) {
		this.picture = picture;
		this.blockedDescription = blockedDescription;
		this.scoreToUnblock = scoreToUnblock;
		this.levelToUnblock = levelToUnblock;
		this.coinsToUnlock = coinsToUnlock;
	}

	public Sprite Picture {
		get { return picture; } 
	}

	public string BlockedDescription {
		get { return blockedDescription; }
	}

	public int ScoreToUnblock {
		get { return scoreToUnblock; }
	}

	public string LevelToUnblock {
		get { return levelToUnblock; }
	}

	public int CoinsToUnlock {
		get {return coinsToUnlock; }
	}

}
