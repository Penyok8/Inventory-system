using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class SquadBase : MonoBehaviour
{
	#region Pubic variables
		public int id;

		public Vector3 position;

		public string squadName;

		public float speed;

		public Sprite worldMapSprite;

		public Character leader;

		public List<Character> troops;

		public int size;

		public int maxSize;

		public int glory;

		public List<RelationData> relations;

		public event BattleStarted onStartBattle;
	#endregion

	public delegate void BattleStarted(SquadBase squad1, SquadBase squad2);

	protected void Awake()
	{
		size = troops.Count;
		maxSize = 20;
	}
	
	public bool AddTroop(Character troop)
	{
		if (troops.Count < maxSize)
		{
			troops.Add(troop);
			return true;
		}
		Debug.Log("Can't add troop, squad is full.");
		return false;
	}

	public void DissmissTroop(Character troop)
	{
		if (troops.Remove(troop))
			Debug.Log(troop.name + " removed from" + this.squadName + "squad");
		else
			Debug.Log("There is no " + troop.name + " in "+ this.squadName + "squad");
	}
	
	public void moveTo()
	{

	}

	public void AttackSquad(SquadBase enemy)
	{
		onStartBattle(this, enemy);
	}
	
}
