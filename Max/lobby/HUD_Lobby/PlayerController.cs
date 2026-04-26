using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Static instance allows easy access from other scripts (e.g., LevelSelect, UI)
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
        // Singleton pattern: Ensure only one Player exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep player between scenes
        }
        else
        {
            Destroy(gameObject); // Remove duplicate players
            return;
        }

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
        TeleportToSpawnPoint();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called automatically when a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TeleportToSpawnPoint();
        sceneTransition(true);
    }

    // Finds the object with "Respawn" tag and moves the player there
    public void TeleportToSpawnPoint()
    {
        GameObject spawnPoint = GameObject.FindWithTag("Respawn");
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position;
            if (myRigidBody != null) myRigidBody.velocity = Vector2.zero;
        }
    }

    private void Update()
    {
        // Debug Cheat: Press 'G' for 100 gold
        if (Input.GetKeyDown(KeyCode.G))
        {
            addToLoot(100);
        }

        // Flashing effect when invincible
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
        if (canMove && myRigidBody != null)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            Vector2 direction = new Vector2(moveX, moveY).normalized;
            Vector2 targetPosition = myRigidBody.position + direction * moveSpeed * Time.fixedDeltaTime;
            myRigidBody.MovePosition(targetPosition);
        }
    }

    public void addToLoot(int amount)
    {
        wallet += Mathf.RoundToInt(amount * lootMultiplier);
    }

    public bool spendMoney(int amount)
    {
        if (wallet >= amount)
        {
            wallet -= amount;
            return true;
        }
        return false;
    }

    public void sceneTransition(bool visible)
    {
        canMove = visible;
        currentHealth = maxHealth;
        invincibilityCounter = 0;

        if (visible)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // --- GETTERS FOR EXTERNAL SCRIPTS (AgentMovement, EnemyHealth) ---

    public int getDamage() { return damage; }
    public int getX() { return (int)transform.position.x; }
    public int getY() { return (int)transform.position.y; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Loot"))
        {
            addToLoot(1);
            Destroy(other.gameObject);
        }
    }
}