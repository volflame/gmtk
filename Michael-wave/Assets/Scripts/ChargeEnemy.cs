using UnityEngine;

public class ChargeEnemy : Enemy
{
    [Header("Charge Settings")]
    public float chargeRadius = 2f;
    public float chargeSpeed = 12f;
    public float telegraphDuration = 0.5f; // pause before charging (good time to flash a warning sprite/animation)
    public float recoverDuration = 0.75f;  // pause after charging before picking a new target
    public float arrivalThreshold = 0.3f;  // how close counts as "reached" the charge target

    private enum ChargeState { Telegraphing, Charging, Recovering }
    private ChargeState state = ChargeState.Telegraphing;

    private Vector2 chargeTarget;
    private float stateTimer = 0f;

    protected override void Start()
    {
        base.Start();
        PickNewChargeTarget();
        state = ChargeState.Telegraphing;
        stateTimer = 0f;
    }

    protected override void Move()
    {
        if (!target) return;

        switch (state)
        {
            case ChargeState.Telegraphing:
                rb.linearVelocity = Vector2.zero;
                if (animator != null)
                {
                    animator.SetBool("Charging", true); // animation starts here, ahead of the actual dash
                }
                break;

            case ChargeState.Charging:
                Vector2 toTarget = chargeTarget - rb.position;
                if (toTarget.magnitude <= arrivalThreshold)
                {
                    rb.linearVelocity = Vector2.zero;
                    state = ChargeState.Recovering;
                    stateTimer = 0f;
                    if (animator != null)
                    {
                        animator.SetBool("Charging", false);
                    }
                }
                else
                {
                    rb.linearVelocity = toTarget.normalized * chargeSpeed * speedMultiplierPublic;
                }
                break;

            case ChargeState.Recovering:
                rb.linearVelocity = Vector2.zero;
                if (animator != null)
                {
                    animator.SetBool("Charging", false); // redundant safety, in case Charging state was skipped somehow
                }
                break;
        }
    }

    protected override void OnUpdate()
    {
        if (!target) return;

        stateTimer += Time.deltaTime;

        switch (state)
        {
            case ChargeState.Telegraphing:
                if (stateTimer >= telegraphDuration)
                {
                    state = ChargeState.Charging;
                    stateTimer = 0f;
                }
                break;

            case ChargeState.Charging:
                // handled in Move(); nothing needed here
                break;

            case ChargeState.Recovering:
                if (stateTimer >= recoverDuration)
                {
                    PickNewChargeTarget();
                    state = ChargeState.Telegraphing;
                    stateTimer = 0f;
                }
                break;
        }
    }

    private void PickNewChargeTarget()
    {
        if (!target) return;

        // Pick a point roughly opposite the player from wherever we currently are,
        // so the charge sweeps THROUGH/past the player rather than just nudging toward them.
        Vector2 awayFromPlayer = ((Vector2)transform.position - (Vector2)target.position).normalized;

        // If we're exactly on top of the player (edge case), pick a random direction instead
        if (awayFromPlayer == Vector2.zero)
        {
            awayFromPlayer = Random.insideUnitCircle.normalized;
        }

        // Target point is on the OPPOSITE side of the player from our current side,
        // at a random distance within chargeRadius
        Vector2 oppositeSideDir = -awayFromPlayer;
        float distance = Random.Range(chargeRadius * 0.6f, chargeRadius); // bias toward using most of the radius
        chargeTarget = (Vector2)target.position + oppositeSideDir * distance;
    }

    // Exposes protected speedMultiplier effect from base class without duplicating slow logic
    private float speedMultiplierPublic => CurrentSpeed / Mathf.Max(moveSpeed, 0.0001f);

    private void OnDrawGizmosSelected()
    {
        // Visualize the charge radius in the Scene view for tuning
        if (target)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(target.position, chargeRadius);
        }
    }
}