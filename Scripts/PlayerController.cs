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

    //animation values
    public Animator anim;
    private bool isMoving;
    public bool isAttacking;
    private float lastMoveX;
    private float lastMoveY;

    //loot values
    public int lootValue;
    private int lootCount;

    [SerializeField]
    private GameObject hitBox;


    void Start()
    {
        //transform.position = new Vector2(startCoordinateX, startCoordinateY);
        myRigidBody = GetComponent<Rigidbody2D>();
        theLevelManager = FindObjectOfType<LevelManager>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        Debug.Log(Input.GetAxisRaw("Horizontal"));
        Debug.Log(Input.GetAxisRaw("Vertical"));
        Animate();

        if (Input.GetButtonDown("Jump") && canMove)
        {
            isAttacking = true;
            myRigidBody.velocity = new Vector2(0, 0);
            StartCoroutine(AttackDelay(delay));
        }
        
    }

    private void FixedUpdate()
    {
        //this system allows old-school 8-directional movement
        if (canMove)
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

        //Health
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (currentHealth <= 0)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            currentHealth = maxHealth;
            transform.position = new Vector2(startCoordinateX, startCoordinateY);
        }

        /*if (knockbackCount <= 0)
            // Player moving right
            if (Input.GetAxisRaw("Horizontal") > 0f)
            {
                myRigidBody.velocity = new Vector2(moveSpeed, myRigidBody.velocity.y);
                transform.localScale = new Vector2(1f, 1f);
            }
            // Player moving left
            else if (Input.GetAxisRaw("Horizontal") < 0f)
            {
                myRigidBody.velocity = new Vector2(-moveSpeed, myRigidBody.velocity.y);
                transform.localScale = new Vector2(-1f, 1f);
            }
            // No slide
            else
            {
                myRigidBody.velocity = new Vector2(0f, myRigidBody.velocity.y);
            }
        //knockback
        else
        {
            if (knockFromRight)
                myRigidBody.velocity = new Vector2(-knockback, knockback);
            if (!knockFromRight)
                myRigidBody.velocity = new Vector2(knockback, knockback);
            knockbackCount -= Time.deltaTime;
        }*/

    }

    //mid-attack controls
    IEnumerator AttackDelay(float delay)
    {
        canMove = false;
        yield return new WaitForSeconds(delay);
        canMove = true;
        isAttacking = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Loot")
        {
            theLevelManager.AddLoot(lootValue);
            Destroy(other.gameObject);
        }
    }

    //animation system
    void Animate()
    {
        if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && canMove)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        if (isMoving)
        {
            anim.SetFloat("X", Input.GetAxisRaw("Horizontal"));
            anim.SetFloat("Y", Input.GetAxisRaw("Vertical"));
        }

        if (isAttacking)
        {
            //canMove= false;
            anim.SetFloat("lastMoveX", lastMoveX);
            anim.SetFloat("lastMoveY", lastMoveY);
        }
        else
        {
            //canMove = true;
        }
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("isMoving", isMoving);
    }

    //for the damage script
    public void Damage(int dmg)
    {
        currentHealth -= dmg;
        //canMove = false;

        //gameObject.GetComponent<Animation>().Play("Scott_redflash");
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
        lootCount += lootValue;
    }
    public void subtractFromLoot(int cost)
    {
        lootCount -= cost;
    }

    public int getX()
    {
        return (int)transform.position.x;
    }

    public int getY()
    {
        return (int)transform.position.y;
    }

}
