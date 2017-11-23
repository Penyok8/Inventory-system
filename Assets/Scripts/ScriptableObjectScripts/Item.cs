using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {Consumeable, MainHand, TwoHand, OffHand, Head, Chest, Generic, GenericWeapon, Stuff}
public enum Rarity {Common, Rare, Epic, Legendary}
///Item scriptable object
public abstract class Item : ScriptableObject
{
	public ItemType itemType;

	public Rarity rarity;

	public Sprite spriteNatural;

	public Sprite spriteHighlighted;

	public string itemName;

	public string description;

	public int maxSize = 1;

	public int commonWorth;

	public abstract void Use(Slot slot, Item item);

	public virtual string GetTooltip()
	{
		string color = string.Empty;
		string newLine = string.Empty;
		if (description != string.Empty)
		{
			newLine = "\n";
		}
		switch (rarity)
		{
			case Rarity.Common:
				color = "white";
				break;
			case Rarity.Epic:
				color = "purple";
				break;
			case Rarity.Legendary:
				color = "orange";
				break;
			case Rarity.Rare:
				color = "navy";
				break;
		}
		return string.Format("<color=" + color + "><size=12>{0}</size></color><size=9><i><color=lime>" + newLine + "{1}</color></i></size>", itemName, description);
	}
}
