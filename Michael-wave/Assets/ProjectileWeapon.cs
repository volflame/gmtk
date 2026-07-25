using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class ProjectileWeapon : Weapon
{
    public GameObject bulletPrefab;
    public GameObject whipPrefab;
    public float whipDuration = 0.25f;
    public GameObject ramenPrefab;
    public float fireForce = 20f;
    private PlayerWeaponController weaponController;
    void Awake()
    {
        weaponController = gameObject.GetComponentInParent<PlayerWeaponController>();
    }

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
            bullet.transform.localScale *= 10f;
            bullet.GetComponent<Bullet>().knockbackForce *= 6f;
            bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
        }

        if (weaponName == "Ramen")
        {
            Debug.Log("Whip whip whippity whip");
            GameObject whip = Instantiate(whipPrefab, firePoint.position, firePoint.rotation);
            whip.transform.SetParent(firePoint); // follows aim direction while active
            whip.transform.localPosition = Vector3.zero;
            whip.transform.localRotation = Quaternion.identity;

            Destroy(whip, whipDuration);
        }
        if (weaponController != null)
        {
            weaponController.CameraShake();
        }
        else
        {
            Debug.Log("not found");
        }

        StartCooldown();
    }
}