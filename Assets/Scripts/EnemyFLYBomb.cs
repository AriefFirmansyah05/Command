using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyBomb : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 20;
    public float explosionRadius = 1f;
    public GameObject explosionEffect;
    public float explosionForce = 5.0f;
    public int explosionDamage = 10;
    public float explosionDuration = 0.5f;

    private bool hasExploded = false; // To track if the bomb has exploded
    private Rigidbody2D rb;
    public float initialVelocity = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.down * initialVelocity; // Adjust as needed
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Check if the bomb has already exploded, if so, return.
        if (hasExploded)
            return;

        // Check if the bullet hits an enemy
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Check if the bullet hits the player
        if (hitInfo.CompareTag("Player"))
        {
            PlayerMovement player = hitInfo.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject); // Destroy the bomb when it hits the player
        }
        else if (hitInfo.CompareTag("Wall"))
        {
            // Destroy the bomb when it hits a wall
            Destroy(gameObject);
        }

        // Create an explosion effect and mark the bomb as exploded
        Explode();
    }

    void Explode()
    {
        Debug.Log("Explosion Occurred");
        // Instantiate the explosion effect
        GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Find all colliders within the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (collider.transform.position - transform.position).normalized;
                rb.AddForce(direction * explosionForce, ForceMode2D.Impulse);
            }

            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(explosionDamage);
            }

            PlayerMovement player = collider.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(explosionDamage);
            }
        }

        // Destroy the explosion effect and the bomb itself
        Destroy(explosion, explosionDuration);
        hasExploded = true;
    }
}
