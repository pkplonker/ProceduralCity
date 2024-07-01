using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

// https://www.youtube.com/watch?v=-UiTp9GomcA
public static class QuickHull
{
	public static IEnumerable<Node> Generate(IList<Node> input)
	{
		var minValue = input.Min(x => x.Position.x);
		var maxValue = input.Max(x => x.Position.x);

		var min = input.First(x => x.Position.x == minValue);
		var max = input.First(x => x.Position.x == maxValue);

		input.Remove(min);
		input.Remove(max);
		var s1 = PointsToRight(input, min, max);
		var s2 = PointsToRight(input, max, min);

		var s1Hull = FindHull(s1, min, max);
		var s2Hull = FindHull(s2, max, min);

		var collection = new List<Node>();
		collection.Add(min);
		collection.AddRange(s1Hull);
		collection.Add(max);
		collection.AddRange(s2Hull);
		input.Add(min);
		input.Add(max);
		return collection;
	}

	//https://stackoverflow.com/questions/53173712/calculating-distance-of-point-to-linear-line
	private static float DistanceFromPointToLine(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
    {
        float a = lineStart.y - lineEnd.y;
        float b = lineEnd.x - lineStart.x;
        float c = lineStart.x * lineEnd.y - lineEnd.x * lineStart.y;

        return Mathf.Abs(a * point.x + b * point.y + c) / Mathf.Sqrt(a * a + b * b);
    }

    private static IList<Node> FindHull(IList<Node> points, Node min, Node max)
    {
        if (points.Count == 0)
            return new List<Node>();
        
        Node furthest = null;
        float furthestDistance = float.MinValue;

        foreach (var point in points)
        {
            var distance = DistanceFromPointToLine(min.Position, max.Position, point.Position);
            if (distance > furthestDistance)
            {
                furthestDistance = distance;
                furthest = point;
            }
        }

        if (furthest == null)
            return new List<Node>();

        var s1 = PointsToRight(points, min, furthest);
        var s2 = PointsToRight(points, furthest, max);

        var s1Hull = FindHull(s1, min, furthest);
        var s2Hull = FindHull(s2, furthest, max);

        var collection = new List<Node>();
        collection.AddRange(s1Hull);
        collection.Add(furthest);
        collection.AddRange(s2Hull);
        return collection;
    }

    private static IList<Node> PointsToRight(IList<Node> input, Node first, Node last)
    {
        var points = new List<Node>();

        foreach (var point in input)
        {
            if (IsPointToRightOfLine(first.Position, last.Position, point.Position))
            {
                points.Add(point);
            }
        }

        return points;
    }

    private static bool IsPointToRightOfLine(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
    {
        var direction = (lineEnd - lineStart);
        var normal = new Vector2(-direction.y, direction.x);
        var vectorToPoint = point - lineStart;
        return Vector2.Dot(normal, vectorToPoint) > 0;
    }
}