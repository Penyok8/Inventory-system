using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : ItemManagerBase{

#region Public variables
	public int slots;
	public int rows;
	public float slotPaddingLeft, slotPaddingTop;
	public float slotSize;
#endregion

#region Private variables
	private List<GameObject> allSlots;
	private int emptySlots;
	public int EmptySlots
	{
		get { return emptySlots; }
		set { emptySlots = value; }
	}
	private float inventoryWidth, inventoryHeight;
	private float hoverYOffset;	
#endregion

	void Start ()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		if(slots > 0 && rows > 0)
			CreateLayout();
		InventoryManager.Instance.MovingSlot = GameObject.Find("MovingSlot").GetComponent<Slot>();
		//Tmp govnokod
		if (canvasGroup.alpha == 1)
		{
			IsOpen = true;
		}
		else
			IsOpen = false;
		//Tmp govnokod
	}

	void Awake ()
	{	
		
	}

	void Update ()
	{
		if (Input.GetMouseButtonUp(0))
		{
			if (!isMouseInside && InventoryManager.Instance.From != null)
			{
				InventoryManager.Instance.From.GetComponent<Image>().color = Color.white;
				InventoryManager.Instance.From.ClearSlot();
				Destroy(GameObject.Find("Hover"));
				InventoryManager.Instance.To = null;
				InventoryManager.Instance.From = null;
				Debug.Log("Destroyed");
			}
			else if (!InventoryManager.Instance.eventSystem.IsPointerOverGameObject(-1) && !InventoryManager.Instance.MovingSlot.IsEmpty)
			{
				InventoryManager.Instance.MovingSlot.ClearSlot();
				emptySlots++;
				Destroy(GameObject.Find("Hover"));
				Debug.Log("Destroyed1");
			}
		}
		if (InventoryManager.Instance.HoverObject != null)
		{			
			Vector2 position;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(InventoryManager.Instance.canvas.transform as RectTransform, Input.mousePosition, InventoryManager.Instance.canvas.worldCamera, out position);
			position.Set(position.x, position.y - hoverYOffset);
			InventoryManager.Instance.HoverObject.transform.position = InventoryManager.Instance.canvas.transform.TransformPoint(position);
		}
		
	}

	public void OnMouseDrag()
	{
		MoveInventory();
	}

	public virtual void CreateLayout()
	{
		if (allSlots != null)
		{
			foreach (GameObject go in allSlots)
			{
				Destroy(go);
			}
		}

		allSlots= new List<GameObject>();
		hoverYOffset = slotSize * 0.1f;
		emptySlots = slots;
		inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
		inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;
		inventoryRect = GetComponent<RectTransform>();
		inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
		inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);
		int columns = slots / rows;
		for (int y = 0; y < rows; y++)
		{
			for (int x = 0; x < columns; x++)
			{				
				GameObject newSlot = (GameObject)Instantiate(InventoryManager.Instance.slotPrefab);
				RectTransform slotRect = newSlot.GetComponent<RectTransform>();
				newSlot.name = "Slot";
				newSlot.transform.SetParent(this.transform.parent);
				slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * InventoryManager.Instance.canvas.scaleFactor);
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * InventoryManager.Instance.canvas.scaleFactor);
				newSlot.transform.SetParent(this.transform);
				allSlots.Add(newSlot);
			}
		}
	}

	public bool AddItem(Item item)
	{
		if(item.maxSize == 1)
		{
			return PlaceEmpty(item);
		}
		else
		{
			foreach (GameObject slot in allSlots)
			{
				Slot tmp = slot.GetComponent<Slot>();
				if (!tmp.IsEmpty)
				{
					if (tmp.CurrentItem.itemType == item.itemType && tmp.isAvailable)
					{
						if (!InventoryManager.Instance.MovingSlot.IsEmpty && InventoryManager.Instance.Clicked.GetComponent<Slot>() == tmp.GetComponent<Slot>())
						{
							continue;
						}
						else
						{
							tmp.AddItem(item);
							return true;
						}
						
					}
				}
			}
			if (emptySlots > 0)
			{
				return PlaceEmpty(item);
			}
			else
				Debug.Log("Inventory full");
		}
		return false;
	}

	public override void SaveInventory()
	{
		string content = string.Empty;

		for (int i = 0; i < allSlots.Count; i++)
		{
			Slot tmp = allSlots[i].GetComponent<Slot>();

			if (!tmp.IsEmpty)
			{
				content += i + "-" + tmp.CurrentItem.itemName + "-" + tmp.Items.Count.ToString() + ";";
			}
		}
		PlayerPrefs.SetString(gameObject.name +  "content", content);
		PlayerPrefs.SetInt(gameObject.name +  "slots", slots);
		PlayerPrefs.SetInt(gameObject.name +  "rows", rows);
		PlayerPrefs.SetFloat(gameObject.name +  "slotPaddingLeft", slotPaddingLeft);
		PlayerPrefs.SetFloat(gameObject.name +  "slotPaddingTop", slotPaddingTop);
		PlayerPrefs.SetFloat(gameObject.name +  "slotSize", slotSize);
		PlayerPrefs.SetFloat(gameObject.name +  "xPos", inventoryRect.position.x);
		PlayerPrefs.SetFloat(gameObject.name +  "yPos", inventoryRect.position.y);
		PlayerPrefs.Save();
	}

	public override void LoadInventory()
	{
		string content = PlayerPrefs.GetString(gameObject.name +  "content");
		slots = PlayerPrefs.GetInt(gameObject.name +  "slots");
		rows = PlayerPrefs.GetInt(gameObject.name +  "rows");
		slotPaddingLeft = PlayerPrefs.GetFloat(gameObject.name +  "slotPaddingLeft");
		slotPaddingTop = PlayerPrefs.GetFloat(gameObject.name +  "slotPaddingTop");
		slotSize = PlayerPrefs.GetFloat(gameObject.name +  "slotSize");
		inventoryRect.position = new Vector3(PlayerPrefs.GetFloat(gameObject.name +  "xPos"), PlayerPrefs.GetFloat(gameObject.name +  "yPos"), inventoryRect.position.z);
		CreateLayout();
		string[] splitContent = content.Split(';');
		for (int x = 0; x < splitContent.Length - 1; x++)
		{
			string[] splitValues = splitContent[x].Split('-');
			int index = Int32.Parse(splitValues[0]);
			string itemName = splitValues[1];
			int amount = Int32.Parse(splitValues[2]);

			Item tmpItem = null;
			for (int i = 0; i < amount; i++)
			{
				tmpItem = ItemsContainer.Instance.FindItemByName(itemName);
				allSlots[index].GetComponent<Slot>().AddItem(tmpItem);
			}
		}
	}

	private bool PlaceEmpty(Item item)
	{
		if (emptySlots > 0)
		{
			foreach (GameObject slot in allSlots)
			{
				Slot tmp = slot.GetComponent<Slot>();
				if (tmp.IsEmpty)
				{
					tmp.AddItem(item);
					return true;
				}
			}
		}
		Debug.Log("Inventory full");
		return false;
	}

	private void MoveInventory()
	{
		Vector2 mousePos;

		RectTransformUtility.ScreenPointToLocalPointInRectangle(InventoryManager.Instance.canvas.transform as RectTransform,
		 new Vector3(Input.mousePosition.x - (inventoryRect.sizeDelta.x/2 * InventoryManager.Instance.canvas.scaleFactor),
		  Input.mousePosition.y + (inventoryRect.sizeDelta.y / 2 * InventoryManager.Instance.canvas.scaleFactor)),
		   InventoryManager.Instance.canvas.worldCamera, out mousePos);
		transform.position = InventoryManager.Instance.canvas.transform.TransformPoint(mousePos);
	}
	//for clicking after split
	public void MoveItemHover(GameObject clicked)
	{
		CanvasGroup cg = clicked.transform.GetComponentInParent<CanvasGroup>();
		if (cg != null && cg.alpha > 0 || clicked.transform.parent.parent.GetComponent<CanvasGroup>().alpha > 0)
		{
			InventoryManager.Instance.Clicked = clicked;
			if (!InventoryManager.Instance.MovingSlot.IsEmpty)
			{
				
				Slot tmp = InventoryManager.Instance.Clicked.GetComponent<Slot>();
				if (tmp.IsEmpty)
				{
					tmp.AddItems(InventoryManager.Instance.MovingSlot.Items);
					InventoryManager.Instance.MovingSlot.Items.Clear();
					Destroy(GameObject.Find("Hover"));
				}
				else if (!tmp.IsEmpty && InventoryManager.Instance.MovingSlot.CurrentItem.itemType == tmp.CurrentItem.itemType && tmp.isAvailable)
				{
					MergeStacks(InventoryManager.Instance.MovingSlot, tmp);
				}
			}
		}

	}

	public void SplitStack()
	{
		InventoryManager.Instance.selectStackSize.SetActive(false);
		if (InventoryManager.Instance.SplitAmount == InventoryManager.Instance.MaxStackCount)
		{
			MoveItem(InventoryManager.Instance.Clicked);
		}
		else if(InventoryManager.Instance.SplitAmount > 0)
		{
			InventoryManager.Instance.MovingSlot.Items = InventoryManager.Instance.Clicked.GetComponent<Slot>().RemoveItems(InventoryManager.Instance.SplitAmount);
			CreateHoverIcon();
		}
	}

	public void ChangeStackText(int i)
	{
		InventoryManager.Instance.SplitAmount += i;
		if (InventoryManager.Instance.SplitAmount < 0)
		{
			InventoryManager.Instance.SplitAmount = 0;
		}
		if (InventoryManager.Instance.SplitAmount > InventoryManager.Instance.MaxStackCount)
		{
			InventoryManager.Instance.SplitAmount = InventoryManager.Instance.MaxStackCount;
		}
		InventoryManager.Instance.stackText.text = InventoryManager.Instance.SplitAmount.ToString();
	}	
}
