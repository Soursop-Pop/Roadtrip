using UnityEngine;

public class CarController2D : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the car
    public Transform player; // Reference to the player object
    public Transform exitPoint; // Point where the player appears after exiting
    private bool isDriving = true; // Start with the player inside the car

    void Update()
    {
        if (isDriving)
        {
            MoveCar();
            CheckForExit();
        }
    }

    void MoveCar()
    {
        float moveInput = Input.GetAxis("Horizontal"); // Get left/right input
        transform.position += new Vector3(moveInput * moveSpeed * Time.deltaTime, 0, 0);
    }

    void CheckForExit()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ExitVehicle();
        }
    }

    void ExitVehicle()
    {
        isDriving = false;
        player.gameObject.SetActive(true); // Enable the player object
        player.position = exitPoint.position; // Move player to the exit point
        Camera.main.GetComponent<CameraFollow2D>().SwitchToPlayer(); // Switch camera back to player
        gameObject.GetComponent<CarController2D>().enabled = false; // Disable car movement
    }

    // Function to be called when entering the vehicle
    public void EnterVehicle()
    {
        isDriving = true;
        player.gameObject.SetActive(false); // Hide the player object
        Camera.main.GetComponent<CameraFollow2D>().SwitchToVehicle(); // Switch camera to car
        gameObject.GetComponent<CarController2D>().enabled = true; // Enable car movement
    }
}