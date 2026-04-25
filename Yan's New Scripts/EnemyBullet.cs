using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float speed = 10f;       // Travel speed of the bullet
    [SerializeField] private int damageAmount = 10;   // Damage dealt to the player
    [SerializeField] private float lifeTime = 3f;     // Time before the bullet destroys itself (prevents memory leaks)

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Make the bullet fly straight in the direction it was spawned facing
        rb.velocity = transform.right * speed;

        // Auto-destroy the bullet after 'lifeTime' seconds if it hits absolutely nothing
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. If the bullet hits the player
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // Attempt to deal damage. If successful, apply knockback to the player
                if (player.Damage(damageAmount))
                {
                    player.ApplyKnockback(transform.position);
                }
            }
            Destroy(gameObject); // Destroy the bullet after hitting the player
        }

        // 2. If the bullet hits a solid object (like a wall or floor)
        // IMPORTANT FIX: We ignore triggers AND ignore other objects tagged as "Enemy" 
        // to prevent the bullet from destroying itself inside the Gunner's own collider.
        else if (!other.isTrigger && !other.CompareTag("Enemy"))
        {
            Destroy(gameObject); // Destroy the bullet upon hitting the environment
        }
    }
}