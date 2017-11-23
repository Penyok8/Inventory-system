using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RelationsType {War, Hostile, Cold, Indeferent, Open, Friendly, VeryFriendly, Alies}

public class RelationsManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
public struct RelationData
{
	public string squadName;
	public RelationsType relation;
}