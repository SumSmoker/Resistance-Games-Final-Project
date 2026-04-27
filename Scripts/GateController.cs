using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{

    public bool hasEntered;
    [SerializeField]
    private BoxCollider2D zoneTrigger;
    private CapsuleCollider2D collider;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        zoneTrigger = GetComponent<BoxCollider2D>();
        collider = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //oneTrigger.enabled = false; //turn off the zone trigger so that the player can't touch the trigger and call the method again
            sr.enabled = false;
            collider.enabled = false;
            zoneTrigger.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
