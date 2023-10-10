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
        Destroy(gameObject);
    }
}
