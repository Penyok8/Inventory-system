using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class Consumable : Item
{
	public override void Use(Slot slot, Item item)
	{
		if(item.itemType == ItemType.Consumeable)
		{
			slot.RemoveItem();
			Debug.Log(itemName + "Used");
		}
		else
		{
			CharacterPanel.Instance.EquipItem(slot, item);
			Debug.Log("Equipped " + itemName);
		}
	}
	public void UnEquipItem(Slot slot, Item item)
	{
		if(item.itemType == ItemType.Stuff)
		{
			CharacterPanel.Instance.UnEquipItem(slot, item);
		//Debug.Log("Unequipped " + itemName);
		}
	}
}
