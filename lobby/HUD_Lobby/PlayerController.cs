using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Static instance allows this script to be accessed easily and stay unique
    public static PlayerController instance;

    [Header("Movement Settings")]
    public float moveSpeed = 55f;
    public bool canMove = true;
    private Rigidbody2D myRigidBody;

    [Header("Health Settings")]
    public int maxHealth = 6;
    public int currentHealth;
    public float invincibilityLength = 1f;
    private float invincibilityCounter;
    private SpriteRenderer sr;

    [Header("Combat & Economy")]
    public int damage = 20;
    public int wallet = 0;
    public float lootMultiplier = 1f;

    private void Awake()
    {
        // --- SINGLETON & PERSISTENCE LOGIC ---
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Don't destroy player when loading Scene 2
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate players if returning to Lobby
            return;
        }

        // Initialize components
        myRigidBody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (myRigidBody != null)
        {
            myRigidBody.gravityScale = 0f;
            myRigidBody.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // --- DEBUG CHEAT: Press 'G' to add 100 gold ---
        if (Input.GetKeyDown(KeyCode.G))
        {
            addToLoot(100);
            Debug.Log("Cheat: +100 gold. Balance: " + wallet);
        }

        // Handle invincibility flashing
        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;
            sr.enabled = Mathf.Sin(Time.time * 25f) > 0f;
        }
        else
        {
            sr.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        // Physics-based movement using MovePosition
        if (canMove && myRigidBody != null)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            Vector2 direction = new Vector2(moveX, moveY).normalized;
            Vector2 targetPosition = myRigidBody.position + direction * moveSpeed * Time.fixedDeltaTime;
            myRigidBody.MovePosition(targetPosition);
        }
        else if (myRigidBody != null)
        {
            myRigidBody.velocity = Vector2.zero;
        }
    }

    // --- Economy & Upgrade Logic ---

    public void addToLoot(int amount)
    {
        int calculatedAmount = Mathf.RoundToInt(amount * lootMultiplier);
        wallet += calculatedAmount;
    }

    public bool spendMoney(int amount)
    {
        if (wallet >= amount)
        {
            wallet -= amount;
            Debug.Log("Spent: " + amount + " | Remaining: " + wallet);
            return true;
        }
        Debug.Log("Not enough gold!");
        return false;
    }

    // --- Scene & State Management ---

    public void sceneTransition(bool visible)
    {
        // You can call this from your Teleport script to reposition the player
        canMove = visible;
        currentHealth = maxHealth;
        invincibilityCounter = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Loot"))
        {
            addToLoot(1);
            Destroy(other.gameObject);
        }
    }

    // --- Getters for external scripts (AgentMovement) ---

    public int getDamage() { return damage; }
    public int getX() { return (int)transform.position.x; }
    public int getY() { return (int)transform.position.y; }
}