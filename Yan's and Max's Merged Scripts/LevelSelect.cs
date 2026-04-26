using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private GameObject missionMenuUI;

    void Start()
    {
        // Ensure the menu is closed when the scene starts
        if (missionMenuUI != null) missionMenuUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) OpenMenu();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) CloseMenu();
    }

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

    // Format for Button OnClick: "SceneName,Price" (e.g., "test_dungeon,10")
    public void LoadLevelWithPrice(string parameters)
    {
        string[] data = parameters.Split(',');

        if (data.Length < 2)
        {
            Debug.LogError("Parameter format error! Use: SceneName,Price");
            return;
        }

        string sceneName = data[0].Trim();
        int cost = int.Parse(data[1].Trim());

        // Call the static PlayerController instance
        if (PlayerController.instance != null)
        {
            if (PlayerController.instance.spendMoney(cost))
            {
                // LoadScene will trigger OnSceneLoaded in PlayerController
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.Log("Not enough gold to enter " + sceneName);
            }
        }
        else
        {
            Debug.LogError("PlayerController not found in the scene!");
        }
    }
}