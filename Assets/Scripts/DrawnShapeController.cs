using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrawnShapeController : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private PolygonCollider2D polygonCollider;
    
    private readonly float _halfWidth = 0.1f;
    
    public void SetupObject(List<Vector2> points)
    {
        UpdatePoints(points);
        polygonCollider.points = PolygonUtils.GeneratePolygon(points, _halfWidth);
        
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;

        Collider2D[] results = new Collider2D[32];

        int count = polygonCollider.Overlap(filter, results);
        
        if (count > 0)
        {
            Destroy(gameObject);
        }
        
        var rb = this.AddComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.mass = polygonCollider.points.Length * 0.5f;
    }

    public void UpdatePoints(List<Vector2> points)
    {
        Vector2 origin = points[0];
        Vector3[] localPoints = new Vector3[points.Count];

        for (int i = 0; i < points.Count; i++)
        {
            localPoints[i] = points[i] - origin;
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(localPoints);
    }
}
