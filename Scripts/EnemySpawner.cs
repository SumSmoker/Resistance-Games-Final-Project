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
            hasEntered = true;
            StartCoroutine(SpawnRoutine());
            //SpawnAgent();
            zoneTrigger.enabled = false;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (true) //replace "true" with a condition later that will stop the enemy spawn
        {
            //determine spawn location
            Vector2 spawnPosition = new Vector2(spawnX, spawnY);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 1f, NavMesh.AllAreas))
            {
                spawnPosition = hit.position;
                //spawn enemy
                GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                NavMeshAgent agent = newEnemy.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    // 4. Warp the agent to the exact point (important!)
                    agent.Warp(spawnPosition);

                    // 5. Enable the agent component if it was disabled in the prefab
                    agent.enabled = true;

                    // 6. Set the destination
                    if (player != null)
                    {
                        agent.SetDestination(player.transform.position);
                    }

                    
                }
            }
            yield return new WaitForSeconds(spawnTime);

        }

        /*public Vector3 position()
        {

            return new Vector3 = (transform.position.x, transform.position.y, 0);
        }*/

        /*public void SpawnAgent()
        {
            // 1. Determine a potential location
            Vector3 randomLocation = transform.position;
            randomLocation.y = transform.position.y; // Keep Y level consistent initially

            NavMeshHit hit;

            // 2. Find the nearest point on the NavMesh
            // The second parameter is the search radius, the third is the area mask
            if (NavMesh.SamplePosition(transform.position, out hit, 1f, NavMesh.AllAreas))
            {
                Vector3 spawnPoint = hit.position;

                // 3. Instantiate the object
                GameObject newAgentObject = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

                // Get the NavMeshAgent component
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


