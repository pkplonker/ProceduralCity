using System;
using UnityEngine;

[Serializable]
public class Node
{
	public Vector2 Position;
	public float SplitNumber;

	public Node(Vector2 position, float splitNumber)
	{
		Position = position;
		SplitNumber = splitNumber;
	}
}