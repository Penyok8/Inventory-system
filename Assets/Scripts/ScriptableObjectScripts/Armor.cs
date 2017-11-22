using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Items/Armor")]
public class Armor : Equipment
{
	public int maxArmor;

	public override string GetTooltip()
	{
		string stats = string.Empty;

		stats += "\n" + "Armor " + maxArmor.ToString();

		string itemTip = base.GetTooltip();
		string equipmentStats = string.Empty;
		equipmentStats += "\n" + "Max fatigue " + maxFatigue.ToString() + "\n" + "Condition " + condition.ToString() + "%";
		return string.Format("{0}" + "<size=9> {1} {2}</size>", itemTip, equipmentStats, stats);
	}
}
