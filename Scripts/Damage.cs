using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class Damage : MonoBehaviour
{
    private PlayerController player;
    
    //enemy stats
    [SerializeField]
    private int damage;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private int maxHealth;

    
    [SerializeField] //loot object
    private GameObject loot;

    private LevelManager theLevelManager; //for communication with kill count


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); //declare player object
        currentHealth = maxHealth; //set health to full
        theLevelManager = FindObjectOfType<LevelManager>();
    }

    private void FixedUpdate()
    {
        //Health
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (currentHealth <= 0)
        {
            Vector2 lootSpawnLocation = new Vector2(transform.position.x, transform.position.y);
            theLevelManager.Addkills(1);
            Destroy(gameObject);
            Instantiate(loot, lootSpawnLocation, Quaternion.identity);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //check if the hitbox is active first, since it extends beyond the player hurtbox
        if (other.CompareTag("playerHitBox"))
        {
            currentHealth -= player.getDamage();
        }
        //use else if to make sure player's hitbox is prioritized over their hurtbox
        else if (other.CompareTag("Player"))
        {
            //deal damage first; only apply knockback if the player actually took damage
            if (player.Damage(damage))
            {
                player.ApplyKnockback(transform.position);
            }
        }

    } 
 }
