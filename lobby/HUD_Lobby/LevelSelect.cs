using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private GameObject missionMenuUI;

    private PlayerController playerController;

    void Start()
    {
        // Find player instance in the scene
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerController = playerObj.GetComponent<PlayerController>();
        }

        // Ensure menu is hidden on start
        if (missionMenuUI != null) missionMenuUI.SetActive(false);
    }

    // --- Detection Logic ---

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) OpenMenu();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) CloseMenu();
    }

    // --- Menu Controls ---

    public void OpenMenu()
    {
        if (missionMenuUI != null)
        {
            missionMenuUI.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void CloseMenu()
    {
        if (missionMenuUI != null)
        {
            missionMenuUI.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // --- Level Loading with Price ---

    // Universal method for all buttons. 
    // Param format: "SceneName,Price" (Example: "test_dungeon,5")
    public void LoadLevelWithPrice(string parameters)
    {
        // Split input string into array
        string[] data = parameters.Split(',');

        if (data.Length < 2)
        {
            Debug.LogError("Invalid button parameters! Use 'SceneName,Price'");
            return;
        }

        string sceneName = data[0].Trim();
        int cost = int.Parse(data[1].Trim());

        if (playerController != null)
        {
            // Attempt to charge the player
            if (playerController.spendMoney(cost))
            {
                // Reset stats and load scene if paid
                playerController.sceneTransition(true);
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.Log("Access Denied: Insufficient gold for " + sceneName);
            }
        }
    }
}