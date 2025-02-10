using UnityEngine;
using Unity.Cinemachine;

public class StationaryCameraZone : MonoBehaviour
{
    [Tooltip("Assign the stationary virtual camera for this zone.")]
    public CinemachineCamera zoneCamera;

    // Optionally, a priority value for when the camera is active.
    // Higher numbers mean higher priority.
    public int activePriority = 11;
    public int inactivePriority = 10;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering is the player.
        if (other.CompareTag("Player"))
        {
            // Raise this camera’s priority.
            zoneCamera.Priority = activePriority;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Lower this camera’s priority.
            zoneCamera.Priority = inactivePriority;
        }
    }
}