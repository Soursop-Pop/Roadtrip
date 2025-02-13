using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [Header("Speed & Acceleration")]
    public float maxSpeed = 25f;  // Top speed
    public float acceleration = 10f;  // How fast we speed up
    public float deceleration = 12f;  // How fast we slow down
    public float reverseSpeed = 10f;  // Slower reverse speed
    public float turnSpeed = 60f;  // Steering speed
    public float driftIntensity = 0.3f; // How much drift is applied

    [Header("Car Handling")]
    public float traction = 1.5f; // How much grip the car has
    public float turnDamping = 2.5f; // Makes high-speed turns smoother
    public float driftControl = 3f; // Controls how much drift vs. turn speed affects handling

    [Header("Camera Effects")]
    public Camera carCamera;
    public float baseFOV = 60f;
    public float maxSpeedFOV = 80f;

    private bool isPlayerInside = false;
    private GameObject player;
    private bool isAutoDriving = false;
    private float currentSpeed = 0f;
    private float velocity = 0f;
    private float driftFactor = 0f;
    private float turnInput = 0f;

    void Update()
    {
        if (isPlayerInside)
        {
            if (isAutoDriving)
            {
                AutoDrive();
            }
            else
            {
                Drive();
            }

            CheckForExit();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            isAutoDriving = !isAutoDriving;
        }

        // Dynamic Camera FOV effect based on speed
        if (carCamera)
        {
            float speedPercent = Mathf.Abs(currentSpeed) / maxSpeed;
            carCamera.fieldOfView = Mathf.Lerp(baseFOV, maxSpeedFOV, speedPercent);
        }
    }

    void Drive()
    {
        float accelerationInput = Input.GetAxis("Vertical"); // Forward/Reverse input
        turnInput = Input.GetAxis("Horizontal"); // Steering input

        if (accelerationInput > 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        }
        else if (accelerationInput < 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, -reverseSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        // Apply movement
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        // Smooth turning with speed factor
        float steerAmount = turnInput * turnSpeed * Time.deltaTime;
        float speedFactor = Mathf.Clamp01(Mathf.Abs(currentSpeed) / maxSpeed);
        steerAmount /= (1 + speedFactor * turnDamping); // Slower turning at high speeds

        // Simulate drifting (fun handling)
        if (Mathf.Abs(turnInput) > 0.1f && Mathf.Abs(currentSpeed) > 5f)
        {
            driftFactor = Mathf.Lerp(driftFactor, driftIntensity, Time.deltaTime * driftControl);
        }
        else
        {
            driftFactor = Mathf.Lerp(driftFactor, 0, Time.deltaTime * driftControl * 2);
        }

        transform.Rotate(Vector3.up * steerAmount * (1 - driftFactor));
        transform.position += transform.right * driftFactor * turnInput * 0.1f;
    }

    void AutoDrive()
    {
        currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed * 0.8f, acceleration * Time.deltaTime);
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

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
        CameraManager.SwitchToCarCamera();
    }

    void ExitVehicle()
    {
        isPlayerInside = false;
        player.GetComponent<ThirdPersonController>().ExitVehicle(gameObject);
        CameraManager.SwitchToPlayerCamera();
    }
}
