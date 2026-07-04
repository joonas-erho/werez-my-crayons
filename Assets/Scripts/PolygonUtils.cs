using System.Collections.Generic;
using UnityEngine;

public static class PolygonUtils
{
    public static Vector2[] GeneratePolygon(List<Vector2> points, float halfWidth)
    {
        List<Vector2> polygon = new();
        
        List<Vector2> left = new();
        List<Vector2> right = new();
        
        Vector2 origin = points[0];
        Vector3[] localPoints = new Vector3[points.Count];

        for (int i = 0; i < points.Count; i++)
        {
            localPoints[i] = points[i] - origin;
        }

        for (int i = 0; i < points.Count; i++)
        {
            Vector3 normal = GetNormal(localPoints, i);

            left.Add(localPoints[i] + normal * halfWidth);
            right.Add(localPoints[i] - normal * halfWidth);
        }
        
        polygon.AddRange(left);
        
        // Reverse order
        for (int i = right.Count - 1; i >= 0; i--)
            polygon.Add(right[i]);
        
        Vector2[] result = new Vector2[polygon.Count];
        for (int i = 0; i < points.Count; i++)
        {
            result[i] = polygon[i];
        }
        return polygon.ToArray();
    }

    private static Vector3 GetNormal(Vector3[] points, int index)
    {
        var previous = points[index == 0 ? 0 : index - 1];
        var next = points[index == points.Length - 1 ? points.Length - 1 : index + 1];
        var dir = (next - previous).normalized;
        return new(-dir.y, dir.x, 0);
    }
}
