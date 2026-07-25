using UnityEngine;
using TMPro;
using System;

public abstract class Weapon : MonoBehaviour
{
    public string weaponName;
    public Sprite coldSprite;
    public Sprite hotSprite;
    public float cooldown = 0.3f;
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