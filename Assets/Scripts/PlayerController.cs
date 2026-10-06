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
    private Vector2 _mousePosition;
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
        RotateTowardsMouse();
    }

    private void HandleInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        _moveDirection = new Vector2(moveX, moveY).normalized;

        if (Camera.main != null)
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (weapon != null)
        {
            _isShooting = weapon.IsShooting;
        }
    }

    private void MovePlayer()
    {
        rb.linearVelocity = _moveDirection * moveSpeed;
    }

    private void RotateTowardsMouse()
    {
        Vector2 aimDirection = _mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }

    private void UpdateAnimations()
    {
        bool isMoving = _moveDirection.magnitude > 0.1f;
        _animator.SetBool("isMoving", isMoving);
        _animator.SetBool("isShooting", _isShooting);
    }
}
