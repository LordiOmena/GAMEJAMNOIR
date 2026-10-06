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

    public bool IsShooting => _isShooting;
    public bool IsReloading => _isReloading;

    private void Start()
    {
        _bulletAmount = maxBulletAmount;
        UpdateAmmoUI();
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

        if (Input.GetMouseButton(0))
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

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
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
