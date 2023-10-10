using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFLY : MonoBehaviour
{
    public Transform bombDropPoint;
    public GameObject bombPrefab;
    public float bombForce = 5f;
    public LayerMask playerLayer;
    public float detectionRange = 10f;
    public float timeBetweenShots = 2f;
    public float patrolSpeed = 2f;
    public float patrolDistance = 5f;

    // Health-related variables
    public float maxHealth = 100f; // Adjust the initial health as needed
    private float currentHealth;

    private float shotTimer = 2f;
    private Transform playerTransform;
    private bool isFrozen = false;
    private bool isMovingRight = true;
    private Vector2 initialPosition;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        initialPosition = transform.position;
        currentHealth = maxHealth; // Initialize currentHealth to maxHealth
    }

    void Update()
    {
        if (isFrozen)
        {
            return;
        }

        Vector2 raycastOrigin = new Vector2(transform.position.x, transform.position.y - 1);
        Vector2 detectionSize = new Vector2(detectionRange * 0.2f, 0.1f);
        bool playerDetected = Physics2D.BoxCast(raycastOrigin, detectionSize, 0f, Vector2.down, detectionRange, playerLayer);

        if (playerDetected)
        {
            if (Time.time >= shotTimer)
            {
                DropBomb();
                shotTimer = Time.time + timeBetweenShots;
            }
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        Vector2 patrolDirection = isMovingRight ? Vector2.right : Vector2.left;
        Vector2 newPosition = (Vector2)transform.position + patrolDirection * patrolSpeed * Time.deltaTime;

        if (Mathf.Abs(newPosition.x - initialPosition.x) >= patrolDistance)
        {
            isMovingRight = !isMovingRight;
        }

        transform.position = newPosition;
    }

    void DropBomb()
    {
        Vector2 bombDirection = Vector2.down;
        GameObject bomb = Instantiate(bombPrefab, bombDropPoint.position, Quaternion.identity);
        Rigidbody2D bombRb = bomb.GetComponent<Rigidbody2D>();
        bombRb.velocity = bombDirection * bombForce;
    }

    // TakeDamage method to handle enemy taking damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Die method to handle enemy death
    void Die()
    {
        
        Destroy(gameObject);
    }

    public void FreezeEnemy()
    {
        isFrozen = true;
    }

    public void UnfreezeEnemy()
    {
        isFrozen = false;
    }
}
