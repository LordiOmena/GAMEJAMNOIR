using UnityEngine;

public sealed class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Weapon weapon;

    private Animator _animator;
    private Vector2 _moveDirection;
    private Vector2 _shootDirection;
    private Vector2 _lastLookDirection = Vector2.down;
    private bool _isShooting;

    public bool IsShooting => _isShooting;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInput();
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleInput()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.D)) moveX = 1f;
        else if (Input.GetKey(KeyCode.A)) moveX = -1f;

        if (Input.GetKey(KeyCode.W)) moveY = 1f;
        else if (Input.GetKey(KeyCode.S)) moveY = -1f;

        _moveDirection = new Vector2(moveX, moveY).normalized;

        float shootX = 0f;
        float shootY = 0f;

        if (Input.GetKey(KeyCode.UpArrow)) shootY = 1f;
        else if (Input.GetKey(KeyCode.DownArrow)) shootY = -1f;

        if (Input.GetKey(KeyCode.RightArrow)) shootX = 1f;
        else if (Input.GetKey(KeyCode.LeftArrow)) shootX = -1f;

        _shootDirection = new Vector2(shootX, shootY).normalized;

        if (_shootDirection.sqrMagnitude > 0.01f)
        {
            _lastLookDirection = _shootDirection;
        }
        else if (_moveDirection.sqrMagnitude > 0.01f)
        {
            _lastLookDirection = _moveDirection;
        }

        if (weapon != null)
        {
            weapon.SetShootDirection(_shootDirection);
            _isShooting = weapon.IsShooting;
        }
    }

    private void MovePlayer()
    {
        rb.linearVelocity = _moveDirection * moveSpeed;
    }

    private void UpdateAnimations()
    {
        bool isMoving = _moveDirection.magnitude > 0.1f;
        _animator.SetBool("isMoving", isMoving);
        _animator.SetBool("isShooting", _isShooting);

        _animator.SetFloat("Horizontal", _lastLookDirection.x);
        _animator.SetFloat("Vertical", _lastLookDirection.y);
    }
}
