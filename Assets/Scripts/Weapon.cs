using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 20f;
    public float time;
    public float reloadTime = 20f;
    public int bulletAmount;
    public int maxBulletAmount = 10;
    public TextMeshProUGUI bulletAmountText;

    void Start()
    {
        Reloaded();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Reloaded();
        }
        bulletAmountText.text = bulletAmount.ToString();

        if(time > reloadTime && bulletAmount > 0 )
        {
            if(Input.GetMouseButton(0))
            {   
                
                bulletAmount -= 1;
                time = 0f;
                Shoot();
            }
        }else
        {
            time += Time.deltaTime;
        }
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
    }

    public void Reloaded()
    {
        bulletAmount = maxBulletAmount;
        bulletAmountText.text = bulletAmount.ToString();
    }

}
