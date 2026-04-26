using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    private PlayerController player;

    [Header("UI Elements")]
    public TextMeshProUGUI speedPriceText;
    public TextMeshProUGUI healthPriceText;
    public TextMeshProUGUI damagePriceText;
    public TextMeshProUGUI lootPriceText;

    [Header("Upgrade Costs")]
    public int speedUpgradeCost = 10;
    public int healthUpgradeCost = 15;
    public int damageUpgradeCost = 20;
    public int lootUpgradeCost = 25;

    [Header("Upgrade Values")]
    public float speedIncrease = 5f; // Increased for better feel
    public int healthIncrease = 2;
    public int damageIncrease = 5;
    public float lootMultiplierIncrease = 0.2f;

    void Start()
    {
        FindPlayer();
        UpdatePriceUI();
    }

    // Modern way to find the player script
    void FindPlayer()
    {
        player = Object.FindAnyObjectByType<PlayerController>();
        if (player == null)
        {
            Debug.LogError("UpgradeManager: CANNOT FIND PLAYER! Make sure PlayerController script is on your Tank.");
        }
    }

    public void UpdatePriceUI()
    {
        if (speedPriceText != null) speedPriceText.text = speedUpgradeCost.ToString();
        if (healthPriceText != null) healthPriceText.text = healthUpgradeCost.ToString();
        if (damagePriceText != null) damagePriceText.text = damageUpgradeCost.ToString();
        if (lootPriceText != null) lootPriceText.text = lootUpgradeCost.ToString();
    }

    public void UpgradeSpeed()
    {
        if (player == null) FindPlayer();

        if (player != null && player.spendMoney(speedUpgradeCost))
        {
            player.moveSpeed += speedIncrease; // Your speed variable is called moveSpeed
            speedUpgradeCost = Mathf.RoundToInt(speedUpgradeCost * 1.5f);
            UpdatePriceUI();
        }
    }

    public void UpgradeHealth()
    {
        if (player == null) FindPlayer();

        if (player != null && player.spendMoney(healthUpgradeCost))
        {
            player.maxHealth += healthIncrease;
            player.currentHealth = player.maxHealth; // Heal the player upon upgrading
            healthUpgradeCost = Mathf.RoundToInt(healthUpgradeCost * 1.5f);
            UpdatePriceUI();
        }
    }

    public void UpgradeDamage()
    {
        if (player == null) FindPlayer();

        if (player != null && player.spendMoney(damageUpgradeCost))
        {
            // Your damage variable is called attackDamage, not damage!
            player.attackDamage += damageIncrease;
            damageUpgradeCost = Mathf.RoundToInt(damageUpgradeCost * 1.5f);
            UpdatePriceUI();
        }
    }

    public void UpgradeLoot()
    {
        if (player == null) FindPlayer();

        if (player != null && player.spendMoney(lootUpgradeCost))
        {
            player.lootMultiplier += lootMultiplierIncrease;
            lootUpgradeCost = Mathf.RoundToInt(lootUpgradeCost * 1.5f);
            UpdatePriceUI();
        }
    }
}