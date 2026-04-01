using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI; // Make sure to include the AI namespace

public class AgentMovement : MonoBehaviour
{
    public GameObject target; // Assign the target (e.g., player) in the Inspector
    private PlayerController player;
    private NavMeshAgent agent;
    private EnemySpawner spawner;

    void Start()
    {
        // Get the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        // NavMesh agents use speed, not velocity directly in 2D
        agent.updateRotation = false; // Disable 3D rotation
        agent.updateUpAxis = false; // Disable vertical movement/rotation
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        spawner = GameObject.FindObjectOfType<EnemySpawner>();
        //NavMeshAgent.Warp(EnemySpawner.position());
    }

    void Update()
    {
        target.transform.position = new Vector2(player.getX(), player.getY());
        // Set the agent's destination to the target's position
        if (target != null && agent.isActiveAndEnabled)
        {
            agent.SetDestination(target.transform.position);
        }
    }
}