using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
	/// how much maximum fatigue you lose when equip item always < 0
	public int maxFatigue;
	public int condition = 100;
	/// Cost of repairing for 1% of condition
	public int repairingCost;

	public override void Use(Slot slot, Item item)
	{
		CharacterPanel.Instance.EquipItem(slot, item);
		Debug.Log("Equipped " + itemName);
	}
	public void UnEquipItem(Slot slot, Item item)
	{
		CharacterPanel.Instance.UnEquipItem(slot, item);
		//Debug.Log("Unequipped " + itemName);
	}
}
