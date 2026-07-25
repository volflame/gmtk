using UnityEngine;
using TMPro;
using System;
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
                // countdown = (float)Math.Round(countdown, 2);
                timer.text = countdown.ToString();
            }
        }

        if (countdown <= startSecondPhase)
        {
            secondPhase = true;
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
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        countdown = 10f;
        secondPhase = false;
        timer.color = Color.white;
        timer.gameObject.SetActive(true);
        if (currentWeapon != null)
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
            currentWeapon.transform.localPosition += new Vector3(0, 2.3f, 0);
        }
        // }

        currentWeapon.gameObject.SetActive(true);
        if (currentWeapon.GetComponent<MeleeWeapon>() != null)
        {
            currentWeapon.gameObject.SetActive(false);
        }
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