using System.Collections;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public GameObject meleeHitbox;
    public float attackDuration = 0.3f;
    private float attackTimer = 0f;
    private bool isAttacking = false;
    public GameObject bulletPrefab;
    public float fireForce = 20f;
    private Vector3 original;

    public GameObject cheeseZonePrefab;
    public int cheeseZoneCount = 3;
    public float cheeseSpawnRadius = 8f;
    public float cheeseMinDistance = 2f;

    private void Awake()
    {
        original = meleeHitbox.transform.localScale;
    }

    private void Update()
    {
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDuration)
            {
                attackTimer = 0f;
                isAttacking = false;
                meleeHitbox.transform.localScale = original;
                meleeHitbox.SetActive(false);
            }
        }
    }

    public override void Attack(Transform firePoint)
    {
        print(CanAttack);
        if (!CanAttack || isAttacking) return;

        meleeHitbox.transform.rotation = firePoint.rotation * new Quaternion(0, 0, -1, 0);
        meleeHitbox.SetActive(true);
        isAttacking = true;
        StartCooldown();
    }

    public override void PhaseAttack(Transform firePoint)
    {
        if (!CanAttack || isAttacking) return;
        meleeHitbox.transform.rotation = firePoint.rotation;
        meleeHitbox.SetActive(true);
        isAttacking = true;
        StartCooldown();

        if (weaponName == "Burrito")
        {
            meleeHitbox.transform.localScale = original * 3f;
            SpawnCheeseZones(firePoint);
            Debug.Log("Burr burrito");
        }

        // Ryan why are you writing genuine spaghetti code lock in
        if (weaponName == "Egg")
        {
            attackDuration = 2.0f;
            Debug.Log("Eggyweggy");
            StartCoroutine(Expand());

            GameObject bulletUp = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bulletUp.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);

            GameObject bulletDown = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bulletDown.GetComponent<Rigidbody2D>().AddForce(-firePoint.up * fireForce, ForceMode2D.Impulse);

            GameObject bulletLeft = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bulletLeft.GetComponent<Rigidbody2D>().AddForce(-firePoint.right * fireForce, ForceMode2D.Impulse);

            GameObject bulletRight = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bulletRight.GetComponent<Rigidbody2D>().AddForce(firePoint.right * fireForce, ForceMode2D.Impulse);
        }
    }

    public float expandDuration = 0.2f; // time to grow
    IEnumerator Expand()
    {
        Vector3 expanded = original * 2f;

        // Grow
        float t = 0f;
        while (t < expandDuration)
        {
            t += Time.deltaTime;
            float normalized = t / expandDuration;
            float eased = Mathf.SmoothStep(0f, 1f, normalized); // <-- declared and used here
            meleeHitbox.transform.localScale = Vector3.Lerp(original, expanded, eased);
            yield return null;
        }
        meleeHitbox.transform.localScale = expanded;
    }

    private void SpawnCheeseZones(Transform firePoint)
    {
        for (int i = 0; i < cheeseZoneCount; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(cheeseMinDistance, cheeseSpawnRadius);
            Vector3 spawnPos = firePoint.position + new Vector3(randomOffset.x, randomOffset.y, 0f);

            Instantiate(cheeseZonePrefab, spawnPos, Quaternion.identity);
        }
    }
}