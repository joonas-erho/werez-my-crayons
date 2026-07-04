using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrawingController : MonoBehaviour
{
    private DrawnShapeController _shapeBeingDrawn;
    private readonly List<Vector2> _points = new();
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject drawnShapeObject;
    [SerializeField] private LevelController levelController;

    private bool _crayonRanOut = false;
    
    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _points.Clear();
            _shapeBeingDrawn = null;
        }
    
        if (Mouse.current.leftButton.isPressed)
        {
            if (_crayonRanOut)
            {
                return;
            }
            
            Vector3 mouse = Mouse.current.position.ReadValue();
            mouse.z = -cam.transform.position.z;

            Vector2 world = cam.ScreenToWorldPoint(mouse);
            if (_points.Count == 0 || Vector2.Distance(_points[^1], world) > 0.05f)
            {
                _points.Add(world);
            }
            
            if (_shapeBeingDrawn == null)
            {
                CreateShapeObject();
            }
            else
            {
                UpdateShapeObject();
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            FinalizeObject();
        }
    }
    
    private void CreateShapeObject()
    {
        var go = Instantiate(drawnShapeObject);
        go.transform.position = new Vector3(_points[0].x, _points[0].y, 0);
        _shapeBeingDrawn = go.GetComponent<DrawnShapeController>();
        _shapeBeingDrawn.UpdatePoints(_points, levelController.GetCrayonId());
    }

    private void UpdateShapeObject()
    {
        _shapeBeingDrawn.UpdatePoints(_points, levelController.GetCrayonId());

        if (!levelController.IsEnoughCrayonCharge(_points.Count))
        {
            _crayonRanOut = true;
        }
    }

    private void FinalizeObject()
    {
        if (_shapeBeingDrawn != null)
        {
            var success = _shapeBeingDrawn.SetupObject(_points, levelController.GetCrayonId());
            if (success)
                _crayonRanOut = levelController.RemoveCrayonCharge(_points.Count);
        }
    }
}
