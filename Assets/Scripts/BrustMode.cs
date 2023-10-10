using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstMode : MonoBehaviour
{
    public float initialForce = 5f; // Initial bullet force.
    public float fasterForce = 10f; // Faster bullet force after a certain number of shots.
    public int shotsBeforeFaster = 3; // Number of shots before bullets become faster.

    private GameObject player;
    private Rigidbody2D rb;
    private float timer;
    private bool hasHitPlayer = false;
    private int shotsFired = 0;

    private bool isFaster = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * initialForce;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);

        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 10)
        {
            Destroy(gameObject);
        }

        if (!isFaster && shotsFired >= shotsBeforeFaster)
        {
            // Enable faster bullets after a certain number of shots
            isFaster = true;
            rb.velocity = rb.velocity.normalized * fasterForce;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Bullet hit something.");

        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject); // Destroy the enemy bullet when it collides with a wall
        }
        else if (other.CompareTag("PlayerBullet"))
        {
            Destroy(other.gameObject); // Destroy the player's bullet
            Destroy(gameObject); // Destroy the enemy bullet when colliding with a player's bullet
        }

        if (!hasHitPlayer && other.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.gameObject.GetComponent<PlayerMovement>();

            if (playerMovement != null && playerMovement.IsAlive())
            {
                playerMovement.health -= 20;

                if (playerMovement.health <= 0)
                {
                    Destroy(other.gameObject);
                }

                hasHitPlayer = true;

                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        // Check if the bullet should speed up after a certain number of shots.
        if (shotsFired >= shotsBeforeFaster)
        {
            rb.velocity = rb.velocity.normalized * fasterForce;
        }
    }

    // Function to track the number of shots fired.
    public void IncrementShotsFired()
    {
        shotsFired++;
    }
}
