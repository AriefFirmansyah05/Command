using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCursor : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 40;
    public float maxRadius = 10f; // Set your desired max radius here
    public GameObject impactEffect;

    private Vector3 startPos;
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = transform.position.z; // Set the z position to match the bullet's

        // Calculate the direction to the cursor
        Vector3 direction = (targetPosition - transform.position).normalized;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
    }

void Update()
    {
        // Calculate the current distance from the starting position
        float currentDistance = Vector3.Distance(startPos, transform.position);

        // Check if the bullet has exceeded the max radius
        if (currentDistance >= maxRadius)
        {
            DestroyBullet();
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
    ShielderEnemy shielderEnemy = hitInfo.GetComponent<ShielderEnemy>();
    
    if (shielderEnemy != null)
    {
        if (shielderEnemy.IsShieldActive())
        {
            Debug.Log("Bullet hit a Shield");
            int shieldDamage = 10; // Adjust the damage value as needed
            shielderEnemy.TakeShieldDamage(shieldDamage);
        }
        else
        {
            Debug.Log("Bullet hit an enemy without an active shield");
            int damage = 20; // Adjust the damage value as needed
            shielderEnemy.TakeDamage(damage);
        }
    }

    // Now, destroy the player's bullet regardless of shield status
    DestroyBullet();

         if (hitInfo.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
         {
              Destroy(hitInfo.gameObject); // Destroy the enemy bullet
              Destroy(gameObject); // Destroy the player's bullet
               return; // Exit the method to prevent further processing
         }

         DestroyBullet();

        Debug.Log("Bullet hit something.");
        
        EnemyFLY flyEnemy = hitInfo.GetComponent<EnemyFLY>();

        if (flyEnemy != null)
        {
        Debug.Log("Bullet hit a Fly enemy.");
        
        int bulletDamage = (int)10f; // Example damage value
        flyEnemy.TakeDamage(bulletDamage);
        }
        else
        {
        Debug.Log("Bullet did not hit a Fly enemy.");
        }

        DestroyBullet();

        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        EnemyMinigun enemyMinigun = hitInfo.GetComponent<EnemyMinigun>();

        if (enemyMinigun != null)
        {
            // Apply damage to the EnemyMinigun
            int minigunDamage = 20; // Adjust the damage value as needed
            enemyMinigun.TakeDamage(minigunDamage);
        }

        DestroyBullet();

        EnemyGrenade enemyGrenade = hitInfo.GetComponent<EnemyGrenade>();
         if (enemyGrenade != null)
            {
        Debug.Log("Bullet hit an EnemyGrenade.");
        
        int grenadeDamage = 15; // Adjust the damage value as needed
        enemyGrenade.TakeDamage(grenadeDamage);
        
        DestroyBullet();
           }

         EnemyMissile enemyMissile = hitInfo.GetComponent<EnemyMissile>();

         if (enemyMissile != null)
          {
            // Apply damage to the EnemyMissile
              int missileDamage = 20; // Adjust the damage value as needed
               enemyMissile.TakeDamage(missileDamage);
           }
        DestroyBullet();

    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
