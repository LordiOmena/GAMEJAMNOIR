using UnityEngine;
using TMPro;
using System.Collections;

public sealed class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireForce = 20f;
    [SerializeField] private float fireRate = 0.2f;

    [Header("Ammo & Reloading")]
    [SerializeField] private int maxBulletAmount = 10;
    [SerializeField] private float reloadTime = 2f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI bulletAmountText;

    private int _bulletAmount;
    private float _nextFireTime;
    private bool _isReloading;
    private bool _isShooting;
    private Vector2 _inputDirection;
    private Vector2 _lastShootDirection = Vector2.up;

    public bool IsShooting => _isShooting;
    public bool IsReloading => _isReloading;

    private void Start()
    {
        _bulletAmount = maxBulletAmount;
        UpdateAmmoUI();
    }

    public void SetShootDirection(Vector2 direction)
    {
        _inputDirection = direction;
        if (direction.sqrMagnitude > 0.01f)
        {
            _lastShootDirection = direction;
        }
    }

    private void Update()
    {
        if (_isReloading)
        {
            _isShooting = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && _bulletAmount < maxBulletAmount)
        {
            StartCoroutine(ReloadingRoutine());
            return;
        }

        if (_inputDirection.sqrMagnitude > 0.01f)
        {
            if (_bulletAmount > 0)
            {
                _isShooting = true;

                if (Time.time >= _nextFireTime)
                {
                    Shoot();
                    _nextFireTime = Time.time + fireRate;
                }
            }
            else
            {
                _isShooting = false;
                StartCoroutine(ReloadingRoutine());
            }
        }
        else
        {
            _isShooting = false;
        }
    }

    private void Shoot()
    {
        _bulletAmount--;
        UpdateAmmoUI();

        float bulletAngle = Mathf.Atan2(_lastShootDirection.y, _lastShootDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion bulletRotation = Quaternion.Euler(0f, 0f, bulletAngle);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.AddForce(_lastShootDirection * fireForce, ForceMode2D.Impulse);
        }
    }

    private IEnumerator ReloadingRoutine()
    {
        _isReloading = true;
        _isShooting = false;

        if (bulletAmountText != null)
        {
            bulletAmountText.text = "Reloading...";
        }

        yield return new WaitForSeconds(reloadTime);

        _bulletAmount = maxBulletAmount;
        UpdateAmmoUI();
        _isReloading = false;
    }

    private void UpdateAmmoUI()
    {
        if (bulletAmountText != null)
        {
            bulletAmountText.text = $"{_bulletAmount} / {maxBulletAmount}";
        }
    }
}