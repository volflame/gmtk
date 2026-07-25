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


    void Awake()
    {
        startSecondPhase = countdown / 2;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    void Update()
    {
        if (currentWeapon.weaponName != "Default")
        {
            countdown -= Time.deltaTime;
            // countdown = (float)Math.Round(countdown, 2);
            timer.text = countdown.ToString();    
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

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentWeapon != null)
            {
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
            currentWeapon = defaultWeapon;
            timer.color = Color.white;
            timer.gameObject.SetActive(false);

            if (glowFadeRoutine != null) StopCoroutine(glowFadeRoutine);
            Color ic = powerupIcon.color; ic.a = 0f; powerupIcon.color = ic;
            Color gc = activeGlow.color; gc.a = 0f; activeGlow.color = gc;
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        countdown = 10f;
        secondPhase = false;
        timer.color = Color.white;
        timer.gameObject.SetActive(true);
        if (currentWeapon != null && currentWeapon != defaultWeapon)
        {
            Destroy(currentWeapon.gameObject); // fully remove the old one, not just hide it
        }

        currentWeapon = newWeapon;
        currentWeapon.transform.SetParent(firePoint);
        if (currentWeapon.name == "Burrito") // shame on you ryan for your spaghetti code
        {
            currentWeapon.transform.localPosition = new Vector3(0f, 2.3f);    
        }
        currentWeapon.transform.localRotation = Quaternion.identity * new Quaternion(0, 0, -1, 0);
        currentWeapon.gameObject.SetActive(true);
        if (currentWeapon.GetComponent<MeleeWeapon>() != null)
        {
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