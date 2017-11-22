using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Scriptd : MonoBehaviour {

	public Sprite spriteNatural;
	public Sprite spriteHighlighted;

	private Item item;
	public Item Item
	{
		get { return item; }
		set {
			 	item = value;
				spriteHighlighted = item.spriteHighlighted;
				spriteNatural = item.spriteNatural;
			}
	}

	public void Use(Slot slot)
	{
		item.Use(slot, this.item);
	}
	public string GetTooltip()
	{
		return Item.GetTooltip();
		//return "nado zapilit variativnost pod vse tipy itemov";
		/*//string stats = string.Empty;
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
		return string.Format("<color=" + color + "><size=14>{0}</size></color><size=11><i><color=lime>" + newLine + "{1}</color></i></size>", itemName, description);
	*/
	}
}
