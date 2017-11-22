using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public Inventory inventory;
	public CharacterPanel characterPanel;
	public BattleReward battleReward;
	public List<Item> itemObj;
	public int kek = 3;

	#region Stats

		public int hpCurrent;
		public int rangeDeff;
		public int meleeDeff;
		public int armor;
		public int currentArmor;
		public int minDamage;
		public int maxDamage;
		public int attakRange;
		public int fatigue;
		private int baseFatigue;
		private int baseRangeDeff;
		private int baseMeleeDeff;
		private int hpBase;
		[HideInInspector]public int resolve;
		[HideInInspector]public int actionPoints;
		[HideInInspector]public int meleeSkill;
		[HideInInspector]public int rangeSkill;	
		public Text statsText;

	#endregion

	void Start ()
	{
		
	}

	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.I))
		{
			inventory.Open();
		}
		if(Input.GetKeyDown(KeyCode.J))
		{
			if (battleReward != null)
			{
				battleReward.battleRewardInventory.Open();
			}			
		}
		if(Input.GetKeyDown(KeyCode.P))
		{
			if	(characterPanel != null)
			characterPanel.Open();
		}
		while (kek >= 0)
		{
			inventory.AddItem(itemObj[kek]);
			//Debug.Log("here" + kek.ToString());
			kek--;
		}
	}

	public void SetStats (int hp, int rangeDeff, int meleeDeff, int fatigue, int minDmg, int maxDmg, int armor, int currentArmor)
	{
		this.hpCurrent = hpBase + hp;
		this.rangeDeff = baseRangeDeff + rangeDeff;
		this.meleeDeff = baseMeleeDeff + meleeDeff;
		this.fatigue = baseFatigue + fatigue;
		this.minDamage = minDmg;
		this.maxDamage = maxDmg;

		statsText.text = string.Format("Helment armor:\nChest armor:\nHp: {0}\nRange deffence: {1}\nMelee deffence: {2}\nFatigue: {3}\nDamage: {4} - {5}", this.hpCurrent, this.rangeDeff, this.meleeDeff, this.fatigue, this.minDamage, this.maxDamage);		
	}
}
