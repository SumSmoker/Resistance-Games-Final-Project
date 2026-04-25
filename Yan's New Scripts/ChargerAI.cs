using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))] // Автоматично додає LineRenderer, якщо його немає
public class ChargerAI : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 2.5f;
    [SerializeField] private float dashSpeed = 18f; // Made slightly faster since it's now dodgeable
    [SerializeField] private float aggroRadius = 10f;
    [SerializeField] private float dashActivationRange = 6f; // Slightly increased range for better telegraphing
    [SerializeField] private float dashDuration = 0.4f; // How long it flies

    [Header("Timing Settings")]
    [SerializeField] private float chargeUpTime = 1f; // Player needs time to react to the line
    [SerializeField] private float postDashStun = 1.2f;
    [SerializeField] private float dashCooldown = 4f; // Increased cooldown to give player breathing room

    private Rigidbody2D rb;
    private Transform player;
    private Knockback knockbackScript;
    private LineRenderer lineRenderer;

    private bool isAggroed = false;
    private bool isBusy = false;
    private float cooldownTimer = 0f;

    private Vector2 fixedDashDirection; // Direction locked at start of charge up

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        knockbackScript = GetComponent<Knockback>();
        lineRenderer = GetComponent<LineRenderer>();

        // Ensure line renderer is hidden at start
        lineRenderer.enabled = false;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;

        // If charging up, constantly update the line to match the locked target position
        if (isBusy && !rb.simulated) // Using rb.simulated check to know if we are in ChargeUp phase
        {
            DrawTelegraphLine();
        }
    }

    void FixedUpdate()
    {
        if (knockbackScript != null && knockbackScript.isGettingKnockedBack) return;

        // isBusy handles chargeUp, dash, and stun phases
        if (isBusy || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (!isAggroed)
        {
            if (distanceToPlayer <= aggroRadius) isAggroed = true;
            else rb.velocity = Vector2.zero;
            return;
        }

        if (distanceToPlayer <= dashActivationRange && cooldownTimer <= 0)
        {
            StartCoroutine(PerformDashSequence());
        }
        else
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * walkSpeed;
        }
    }

    private IEnumerator PerformDashSequence()
    {
        isBusy = true;
        rb.velocity = Vector2.zero;

        // --- 1. PHASE: Lock Target & Telegraph ---

        // FIX DIRECTION HERE: We calculate direction exactly when preparation starts
        fixedDashDirection = (player.position - transform.position).normalized;

        // Fallback if player is exactly on top of enemy
        if (fixedDashDirection == Vector2.zero) fixedDashDirection = transform.right;

        // Show and setup the line
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        rb.simulated = false; // Temporarily disable physics to keep line update logic working cleanly in Update()

        yield return new WaitForSeconds(chargeUpTime);

        // --- 2. PHASE: Dash ---
        lineRenderer.enabled = false; // Hide line right before dash
        rb.simulated = true; // Re-enable physics

        // Fly in the PREVIOUSLY LOCKED direction
        rb.velocity = fixedDashDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        // --- 3. PHASE: Post-dash stun ---
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(postDashStun);

        // --- 4. Cleanup ---
        isBusy = false;
        cooldownTimer = dashCooldown;
    }

    private void DrawTelegraphLine()
    {
        // Start of line is enemy center
        lineRenderer.SetPosition(0, transform.position);

        // End of line is far away in the locked direction
        // We use a large number (50) to make sure it goes through the player and off-screen/to a wall
        Vector3 lineEndPosition = (Vector2)transform.position + (fixedDashDirection * 50f);
        lineRenderer.SetPosition(1, lineEndPosition);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dashActivationRange);
    }
}