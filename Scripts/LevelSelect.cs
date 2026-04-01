using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    //private InventoryManager inventoryManager; //create this reference to manipulate when the player can open the inventory
    private PlayerController playerController; //create this reference to manipulate when the player can move

    void Start()
    {
        //inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>(); //find the inventoryManager
        //inventoryManager.canInput = false; //disable usage
        playerController = GameObject.Find("PlayerCharacter").GetComponent<PlayerController>(); //initialize in a start method for everything that requires it
        playerController.canMove = false; //disable movement
        playerController.GetComponent<SpriteRenderer>().enabled = false; //disable sprite renderer
    }

    public void startRun()
    {
        //if player loot is greater than required loot, load game scene
        SceneManager.LoadScene(1);
    }
    /*
    public void loadSuperstore()
    {
        SceneManager.LoadScene(4);
    }
    public void loadPark()
    {
        SceneManager.LoadScene(5);
    }
    public void loadJunkyard()
    {
        SceneManager.LoadScene(6);
    }
    public void loadMultiplayer()
    {
        SceneManager.LoadScene(7);
    }
    */
}
