using UnityEngine;
using TMPro;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEditor.ShaderGraph.Internal;
using Cinemachine;
using UnityEngine.AI;

public class PlayerWeaponController : MonoBehaviour
{
    public Transform firePoint;
    public Weapon currentWeapon; // assign default, or set via pickup
    public Weapon defaultWeapon;
    public TextMeshProUGUI timer;
    public float countdown = 10f;
    private float startSecondPhase;
    public bool secondPhase = false;
    public float impulseShakeForce = 1f;
    private CinemachineImpulseSource impulseSource;

    public Image powerupIcon;
    public Image activeGlow;
    public float glowFadeDuration = 0.3f;
    private Coroutine glowFadeRoutine;
    public Animator animator;

    void Awake()
    {
        startSecondPhase = countdown / 2;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    void Update()
    {
        if (currentWeapon)
        {
            if (currentWeapon.weaponName != "Default")
            {
                countdown -= Time.deltaTime;
                int minutes = Mathf.FloorToInt(countdown / 60f);
                int seconds = Mathf.FloorToInt(countdown % 60f);
                timer.text = $"{minutes}:{seconds:00}";
            }
        }

        if (countdown <= startSecondPhase && !secondPhase)
        {
            secondPhase = true;
            if (currentWeapon != null && currentWeapon != defaultWeapon)
            {
                powerupIcon.sprite = currentWeapon.hotSprite; // instant swap, no fade
            }
        }

        if (secondPhase)
        {
            timer.color = Color.red;
        }

        if (currentWeapon != null)
        {
            bool holdToFire = secondPhase ? currentWeapon.holdToFirePhase2 : currentWeapon.holdToFire;
            bool firing = holdToFire ? Input.GetKey(KeyCode.Z) : Input.GetKeyDown(KeyCode.Z);

            if (firing)
            {
                animator.SetTrigger("Attack");
                if (!secondPhase)
                {
                    currentWeapon.Attack(firePoint);
                }
                else
                {
                    currentWeapon.PhaseAttack(firePoint);
                }
            }
        }
        if (countdown <= 0)
        {
            EquipWeapon(defaultWeapon);
            defaultWeapon.cooldown = 0;
            defaultWeapon.transform.localPosition += new Vector3(0, 2.3f, 0);
            timer.color = Color.white;
            timer.gameObject.SetActive(false);

            if (glowFadeRoutine != null) StopCoroutine(glowFadeRoutine);
            Color ic = powerupIcon.color; ic.a = 0f; powerupIcon.color = ic;
            Color gc = activeGlow.color; gc.a = 0f; activeGlow.color = gc;
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        countdown = newWeapon.duration;
        startSecondPhase = newWeapon.secondPhaseAt;
        secondPhase = false;
        timer.color = Color.white;
        timer.gameObject.SetActive(true);
        if (currentWeapon != null && currentWeapon != defaultWeapon)
        {
            Destroy(currentWeapon.gameObject);
        }
        else if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false);
        }

        currentWeapon = newWeapon;

        // {
        //     currentWeapon.transform.SetParent(transform);

        //     // Use firePoint's current facing direction to push the burrito slightly in front of the player
        //     Vector3 facingDir = firePoint.up; // matches how firePoint's rotation is set elsewhere (angle - 90)
        //     Vector3 forwardOffset = facingDir * 1f;   // tweak "1f" to taste — how far in front
        //     Vector3 upOffset = Vector3.up * 2.3f;      // keep your existing vertical offset

        //     currentWeapon.transform.position = transform.position + forwardOffset + upOffset;
        //     currentWeapon.transform.localRotation = Quaternion.identity; // stays upright, doesn't rotate with aim
        // }
        // else
        // {
        currentWeapon.transform.SetParent(firePoint);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity * new Quaternion(0, 0, -1, 0);

        if (currentWeapon.weaponName == "Burrito")
        {
            currentWeapon.transform.localPosition += new Vector3(0, 7f, 0);
            currentWeapon.transform.localRotation = Quaternion.identity; // swing animation is already oriented correctly, undo the shared 180 flip
        }
        // }

        currentWeapon.gameObject.SetActive(true);
        if (currentWeapon.GetComponent<MeleeWeapon>() != null)
        {
            currentWeapon.GetComponent<MeleeWeapon>().isAttacking = false;
            currentWeapon.gameObject.SetActive(false);
        }

        powerupIcon.sprite = newWeapon.coldSprite;
        Color pc = powerupIcon.color; pc.a = 1f; powerupIcon.color = pc; // instant, no fade for the icon itself

        if (glowFadeRoutine != null) StopCoroutine(glowFadeRoutine);
        glowFadeRoutine = StartCoroutine(FadeGlowIn());
    }

    private IEnumerator FadeGlowIn()
    {
        float t = 0f;
        Color start = activeGlow.color;
        while (t < glowFadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(start.a, 1f, t / glowFadeDuration);
            activeGlow.color = new Color(start.r, start.g, start.b, a);
            yield return null;
        }
        activeGlow.color = new Color(start.r, start.g, start.b, 1f);
    }

    public void CameraShake()
    {
        if (impulseSource)
        {
            impulseSource.GenerateImpulseWithForce(impulseShakeForce);
        }
        else
        {
            Debug.Log("Not found");
        }
    }
}