using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapone", menuName = "Items/Weapone")]
public class Weapon : Equipment
{
	public int minDamage;
	public int maxDamage;
	public int attakRange;

	public override void Use(Slot slot, Item item)
	{
		CharacterPanel.Instance.EquipItem(slot, item);
		Debug.Log("Equipped " + itemName);
	}
	
	public override string GetTooltip()
	{
		string stats = string.Empty;

		stats += "\n" + "Damage " + minDamage.ToString() + " - " + maxDamage.ToString() + "\n" + "Attak range " + attakRange.ToString();

		string itemTip = base.GetTooltip();
		string equipmentStats = string.Empty;
		equipmentStats += "\n" + "Max fatigue " + maxFatigue.ToString() + "\n" + "Condition " + condition.ToString() + "%";
		return string.Format("{0}" + "<size=9> {1}{2}</size>", itemTip, equipmentStats, stats);
	}
}
