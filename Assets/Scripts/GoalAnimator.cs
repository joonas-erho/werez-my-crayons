using UnityEngine;

public class GoalAnimator : MonoBehaviour
{
    [SerializeField] private float angle = 20f;
    [SerializeField] private float speed = 1f;

    private Quaternion _startRotation;

    private void Awake()
    {
        _startRotation = transform.localRotation;
    }

    private void Update()
    {
        float rotation = Mathf.Sin(Time.time * speed * Mathf.PI * 2f) * angle;
        transform.localRotation = _startRotation * Quaternion.Euler(0f, 0f, rotation);
    }
}
