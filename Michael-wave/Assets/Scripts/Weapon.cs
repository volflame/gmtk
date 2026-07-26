using UnityEngine;
using TMPro;
using System;

public abstract class Weapon : MonoBehaviour
{
    public string weaponName;
    public Sprite coldSprite;
    public Sprite hotSprite;
    public float cooldown = 0.3f;

    [Header("Powerup Timing")]
    public float duration = 30f;      // how long this powerup lasts once picked up
    public float secondPhaseAt = 15f; // seconds REMAINING when the second phase kicks in

    // When true the attack repeats while the fire key is held (rate limited by cooldown),
    // instead of requiring a fresh press each shot.
    public bool holdToFire = false;
    public bool holdToFirePhase2 = false;

    private float cooldownReadyTime = 0f;
    public TextMeshProUGUI timer;

    public bool CanAttack => Time.time >= cooldownReadyTime;

    public abstract void Attack(Transform firePoint);
    public abstract void PhaseAttack(Transform firePoint);

    public void StartCooldown()
    {
        cooldownReadyTime = Time.time + cooldown;
    }
}