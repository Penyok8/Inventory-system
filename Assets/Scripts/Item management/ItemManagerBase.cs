using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// A class implements the tools for working with slots and opening and closing inventorys.
public abstract class ItemManagerBase : MonoBehaviour {

#region Public variables
	public float fadeTime;
	public static bool isMouseInside = false;
#endregion
#region Private variables
	protected RectTransform inventoryRect;
	public CanvasGroup canvasGroup;
	protected bool fadingIn;
	protected bool fadingOut;
	protected bool isOpen;
	public bool IsOpen
	{
		get { return isOpen; }
		set { isOpen = value; }
	}
#endregion

	public abstract void SaveInventory();
	public abstract void LoadInventory();

	public void MoveItem(GameObject clicked)
	{
		//Debug.Log("her3e");
		CanvasGroup cg = clicked.transform.GetComponentInParent<CanvasGroup>();
		if (cg != null && cg.alpha > 0 || clicked.transform.parent.parent.GetComponent<CanvasGroup>().alpha > 0)
		{
			InventoryManager.Instance.Clicked = clicked;
			if (InventoryManager.Instance.From == null && !Input.GetKey(KeyCode.LeftShift))
			{	//Debug.Log("her3e");			
				if(!InventoryManager.Instance.Clicked.GetComponent<Slot>().IsEmpty)
				{
					InventoryManager.Instance.From = InventoryManager.Instance.Clicked.GetComponent<Slot>();
					InventoryManager.Instance.From.GetComponent<Image>().color = Color.gray;					
					CreateHoverIcon();				
				}
			}
			else if (InventoryManager.Instance.To == null && !Input.GetKey(KeyCode.LeftShift))
			{
				///Debug.Log("her2e");
				InventoryManager.Instance.To = InventoryManager.Instance.Clicked.GetComponent<Slot>();
				Destroy(GameObject.Find("Hover"));					
			}
			
			if (InventoryManager.Instance.To != null && InventoryManager.Instance.From != null)
			{				
				//Debug.Log(InventoryManager.Instance.To.canContain.ToString());
				if (InventoryManager.Instance.To.Items.Count > 0 && InventoryManager.Instance.To.CurrentItem.itemType == ItemType.Consumeable &&
				 InventoryManager.Instance.From.CurrentItem.itemType == ItemType.Consumeable)
				{
					MergeStacks(InventoryManager.Instance.From, InventoryManager.Instance.To);					
				}
				else
				{					
					if (gameObject.GetComponent<Inventory>() != null)
					{
						Slot.SwapItems(InventoryManager.Instance.From, InventoryManager.Instance.To);
					}
					else
					{
						DragSwap(InventoryManager.Instance.From, InventoryManager.Instance.To);
					}
				}
				InventoryManager.Instance.From.GetComponent<Image>().color = Color.white;
				InventoryManager.Instance.To = null;
				InventoryManager.Instance.From = null;
				Destroy(GameObject.Find("Hover"));
			}	
		}
	}
	///call equip method for equipment item
	private void DragSwap(Slot from, Slot to)
	{
		if (from.CurrentItem.itemType != ItemType.Consumeable)
		{
			if (to.Items.Count == 0 || from.CurrentItem.itemType != to.CurrentItem.itemType)
				from.Items.Peek().Use(from, from.CurrentItem);
				else
					Slot.SwapItems(from, to);
		}
	}

