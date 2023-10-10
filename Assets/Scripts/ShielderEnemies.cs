using UnityEngine;

public class ShielderEnemy : MonoBehaviour
{
    public GameObject shieldObject; // Reference to the shield GameObject
    public GameObject bulletPrefab; // Reference to your ShielderBullet prefab
    public int maxShieldHealth = 100; // Maximum shield health
    private int currentShieldHealth; // Current shield health

    public int maxHealth = 100; // Maximum health
    private int currentHealth;

    public float detectionRange = 10f; // Range within which the enemy can detect the player
    private Transform playerTransform; // Reference to the player's transform
    public float baseBulletSpawnRate = 2f; // Base shooting rate (when the shield is not broken)
    public float fasterBulletSpawnRate = 0.5f; // Faster shooting rate (when the shield is broken)
    private float bulletSpawnRate; // Current shooting rate
    private float timeSinceLastSpawn = 0f;

    private void Start()
    {
        currentShieldHealth = maxShieldHealth;
        EnableShield(); // Start with the shield disabled

        currentHealth = maxHealth;

        // Find the player's transform based on their tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }

        // Initialize shield health
        currentShieldHealth = maxShieldHealth;

        // Initialize bullet spawn rate
        bulletSpawnRate = baseBulletSpawnRate;
    }


    private void Update()
{
   if (playerTransform != null && Vector2.Distance(transform.position, playerTransform.position) <= detectionRange)
        {
            // The player is within range, so we can spawn a ShielderBullet
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn >= bulletSpawnRate)
            {
                // Spawn the ShielderBullet
                SpawnBullet();

                // Reset the timer
                timeSinceLastSpawn = 0f;
            }
        }
}

    private void SpawnBullet()
    {
        if (bulletPrefab != null)
        {
            // Instantiate the bullet at the enemy's position
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // You may need to set any additional properties or references for the bullet here
        }
    }

    // Method to enable the shield
    private void EnableShield()
    {
        shieldObject.SetActive(true);
    }

    // Method to disable the shield
    private void DisableShield()
    {
        shieldObject.SetActive(false);
    }

    // Method to take damage to the shield
    public void TakeShieldDamage(int damage)
    {
        if (currentShieldHealth > 0)
        {
            currentShieldHealth -= damage;
            if (currentShieldHealth <= 0)
            {
                // The shield is depleted; disable it
                currentShieldHealth = 0;
                DisableShield();
                
                // If the shield is broken, apply damage to the enemy's health
                int shieldBreakDamage = 20; // Adjust the damage value as needed
                TakeDamage(shieldBreakDamage);

                bulletSpawnRate = fasterBulletSpawnRate;
                currentShieldHealth = 0;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentShieldHealth <= 0)
        {
            // Only apply damage to the health if the shield is depleted
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die(); // If health reaches zero or below, destroy the enemy
            }
        }
    }

    private void Die()
    {
        Destroy(gameObject); // For example, destroy the enemy when it runs out of health
    }

    public bool IsShieldActive()
    {
        return shieldObject.activeSelf;
    }

}

