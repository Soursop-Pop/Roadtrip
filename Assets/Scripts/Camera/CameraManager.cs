using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineCamera playerCam; // Assigned via Inspector
    public CinemachineCamera carCam;    // Assigned via Inspector

    public Transform player; // The player transform
    public Transform car;    // The car transform

    private static CameraManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Check if the player is active in the scene
        if (player != null && player.gameObject.activeInHierarchy)
        {
            // Set up the player camera
            playerCam.Follow = player;
            playerCam.LookAt = player;
            playerCam.Priority = 11;
            // Optionally, lower carCam's priority
            carCam.Priority = 10;
        }
        else if (car != null)
        {
            // Player not found or inactive, use the car camera
            carCam.Follow = car;
            carCam.LookAt = car;
            carCam.Priority = 11;
            playerCam.Priority = 10;
        }
    }

    public static void SwitchToCarCamera()
    {
        if (instance == null) return;
        instance.carCam.Priority = 11;
        instance.playerCam.Priority = 10;
    }

    public static void SwitchToPlayerCamera()
    {
        if (instance == null) return;
        instance.playerCam.Priority = 11;
        instance.carCam.Priority = 10;
    }
}