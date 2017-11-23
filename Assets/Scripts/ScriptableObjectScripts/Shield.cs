using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shield", menuName = "Items/Shield")]
public class Shield : Equipment
{
	public int rangeDeff;
	public int meleeDeff;

	public override string GetTooltip()
	{
		string stats = string.Empty;

		stats += "\n" + "Range defense " + rangeDeff.ToString() + "\n" + "Melee defense " + meleeDeff.ToString();

		string itemTip = base.GetTooltip();
		string equipmentStats = string.Empty;
		equipmentStats += "\n" + "Max fatigue " + maxFatigue.ToString() + "\n" + "Condition " + condition.ToString() + "%";
		return string.Format("{0}" + "<size=9> {1} {2}</size>", itemTip, equipmentStats, stats);
	}
}
