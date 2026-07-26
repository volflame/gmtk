using System.Collections;
using Cinemachine;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public Animator animator;
    public int contactDamage = 1;
    public float contactKnockback = 8f;
    public CinemachineImpulseSource impulseSource;
    public float impulseForce = 1f;

    [Header("Visual Variants")]
    public GameObject normalVisual;
    public GameObject prayerVisual;
    public GameObject fistVisual;

    [Header("Idle Bob")]
    public float idleBobHeight = 0.4f;    // how far the hand drifts up
    public float idleBobDuration = 2f;    // seconds for a full up-and-back cycle
    [Range(0f, 1f)] public float idleBobPhaseOffset = 0f; // stagger the two hands if desired

    private bool isDamaging = false;
    private Vector3 restPosition;
    private Coroutine idleBobRoutine;

    void Awake()
    {
        restPosition = transform.position;
        SetVisual(normalVisual);
    }

    // Attacks drive transform.position directly, so the bob has to be off while one runs.
    public void StartIdleBob()
    {
        if (idleBobRoutine != null) return;
        idleBobRoutine = StartCoroutine(IdleBob());
    }

    public void StopIdleBob()
    {
        if (idleBobRoutine == null) return;
        StopCoroutine(idleBobRoutine);
        idleBobRoutine = null;
    }

    private IEnumerator IdleBob()
    {
        float t = idleBobPhaseOffset * idleBobDuration;

        while (true)
        {
            t += Time.deltaTime;

            // starts at 0 (rest), eases up to idleBobHeight, eases back down, repeat
            float phase = (Mathf.Sin((t / idleBobDuration) * Mathf.PI * 2f - Mathf.PI * 0.5f) + 1f) * 0.5f;
            transform.position = restPosition + Vector3.up * (idleBobHeight * phase);

            yield return null;
        }
    }

    private void SetVisual(GameObject activeVisual)
    {
        if (normalVisual != null) normalVisual.SetActive(activeVisual == normalVisual);
        if (prayerVisual != null) prayerVisual.SetActive(activeVisual == prayerVisual);
        if (fistVisual != null) fistVisual.SetActive(activeVisual == fistVisual);
    }

    public void ShowPrayerVisual() => SetVisual(prayerVisual);
    public void ShowFistVisual() => SetVisual(fistVisual);
    public void ShowNormalVisual() => SetVisual(normalVisual);

    public IEnumerator FlyToTarget(Vector3 targetPos, float duration)
    {
        Vector3 start = transform.position;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.SmoothStep(0f, 1f, t / duration);
            transform.position = Vector3.Lerp(start, targetPos, normalized);
            yield return null;
        }

        transform.position = targetPos;
    }

    public IEnumerator ReturnToRest(float duration)
    {
        yield return FlyToTarget(restPosition, duration);
    }

    public void SetDamaging(bool value)
    {
        isDamaging = value;

        if (animator != null)
        {
            animator.SetBool("Attacking", value);
        }

        if (value && impulseSource != null)
        {
            impulseSource.GenerateImpulseWithForce(impulseForce);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDamaging) return;

        PlayerHealth player = collision.GetComponent<PlayerHealth>();
        if (player != null)
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            player.TakeDamage(contactDamage, direction, contactKnockback);
        }
    }
}