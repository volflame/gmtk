using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public HandController leftHand;
    public HandController rightHand;
    public Transform player;
    public ProjectileSpawner projectileSpawner;
    public Animator bossAnimator;

    public float timeBetweenAttacks = 2.5f;
    public float telegraphDuration = 0.6f;
    private bool isAttacking = false;

    void Start()
    {
        StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenAttacks);

            if (!isAttacking)
            {
                int attackIndex = Random.Range(0, 5);
                yield return StartCoroutine(RunAttack(attackIndex));
            }
        }
    }

    IEnumerator RunAttack(int index)
    {
        isAttacking = true;

        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("Telegraph");
        }
        yield return new WaitForSeconds(telegraphDuration);


        switch (index)
        {
            case 0: yield return StartCoroutine(HandsChargeAttack()); break;
            case 1: yield return StartCoroutine(HandsPrayerAttack()); break;
            case 2: yield return StartCoroutine(HandsFromTopAttack()); break;
            case 3: yield return StartCoroutine(CascadeAttackVariant()); break;
            case 4: yield return StartCoroutine(FistSlamAttack()); break;
        }

        isAttacking = false;
    }

    IEnumerator HandsChargeAttack()
    {
        leftHand.SetDamaging(true);
        rightHand.SetDamaging(true);

        Coroutine l = StartCoroutine(leftHand.FlyToTarget(player.position, 0.6f));
        Coroutine r = StartCoroutine(rightHand.FlyToTarget(player.position, 0.6f));
        yield return l;
        yield return r;

        leftHand.SetDamaging(false);
        rightHand.SetDamaging(false);

        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(leftHand.ReturnToRest(0.8f));
        yield return StartCoroutine(rightHand.ReturnToRest(0.8f));
    }

    IEnumerator HandsPrayerAttack()
    {
        Vector3 leftSide = player.position + Vector3.left * 3f;
        Vector3 rightSide = player.position + Vector3.right * 3f;

        Coroutine l = StartCoroutine(leftHand.FlyToTarget(leftSide, 1f));
        Coroutine r = StartCoroutine(rightHand.FlyToTarget(rightSide, 1f));
        yield return l;
        yield return r;

        leftHand.ShowPrayerVisual();
        rightHand.ShowPrayerVisual();

        yield return new WaitForSeconds(0.3f);

        Vector3 midpoint = (leftHand.transform.position + rightHand.transform.position) / 2f;

        leftHand.SetDamaging(true);
        rightHand.SetDamaging(true);

        Coroutine l2 = StartCoroutine(leftHand.FlyToTarget(midpoint, 0.5f));
        Coroutine r2 = StartCoroutine(rightHand.FlyToTarget(midpoint, 0.5f));
        yield return l2;
        yield return r2;

        leftHand.SetDamaging(false);
        rightHand.SetDamaging(false);

        yield return new WaitForSeconds(0.3f);

        leftHand.ShowNormalVisual(); // swap back before/during return flight
        rightHand.ShowNormalVisual();

        yield return StartCoroutine(leftHand.ReturnToRest(0.8f));
        yield return StartCoroutine(rightHand.ReturnToRest(0.8f));
    }

    IEnumerator HandsFromTopAttack()
    {
        Vector3 leftStart = new Vector3(player.position.x - 2f, 10f, 0);
        Vector3 rightStart = new Vector3(player.position.x + 2f, 10f, 0);

        leftHand.transform.position = leftStart;
        rightHand.transform.position = rightStart;

        leftHand.SetDamaging(true);
        rightHand.SetDamaging(true);

        Coroutine l = StartCoroutine(leftHand.FlyToTarget(new Vector3(leftStart.x, player.position.y, 0), 0.8f));
        Coroutine r = StartCoroutine(rightHand.FlyToTarget(new Vector3(rightStart.x, player.position.y, 0), 0.8f));
        yield return l;
        yield return r;

        leftHand.SetDamaging(false);
        rightHand.SetDamaging(false);

        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(leftHand.ReturnToRest(1f));
        yield return StartCoroutine(rightHand.ReturnToRest(1f));
    }

    IEnumerator FistSlamAttack()
    {
        float slamX = player.position.x;
        Vector3 startPos = new Vector3(slamX, 20f, 0); // much further above the screen now
        Vector3 groundPos = new Vector3(slamX, player.position.y, 0);

        rightHand.transform.position = startPos;
        rightHand.ShowFistVisual();

        yield return new WaitForSeconds(0.4f); // telegraph pause at the top, still visible/threatening

        rightHand.SetDamaging(true);
        yield return StartCoroutine(rightHand.FlyToTarget(groundPos, 0.12f)); // much faster slam — was 0.25f

        yield return new WaitForSeconds(0.2f);
        rightHand.SetDamaging(false);

        yield return new WaitForSeconds(0.3f);
        rightHand.ShowNormalVisual();

        yield return StartCoroutine(rightHand.ReturnToRest(1f));
    }

    IEnumerator CascadeAttackVariant()
    {
        // randomly pick side-ramp or downward cascade each time this attack is chosen
        if (Random.value > 0.5f)
        {
            yield return StartCoroutine(projectileSpawner.CascadeRampAttack());
        }
        else
        {
            yield return StartCoroutine(projectileSpawner.CascadeDownwardAttack());
        }
    }
}