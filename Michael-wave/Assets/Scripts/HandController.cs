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

    private bool isDamaging = false;
    private Vector3 restPosition;

    void Awake()
    {
        restPosition = transform.position;
        SetVisual(normalVisual);
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