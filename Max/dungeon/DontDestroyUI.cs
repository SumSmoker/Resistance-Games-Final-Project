using UnityEngine;

public class DontDestroyUI : MonoBehaviour
{
    public static DontDestroyUI instance;

    void Awake()
    {
        // Singleton pattern for UI
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the UI alive
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate UI
        }
    }
}