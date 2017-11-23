using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex
{
	///x + y + z = 0
	///z = -(x + y)
	public readonly int x;
	public readonly int y;
	public readonly int z;

	private static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;

	public Hex(int x, int y)
	{
		this.x = x;
		this.y = y;
		this.z = -(x + y);
	}

	///Return world pos of this hex
	public Vector3 Position()
	{
		float radius = 1.25f;
		float height = radius * 2 * WIDTH_MULTIPLIER;
		float width = 2 * radius;
		float horizontalSpacing = radius * 1.5f;
		float verticalSpacing = radius * WIDTH_MULTIPLIER;
		return new Vector3(horizontalSpacing * this.x, verticalSpacing * this.y, 0);
	}
}
