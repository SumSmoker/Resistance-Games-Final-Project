using System.Collections;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    // Create a list of item types that can be selected in the Inspector
    public enum CollectibleType { Coin, Health, SpeedBoost, DamageBoost }

    [Header("Item Settings")]
    public CollectibleType itemType;  // Select loot type
    public int amount = 1;            // How much it gives (10 coins, 20 HP, etc.)

    [Header("PowerUp Settings")]
    public float powerUpDuration = 5f; // How long the effect lasts (only for PowerUps)

    [Header("Effects")]
    [SerializeField] private GameObject pickupEffect; // Visual effect (optional)

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if it's the player
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                ApplyEffect(player);
            }

            // Spawn particle effects, if any
            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            // Destroy the item after pickup
            Destroy(gameObject);
        }
    }

    private void ApplyEffect(PlayerController player)
    {
        // Call different player functions depending on the selected type
        switch (itemType)
        {
            case CollectibleType.Coin:
                player.AddCoins(amount);
                break;

            case CollectibleType.Health:
                player.Heal(amount);
                break;

            case CollectibleType.SpeedBoost:
                player.ApplySpeedBoost(amount, powerUpDuration);
                break;

            case CollectibleType.DamageBoost:
                player.ApplyDamageBoost(amount, powerUpDuration);
                break;
        }
    }
}