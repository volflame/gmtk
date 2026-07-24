using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public Transform firePoint;
    public Weapon currentWeapon; // assign default, or set via pickup

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentWeapon != null)
            {
                currentWeapon.Attack(firePoint);
            }
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false); // hide old weapon visuals if needed
        }

        currentWeapon = newWeapon;
        currentWeapon.gameObject.SetActive(true);
    }
}