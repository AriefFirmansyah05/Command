using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float initialStraightTime = 2f; // Time the missile goes straight initially.
    public float homingDuration = 5f; // Time to follow the player after initial straight movement.
    public float straightDuration = 2f; // Time to move straight before exploding.
    public float homingSpeed = 5f; // Speed when homing towards the player.
    public float straightSpeed = 10f; // Speed when moving straight.
    public float explosionRadius = 3f; // Radius of the explosion.
    public float explosionForce = 10f; // Force applied to objects within the explosion.
    public float explosionDamage = 20f; // Damage caused by the explosion.
    public float explosionDuration = 0.5f; // Duration of the explosion effect.

    public GameObject explosionEffect; // Reference to the explosion effect prefab.

    private Transform target;
    private Rigidbody2D rb;
    private float timer = 0f;
    private float homingTimer = 0f;
    private float straightTimer = 0f;
    private bool isHoming = false;
    private bool hasExploded = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Find the player GameObject based on its tag "Player"
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // Set initial velocity to the left.
        rb.velocity = -transform.right * straightSpeed;
    }

    private void Update()
    {
        if (target == null)
        {
            // If the player is destroyed or not found, do something (e.g., explode).
            Destroy(gameObject);
            return;
        }

        timer += Time.deltaTime;

        if (!isHoming)
        {
            // Missile is in initial straight movement.
            if (timer >= initialStraightTime)
            {
                // Switch to homing mode after the initial straight movement.
                isHoming = true;
                rb.velocity = Vector2.zero; // Stop the initial straight movement.
            }
        }
        else
        {
            // Missile is in homing mode.
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * homingSpeed;

            // Calculate the angle to rotate the missile towards the player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            homingTimer += Time.deltaTime;

            if (homingTimer >= homingDuration)
            {
                // Explode after homing duration.
                Explode();
            }
        }

        if (!isHoming && !hasExploded)
        {
            // Missile is in straight mode.
            straightTimer += Time.deltaTime;

            if (straightTimer >= straightDuration)
            {
                // Explode or perform any other desired action after straight duration.
                Explode();
            }
        }
    }

   private void OnTriggerEnter2D(Collider2D other)
{
    if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
    {
        // Handle collision with the player (e.g., deal damage).
        Explode();
    }
    else if (LayerMask.LayerToName(other.gameObject.layer) == "Wall" || LayerMask.LayerToName(other.gameObject.layer) == "PlayerBullet")
    {
        // Handle collision with a wall or player bullet by exploding.
        Explode();
    }
}


    private void Explode()
    {
        if (hasExploded)
            return;

        // Create an explosion effect
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
int damage = Mathf.RoundToInt(explosionDamage);

Enemy enemy = collider.GetComponent<Enemy>();
if (enemy != null)
{
    enemy.TakeDamage(damage);
}

PlayerMovement player = collider.GetComponent<PlayerMovement>();
if (player != null)
{
    player.TakeDamage(damage);
}
        }

        // Destroy the explosion effect and the missile
        Destroy(explosion, explosionDuration);
        Destroy(gameObject);
        hasExploded = true;
    }
}
