using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    private Damage enemyScript;
    public int lootCount;
    public TextMeshProUGUI lootText;
    public int killCount;
    public TextMeshProUGUI killText;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        enemyScript = FindObjectOfType<Damage>();
    }

    // Update is called once per frame
    void Update()
    {
        lootText.text = "" + lootCount;
        killText.text = "" + killCount;
    }

    public void AddLoot(int lootToAdd)
    {
        player.addToLoot(lootToAdd);
        lootCount = lootCount + lootToAdd;
        lootText.text = "" + lootCount;
    }
    public void Addkills(int killsToAdd)
    {
        killCount = killCount + killsToAdd;
        killText.text = "" + killCount;
    }
}