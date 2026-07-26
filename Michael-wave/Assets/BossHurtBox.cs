using UnityEngine;

public class BossHurtbox : MonoBehaviour
{
    public BossHealth bossHealth; // drag the same BossHealth reference on every hurtbox

    public void TakeDamage(float amount)
    {
        bossHealth.TakeDamage(amount);
    }
}