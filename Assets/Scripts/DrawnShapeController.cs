using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrawnShapeController : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private PolygonCollider2D polygonCollider;
    
    [SerializeField] private PhysicsMaterial2D bouncyMaterial;
    [SerializeField] private PhysicsMaterial2D slipperyMaterial;
    
    private readonly float _halfWidth = 0.1f;
    private int _selectedCrayon;
    private Rigidbody2D rb;
    
    public bool SetupObject(List<Vector2> points, int selectedCrayon)
    {
        _selectedCrayon = selectedCrayon;
        UpdatePoints(points, selectedCrayon);
        polygonCollider.points = PolygonUtils.GeneratePolygon(points, _halfWidth);
        
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;

        Collider2D[] results = new Collider2D[32];

        int count = polygonCollider.Overlap(filter, results);
        
        if (count > 0)
        {
            Destroy(gameObject);
            return false;
        }
        
        rb = this.AddComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.mass = polygonCollider.points.Length * 0.5f;
        rb.sharedMaterial = SetMaterial(selectedCrayon);

        if (selectedCrayon == 3)
        {
            gameObject.layer = 0;
        }
        return true;
    }

    public void UpdatePoints(List<Vector2> points, int selectedCrayon)
    {
        Vector2 origin = points[0];
        Vector3[] localPoints = new Vector3[points.Count];

        for (int i = 0; i < points.Count; i++)
        {
            localPoints[i] = points[i] - origin;
        }

        lineRenderer.colorGradient = GetGradient(selectedCrayon);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(localPoints);
    }
    
    private Gradient GetGradient(int index) => index switch
    {
        0 => CreateGradient(new Color(0.21f, 0.83f, 0.2f)),
        1 => CreateGradient(new Color(0.91f, 0.52f, 0.8f)),
        2 => CreateGradient(new Color(0.98f, 0.28f, 0.31f)),
        3 => CreateGradient(new Color(0.34f, 0.39f, 0.87f)),
        4 => CreateGradient(new Color(1f, 0.9f, 0f)),
        _ => CreateGradient(Color.white)
    };

    private Gradient CreateGradient(Color color)
    {
        Gradient gradient = new();

        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(color, 0f),
                new GradientColorKey(color, 1f)
            },
            new[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            });

        return gradient;
    }

    private PhysicsMaterial2D SetMaterial(int index) => index switch
    {
        // 3 => slipperyMaterial,
        4 => bouncyMaterial,
        _ => null
    };
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_selectedCrayon == 3)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
