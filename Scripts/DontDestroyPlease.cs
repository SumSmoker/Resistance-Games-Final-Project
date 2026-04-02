using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyPlease : MonoBehaviour
{

    private static GameObject[] persistentObjects = new GameObject[1]; //this is a hard-coded number, increase it depending on how many items need to be fit in
    public int objectIndex; //the object's index in the "persistantObjects" array

    void Awake()
    {
        if (persistentObjects[objectIndex] == null) //if the slot is empty...
        {
            persistentObjects[objectIndex] = gameObject; //...fill the slot with it...
            DontDestroyOnLoad(gameObject); //...and apply the code
        }
        else if (persistentObjects[objectIndex] != gameObject) //if the object doesn't equal the current slot item...
        {
            Destroy(gameObject); //...destroy it
        }
    }
}
