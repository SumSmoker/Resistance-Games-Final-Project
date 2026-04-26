using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using static Unity.VisualScripting.Member;

public class PlayerController : MonoBehaviour
{
    // Basic values
    public float moveSpeed;
    private Rigidbody2D myRigidBody;
    public bool canMove;
    public float delay = 0.5f;
    public int maxHealth;
    public int currentHealth;
    [SerializeField]
    private int startCoordinateX;
    [SerializeField]
    private int startCoordinateY;

    private LevelManager theLevelManager;

    // Knockback values
    public float knockback; // Power
    public float knockbackLength; // How long we get pushed back
    public float knockbackCount;
    public bool knockFromRight;
    private Vector2 knockbackDirection; // NEW: stores the direction of the knockback

    // Animation values
    public Animator anim;
    private bool isMoving;
    public bool isAttacking;
    private float lastMoveX;
    private float lastMoveY;

    [SerializeField] // For attack animations
    private GameObject hitBox;

    // Variables for invincibility (I-frames)
    public float invincibilityLength = 1f; // How long the invincibility lasts
    public float invincibilityCounter; // Current timer value
    private SpriteRenderer sr; // Flashing animation for invincibility

    // Variables to store base stats
    private float baseSpeed;
    private int baseDamage;
    public int attackDamage = 10;
    public int coins = 0;

    // Add the Singleton instance and loot multiplier from Max's code
    public static PlayerController instance;
    public float lootMultiplier = 1f;

