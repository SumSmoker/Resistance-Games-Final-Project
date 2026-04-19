using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 2f;
    public float aggroRadius = 5f;

    private bool isAggroed = false;

    private Transform playerTarget;
    private Rigidbody2D rb;

    // Reference to our new universal Knockback script
    private Knockback knockbackScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        knockbackScript = GetComponent<Knockback>(); // Find the script on startup

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerTarget = playerObject.transform;
        }
    }

    void FixedUpdate()
    {
        // MOST IMPORTANT: If the enemy is currently knocked back or stunned - PAUSE AI MOVEMENT
        if (knockbackScript != null && knockbackScript.isGettingKnockedBack)
        {
            return; // Exit the function, don't allow walking
        }

        // --- Normal walking logic ---
        if (playerTarget != null)
        {
            // Check if player is within aggro range
            if (!isAggroed)
            {
                float distance = Vector2.Distance(transform.position, playerTarget.position);
                if (distance <= aggroRadius)
                {
                    isAggroed = true;
                }
            }

            // Chase the player if aggroed
            if (isAggroed)
            {
                Vector2 direction = (playerTarget.position - transform.position).normalized;
                rb.velocity = direction * speed;
            }
            else
            {
                rb.velocity = Vector2.zero; // Stand still
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw aggro radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
    }
}