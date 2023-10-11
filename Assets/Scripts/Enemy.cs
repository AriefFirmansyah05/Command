using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public GameObject deathEffect;
    public GameObject bullet;
    public Transform bulletPos;

    private GameObject player;
    private float timer;

    public GameObject scoreDisplayPrefab;
    private GameObject scoreDisplayObject;
    private float displayDuration = 2.0f; // Adjust this to the desired duration in seconds

    // Reference to the Win UI prefab
    public GameObject winUIPrefab;
    private GameObject winUIObject;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < 10)
        {
            timer += Time.deltaTime;

            if (timer > 2)
            {
                timer = 0;
                Shoot();
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

    void Shoot()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        ScoreManager.instance.AddScore(10);

        GameObject scoreDisplayObject = Instantiate(scoreDisplayPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);

        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);

        // Show the Win UI when the enemy dies
        if (winUIPrefab != null)
        {
            winUIObject = Instantiate(winUIPrefab);
        }
    }
}
