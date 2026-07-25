using Unity.VisualScripting;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public GameObject bulletPrefab;
    public GameObject whipPrefab;
    public GameObject ramenPrefab;
    public float fireForce = 20f;
    
    public override void Attack(Transform firePoint)
    {
        if (!CanAttack) return;
        // Ryan is about to write horrible code that he should go to hell for. Ignore it for this version. Please.
        if (weaponName == "Popcorn")
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);    
        }

        if (weaponName == "Ramen")
        {
            GameObject boomerang = Instantiate(ramenPrefab, firePoint.position, firePoint.rotation);
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            boomerang.GetComponent<Boomerang>().Init(firePoint.up, player);
        }

        StartCooldown();
    }

    public override void PhaseAttack(Transform firePoint)
    {
        if (!CanAttack) return;

        if (weaponName == "Popcorn")
        {
            Debug.Log("poppy pop pop pop");
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                bullet.transform.localScale *= 4f;
                bullet.GetComponent<Bullet>().knockbackForce *= 4f;
                bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
        }

        if (weaponName == "Ramen")
        {
            Debug.Log("Whip whip whippity whip");
            // turns into a whip
        }

        StartCooldown();
    }
}