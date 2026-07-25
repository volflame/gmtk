using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponPrefab; // the weapon this pickup grants

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerWeaponController controller = collision.GetComponent<PlayerWeaponController>();
        if (controller != null)
        {
            Weapon newWeapon = Instantiate(weaponPrefab, controller.transform);
            controller.EquipWeapon(newWeapon);

            Destroy(gameObject); // remove pickup from world
        }
    }
}