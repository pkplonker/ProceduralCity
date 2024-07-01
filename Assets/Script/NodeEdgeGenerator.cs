using System.Collections.Generic;
using UnityEngine;

public static class NodeEdgeGenerator
{
	public static List<Edge> Generate(List<Node> inputs, float maxConnectionDistance)
	{
		var output = new List<Edge>();

		for (int i = 0; i < inputs.Count; i++)
		{
			for (int y = i + 1; y < inputs.Count; y++)
			{
				var distance = Vector2.Distance(inputs[i].Position, inputs[y].Position);
				if (distance <= maxConnectionDistance)
				{
					var newEdge = new Edge(inputs[i], inputs[y]);
					if (!Intersects(newEdge, output))
					{
						AddEdge(newEdge, output);
					}
				}
			}
		}

		return output;
	}

	private static void AddEdge(Edge newEdge, List<Edge> edges)
	{
		foreach (var edge in edges)
		{
			if ((edge.Start == newEdge.Start && edge.End == newEdge.End) ||
			    (edge.Start == newEdge.End && edge.End == newEdge.Start))
			{
				return;
			}
		}

		edges.Add(newEdge);
	}

	private static bool Intersects(Edge newEdge, List<Edge> edges)
	{
		foreach (var edge in edges)
		{
			if (Intersects(newEdge, edge)) return true;
		}

		return false;
	}

	// https://forum.unity.com/threads/line-intersection.17384/#post-4442284
	private static bool Intersects(Edge edge1, Edge edge2)
	{
		Vector2 a = edge1.End.Position - edge1.Start.Position;
		Vector2 b = edge2.Start.Position - edge2.End.Position;
		Vector2 c = edge1.Start.Position - edge2.Start.Position;

		float alphaNumerator = b.y * c.x - b.x * c.y;
		float betaNumerator = a.x * c.y - a.y * c.x;
		float denominator = a.y * b.x - a.x * b.y;

		if (denominator == 0)
		{
			return false;
		}

		if (denominator > 0)
		{
			if (alphaNumerator < 0 || alphaNumerator > denominator || betaNumerator < 0 ||
			    betaNumerator > denominator)
			{
				return false;
			}
		}
		else if (alphaNumerator > 0 || alphaNumerator < denominator || betaNumerator > 0 ||
		         betaNumerator < denominator)
		{
			return false;
		}

		return true;
	}
}