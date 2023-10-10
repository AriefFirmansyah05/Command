using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowGrenade : MonoBehaviour
{
    public float throwForce = 10.0f;
    public float explosionRadius = 1f;
    public GameObject explosionEffect;
    public float explosionForce = 5.0f;
    public int explosionDamage = 10;
    public float explosionDuration = 0.5f;
    public float gravityScale = 2.0f;
    public float minDistanceToStartUpwardMovement = 1.0f;
    public float upwardForceDuration = 0.5f;

    private bool hasExploded = false;
    private bool isGoingUp = true;
    private Rigidbody2D rb;
    private Vector2 throwDirection;
    private Vector3 playerPosition;
    private float timer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerPosition = playerObject.transform.position;
        }
    }

    void Start()
    {
        UpdateThrowDirection();
        rb.velocity = throwDirection * throwForce;
        rb.gravityScale = gravityScale;
    }

    void Update()
    {
        if (hasExploded)
        {
            return;
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerPosition = playerObject.transform.position;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerPosition);

        if (isGoingUp && (timer >= upwardForceDuration || distanceToPlayer <= minDistanceToStartUpwardMovement))
        {
            isGoingUp = false;
        }

        if (isGoingUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, throwForce);
            timer += Time.deltaTime;
        }

        UpdateThrowDirection();
    }

    void UpdateThrowDirection()
    {
        throwDirection = (playerPosition - transform.position).normalized;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hasExploded)
            return;

        if (hitInfo.CompareTag("Player"))
        {
            PlayerMovement player = hitInfo.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(explosionDamage);
            }
            Destroy(gameObject);
        }
        else if (hitInfo.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        Explode();
    }

    void Explode()
    {
        Debug.Log("Explosion Occurred");
        GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
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

        Destroy(explosion, explosionDuration);
        hasExploded = true;
    }
}
