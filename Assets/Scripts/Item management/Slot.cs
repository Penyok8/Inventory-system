using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler {

	private Stack<Item> items;
	public Stack<Item> Items
	{
		get { return items; }
		set { items = value; }
	}
	public Text stackText;
	public Sprite slotEmpty;
	public Sprite slotHighligted;
	public bool IsEmpty
	{
		get { return items.Count == 0; }
	}
	public Item CurrentItem
	{
		get { return items.Peek(); }
	}
	public bool isAvailable
	{
		 get { return CurrentItem.maxSize> items.Count; }
	}
	protected CanvasGroup canvasGroup;

	public ItemType canContain;

	void Awake()
	{
		items = new Stack<Item>();
		if (transform.parent != null)
		{
			canvasGroup = GetComponentInParent<CanvasGroup>();
		}		
	}

	void Start ()
	{
		
		RectTransform slotRect = GetComponent<RectTransform>();
		RectTransform textRect = stackText.GetComponent<RectTransform>();
		int textScale = (int)(slotRect.sizeDelta.x * 0.60);
		stackText.resizeTextMaxSize = textScale;
		stackText.resizeTextMinSize = textScale;
		textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
		textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
	}

	protected void ChangeSprite(Sprite netural, Sprite highlight)
	{
		GetComponent<Image>().sprite = netural;
		SpriteState st = new SpriteState();
		st.highlightedSprite = highlight;
		st.pressedSprite = netural;
		if (GetComponent<Button>() != null)
			GetComponent<Button>().spriteState = st;
	}

	public void AddItem(Item item)
	{
		if(IsEmpty)
		{
			if(transform.parent.GetComponent<Inventory>() != null)
				transform.parent.GetComponent<Inventory>().EmptySlots--;
		}
		items.Push(item);
		if(items.Count > 1)
		{
			stackText.text = items.Count.ToString();
		}
		ChangeSprite(item.spriteNatural, item.spriteHighlighted);
	}

	public void AddItems(Stack<Item> items)
	{
		this.items = new Stack<Item>(items);
		stackText.text= items.Count > 1 ? items.Count.ToString() : string.Empty;
		ChangeSprite(CurrentItem.spriteNatural, CurrentItem.spriteHighlighted);
	}

	public static void SwapItems(Slot from, Slot to)
	{
		if (to != null && from != null)
		{	
			Stack<Item> tmpTo = new Stack<Item>(to.Items);
			to.AddItems(from.Items);
			if(tmpTo.Count == 0)
			{
				if(to.transform.parent.GetComponent<Inventory>() != null)
				to.transform.parent.GetComponent<Inventory>().EmptySlots--;
				from.ClearSlot();					
			}
			else
			{
				from.AddItems(tmpTo);
			}				
		}	
		
	}

	

	public void ClearSlot()
	{
		items.Clear();
		ChangeSprite(slotEmpty, slotHighligted);
		stackText.text = string.Empty;
		if(transform.parent.GetComponent<Inventory>() != null)
			transform.parent.GetComponent<Inventory>().EmptySlots++;
	}

	public Item RemoveItem()
	{
		Item tmp;
		tmp = items.Pop();
		stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
		
		return tmp;
	}

	public Stack<Item> RemoveItems (int amount)
	{
		Stack<Item> tmp = new Stack<Item>();
		for (int i = 0; i < amount; i++)
		{
			tmp.Push(items.Pop());
		}
		stackText.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
		return tmp;
	}

	protected void UseItem()
	{
		if (!IsEmpty)
		{
			items.Peek().Use(this, this.CurrentItem);
			stackText.text= items.Count > 1 ? items.Count.ToString() : string.Empty;
			if (IsEmpty)
			{
				ChangeSprite(slotEmpty, slotHighligted);
				if(transform.parent.GetComponent<Inventory>() != null)
					transform.parent.GetComponent<Inventory>().EmptySlots++;
			}
		}
	}

	private void UnEquipItem()
	{
		if (!IsEmpty)
		{
			Equipment equipmentItem = (Equipment)items.Peek();
			equipmentItem.UnEquipItem(this, this.CurrentItem);
			if (IsEmpty)
			{
				ChangeSprite(slotEmpty, slotHighligted);
			}
		}
	}

	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover") )
		{
			if(transform.GetComponentInParent<Inventory>() != null && transform.GetComponentInParent<Inventory>().IsOpen)
				UseItem();
			if(transform.GetComponentInParent<CharacterPanel>() != null && transform.GetComponentInParent<CharacterPanel>().IsOpen)
				UnEquipItem();			
		}
		else if (transform.GetComponentInParent<Inventory>() != null && eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift) && !IsEmpty && !GameObject.Find("Hover") && items.Count > 1)
		{
			Vector2 position;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(InventoryManager.Instance.canvas.transform as RectTransform, Input.mousePosition,
			InventoryManager.Instance.canvas.worldCamera, out position);
			InventoryManager.Instance.selectStackSize.SetActive(true);
			InventoryManager.Instance.selectStackSize.transform.position = InventoryManager.Instance.canvas.transform.TransformPoint(position);
			InventoryManager.Instance.SetStackInfo(items.Count);
		}
	}
}
