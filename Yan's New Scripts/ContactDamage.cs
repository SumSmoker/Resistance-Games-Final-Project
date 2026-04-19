using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damageAmount = 10; // How much damage to deal to the player

    void OnTriggerEnter2D(Collider2D other)
    {
        // DEALING DAMAGE TO PLAYER
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                // Attempt to damage player. If successful, push player back.
                if (player.Damage(damageAmount))
                {
                    player.ApplyKnockback(transform.position);
                }
            }
        }
    }
}