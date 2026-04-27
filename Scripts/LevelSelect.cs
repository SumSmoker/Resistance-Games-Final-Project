using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


//attach this script to the Main Camera that is a child of the Player Character

public class LevelSelect : MonoBehaviour
{
    private PlayerController playerController; //create this reference to manipulate when the player can move
    private GrayscaleEffect grayscaleEffect;
    private Camera mainCamera;
    //run count variables
    [SerializeField]
    private float runCost;
    [SerializeField]
    private float inflation;
    [SerializeField]
    private int freeTrial;

    //UI stuff
    public TextMeshProUGUI walletText;
    public TextMeshProUGUI runText;
    public TextMeshProUGUI costText;

    [SerializeField]
    private GameObject performancePanel;
    //framerate variables
    public int frameRatePayout;
    public int frameRateCost;
    [SerializeField]
    private int goodFrameRate;
    [SerializeField]
    private int badFrameRate;
    //color variables
    public int colorPayout;
    public int colorCost;
    public bool fullColor;
    public TextMeshProUGUI colorText;

    void Start()
    {
        playerController = GameObject.Find("PlayerCharacter").GetComponent<PlayerController>(); //initialize in a start method for everything that requires it
        grayscaleEffect = GameObject.Find("PlayerCharacter").GetComponentInChildren<GrayscaleEffect>();
        runCost = (int)(runCost * inflation);
        //set up UI text and keep it updated
        walletText.text = "" + playerController.getWallet();
        runText.text = "" + playerController.runCount;
        if (freeTrial - playerController.runCount > 0)
        {
            runCost = 0f;
        }
        else
        {
            runCost = runCost * inflation;
        }
        costText.text = "" + runCost;
        if (playerController.fullColor)
        {
            colorText.text = "Sell: +" + colorPayout;
        }
        else
        {
            colorText.text = "Buy: -" + colorCost;
        }

    }

    public void startRun()
    {
        if (freeTrial - playerController.runCount > 0) //if the number of free trials remaining is greater than the number of player's runs so far
        {
            playerController.addToRun(1);
            SceneManager.LoadScene("SampleScene");
        }
        else if (playerController.getWallet() - runCost >= 0) //if the player's wallet - run cost is >= 0
        {
            //start the level, or activate the button
            playerController.sceneReset();
            playerController.addToRun(1);
            playerController.subtractFromLoot(runCost);
            SceneManager.LoadScene("SampleScene");
        }
        
    }

    public void changeColor()
    {
        
        if (playerController.fullColor)
        {
            playerController.addToLoot(colorPayout);
            playerController.changeColor(!playerController.fullColor);
            walletText.text = "" + playerController.getWallet();
            colorText.text = "Buy: -" + colorCost;
        }
        else if(!playerController.fullColor && playerController.getWallet() - colorCost >= 0)
        {
            playerController.subtractFromLoot(colorCost);
            playerController.changeColor(!playerController.fullColor);
            walletText.text = "" + playerController.getWallet();
            colorText.text = "Sell: +" + colorPayout;
        }
    }

    public void changeFrameRate()
    {
        if (playerController.frameRateIsGood)
        {
            playerController.addToLoot(frameRatePayout);
            Application.targetFrameRate = badFrameRate;
            playerController.changeFrameRateGood(false);
            walletText.text = "" + playerController.getWallet();
            //GetComponentInChildren<TextMeshProUGUI>().text = "Buy";
        }
        else if (playerController.getWallet() - frameRateCost >= 0)
        {
            playerController.subtractFromLoot(frameRateCost);
            Application.targetFrameRate = goodFrameRate;
            playerController.changeFrameRateGood(true);
        }
    }
}