	protected void CreateHoverIcon()
	{
		InventoryManager.Instance.HoverObject = (GameObject)Instantiate(InventoryManager.Instance.iconPrefab);
		InventoryManager.Instance.HoverObject.name = "Hover";
		InventoryManager.Instance.HoverObject.GetComponent<Image>().sprite = InventoryManager.Instance.Clicked.GetComponent<Image>().sprite;
		RectTransform hoverTransform = InventoryManager.Instance.HoverObject.GetComponent<RectTransform>();
		RectTransform clickedTransform = InventoryManager.Instance.Clicked.GetComponent<RectTransform>();
		hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
		hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);
		InventoryManager.Instance.HoverObject.transform.SetParent(GameObject.Find("Canvas").transform, true);
		InventoryManager.Instance.HoverObject.transform.localScale = InventoryManager.Instance.Clicked.gameObject.transform.localScale;
		InventoryManager.Instance.HoverObject.transform.GetChild(0).GetComponent<Text>().text = InventoryManager.Instance.MovingSlot.Items.Count > 1 ? InventoryManager.Instance.MovingSlot.Items.Count.ToString() : string.Empty;
	}

	public void PointerExit()
	{		
		isMouseInside = false;
	}

	public void PointerEnter()
	{
		if (isOpen)
			isMouseInside = true;	
	}

	private void PutItemBack()
	{
		if (InventoryManager.Instance.From != null)
		{
			Destroy(GameObject.Find("Hover"));
			InventoryManager.Instance.From.GetComponent<Image>().color = Color.white;
			InventoryManager.Instance.From = null;
		}
		else if (!InventoryManager.Instance.MovingSlot.IsEmpty)
		{
			Destroy(GameObject.Find("Hover"));
			foreach (Item item in InventoryManager.Instance.MovingSlot.Items)
			{
				InventoryManager.Instance.Clicked.GetComponent<Slot>().AddItem(item);
			}
			InventoryManager.Instance.MovingSlot.ClearSlot();
		}
		InventoryManager.Instance.selectStackSize.SetActive(false);
	}

	public void MergeStacks(Slot source, Slot destination)
	{
		int max = destination.CurrentItem.maxSize - destination.Items.Count;
		int count = source.Items.Count < max ? source.Items.Count : max;
		for (int i = 0; i < count; i++)
		{
			destination.AddItem(source.RemoveItem());
			InventoryManager.Instance.HoverObject.transform.GetChild(0).GetComponent<Text>().text = InventoryManager.Instance.MovingSlot.Items.Count.ToString();
		}
		if (source.Items.Count == 0)
		{
			source.ClearSlot();
			Destroy(GameObject.Find("Hover"));
		}
	}

	public void ShowTooltip(GameObject slot)
	{
		Slot tmpSlot = slot.GetComponent<Slot>();
		if (!tmpSlot.IsEmpty && InventoryManager.Instance.HoverObject == null && !InventoryManager.Instance.selectStackSize.activeSelf)
		{
			InventoryManager.Instance.visualTextObject.text = tmpSlot.CurrentItem.GetTooltip();
			InventoryManager.Instance.sizeTextObject.text = InventoryManager.Instance.visualTextObject.text;
			InventoryManager.Instance.toolTipObject.SetActive(true);
			float xPos = slot.transform.position.x + 30;//set padding!!
			float yPos = slot.transform.position.y - 5;//set padding!!
			InventoryManager.Instance.toolTipObject.transform.position = new Vector2(xPos, yPos);
		}
		

	}

	public void HideTooltip()
	{
		InventoryManager.Instance.toolTipObject.SetActive(false);
	}

	public void Open()
	{		
		if (canvasGroup.alpha > 0)
		{
			StartCoroutine("FadeOut");
			PutItemBack();
			HideTooltip();
			IsOpen = false;
		}
		else
		{
			StartCoroutine("FadeIn");			
			IsOpen = true;
		}		
	}

	private IEnumerator FadeOut()
	{
		if (!fadingOut)
		{
			fadingOut = true;
			fadingIn = false;
			StopCoroutine("FadeIn");

			float startAlpha = canvasGroup.alpha;
			float rate = (float)(1.0 / fadeTime);
			float progress = 0.0f;
			while (progress < 1.0)
			{
				canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);
				progress+= rate * Time.deltaTime;
				yield return null;
			}
			canvasGroup.alpha = 0;
			fadingOut = false;
		}
	}

	private IEnumerator FadeIn()
	{
		
		if (!fadingIn)
		{
			fadingOut = false;
			fadingIn = true;
			StopCoroutine("FadeOut");

			float startAlpha = canvasGroup.alpha;
			float rate = (float)(1.0 / fadeTime);
			float progress = 0.0f;
			while (progress < 1.0)
			{
				canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, progress);
				progress+= rate * Time.deltaTime;
				yield return null;
			}
			canvasGroup.alpha = 1;
			fadingIn = false;
		}
	}
}
