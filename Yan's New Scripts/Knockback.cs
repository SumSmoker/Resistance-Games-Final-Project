using System.Collections;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [Header("Knockback Settings")]
    public float knockbackForce = 10f; // Force applied when knocked back
    public float knockbackTime = 0.2f; // Duration of the knockback flight
    public float stunTime = 0.5f;      // Duration the enemy stands still after flying

    private Rigidbody2D rb;

    // This flag tells other scripts (like AI) that the enemy is currently incapacitated
    public bool isGettingKnockedBack { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // This function is called by the Damage script
    public void ApplyKnockback(Vector2 damageSource)
    {
        // Stop any previous knockback if hit multiple times in a row
        StopAllCoroutines();
        StartCoroutine(KnockbackRoutine(damageSource));
    }

    // Coroutine to handle the knockback timer sequence
    private IEnumerator KnockbackRoutine(Vector2 damageSource)
    {
        isGettingKnockedBack = true; // Tell the enemy's brain: "Stop, I'm flying!"

        // Calculate flight direction away from the damage source
        Vector2 knockbackDirection = ((Vector2)transform.position - damageSource).normalized;
        if (knockbackDirection == Vector2.zero) knockbackDirection = Vector2.up;

        // 1. Apply hard push
        rb.velocity = knockbackDirection * knockbackForce;

        // Wait until the flight duration is over
        yield return new WaitForSeconds(knockbackTime);

        // 2. Stun phase (stop the enemy completely)
        rb.velocity = Vector2.zero;

        // Wait until the stun duration is over
        yield return new WaitForSeconds(stunTime);

        // 3. Release the enemy
        isGettingKnockedBack = false; // Tell the brain: "I've recovered, you can move now"
    }
}