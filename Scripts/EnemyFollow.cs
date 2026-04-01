using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float moveSpeed;
    private Transform playerTarget;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if(playerObject != null)
        {
            playerTarget = playerObject.transform;//GetComponent<Transform>();
        }
        else
        {
            Debug.LogError("Player gameObject not found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTarget != null)
        {
            //moves the scriptholder's position towards the player position
            transform.position = Vector3.MoveTowards(transform.position, playerTarget.position, moveSpeed * Time.deltaTime);
        }

    }
}
