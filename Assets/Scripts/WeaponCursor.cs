using UnityEngine;

public class WeaponCursor : MonoBehaviour
{
    public GameObject cursorReticle; // The cursor reticle prefab
    public Transform firePoint;     // Fire point for bullets
    public GameObject bulletPrefab; // Bullet prefab
    public float bulletSpeed = 10f;

    private GameObject currentReticle; // Reference to the active cursor reticle

    private void Start()
    {
        Cursor.visible = false; // Hide the system cursor
    }

    private void Update()
    {
        // Update the position of the cursor reticle
        UpdateCursorReticle();
                            
        // Check for mouse input to shoot
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void UpdateCursorReticle()
    {
        // Get the mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // If the reticle doesn't exist, create it
        if (currentReticle == null)
        {
            currentReticle = Instantiate(cursorReticle, mousePos, Quaternion.identity);
        }
        else
        {
            // Update the reticle's position
            currentReticle.transform.position = mousePos;
        }
    }

    private void Shoot()
{
    // Spawn a bullet at the fire point
    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

    // Calculate the direction towards the cursor
    Vector2 cursorDirection = (currentReticle.transform.position - bullet.transform.position).normalized;

    // Get the Bullet component from the instantiated bullet
    Bullet bulletScript = bullet.GetComponent<Bullet>();

    if (bulletScript != null)
    {
        // Set the bullet's speed using the bulletSpeed value from WeaponCursor
        bulletScript.speed = bulletSpeed;
    }
}
}
