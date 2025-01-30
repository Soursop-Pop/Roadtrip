using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineCamera playerCam;
    public CinemachineCamera carCam;

    private static CameraManager instance;

    void Awake()
    {
        instance = this;
    }

    public static void SwitchToCarCamera()
    {
        instance.playerCam.Priority = 5;
        instance.carCam.Priority = 10;  // Make the car camera active
    }

    public static void SwitchToPlayerCamera()
    {
        instance.carCam.Priority = 5;
        instance.playerCam.Priority = 10;  // Make the player camera active
    }
}