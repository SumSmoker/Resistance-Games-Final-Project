using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour
{
    public GameObject target; // Assign the target in the Inspector
    private PlayerController player;
    private NavMeshAgent agent;
    private EnemySpawner spawner;

    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true; //make sure the component is enabled
        agent.updateRotation = false; // Disable 3D rotation
        agent.updateUpAxis = false; // Disable vertical movement/rotation
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        spawner = GameObject.FindObjectOfType<EnemySpawner>();
    }

    void Update()
    {
        target.transform.position = new Vector2(player.getX(), player.getY());
        if (target != null && agent.isActiveAndEnabled)
        {
            agent.SetDestination(target.transform.position);
        }
    }
}
