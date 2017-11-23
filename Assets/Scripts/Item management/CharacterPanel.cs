using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterPanel : ItemManagerBase
{
	public Slot[] equipmentSlots;
	public Inventory inventory;

	#region Character Pannel singltone
		private static CharacterPanel instance;
		/// Character Pannel singltone
		public static CharacterPanel Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<CharacterPanel>();
				}
				return CharacterPanel.instance; 
			}
		}
	#endregion
	public Slot WeaponSlot
	{
		get { return equipmentSlots[3]; }
	}

	public Slot OffhandSlot
	{
		get { return equipmentSlots[5]; }
	}

	void Start ()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		InventoryManager.Instance.MovingSlot = GameObject.Find("MovingSlot").GetComponent<Slot>();
		if (canvasGroup.alpha == 1)
		{
			IsOpen = true;
		}
		else
			IsOpen = false;
	}
	void Awake ()
	{
		equipmentSlots = transform.GetComponentsInChildren<Slot>();
	}

	public override void SaveInventory()
	{
		string content = string.Empty;

		for (int i = 0; i < equipmentSlots.Length; i++)
		{
			Slot tmp = equipmentSlots[i].GetComponent<Slot>();

			if (!tmp.IsEmpty)
			{
				content += i + "-" + tmp.CurrentItem.itemName + "-" + tmp.Items.Count.ToString() + ";";
			}
		}
		PlayerPrefs.SetString(gameObject.name +  "content", content);
		PlayerPrefs.Save();
	}
	public override void LoadInventory()
	{
		string content = PlayerPrefs.GetString(gameObject.name +  "content");
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
				equipmentSlots[index].GetComponent<Slot>().AddItem(tmpItem);
			}
		}
	}
	public void EquipItem(Slot slot, Item item)
	{
		Slot[] tmp = Array.FindAll(equipmentSlots, x => x.canContain == item.itemType);
		//if itemtype Helment or Chest
		if (tmp.Length == 1)
		{
			Slot.SwapItems(slot, tmp[0]);
			return;
		}
		switch (item.itemType)
		{
			//if spear
			case ItemType.MainHand:
				{
					Slot.SwapItems(slot, WeaponSlot);
					if (!OffhandSlot.IsEmpty)
					{
						if (OffhandSlot.CurrentItem.itemType == ItemType.GenericWeapon)
							UnEquipItem(OffhandSlot, OffhandSlot.CurrentItem);	
					}
					return ;
				}
			//if shield
			case ItemType.OffHand:
				{
					Slot.SwapItems(slot, OffhandSlot);
					if (WeaponSlot.CurrentItem.itemType == ItemType.TwoHand)
						UnEquipItem(WeaponSlot, WeaponSlot.CurrentItem);
						return ;
				}
			//if twohanded
			case ItemType.TwoHand:
				{
					
					Slot.SwapItems(slot, WeaponSlot);
					if (!OffhandSlot.IsEmpty)
						UnEquipItem(OffhandSlot, OffhandSlot.CurrentItem);
					return ;
				}
			default:
				Console.WriteLine("Item.ItemType invalid");
				break;
		}
	  	//if itemtype Stuff or GenericWeapon
		//all slots of stuff or GenericWeapones are occupied by items or twohand weapon equiped?
		if (!TryEquipInEachSlot(tmp, item, slot) && item.itemType != ItemType.Consumeable)
		{
			Slot.SwapItems(slot, tmp[0]);
			return;
		}
	}

	//If item type stuff or generic weapon
	public bool TryEquipInEachSlot(Slot[] tmp, Item item, Slot slot)
	{
		for (int i = 0; i < tmp.Length; i++)
		{
			if (item.itemType == ItemType.Stuff)
			{
				if (tmp[i].IsEmpty)
				{
					Slot.SwapItems(slot, tmp[i]);
					return true;
				}
			}
			if (item.itemType == ItemType.GenericWeapon)
			{
				if (tmp[i].IsEmpty && !WeaponSlot.IsEmpty && WeaponSlot.CurrentItem.itemType != ItemType.TwoHand)
				{
					Slot.SwapItems(slot, tmp[i]);
					return true;
				}
				///if shield and 1h founded
				else if (!tmp[i].IsEmpty && !OffhandSlot.IsEmpty && OffhandSlot.CurrentItem.itemType == ItemType.OffHand)
				{
					//Debug.Log("shield Detected");
					Slot.SwapItems(slot, OffhandSlot);
					return true;
				}
			}			
		}
		return false;
	}
	public void UnEquipItem(Slot slot, Item item)
	{
		Debug.Log("Unequip :" + item.name);
		if (!slot.IsEmpty)
		{
			inventory.AddItem(item);
			slot.ClearSlot();
		}
	}
}
