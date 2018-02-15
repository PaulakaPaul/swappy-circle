using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {

	public static ShopManager instance;

	public const string CIRCLE_BUTTON = "CircleButton", DROP_BUTTON = "DropsButton", BUFF_BUTTON = "BuffsButton"; // button names

	[SerializeField]
	private Button CirclesButton, DropButton, BuffsButton;
	[SerializeField]
	private GameObject holdingPanel, BackButton, CirclePanelItemHolder, DropPanelItemHolder, BuffPanelItemHolder, moveButtonsPanel;
	[SerializeField]
	private GameObject itemHolderPrefab;

	// we keep a reference to the currentPanel
	private GameObject currentPanelSelected;

	private string selectedButtonName; // we keep the selected Button name from the Options Panel in this variable
	private bool isPanelSelected; // we keep the same back button for all the states of the game, but it will have different functionalities -> we control them with this bool


	//the indexes of the currently showing items in the panel
	private int circleModelsIndex;
	private int dropModelsIndex;
	private int buffModelsIndex;

	//references to the items that are displayed 
	private GameObject firstItem;
	private GameObject secondItem;

	//we keep the view data in those arrays
	private ItemState[] circleStates, dropStates, buffStates;

	// we need a reference to the button of the current selected item
	private Button currentButton;

	// we go left or right with those buttons
	[SerializeField]
	private Button leftArrow, rightArrow;


	void Awake() {
		if (instance == null)
			instance = this;

		circleStates = new ItemState[ShopConstants.NUMBER_CIRCLE_MODELS];
		dropStates = new ItemState[ShopConstants.NUMBER_DROP_MODELS];
		buffStates = new ItemState[ShopConstants.NUMBER_BUFF_MODELS];

	}

	// Use this for initialization
	void Start () {
		//listeners 
		CirclesButton.onClick.AddListener ( () => selectedButton());
		DropButton.onClick.AddListener ( () => selectedButton());
		BuffsButton.onClick.AddListener ( () => selectedButton());
		BackButton.GetComponent<Button> ().onClick.AddListener (() => backButton() );

		// default state
		isPanelSelected = false;

		// the first two items are displayed in the beggining
		circleModelsIndex = 2;
		dropModelsIndex = 2;
		buffModelsIndex = 2;

		createItemStateArrays ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void selectedButton() {
	
		selectedButtonName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name; // gets the current clicked object name

		// we choose the behaviour depending on the button 
		switch(selectedButtonName) {
		case CIRCLE_BUTTON: 
			CirclePanelItemHolder.SetActive (true);
			currentPanelSelected = CirclePanelItemHolder;
			break;
		case DROP_BUTTON:
			DropPanelItemHolder.SetActive (true);
			currentPanelSelected = DropPanelItemHolder;
			break;
		case BUFF_BUTTON:
			BuffPanelItemHolder.SetActive (true);
			currentPanelSelected = BuffPanelItemHolder;
			break;
		}
			
		isPanelSelected = true;
		holdingPanel.SetActive (false);
		BackButton.SetActive (true);
		moveButtonsPanel.SetActive (true);
		populate (selectedButtonName);
	}

	private void backButton() {
		if (isPanelSelected) {
			holdingPanel.SetActive (true);
			isPanelSelected = false;
			currentPanelSelected.SetActive(false);
			moveButtonsPanel.SetActive (false);

			// destroying the current items -> otherwise they will remain on the background
			Destroy (firstItem);
			Destroy (secondItem);

			//setting back the indexes
			circleModelsIndex = 2;
			dropModelsIndex = 2;
			buffModelsIndex = 2;

			//resetting the buttons logic
			leftArrow.interactable = false;
			rightArrow.interactable = true;
		} else {
			SceneFader.instance.fadeIn (GameManager.menuScene);
		}
	}

	private void populate(string selectedButton) {

		Transform transformParent = null;

		//getting the postion of the parent
		switch(selectedButton) {
		case CIRCLE_BUTTON: 
			transformParent = CirclePanelItemHolder.transform;
			break;
		case DROP_BUTTON:
			transformParent = DropPanelItemHolder.transform;
			break;
		case BUFF_BUTTON:
			transformParent = BuffPanelItemHolder.transform;
			break;
		}
	
		if (transformParent != null) {
			 firstItem = Instantiate (itemHolderPrefab, transformParent.position, Quaternion.identity) as GameObject; 
			 secondItem = Instantiate (itemHolderPrefab, transformParent.position, Quaternion.identity) as GameObject; 


			firstItem.transform.SetParent (transformParent, false); // setting the parent of the item threw the transform property
			secondItem.transform.SetParent (transformParent, false); // setting the parent of the item threw the transform property
		}

		int zero = 0; // the first items listed
		int one = 1;

		//populate depending on what we selected
		switch(selectedButton) {
		case CIRCLE_BUTTON: 
			populateItemHolder(circleStates, firstItem, ref zero);
			populateItemHolder (circleStates, secondItem, ref one);
			break;
		case DROP_BUTTON:
			populateItemHolder(dropStates, firstItem, ref zero);
			populateItemHolder (dropStates, secondItem, ref one);
			break;
		case BUFF_BUTTON:
			populateItemHolder(buffStates, firstItem, ref zero);
			populateItemHolder (buffStates, secondItem, ref one);
			if (buffModelsIndex >= buffStates.Length) // last items on the list ( at the very start we had only 2 buffs, that is why i added this here)
				rightArrow.interactable = false; 
			break;
		}
			
	}



	private void createItemStateArrays() {

		for (int i = 0; i < ShopConstants.NUMBER_CIRCLE_MODELS; i++) {
			// circles 
			Sprite pictureCircle = Resources.Load<Sprite> ("Sprites/Players/" + ShopConstants.circleModelNames [i]);
			circleStates [i] = new ItemState (pictureCircle, ShopConstants.BlockedText [i], 
				ShopConstants.ScoreUnblock [i], ShopConstants.LevelUnblock [i], 
				ShopConstants.coinsUnlock [i]);
			
		}

		for (int i = 0; i < ShopConstants.NUMBER_DROP_MODELS; i++) {
			//drops
			Sprite pictureDrop = Resources.Load<Sprite> ("Sprites/Drops/" + ShopConstants.dropModelNames[i]);
			dropStates [i] = new ItemState (pictureDrop, ShopConstants.BlockedText [i], 
				ShopConstants.ScoreUnblock [i], ShopConstants.LevelUnblock [i], 
				ShopConstants.coinsUnlock [i]);
		}

		Sprite[] allPictureBuffs = Resources.LoadAll<Sprite> ("Sprites/Buffs/buffs"); // it worked only like this  ^ ^ 
		for (int i = 0; i < ShopConstants.NUMBER_BUFF_MODELS; i++) {
			//buffs
			//Sprite pictureBuff = Resources.Load<Sprite>("Sprites/Buffs/buffs/" + ShopConstants.buffModelNames[i]);
			buffStates [i] = new ItemState (allPictureBuffs[i], ShopConstants.BlockedTextBuffs [i],
				ShopConstants.ScoreUnblockBuffs [i], ShopConstants.LevelUnblockBuffs [i], 
				ShopConstants.coinsUnlockBuffs [i]);
		}
	}

	public void buttonNext() {

		//we have the same button for all the panels so we need to see parse the info
		switch(selectedButtonName) { // we show the next 2 items
		case CIRCLE_BUTTON: 
			nextItems (circleStates,ref circleModelsIndex, CirclePanelItemHolder.transform);
			break;
		case DROP_BUTTON:
			nextItems (dropStates, ref dropModelsIndex, DropPanelItemHolder.transform);
			break;
		case BUFF_BUTTON:
			nextItems (buffStates,ref buffModelsIndex, BuffPanelItemHolder.transform);
			break;
		}

	}

	public void buttonPrevious() {

		//we have the same button for all the panels so we need to see parse the info
		switch(selectedButtonName) { // we show the previous 2 items
		case CIRCLE_BUTTON: 
			nextItems (circleStates, ref circleModelsIndex, CirclePanelItemHolder.transform, true); // true for previous
			break;
		case DROP_BUTTON:
			nextItems (dropStates,ref dropModelsIndex, DropPanelItemHolder.transform, true);
			break;
		case BUFF_BUTTON:
			nextItems (buffStates,ref buffModelsIndex, BuffPanelItemHolder.transform, true);
			break;
		}
	}


	private GameObject createItemHolder(GameObject item,  ItemState[] items, Transform parentTransform, bool previous, ref int index) {

		Destroy (item); // destroy what was before



		//recreate the view 
		item = Instantiate (itemHolderPrefab, parentTransform.position, Quaternion.identity) as GameObject;
		item.transform.SetParent (parentTransform, false); // setting the parent of the item threw the transform property

		if (!previous) { // go next
			populateItemHolder (items, item,ref index);
			index += 1;
		} else { // go previous
			index -= 1;
			populateItemHolder (items, item,ref index); 
		}
		return item;
	}

	private void nextItems(ItemState[] items,ref int index, Transform parentTransform, bool previous = false) {
	

		rightArrow.interactable = true;
		leftArrow.interactable = true;

			if (!previous) {

				
			
				if (index < items.Length) { 
					firstItem = createItemHolder (firstItem, items, parentTransform, previous, ref index);
					}

				if (index < items.Length) { // index it is increased in the createItemHolder() function
					secondItem = createItemHolder (secondItem, items, parentTransform, previous, ref index);

					if (index >= items.Length) // last items on the list
						rightArrow.interactable = false; 

			
				} else { // logic if this item it's missing and we have on the last page only the first item
				rightArrow.interactable = false; // it means we have reached the end of the list
				if (secondItem != null) {
					Destroy (secondItem);
					index += 1; // preserve the index ( even if we show only one item , the index has to be proccesed as a multiple of 2 -> this is how the program works)
				}
			}

			} else {

			if(index - 5 < 0) // we have reached the end of the list
				leftArrow.interactable = false;

			if (index - 4 >= 0) { // it always has to be in the domain ( the current set and the further set -> -4 )  

				index -= 2; // we always keep the index of the next set of items in the index so subtracting 2 we get the current index of the set of items

				index -= 1; // to keep the order
				firstItem = createItemHolder (firstItem, items, parentTransform, previous, ref index);
				index += 2; // to keep the order
				secondItem = createItemHolder (secondItem, items, parentTransform, previous, ref index);
				index += 1; // to get the corrent index
			} 

		}



	}
	


	private void populateItemHolder(ItemState[] items, GameObject itemHolder,ref int index) {


		if (index < items.Length) { // we start populating only if the index is valid


			int itemUnBlockScore = items [index].ScoreToUnblock;
			string itemLevelToUnblock = items [index].LevelToUnblock;
			bool itemBlocked = false;

			// we see if the item it's blocked or not depending on what the itemState arrays says
			switch (itemLevelToUnblock) {
			case "easy":
				if (GamePreferences.GetEasyDifficultyHighscore () < itemUnBlockScore)
					itemBlocked = true;
				break;
			case "medium":
				if (GamePreferences.GetMediumDifficultyHighscore () < itemUnBlockScore)
					itemBlocked = true;
				break;
			case "hard":
				if (GamePreferences.GetHardDifficultyHighscore () < itemUnBlockScore)
					itemBlocked = true;
				break;
			default: // for the default items (circle and drops)
				itemBlocked = false;
				break;
			}

			// get the two major childs of the itemHolder
			GameObject blocked = itemHolder.transform.Find ("Blocked").gameObject;
			GameObject unblocked = itemHolder.transform.Find ("Item").gameObject;

			if (itemBlocked) { // item it's blocked
				blocked.SetActive (true);
				unblocked.SetActive (false);

				Text lockText = blocked.transform.Find ("LockText").gameObject.GetComponent<Text> ();
				lockText.text = items [index].BlockedDescription;
			} else { //item it's unblocked
		
				blocked.SetActive (false);
				unblocked.SetActive (true);
		
				Image itemImage = unblocked.transform.Find ("ItemImage").gameObject.GetComponent<Image> ();
				itemImage.sprite = items [index].Picture;



				//we have to know the type we pass to the getItemStatus() method, depending on what item we want to query
				string getItemStatusType = "";
				switch (selectedButtonName) {
				case CIRCLE_BUTTON:
					getItemStatusType = GameSaver.CIRCLE;
					break;
				case DROP_BUTTON:
					getItemStatusType = GameSaver.DROP;
					break;
				case BUFF_BUTTON:
					getItemStatusType = GameSaver.BUFF;
					break;
				}


				Button button = unblocked.transform.Find ("UnlockButton").gameObject.GetComponent<Button> ();

				if (GameSaver.instance.getItemStatus (getItemStatusType, index) == true) { // if it's unlocked
					GameObject unLockedText = unblocked.transform.Find ("UnlockButton/UnlockedText").gameObject;
					unLockedText.SetActive (true);

					//making the current selected item uninteractable
					switch (selectedButtonName) {
					case CIRCLE_BUTTON:
						if (GamePreferences.GetCircleIndex () == index) {
							button.interactable = false;
							currentButton = button;
						}
						break;
					case DROP_BUTTON:
						if (GamePreferences.GetDropIndex () == index) {
							button.interactable = false;
							currentButton = button;
						}
						break;
					case BUFF_BUTTON:
						button.interactable = false;   // the buffs are passive so we don't need to choose them after unlocking them
						break;
					}

					int currentIndex = index;

					if(!selectedButtonName.Equals(ShopManager.BUFF_BUTTON))  // the buffs are passive so we don't need to choose them after unlocking them
						button.onClick.AddListener (() => chooseCurrentItem (currentIndex, button));

				} else { // if it's locked
					GameObject lockedGameObject = unblocked.transform.Find ("UnlockButton/LockedState").gameObject;
					lockedGameObject.SetActive (true);
					Text coinCostText = lockedGameObject.transform.Find ("coinCostText").gameObject.GetComponent<Text> ();
					coinCostText.text = items[index].CoinsToUnlock.ToString ();

					int currentIndex = index; // so we pass it to the listener ( index it's sent as a reference)

					button.onClick.AddListener( () => unlockItem(currentIndex, button, items, lockedGameObject, getItemStatusType) );
				}
		
			} 
		} 
	}


	private void chooseCurrentItem(int index, Button button) {
		switch (selectedButtonName) {
		case CIRCLE_BUTTON:
			GamePreferences.SetCircleIndex (index);
			break;
		case DROP_BUTTON:
			GamePreferences.SetDropIndex (index);
			break;
		case BUFF_BUTTON:
			//no logic cuz ->  the buffs are passive so we don't need to choose them after unlocking them
			break;
		}

		// we make the interacteble swap between the items
		if(currentButton != null) // it is possible that the current button is null cuz a circle from a different page was selected -> therefor at this point in time it will be null
			currentButton.interactable = true;
		button.interactable = false;

		// give reference to the current selected button
		currentButton = button;
	}

	private void unlockItem(int index, Button button, ItemState[] items, GameObject lockedGameObject, string type) {

		int coinsToUnlock = items[index].CoinsToUnlock;

		if (coinsToUnlock <= GamePreferences.GetCoinScore ()) {// if you have enough coins we proceed
			GamePreferences.SetCoinScore (GamePreferences.GetCoinScore () - coinsToUnlock); //make the transition
			GameSaver.instance.unlockItem(type, index);
	
			//make the locked view invisible
			lockedGameObject.SetActive (false);

			//make the choose options visible
			GameObject unlockedGameObject = button.gameObject.transform.Find ("UnlockedText").gameObject;
			unlockedGameObject.SetActive (true);

			//change the listener of the button
			if (!type.Equals (GameSaver.BUFF)) {
				button.onClick.RemoveAllListeners ();
				button.onClick.AddListener (() => chooseCurrentItem (index, button));
			} else if (type.Equals (GameSaver.BUFF)) { // the buffs are passive so we don't need to choose them after unlocking them
				button.interactable = false;
				PanelBrain.instance.EnableBuffPictures ();
			}
		}
	}


}
