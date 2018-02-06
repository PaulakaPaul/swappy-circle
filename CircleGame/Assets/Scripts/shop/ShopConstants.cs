using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopConstants { // those constants will help creating the layout to the shop scene

	public const int NUMBER_CIRCLE_MODELS = 5;
	public const int NUMBER_DROP_MODELS = 4;
	public const int NUMBER_BUFF_MODELS = 2;

	//circle and dropds constants -> logic starts from 1 ( the 0 spot is reserved for the default items)
	public static readonly string[] BlockedText = new string[]{ "", "1000 on easy level\nto unlock", "1000 on medium level\nto unlock", "1400 on medium level\nto unlock", "1000 on hard level\nto unlock"};
	public static readonly int[] ScoreUnblock = new int[] { -1, 1000, 1000, 1400, 1000};
	public static readonly string[] LevelUnblock = new string[] { "", "easy", "medium", "medium", "hard"};
	public static readonly int[] coinsUnlock = new int[] { -1, 1000, 3000,  5000, 7000};

	// buffs constants -> logic starts from 0 ( we have no default buff)
	public static readonly string[] BlockedTextBuffs = new string[]{"1400 on medium level\nto unlock", "1000 on hard level\nto unlock"};
	public static readonly int[] ScoreUnblockBuffs = new int[] {1400, 1000};
	public static readonly string[] LevelUnblockBuffs = new string[] {"medium", "hard"};
	public static readonly int[] coinsUnlockBuffs = new int[] {3500, 5000};

	public static readonly string[] circleModelNames = new string[]{"startCircle", "circle1", "circle2", "circle3", "circle4"};
	public static readonly string[] dropModelNames = new string[]{"startDrop", "forms1", "forms2", "forms3"};
	public static readonly string[] buffModelNames = new string[]{"coin_buff", "score_buff"};
}
