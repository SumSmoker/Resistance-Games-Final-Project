using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Stats")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    private LevelManager theLevelManager;
    private float hitCooldown = 0f;

    void Start()
    {
        currentHealth = maxHealth;
        theLevelManager = FindObjectOfType<LevelManager>();
    }

    void Update()
    {
        // Decrease invulnerability timer
        if (hitCooldown > 0) hitCooldown -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // RECEIVING DAMAGE ONLY
        if (other.CompareTag("playerHitBox") && hitCooldown <= 0f)
        {
            // Find player to get damage amount
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

            if (player != null)
            {
                TakeDamage(player.getDamage(), other.transform.position);
            }
        }
    }

    // Function to handle taking damage and dying
    public void TakeDamage(int damageAmount, Vector2 damageSource)
    {
        currentHealth -= damageAmount;
        hitCooldown = 0.3f; // I-frames

        // Trigger knockback
        Knockback kb = GetComponent<Knockback>();
        if (kb != null) kb.ApplyKnockback(damageSource);

        // Check for death
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        if (theLevelManager != null) theLevelManager.Addkills(1);
        Destroy(gameObject);
    }
}