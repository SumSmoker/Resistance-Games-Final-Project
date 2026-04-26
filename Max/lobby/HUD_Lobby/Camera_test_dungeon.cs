using UnityEngine;

public class CameraFollowFix : MonoBehaviour
{
    private Transform target;

    void Start()
    {
        // Find the player that survived the scene transition
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Follow the player (keeping the camera's Z position)
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }
    }
}