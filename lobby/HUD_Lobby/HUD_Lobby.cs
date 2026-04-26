using UnityEngine;
using UnityEngine.UI;
using TMPro; // Important for TextMeshPro!

public class HUD_Lobby : MonoBehaviour
{
    [Header("Health UI")]
    [SerializeField] private Sprite[] HealthSprites;
    [SerializeField] private Image HealthUI;

    [Header("Gold UI")]
    [SerializeField] private TextMeshProUGUI goldText; // Drag your TextMeshPro here

    private PlayerController player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        if (player == null) return;

        // 1. Update Health Bar
        if (HealthSprites.Length > 0 && HealthUI != null)
        {
            int healthIndex = Mathf.Clamp(player.currentHealth, 0, HealthSprites.Length - 1);
            HealthUI.sprite = HealthSprites[healthIndex];
        }

        // 2. Update Gold Text
        if (goldText != null)
        {
            // Display only the number or add a label like "Gold: "
            goldText.text = player.wallet.ToString();
        }
    }
}