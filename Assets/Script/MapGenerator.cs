using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour
{
	[SerializeField]
	private Vector2Int size;

	[SerializeField]
	private int maxCandidates = 10;

	[SerializeField]
	private float minDistance = 5;
	[SerializeField]
	private float maxConnectionDistance = 10;

	private bool nodesGenerated;
	private List<Node> nodes;

	[SerializeField]
	private List<Node> initialNodes;

	private List<Node> hullPoints;
	private IEnumerable<Node> internalPoints;
	private List<Edge> edges;

	private void Start()
	{
		GeneratePointCloud();
	}

	public void GeneratePointCloud()
	{
		var generator = new NodeGenerator(initialNodes);
		nodes = generator.GenerateNodes(minDistance, maxCandidates);
		hullPoints = QuickHull.Generate(nodes).ToList();
		internalPoints = nodes.Except(hullPoints);
		edges = NodeEdgeGenerator.Generate(nodes, maxConnectionDistance);
		nodesGenerated = true;
	}

	private void OnDrawGizmos()
	{
		if (nodes == null) return;
		foreach (var t in nodes)
		{
			Gizmos.DrawSphere(transform.position + new Vector3(t.Position.x, 0, t.Position.y), 0.5f);
		}

		if (hullPoints?.Count >= 2)
		{
			var points = hullPoints.Select(x => new Vector3(x.Position.x, 0, x.Position.y)).ToList();

			for (var i = 1; i < points.Count; i++)
			{
				Gizmos.DrawLine(points[i], i != points.Count ? points[i - 1] : points[0]);
			}

			Gizmos.DrawLine(points[^1], points[0]);
		}

		if (edges?.Count >= 2)
		{
			for (var i = 1; i < edges.Count; i++)
			{
				Gizmos.DrawLine(new Vector3(edges[i].Start.Position.x,0,edges[i].Start.Position.y), new Vector3(edges[i].End.Position.x,0,edges[i].End.Position.y));
			}
			
		}
	}
}