using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private EnemyHealth enemyScript;
    [SerializeField]
    private EnemySpawner enemySpawner;

    public int lootCount;
    public TextMeshProUGUI lootText;
    public int killCount;
    public TextMeshProUGUI killText;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        enemyScript = FindObjectOfType<EnemyHealth>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        player.StopAllCoroutines();
    }

    // Update is called once per frame
    void Update()
    {

        //set up UI text and keep it updated
        lootText.text = "" + player.coins;
        killText.text = "" + killCount;

        //reset non-player elements when killed
        if (player.currentHealth <= 0)
        {
            resetEnemies();
        }
    }

    public void resetEnemies()
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner"); //find every enemy spawner and put in array
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); //find every enemy and put in array
        GameObject[] loot = GameObject.FindGameObjectsWithTag("Loot"); //find every enemy and put in array
        GameObject[] gates = GameObject.FindGameObjectsWithTag("Gate"); //find every enemy and put in array


        //goes through the array of enemies and destroys them; make sure the enemy prefab has the "Enemy" tag
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        //}
        //goes through the array of spawners and resets them; the spawner prefab should already have the "Spawner" tag
        //for (int i = 0; i < enemies.Length; i++)
        //{
            spawners[i].GetComponent<EnemySpawner>().StopAllCoroutines(); //stop enemy instantiating coroutine
            spawners[i].GetComponent<EnemySpawner>().hasEntered = false;
            spawners[i].GetComponent<CapsuleCollider2D>().enabled = true; //set zone trigger to true so the player can collide with it again and start the spawning coroutine
        }
        for (int i = 0; i < loot.Length; i++)
        {
            Destroy(loot[i]);
        }
        for (int i = 0; i < gates.Length; i++)
        {
            gates[i].GetComponent<CapsuleCollider2D>().enabled = true;
            gates[i].GetComponent<BoxCollider2D>().enabled = true;
            gates[i].GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public void Addkills(int killsToAdd)
    {
        killCount = killCount + killsToAdd;
        killText.text = "" + killCount;
    }
}
