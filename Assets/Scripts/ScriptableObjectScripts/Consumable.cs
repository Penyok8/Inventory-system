using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class Consumable : Item
{
	public override void Use(Slot slot, Item item)
	{
		CharacterPanel.Instance.EquipItem(slot, item);
		Debug.Log("Equipped " + itemName);
	}
}
