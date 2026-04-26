using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleTeleport : MonoBehaviour
{
    [SerializeField] private string lobbySceneName = "test1";

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the Player touched the square
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(lobbySceneName);
            Debug.Log("Teleporting to Lobby...");
        }
    }
}