using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyMinigun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    public float detectionRange = 10f;
    public int maxHealth = 100; // Maximum health for the enemy
    private int currentHealth; // Current health for the enemy
    private float nextFireTime;

    private Transform player;
    public GameObject scoreDisplayPrefab;
    private GameObject scoreDisplayObject;

    private float displayDuration = 2.0f; // Adjust this to the desired duration in seconds
    private float timer = 0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nextFireTime = Time.time;
        currentHealth = maxHealth; // Initialize the current health to max health
    }

    private void Update()
    {
        // Calculate the direction to the player
        Vector3 directionToPlayer = player.position - transform.position;

        // Check if the player is within the detection range and in front of the enemy
        if (directionToPlayer.magnitude <= detectionRange &&
            Vector3.Dot(transform.up, directionToPlayer.normalized) > 0)
        {
            // Player is in range and in front of the enemy, so fire bullets
            if (Time.time >= nextFireTime)
            {
                FireBullet();
                nextFireTime = Time.time + 1 / fireRate;
            }
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

    private void FireBullet()
{
    if (bulletPrefab != null && firePoint != null)
    {
        // Calculate the direction to the player
        Vector3 directionToPlayer = player.position - firePoint.position;

        // Create a bullet facing the direction of the player
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(directionToPlayer));
        BurstMode enemyBullet = bullet.GetComponent<BurstMode>();

        if (enemyBullet != null)
        {
            // Track the number of shots fired in the enemy bullet script
            enemyBullet.IncrementShotsFired();
        }
    }
}


    // Method to handle taking damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy's Current Health: " + currentHealth); // Add this line

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to handle the enemy's death
    private void Die()
{
    // Update the player's score when the enemy dies
    ScoreManager.instance.AddScore(10);

    // Instantiate the ScoreDisplayAboveEnemy prefab
    GameObject scoreDisplayObject = Instantiate(scoreDisplayPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);

    Destroy(gameObject); // Destroy the enemy GameObject

}

}
