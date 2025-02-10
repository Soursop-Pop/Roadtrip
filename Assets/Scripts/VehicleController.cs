using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public float speed = 10f;
    public float turnSpeed = 50f;
    private bool isPlayerInside = false;
    private GameObject player;
    private bool isAutoDriving = false;

    void Update()
    {
        if (isPlayerInside)
        {
            if (isAutoDriving)
            {
                AutoDrive();  // Keep moving forward if auto-drive is enabled
            }
            else
            {
                Drive();  // Manual drive when auto-drive is off
            }

            CheckForExit();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            isAutoDriving = !isAutoDriving;  // Toggle AutoDrive
        }
    }

    void Drive()
    {
        float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        transform.Translate(Vector3.forward * move);
        transform.Rotate(Vector3.up * turn);
    }

    void AutoDrive()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);


        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * turn);

    }

    void CheckForExit()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ExitVehicle();
        }
    }

    public void EnterVehicle(GameObject playerObj)
    {
        isPlayerInside = true;
        player = playerObj;

        // Switch to car camera
        CameraManager.SwitchToCarCamera();
    }

    void ExitVehicle()
    {
        isPlayerInside = false;
        player.GetComponent<ThirdPersonController>().ExitVehicle(gameObject);

        // Switch back to player camera
        CameraManager.SwitchToPlayerCamera();
    }
}

