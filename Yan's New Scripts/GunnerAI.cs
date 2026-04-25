using UnityEngine;

public class GunnerAI : MonoBehaviour
{
    [Header("Distances")]
    [SerializeField] private float aggroRadius = 12f;     // Distance to spot player
    [SerializeField] private float shootingRange = 7f;    // Distance to stop and shoot
    [SerializeField] private float retreatRange = 4f;     // Distance to run away

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float retreatSpeed = 2.5f;

    [Header("Combat")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;         // Where the bullet spawns
    [SerializeField] private float fireRate = 1.5f;       // Time between shots

    private Rigidbody2D rb;
    private Transform player;
    private Knockback knockbackScript;

    private bool isAggroed = false;
    private float nextFireTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        knockbackScript = GetComponent<Knockback>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void FixedUpdate()
    {
        // Pause AI if knocked back
        if (knockbackScript != null && knockbackScript.isGettingKnockedBack) return;
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Aggro logic
        if (!isAggroed)
        {
            if (distanceToPlayer <= aggroRadius) isAggroed = true;
            else rb.velocity = Vector2.zero;
            return;
        }

        // Aiming the firePoint at the player
        AimAtPlayer();

        // --- Distance Logic ---
        if (distanceToPlayer > shootingRange)
        {
            // Player is too far -> Move closer
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
        else if (distanceToPlayer < retreatRange)
        {
            // Player is too close -> Run away
            Vector2 direction = (transform.position - player.position).normalized;
            rb.velocity = direction * retreatSpeed;
        }
        else
        {
            // Player is in the "Sweet Spot" -> Stop and shoot
            rb.velocity = Vector2.zero;

            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    private void AimAtPlayer()
    {
        if (firePoint != null)
        {
            Vector2 direction = player.position - firePoint.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualizing the ranges in the Unity Editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, retreatRange);
    }
}