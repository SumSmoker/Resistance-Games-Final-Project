using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private Damage enemyScript;
    [SerializeField]
    private EnemySpawner enemySpawner;

    public int lootCount;
    public TextMeshProUGUI lootText;
    public int killCount;
    public TextMeshProUGUI killText;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        enemyScript = FindObjectOfType<Damage>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {

        //set up UI text and keep it updated
        lootText.text = "" + lootCount;
        killText.text = "" + killCount;

        //reset non-player elements when killed
        if(player.currentHealth <= 0)
        {
            resetEnemies();
        }
    }

    public void resetEnemies()
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner"); //find every enemy spawner and put in array
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); //find every enemy and put in array
        
        //goes through the array of enemies and destroys them; make sure the enemy prefab has the "Enemy" tag
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }
        //goes through the array of spawners and resets them; the spawner prefab should already have the "Spawner" tag
        for (int i = 0; i < enemies.Length; i++)
        {
            spawners[i].GetComponent<EnemySpawner>().StopAllCoroutines(); //stop enemy instantiating coroutine
            spawners[i].GetComponent<CapsuleCollider2D>().enabled = true; //set zone trigger to true so the player can collide with it again and start the spawning coroutine
        }
    }

    public void AddLoot(int lootToAdd)
    {
        player.addToLoot(lootToAdd);
        lootCount = lootCount + lootToAdd;
        lootText.text = "" + lootCount;
    }
    public void Addkills(int killsToAdd)
    {
        killCount = killCount + killsToAdd;
        killText.text = "" + killCount;
    }
}
