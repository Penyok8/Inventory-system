using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemsContainer : MonoBehaviour {

	#region ItemsContainer singltone
		private static ItemsContainer instance;
		/// Items Container singltone
		public static ItemsContainer Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<ItemsContainer>();
				}
				return ItemsContainer.instance; 
			}
		}
	#endregion
	public List<Weapon> weapons;
	public List<Armor> armors;
	public List<Shield> shields;
	public List<Item> items;

	void Awake()
	{
		#region Load weaponry
			Object[] weaponsObj;
			weaponsObj = Resources.LoadAll("Weapons", typeof(Weapon));
			foreach (Object weapone in weaponsObj)
			{
				weapons.Add((Weapon)weapone);
			}
		#endregion
		
		#region Load armor
			Object[] armorsObj;
			armorsObj = Resources.LoadAll("Armor", typeof(Armor));
			foreach (Object armor in armorsObj)
			{
				armors.Add((Armor)armor);
			}
		#endregion
		
		#region Load shields
			Object[] shieldsObj;
			shieldsObj = Resources.LoadAll("Shields", typeof(Shield));
			foreach (Object shield in shieldsObj)
			{
				shields.Add((Shield)shield);
			}
		#endregion
		
		#region Load items
			Object[] itemsObj;
			itemsObj = Resources.LoadAll("Items", typeof(Consumable));
			foreach (Object item in itemsObj)
			{
				items.Add((Consumable)item);
			}
		#endregion
		
	}
	public Item FindItemByName(string name)
	{

		for (int i = 0; i < weapons.Count; i++)
		{
			if (weapons[i].itemName == name)
				return(weapons[i]);
		}
		for (int i = 0; i < armors.Count; i++)
		{
			if (armors[i].itemName == name)
				return(armors[i]);
		}
		for (int i = 0; i < shields.Count; i++)
		{
			if (shields[i].itemName == name)
				return(shields[i]);
		}
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].itemName == name)
				return(items[i]);
		}
		return null;
	}
}
