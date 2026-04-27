using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    //make reference to prefab
    [SerializeField]
    private GameObject enemyPrefab;
   
    public bool hasEntered;

    //determine spawn time range
    [SerializeField]
    private float spawnTime;

    //determine spawn location range
    [SerializeField]
    private float spawnX;
    [SerializeField]
    private float spawnY;

    [SerializeField]
    private CapsuleCollider2D zoneTrigger;

    [SerializeField]//for the navMesh stuff
    private GameObject target;

    //find player object for navmesh stuff
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        spawnX = transform.position.x;
        spawnY = transform.position.y;
        zoneTrigger = GetComponent<CapsuleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            zoneTrigger.enabled = false; //turn off the zone trigger so that the player can't touch the trigger and call the method again
            HandleSpawn();
        }
    }

    private void HandleSpawn()
    {
        if (hasEntered) return;

        hasEntered = true;
        StartCoroutine(SpawnRoutine());
    }

    //preliminary spawn method, no NavMesh magic.
    private IEnumerator SpawnRoutine()
    {
        while (hasEntered) //replace "true" with a condition later that will stop the enemy spawn
        {
            Vector2 spawnPosition = new Vector2(spawnX, spawnY); //determine spawn location
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity); //instantiate enemy prefab
            yield return new WaitForSeconds(spawnTime); //wait to rerun the method
        }

        //my attempt at NavMesh. Feel free to overhaul because it isn't working great.
        /*public void SpawnAgent() 
        {
            //Determine a potential location
            Vector3 randomLocation = transform.position;
            randomLocation.y = transform.position.y; // Keep Y level consistent initially

            NavMeshHit hit;

            // The second parameter is the search radius, the third is the area mask
            if (NavMesh.SamplePosition(transform.position, out hit, 1f, NavMesh.AllAreas))
            {
                Vector3 spawnPoint = hit.position;

                //Instantiate the object
                GameObject newAgentObject = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

                NavMeshAgent agent = newAgentObject.GetComponent<NavMeshAgent>();

                if (agent != null)
                {
                    // 4. Warp the agent to the exact point (important!)
                    agent.Warp(spawnPoint);

                    // 5. Enable the agent component if it was disabled in the prefab
                    agent.enabled = true;

                    // 6. Set the destination
                    if (target != null)
                    {
                        agent.SetDestination(target.transform.position);
                    }
                }
            }
            else
            {
                Debug.LogWarning("Could not find a valid position on the NavMesh to spawn the agent.");
            }
        }*/
    }
}


