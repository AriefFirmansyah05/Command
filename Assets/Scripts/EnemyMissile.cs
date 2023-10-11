using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    public GameObject homingMissilePrefab;
    public Transform leftFirePoint;
    public Transform rightFirePoint;
    public float moveSpeed = 2f;
    public float patrolDistance = 5f;
    public float fireRate = 2f;
    public int maxHealth = 100;
    public float attackRadius = 10f; // The radius within which the enemy will attack.
    private int currentHealth;

    private Transform player;
    private float nextFireTime;
    private bool isMovingRight = true;
    private Vector3 initialPosition;
    private Vector3 attackPosition;

    public GameObject scoreDisplayPrefab;
    private GameObject scoreDisplayObject;
    private float displayDuration = 2.0f; // Adjust this to the desired duration in seconds
    private float timer = 0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nextFireTime = Time.time;
        currentHealth = maxHealth;
        initialPosition = transform.position;
        attackPosition = rightFirePoint.position; // Start with the right fire point
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRadius)
        {
            if (Time.time >= nextFireTime)
            {
                FireHomingMissile(isMovingRight); // Pass the direction to FireHomingMissile
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

        Patrol();
    }

    private void Patrol()
    {
        Vector2 patrolDirection = isMovingRight ? Vector2.right : Vector2.left;
        Vector2 newPosition = (Vector2)transform.position + patrolDirection * moveSpeed * Time.deltaTime;

        if (Mathf.Abs(newPosition.x - initialPosition.x) >= patrolDistance)
        {
            isMovingRight = !isMovingRight;

            // Update the attack position when changing direction
            if (isMovingRight)
            {
                attackPosition = rightFirePoint.position; // Use the right fire point
            }
            else
            {
                attackPosition = leftFirePoint.position; // Use the left fire point
            }
        }

        transform.position = newPosition;
    }

    private void FireHomingMissile(bool moveRight)
    {
        if (homingMissilePrefab != null)
        {
            Transform currentFirePoint = moveRight ? rightFirePoint : leftFirePoint;

            Quaternion rotation = Quaternion.Euler(0, 0, moveRight ? 0f : 180f);

            // Fire from the appropriate fire point
            GameObject missile = Instantiate(homingMissilePrefab, currentFirePoint.position, rotation);

            // Set the missile's initial velocity to match the enemy's direction
            Rigidbody2D missileRb = missile.GetComponent<Rigidbody2D>();
            missileRb.velocity = moveRight ? currentFirePoint.right : -currentFirePoint.right;
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

    private void Die()
    {
        ScoreManager.instance.AddScore(10);

        GameObject scoreDisplayObject = Instantiate(scoreDisplayPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
        
        Destroy(gameObject);
    }
}
