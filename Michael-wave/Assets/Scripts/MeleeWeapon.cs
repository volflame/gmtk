using System.Collections;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public GameObject meleeHitbox;
    public float attackDuration = 0.3f;
    private float attackTimer = 0f;
    public bool isAttacking = false;
    public GameObject bulletPrefab;
    public float fireForce = 20f;
    private Vector3 original;

    public GameObject cheeseZonePrefab;
    public int cheeseZoneCount = 3;
    public float cheeseSpawnRadius = 8f;
    public float cheeseMinDistance = 2f;
    private PlayerWeaponController weaponController;

    public Animator attackAnimator; // optional: if assigned, attackDuration syncs to its clip length

    // Only the collider needs to be gated to a frame window; the GameObject (sprite/animator)
    // stays active for the whole attackDuration so the animation keeps playing normally.
    private Collider2D hitboxCollider;
    public float hitboxActiveStart = 0.375f; // frame 3 at 8fps
    public float hitboxActiveEnd = 0.875f;   // frame 7 at 8fps

    private void Awake()
    {
        original = meleeHitbox.transform.localScale;
        weaponController = gameObject.GetComponentInParent<PlayerWeaponController>();
        hitboxCollider = meleeHitbox.GetComponent<Collider2D>();
        if (attackAnimator != null && attackAnimator.runtimeAnimatorController != null)
        {
            AnimationClip[] clips = attackAnimator.runtimeAnimatorController.animationClips;
            if (clips.Length > 0)
            {
                attackDuration = clips[0].length;
            }
        }
    }

    private void Update()
    {
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;

            if (hitboxCollider != null)
            {
                hitboxCollider.enabled = attackTimer >= hitboxActiveStart && attackTimer < hitboxActiveEnd;
            }

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
        print(isAttacking);
        print(weaponName);
        if (!CanAttack || isAttacking) return;

        // meleeHitbox.transform.rotation = firePoint.rotation * new Quaternion(0, 0, -1, 0);
        meleeHitbox.SetActive(true);
        isAttacking = true;
        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = false;
        }
        if (attackAnimator != null)
        {
            attackAnimator.Play("Swing", 0, 0f);
        }
        if (weaponName == "Egg")
        {
            StartCoroutine(Expand(1.5f));
        }
        StartCooldown();
    }

    public override void PhaseAttack(Transform firePoint)
    {
        if (!CanAttack || isAttacking) return;
        // meleeHitbox.transform.rotation = firePoint.rotation;
        meleeHitbox.SetActive(true);
        isAttacking = true;
        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = false;
        }
        if (attackAnimator != null)
        {
            attackAnimator.Play("Swing", 0, 0f);
        }
        if (weaponController != null && weaponName != "Default") // RYAN ISTG STOP YOUR SPAGHETTI CODE
        {
            weaponController.CameraShake();
        }

        if (weaponName == "Burrito")
        {
            meleeHitbox.transform.localScale = original * 2f;
            Vector3 bigPos = meleeHitbox.transform.localPosition;
            bigPos.y = 14f;
            meleeHitbox.transform.localPosition = bigPos;
            SpawnCheeseZones(firePoint);
            Debug.Log("Burr burrito");
        }

        // Ryan why are you writing genuine spaghetti code lock in
        if (weaponName == "Egg")
        {
            attackDuration = 0.5f;
            cooldown = 0.5f;
            Debug.Log("Eggyweggy");
            StartCoroutine(Expand(3f));

            GameObject bulletUp = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bulletUp.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);

            GameObject bulletDown = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bulletDown.GetComponent<Rigidbody2D>().AddForce(-firePoint.up * fireForce, ForceMode2D.Impulse);

            GameObject bulletLeft = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bulletLeft.GetComponent<Rigidbody2D>().AddForce(-firePoint.right * fireForce, ForceMode2D.Impulse);

            GameObject bulletRight = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bulletRight.GetComponent<Rigidbody2D>().AddForce(firePoint.right * fireForce, ForceMode2D.Impulse);
        }
        StartCooldown();
    }

    public float expandDuration = 0.2f; // time to grow
    IEnumerator Expand(float factor)
    {
        Vector3 expanded = original * factor;

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