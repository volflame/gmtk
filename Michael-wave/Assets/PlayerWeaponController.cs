using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class PlayerWeaponController : MonoBehaviour
{
    public Transform firePoint;
    public Weapon currentWeapon; // assign default, or set via pickup
    public TextMeshProUGUI timer;
    public float countdown = 10f;
    private float startSecondPhase;
    public bool secondPhase = false;


    void Awake()
    {
        startSecondPhase = countdown / 2;
    }
    void Update()
    {
        countdown -= Time.deltaTime;
        // countdown = (float)Math.Round(countdown, 2);
        timer.text = countdown.ToString();

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
            currentWeapon = null;
            timer.color = Color.white;
            timer.gameObject.SetActive(false);
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        countdown = 30f;
        timer.color = Color.white;
        timer.gameObject.SetActive(true);
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false); // hide old weapon visuals if needed
        }

        currentWeapon = newWeapon;
        currentWeapon.transform.SetParent(firePoint);
        currentWeapon.transform.localPosition = new Vector3(0f, 2.3f);
        currentWeapon.transform.localRotation = Quaternion.identity * new Quaternion(0, 0, -1, 0);
        currentWeapon.gameObject.SetActive(true);
        if (currentWeapon.GetComponent<MeleeWeapon>() != null)
        {
            currentWeapon.gameObject.SetActive(false);
        }
    }
}