    void Start()
    {
        baseSpeed = moveSpeed; // Store your initial speed here
        baseDamage = attackDamage; // Store your initial damage here
        myRigidBody = GetComponent<Rigidbody2D>();
        theLevelManager = FindObjectOfType<LevelManager>();
        anim = GetComponent<Animator>();

        // Get the SpriteRenderer component to control visibility for flashing
        sr = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth; // Make sure to set health back to full

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            canMove = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            canMove = true;
            GetComponent<SpriteRenderer>().enabled = true;
            transform.position = new Vector2(startCoordinateX, startCoordinateY); // Sometimes necessary for going into new scenes
        }
    }

    private void Update()
    {
        // --- UPDATED VISUAL LOGIC ---
        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;

            // Simple flashing: Toggle sprite visibility rapidly based on a sine wave over time
            if (Mathf.Sin(Time.time * 25f) > 0f)
            {
                sr.enabled = true; // Show sprite
            }
            else
            {
                sr.enabled = false; // Hide sprite
            }
        }
        else
        {
            // Ensure sprite is definitely visible when invincibility ends
            sr.enabled = true;
        }

        Debug.Log(Input.GetAxisRaw("Horizontal"));
        Debug.Log(Input.GetAxisRaw("Vertical"));
        Animate(); // Call this to make sure animations are correct

        if (Input.GetButtonDown("Jump") && canMove) // I can rename "jump," that's just the default spacebar command. Pressing it here results in the attack animation
        {
            isAttacking = true;
            myRigidBody.velocity = new Vector2(0, 0); // Stop movement/velocity or whatever
            StartCoroutine(AttackDelay(delay)); // Halt other things until attack is finished
        }
    }

    private void FixedUpdate()
    {
        // This system allows 8-directional movement
        // NEW: if knockback is active, temporarily override normal movement
        if (knockbackCount > 0)
        {
            myRigidBody.velocity = knockbackDirection * knockback;
            knockbackCount -= Time.fixedDeltaTime;
        }
        else if (canMove)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                myRigidBody.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, 0);
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                {
                    myRigidBody.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);
                }
                lastMoveX = Input.GetAxisRaw("Horizontal");
                lastMoveY = Input.GetAxisRaw("Vertical");
            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                myRigidBody.velocity = new Vector2(0, Input.GetAxis("Vertical") * moveSpeed);
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    myRigidBody.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);
                }
                lastMoveX = Input.GetAxisRaw("Horizontal");
                lastMoveY = Input.GetAxisRaw("Vertical");
            }
            else
            {
                myRigidBody.velocity = new Vector2(0, 0);
            }
        }
        else
        {
            myRigidBody.velocity = new Vector2(0, 0);
        }

        // Health constraints
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Don't let health go over full; mostly necessary if there are healing items
        }
        if (currentHealth <= 0)
        {
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload scene. This is commented out because in this project the scene isn't fully reset
            currentHealth = maxHealth; // Resets player health; needs to be called because the scene is reloading and Start() isn't being called again
            transform.position = new Vector2(startCoordinateX, startCoordinateY); // Go back to start position
        }
    }

    // Mid-attack controls
    IEnumerator AttackDelay(float delay)
    {
        canMove = false; // Make sure player can't move
        yield return new WaitForSeconds(delay); // Wait; "delay" is the variable, can be changed depending on length of animation
        canMove = true;
        isAttacking = false;
    }

    // Animation system
    void Animate()
    {
        if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && canMove) // If the movement key(s) is/are being pressed and canMove
        {
            isMoving = true;
        }
        else // Just to make sure
        {
            isMoving = false;
        }

        if (isMoving)
        {
            anim.SetFloat("X", Input.GetAxisRaw("Horizontal")); // These values go to the animator, which has logic that determines which animation to play...
            anim.SetFloat("Y", Input.GetAxisRaw("Vertical")); // ...depending on the x and y values of the input
        }

        if (isAttacking)
        {
            anim.SetFloat("lastMoveX", lastMoveX); // ...lastMoveX/Y determine which way the player was facing before the attack key is pressed
            anim.SetFloat("lastMoveY", lastMoveY); // Once again, the animator tool in Unity takes care of the logic once the values are passed
        }

        // Send values to the animator to satisfy the conditions for playing animations
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("isMoving", isMoving);
    }

    // For the damage script
    public bool Damage(int dmg)
    {
        // --- ADD THIS CONDITION ---
        // Check if the player is currently vulnerable
        if (invincibilityCounter <= 0)
        {
            currentHealth -= dmg; // Simple

            // --- START THE TIMER ---
            // Reset the invincibility timer (Start I-frames)
            invincibilityCounter = invincibilityLength;
            return true; // NEW: damage was applied successfully
        }

        return false; // NEW: player is still invincible, so no damage was taken
    }

    // NEW: applies knockback away from the source of damage
    public void ApplyKnockback(Vector2 sourcePosition)
    {
        knockbackDirection = ((Vector2)transform.position - sourcePosition).normalized;

        // NEW: safety check in case positions overlap exactly
        if (knockbackDirection == Vector2.zero)
        {
            knockbackDirection = Vector2.up;
        }

        knockbackCount = knockbackLength;
    }

    public bool getAttacking()
    {
        return isAttacking;
    }

    public int getDamage()
    {
        return attackDamage;
    }

    public int getX()
    {
        return (int)transform.position.x;
    }

    public int getY()
    {
        return (int)transform.position.y;
    }
    // Called by Collectible.cs when a coin is picked up
    public void AddCoins(int amount)
    {
        // Now we apply the loot multiplier (for the upgrades)
        coins += Mathf.RoundToInt(amount * lootMultiplier);
        Debug.Log("Coins updated! Total: " + coins);

        if (theLevelManager != null)
        {
            theLevelManager.AddLoot(Mathf.RoundToInt(amount * lootMultiplier));
        }
    }

    // This function is required by Max's scripts (LevelSelect and UpgradeManager) to spend money
    public bool spendMoney(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            return true; // Money successfully spent
        }
        return false; // Not enough money
    }

    public void Heal(int healAmount)
    {
        // Assuming you have currentHealth and maxHealth variables
        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        Debug.Log("Healed! Current HP: " + currentHealth);
    }

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        StartCoroutine(SpeedBoostRoutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        moveSpeed = baseSpeed * multiplier; // Increase speed (e.g., x1.5)
        Debug.Log("Speed Boost Active!");

        yield return new WaitForSeconds(duration);

        moveSpeed = baseSpeed; // Revert to original speed
        Debug.Log("Speed Boost Ended!");
    }

    public void ApplyDamageBoost(int extraDamage, float duration)
    {
        StartCoroutine(DamageBoostRoutine(extraDamage, duration));
    }

    private IEnumerator DamageBoostRoutine(int extraDamage, float duration)
    {
        attackDamage = baseDamage + extraDamage; // Add bonus damage
        Debug.Log("Damage Boost Active! Attack: " + attackDamage);

        yield return new WaitForSeconds(duration); // Wait for the buff to expire

        attackDamage = baseDamage; // Revert to original damage
        Debug.Log("Damage Boost Ended! Attack: " + attackDamage);
    }
    private void Awake()
    {
        // Make the player a Singleton so Max's scripts can find it easily
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the player between scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        myRigidBody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
}