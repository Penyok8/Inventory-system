using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {
	#region BattleManager singltone
		private static BattleManager instance;
		/// Battle Manager singltone
		public static BattleManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<BattleManager>();
				}
				return BattleManager.instance; 
			}
		}
	#endregion
	
	#region Pubic variables
		public List<SquadBase> allSquads;
	#endregion
	
	void OnEnable ()
	{
		GameObject[] allSquadsGameObj = GameObject.FindGameObjectsWithTag("Squad");
		foreach (GameObject squad in allSquadsGameObj)
		{
			allSquads.Add(squad.GetComponent<SquadBase>());
		}
		SubscribeAllSquads(allSquads);
	}
	

	void Update ()
	{
		
	}


	void OnDisable()
	{
		UnSubscribeAllSquads(allSquads);
	}

	void SubscribeAllSquads(List<SquadBase> allSquads)
	{
		foreach (SquadBase squad in allSquads)
		{
			squad.onStartBattle += this.StartFight;
		}
	}

	void UnSubscribeAllSquads(List<SquadBase> allSquads)
	{
		foreach (SquadBase squad in allSquads)
		{
			squad.onStartBattle -= this.StartFight;
		}
	}

	void StartFight(SquadBase squad1, SquadBase squad2)
	{

	}

}
