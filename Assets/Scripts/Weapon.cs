using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePointDefault;  // Default fire point
    public Transform firePointUpwards;  // Fire point when looking up

    public GameObject bulletPrefab;

    public CharacterController2D controller;

    void Start()
    {
        controller = GetComponentInParent<CharacterController2D>();
    }

    void Update()
    {
        // Check if the player is looking up from the CharacterController2D script
        if (controller != null && controller.isLookingUp && Input.GetButtonDown("Fire1"))
        {
            // Shoot upwards when looking up using the firePointUpwards
            ShootUp(firePointUpwards);
        }
        else if (Input.GetButtonDown("Fire1")) // Default shooting condition using the firePointDefault
        {
            Shoot(firePointDefault);
        }
    }

    void Shoot(Transform firePoint)
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    void ShootUp(Transform firePoint)
    {
        Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0f, 0f, 90f));
    }
}
