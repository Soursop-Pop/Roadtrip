using UnityEngine;
using Unity.Cinemachine;  // Make sure you have Cinemachine imported

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
        // Set up the player camera
        if (playerCam != null && player != null)
        {
            playerCam.Follow = player;
            playerCam.LookAt = player;
            playerCam.Priority = 11; // Start with the player camera active
        }

        // Set up the car camera
        if (carCam != null && car != null)
        {
            carCam.Follow = car;
            carCam.LookAt = car;
            carCam.Priority = 10; // Lower priority so it’s inactive
        }
    }

    /// <summary>
    /// Switch to the car camera so that it is locked on the car.
    /// </summary>
    public static void SwitchToCarCamera()
    {
        if (instance == null) return;
        instance.carCam.Priority = 11;   // Raise car camera's priority
        instance.playerCam.Priority = 10;  // Lower player camera's priority
    }

    /// <summary>
    /// Switch back to the player camera so that it is locked on the player.
    /// </summary>
    public static void SwitchToPlayerCamera()
    {
        if (instance == null) return;
        instance.playerCam.Priority = 11;  // Raise player camera's priority
        instance.carCam.Priority = 10;     // Lower car camera's priority
    }
}