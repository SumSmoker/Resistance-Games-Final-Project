using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using static Unity.VisualScripting.Member;

public class PlayerController : MonoBehaviour
{
    //basic values
    public float moveSpeed;
    private Rigidbody2D myRigidBody;
    private BoxCollider2D myBoxCollider2D;
    public bool canMove;
    public float delay = 0.5f;
    public int maxHealth;
    public int currentHealth;
    public int damage;
    [SerializeField]
    private int startCoordinateX;
    [SerializeField]
    private int startCoordinateY;

    private LevelManager theLevelManager;

    //knockback values
    public float knockback; //power
    public float knockbackLength; //how long we get pushed back
    public float knockbackCount;
    public bool knockFromRight;
    private Vector2 knockbackDirection; //NEW: stores the direction of the knockback

    //animation values
    public Animator anim;
    private bool isMoving;
    public bool isAttacking;
    private float lastMoveX;
    private float lastMoveY;

    //loot values
    public int lootValue;
    [SerializeField]
    private int wallet;

    [SerializeField] //for attack animations
    private GameObject hitBox;

    // Variables for invincibility (I-frames)
    public float invincibilityLength = 1f; // How long the invincibility lasts
    public float invincibilityCounter; // Current timer value
    private SpriteRenderer sr; // Flashing animation for invincibility

    void Start()
    {

    }

    private void OnEnable()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component to control visibility for flashing
        currentHealth = maxHealth; //make sure to set health back to full
    }

    private void Update()
    {
        if(theLevelManager == null)
        {
            theLevelManager = FindObjectOfType<LevelManager>();
        }

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
        Animate(); //call this to make sure animations are correct

        if (Input.GetButtonDown("Jump") && canMove) //I can rename "jump," that's just the default spacebar command. Pressing it here results in the attack animation
        {
            isAttacking = true;
            myRigidBody.velocity = new Vector2(0, 0); //stop movement/velocity or whatever
            StartCoroutine(AttackDelay(delay)); //halt other things until attack is finished
        }
    }

    private void FixedUpdate()
    {
        //NEW: if knockback is active, temporarily override normal movement
        if (knockbackCount > 0)
        {
            myRigidBody.velocity = knockbackDirection * knockback;
            knockbackCount -= Time.fixedDeltaTime;
        }
        //this system allows 8-directional movement
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

        //Health
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; //don't let health go over full; mostly necessary if there are healing items
        }
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth; //resets player health; needs to be called because the scene is reloading and Start() isn't being called again
            sceneTransition(true);//transform.position = new Vector2(startCoordinateX, startCoordinateY); //go back to start position
        }
    }

    //mid-attack controls
    IEnumerator AttackDelay(float delay)
    {
        canMove = false; //make sure player can't move
        yield return new WaitForSeconds(delay); //"delay" is the variable, can be changed depending on length of animation
        canMove = true;
        isAttacking = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Loot")
        {
            theLevelManager.AddLoot(lootValue);
            Destroy(other.gameObject); //destroy the loot
        }
    }

    //animation system
    void Animate()
    {
        if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && canMove) //if the movement key(s) is/are being pressed and canMove
        {
            isMoving = true;
        }
        else //just to make sure
        {
            isMoving = false;
        }
        if (isMoving)
        {
            anim.SetFloat("X", Input.GetAxisRaw("Horizontal")); //these values go to the animator, which has logic that determines which animation to play...
            anim.SetFloat("Y", Input.GetAxisRaw("Vertical")); //...depending on the x and y values of the input
        }

        if (isAttacking)
        {
            //canMove= false;
            anim.SetFloat("lastMoveX", lastMoveX); //...lastMoveX/Y determine which way the player was facing before the attack key is pressed
            anim.SetFloat("lastMoveY", lastMoveY); //once again, the animator tool in Unity takes care of the logic once the values are passed
        }

        //send values to the animator to satisfy the conditions for playing animations
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("isMoving", isMoving);
    }

    //for the damage script
    public bool Damage(int dmg)
    {
        // --- ADD THIS CONDITION ---
        // Check if the player is currently vulnerable
        if (invincibilityCounter <= 0)
        {
            currentHealth -= dmg; //simple

            // --- START THE TIMER ---
            // Reset the invincibility timer (Start I-frames)
            invincibilityCounter = invincibilityLength;
            return true; //NEW: damage was applied successfully
        }
        return false; //NEW: player is still invincible, so no damage was taken
    }

    //NEW: applies knockback away from the source of damage
    public void ApplyKnockback(Vector2 sourcePosition)
    {
        knockbackDirection = ((Vector2)transform.position - sourcePosition).normalized;

        //NEW: safety check in case positions overlap exactly
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
        return damage;
    }

    public void addToLoot(int lootValue)
    {
        wallet += lootValue;
    }
    public void subtractFromLoot(int cost) //for when loot needs to be taken away
    {
        wallet -= cost;
    }

    public int getX()
    {
        return (int)transform.position.x;
    }

    public int getY()
    {
        return (int)transform.position.y;
    }

    public void setActive(bool active)
    {
        gameObject.SetActive(active);
    }

    //reference in the LevelManager to reset the PlayerCharacter position after changing scenes
    public void setPosition(int x, int y)
    {
        gameObject.transform.position = new Vector2(x, y);
    }

    //referenced in the LevelManager to allow the PlayerCharacter to move after changing scenes
    public void setMoving(bool moving)
    {
        canMove = moving;
    }

    public void sceneTransition(bool visible)
    {
        //gameObject.SetActive(visible);
        gameObject.transform.position = new Vector2(0, 0);
        canMove = visible;
        currentHealth = maxHealth;
        knockbackCount = 0;
        invincibilityCounter = 0;
    }

}
