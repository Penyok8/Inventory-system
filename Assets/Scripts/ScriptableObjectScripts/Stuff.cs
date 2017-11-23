using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StuffItem", menuName = "Items/StuffItem")]
public class Stuff : Item
{

	public override void Use(Slot slot, Item item)
	{
		slot.RemoveItem();
		Debug.Log(itemName + "Used");
	}
}
