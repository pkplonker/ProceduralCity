using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

	public class NodeGenerator
	{
		private List<Node> openNodes;
		private List<Node> allNodes;

		public NodeGenerator(List<Node> initialNodes)
		{
			openNodes = new List<Node>(initialNodes);
			allNodes = new List<Node>(initialNodes);
		}

		public List<Node> GenerateNodes(float minDistance, int maxCandidates)
		{
			while (openNodes.Any())
			{
				var node = openNodes[0];
				openNodes.RemoveAt(0);

				if (ShouldSplit(node.SplitNumber))
				{
					var candidates = GenerateCandidates(node, maxCandidates);

					foreach (var candidate in candidates)
					{
						if (IsDistantEnough(candidate, minDistance))
						{
							var newNode = new Node(new Vector2(candidate.Position.x, candidate.Position.y), node.SplitNumber - 1);
							openNodes.Add(newNode);
							allNodes.Add(newNode);
						}
					}
				}
			}

			return allNodes;
		}

		private bool ShouldSplit(float splitNumber)
		{
			if (splitNumber >= 1)
			{
				return true;
			}
			
			if (splitNumber > 0)
			{
				return Random.value < splitNumber;
			}

			return false;
		}

		private List<Node> GenerateCandidates(Node node, int count)
		{
			var candidates = new List<Node>();

			for (int i = 0; i < count; i++)
			{
				float angle = Random.value * 2 * Mathf.PI;
				float distance = Random.value * 10; 
				float newX = node.Position.x + Mathf.Cos(angle) * distance;
				float newY = node.Position.y + Mathf.Sin(angle) * distance;
				candidates.Add(new Node(new Vector2(newX,newY), node.SplitNumber - 1));
			}

			return candidates;
		}

		private bool IsDistantEnough(Node candidate, float minDistance)
		{
			foreach (var node in allNodes)
			{
				float distance = Mathf.Sqrt(Mathf.Pow(candidate.Position.x - node.Position.x, 2) + Mathf.Pow(candidate.Position.y - node.Position.y, 2));
				if (distance < minDistance)
				{
					return false;
				}
			}

			return true;
		}
	}

	
