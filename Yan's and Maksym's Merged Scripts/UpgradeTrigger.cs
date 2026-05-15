using UnityEngine;

public class UpgradeTrigger : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject upgradeMenuCanvas; // Drag your UpgradeMenu object here

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger has the "Player" tag
        if (other.CompareTag("Player"))
        {
            OpenMenu();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Automatically close the menu when the player leaves the area
        if (other.CompareTag("Player"))
        {
            CloseMenu();
        }
    }

    public void OpenMenu()
    {
        if (upgradeMenuCanvas != null)
        {
            upgradeMenuCanvas.SetActive(true); // Show the UI

            // Optional: Pause the game and enable cursor
            // Time.timeScale = 0f; 
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void CloseMenu()
    {
        if (upgradeMenuCanvas != null)
        {
            upgradeMenuCanvas.SetActive(false); // Hide the UI

            // Resume the game and hide cursor
            // Time.timeScale = 1f; 
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}