using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrenade : MonoBehaviour
{
    public GameObject grenadePrefab;
    public Transform leftFirePoint;
    public Transform rightFirePoint;
    public float throwForce = 10f;
    public float throwInterval = 5f;
    private float nextThrowTime;
    private Transform playerTransform;

    public int maxHealth = 100;
    private int currentHealth;
    public float detectionRange = 10f;
    private bool playerDetected = false;

    public GameObject scoreDisplayPrefab;
    private GameObject scoreDisplayObject;
    private float displayDuration = 2.0f; // Adjust this to the desired duration in seconds
    private float timer = 0f;

    private void Start()
    {
        currentHealth = maxHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (playerTransform == null)
        {
            Debug.LogError("Player not found!");
        }

        // Initialize the next throw time
        nextThrowTime = Time.time + throwInterval;
    }

    private void Update()
    {
        // Calculate the distance between the player and the enemy
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // Check if the player is within the detection range and below the enemy
        if (distanceToPlayer <= detectionRange && playerTransform.position.y < transform.position.y)
        {
            playerDetected = true;

            // Determine whether the player is to the left or right of the enemy
            if (playerTransform.position.x < transform.position.x)
            {
                // Player is on the left, use the left firepoint for throwing grenades
                TryThrowGrenade(leftFirePoint);
            }
            else
            {
                // Player is on the right, use the right firepoint for throwing grenades
                TryThrowGrenade(rightFirePoint);
            }
        }
        else
        {
            playerDetected = false;
        }

        if (scoreDisplayObject != null)
        {
            if (timer >= displayDuration)
            {
                // Destroy the score display object
                Destroy(scoreDisplayObject);
                scoreDisplayObject = null; // Set it to null to avoid issues

                // Reset the timer
                timer = 0f;
            }

            // Increment the timer
            timer += Time.deltaTime;
        }

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        ScoreManager.instance.AddScore(10);

    // Instantiate the ScoreDisplayAboveEnemy prefab
        GameObject scoreDisplayObject = Instantiate(scoreDisplayPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);

        Destroy(gameObject);
    }

    private void TryThrowGrenade(Transform firePoint)
    {
        // Check if it's time to throw a grenade
        if (Time.time >= nextThrowTime)
        {
            ThrowGrenade(firePoint);
            // Set the next throw time after the cooldown period
            nextThrowTime = Time.time + throwInterval;
        }
    }

    private void ThrowGrenade(Transform firePoint)
    {
        if (grenadePrefab != null && firePoint != null && playerTransform != null)
        {
            // Calculate the throw direction based on the fire point's position
            Vector2 throwDirection = (firePoint.position - transform.position).normalized;

            GameObject grenade = Instantiate(grenadePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D grenadeRb = grenade.GetComponent<Rigidbody2D>();
            grenadeRb.velocity = throwDirection * throwForce;
            grenadeRb.gravityScale = 1.0f;
        }
    }
}
