using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputSystem_Actions _input;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Animator animator;
    
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpSpeed;
    
    private void Awake() => _input = new InputSystem_Actions();
    private void OnEnable() => _input.Enable();
    private void OnDisable() => _input.Disable();
    
    private Vector2 _moveInput;
    private float _jumpInput;

    private bool _canJump = true;
    
    private void Update()
    {
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

        if (!_canJump)
        {
            Collider2D[] results = new Collider2D[1];
            ContactFilter2D filter = new ContactFilter2D();
            filter.useLayerMask = true;
            filter.layerMask = LayerMask.GetMask("Ground");
            var count = Physics2D.OverlapCircle(groundCheck.position, .25f, filter, results);
            if (count > 0)
            {
                _canJump = true;
            }
        }
        
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("Grounded", _canJump);
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
}
