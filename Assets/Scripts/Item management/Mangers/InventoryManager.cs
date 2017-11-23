using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
	#region InventoryManager singltone
		private static InventoryManager instance;
		/// Inventory Manager singltone
		public static InventoryManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<InventoryManager>();
				}
				return InventoryManager.instance; 
			}
		}
	#endregion

	#region Public variables
		/// The slot prefab
		public GameObject slotPrefab;
		/// Icon prefab
		public GameObject iconPrefab;
		/// Reference to tooltip object
		public GameObject toolTipObject;
		/// Reference to Stack size background UI element, for spliting the stacks
		public GameObject selectStackSize;
		/// Reference to SizeText UI element
		public Text sizeTextObject;
		/// Reference to VisualText UI element
		public Text visualTextObject;
		/// The amount of items to pickup (text on UI element used for splitting)
		public Text stackText;
		/// A reference to the inventory Canvas
		public Canvas canvas;
		/// A reference to EventSystem
		public EventSystem eventSystem;
	#endregion

	#region Private variables	
		/// A reference to the object that hovers next to the mouse
		private GameObject hoverObject;
		public GameObject HoverObject
		{
			get { return hoverObject; }
			set { hoverObject = value; }
		}
		/// The slot that we are moving an item from
		private Slot from;
		public Slot From
		{
			get { return from; }
			set { from = value; }
		}
		/// The slot that we are moving an item to
		private Slot to;
		public Slot To
		{
			get { return to; }
			set { to = value; }
		}
		/// The clicked object
		private GameObject clicked;
		public GameObject Clicked
		{
			get { return clicked; }
			set { clicked = value; }
		}
		/// The amount of items we have in our "hand"
		private int splitAmount;
		public int SplitAmount
		{
			get { return splitAmount; }
			set { splitAmount = value; }
		}
		/// The maximum amount of items we are allowed to remove from the stack
		private int maxStackCount;
		public int MaxStackCount
		{
			get { return maxStackCount; }
			set { maxStackCount = value; }
		}
		/// Slot for storing items when moving from one slot to another
		private Slot movingSlot;
		public Slot MovingSlot
		{
			get { return movingSlot; }
			set { movingSlot = value; }
		}
	#endregion

	/// Sets the stack info, and allow to know how many items can be removed
	public void SetStackInfo (int maxStackCount)
	{
		selectStackSize.SetActive(true);
		toolTipObject.SetActive(false);
		splitAmount = 0;
		this.maxStackCount = maxStackCount;
		stackText.text = splitAmount.ToString();
	}

	public void Save()
	{
		GameObject[] inventories = GameObject.FindGameObjectsWithTag("Inventory");
		foreach (GameObject inventory in inventories)
		{
			inventory.GetComponent<Inventory>().SaveInventory();
		}
		GameObject charPannel = GameObject.FindGameObjectWithTag("CharacterPanel");
		charPannel.GetComponent<CharacterPanel>().SaveInventory();
	}

	public void Load()
	{
		GameObject[] inventories = GameObject.FindGameObjectsWithTag("Inventory");
		foreach (GameObject inventory in inventories)
		{
			inventory.GetComponent<Inventory>().LoadInventory();
		}
		GameObject charPannel = GameObject.FindGameObjectWithTag("CharacterPanel");
		charPannel.GetComponent<CharacterPanel>().LoadInventory();
	}
}
