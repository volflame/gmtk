using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 20f;
    public float specialFireForce = 10f;
    public GameObject Melee;
    bool isAttacking = false;
    float attackDuration = 0.3f;
    float attackTimer = 0f;
    void Update()
    {
        CheckMeleeTimer();
    }
    public void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
    }

    public void SpecialFire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.transform.localScale *= 5f;
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * specialFireForce, ForceMode2D.Impulse);
    }

    public void MeleeAttack()
    {
        if (!isAttacking)
        {
            Melee.SetActive(true);
            isAttacking = true;
        }
    }

    void CheckMeleeTimer()
    {
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDuration)
            {
                attackTimer = 0;
                isAttacking = false;
                Melee.SetActive(false);
            }
        }
    }
}
