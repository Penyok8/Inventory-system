using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
	public GameObject HexPrefab;
	void Start ()
	{
		GenerateMap();
	}

	public void GenerateMap()
	{
		for (int x = 0; x < 10; x++)
		{
			for (int y = 0; y < 10; y++)
			{
				Hex hex = new Hex(x, y);
				GameObject hexGO = HexPrefab;
				SpriteRenderer hui = hexGO.GetComponent<SpriteRenderer>();

				Debug.Log(hui.size);

				hexGO.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
				Instantiate(hexGO, new Vector3(hex.Position().x, hex.Position().y, 0), Quaternion.identity, this.transform);
			}
		}
	}

}
