using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputSystem_Actions _input;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform stuckInTerrainCheck;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D col;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private LevelController levelController;
    
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpSpeed;
    
    private void Awake() => _input = new InputSystem_Actions();
    private void OnEnable() => _input.Enable();
    private void OnDisable() => _input.Disable();
    
    private Vector2 _moveInput;
    private float _jumpInput;

    private bool _canJump = true;

    private bool _isDead = false;
    
    private void Update()
    {
        if (_isDead)
        {
            return;
        }
        
        _moveInput = _input.Player.Move.ReadValue<Vector2>();

        if (_moveInput.x < 0.0)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (_moveInput.x > 0.0)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        
        _jumpInput = _input.Player.Jump.ReadValue<float>();

        CheckIfGrounded();
        CheckIfStuckInTerrain();
        
        if (_input.Player.Previous.WasPressedThisFrame())
        {
            levelController.SwitchCrayon(-1);
        }
        else if (_input.Player.Next.WasPressedThisFrame())
        {
            levelController.SwitchCrayon(1);
        }
        
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("Grounded", _canJump);
        animator.SetBool("Died", _isDead);
    }
    
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(_moveInput.x * movementSpeed, rb.linearVelocity.y);

        if (_jumpInput > 0.0f && _canJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
            _canJump = false;
        }
    }
    
    private void CheckIfGrounded()
    {
        if (!_canJump)
        {
            var results = new Collider2D[1];
            var filter = new ContactFilter2D();
            filter.useLayerMask = true;
            filter.layerMask = LayerMask.GetMask("Ground");
            var count = Physics2D.OverlapCircle(groundCheck.position, .25f, filter, results);
            if (count > 0)
            {
                _canJump = true;
            }
        }
    }

    private void CheckIfStuckInTerrain()
    {
        var results = new Collider2D[1];
        var filter = new ContactFilter2D();
        filter.useLayerMask = true;
        filter.layerMask = LayerMask.GetMask("Ground");
        var count = Physics2D.OverlapCircle(stuckInTerrainCheck.position, .1f, filter, results);
        if (count > 0)
        {
            Die();
        }
    }
    
    private IEnumerator DeathAnimation()
    {
        float upDuration = 0.5f;
        float downDuration = 3f;
        float rotationSpeed = 720f;

        Vector3 startPosition = transform.position;
        Vector3 upPosition = startPosition + new Vector3(0f, 5f, 0f);
        Vector3 downPosition = startPosition - new Vector3(0f, 20f, 0f);

        float angle = transform.eulerAngles.z;

        float t = 0f;

        while (t < upDuration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / upDuration);
            progress = 1f - Mathf.Pow(1f - progress, 2f);

            transform.position = Vector3.Lerp(startPosition, upPosition, progress);

            angle += rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            yield return null;
        }

        t = 0f;

        while (t < downDuration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / downDuration);
            progress *= progress;

            transform.position = Vector3.Lerp(upPosition, downPosition, progress);

            angle += rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            yield return null;
        }

        transform.position = downPosition;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Danger"))
        {
            Die();
        }
        
        else if (other.gameObject.layer == LayerMask.NameToLayer("Goal"))
        {
            levelController.Advance();
        }
    }

    private void Die()
    {
        _isDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        sr.sortingOrder = 100;
        Destroy(col);
        StartCoroutine(DeathAnimation());
    }
}
