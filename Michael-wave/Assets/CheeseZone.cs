using UnityEngine;
using System.Collections.Generic;

public class CheeseZone : MonoBehaviour
{
    public float slowMultiplier = 0.5f; // 50% speed while inside
    public float lifetime = 5f;         // how long the zone exists before disappearing

    private List<Enemy> affectedEnemies = new List<Enemy>();

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && !affectedEnemies.Contains(enemy))
        {
            enemy.ApplySlow(slowMultiplier);
            affectedEnemies.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && affectedEnemies.Contains(enemy))
        {
            enemy.RemoveSlow();
            affectedEnemies.Remove(enemy);
        }
    }

    private void OnDestroy()
    {
        // Make sure enemies aren't stuck slowed if zone disappears while they're inside
        foreach (var enemy in affectedEnemies)
        {
            if (enemy != null)
                enemy.RemoveSlow();
        }
    }
}