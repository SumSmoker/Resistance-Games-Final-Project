using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

//attach this script to the Main Camera that is a child of the Player Character

public class LevelSelect : MonoBehaviour
{
    private PlayerController playerController; //create this reference to manipulate when the player can move

    void Start()
    {
        
        playerController = GameObject.Find("PlayerCharacter").GetComponent<PlayerController>(); //initialize in a start method for everything that requires it
        playerController.setActive(false);
    }

    public void startRun()
    {
        //later, we'll have the conditional: "if player loot is greater than required loot, load game scene"
        playerController.sceneTransition(true);
        playerController.setActive(true);
        SceneManager.LoadScene("SampleScene");
    }
}
