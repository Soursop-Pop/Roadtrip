using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [Header("Speed & Acceleration")]
    public float maxSpeed = 25f;
    public float acceleration = 8f;
    public float deceleration = 10f;
    public float brakeStrength = 20f;
    public float reverseSpeed = 8f;

    [Header("Steering & Handling")]
    public float turnSpeed = 60f;
    public float speedSteerFactor = 0.4f; // Steering is harder at high speeds
    public float brakingRotationFactor = 1.5f; // Small rotation when braking
    public float traction = 0.9f; // How much grip the car has

    [Header("Camera Settings")]
    public Camera carCamera;
    public Transform cameraFollowPoint;
    public float baseFOV = 60f;
    public float maxSpeedFOV = 80f;
    public float cameraLagSpeed = 2f;
    public float slideCameraLag = 0.3f; // Extra camera lag when sliding

    private bool isPlayerInside = false;
    private GameObject player;
    private bool isAutoDriving = false;
    private float currentSpeed = 0f;
    private float turnInput = 0f;
    private bool isBraking = false;
    private Quaternion targetCameraRotation;

    void Update()
    {
        if (isPlayerInside)
        {
            if (isAutoDriving)
                AutoDrive();
            else
                Drive();

            CheckForExit();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            isAutoDriving = !isAutoDriving;
        }

        UpdateCamera();
        //StabilizeCar();
    }

    void Drive()
    {
        float accelerationInput = Input.GetAxis("Vertical"); // Forward/Reverse input
        turnInput = Input.GetAxis("Horizontal"); // Steering input
        isBraking = Input.GetKey(KeyCode.Space);

        // Determine if the car is going forward or backward
        float direction = Mathf.Sign(currentSpeed); // 1 for forward, -1 for reverse

        //if (!IsGrounded())
        //{
        //    GetComponent<Rigidbody>().AddForce(Vector3.down * 20f, ForceMode.Acceleration);
        //}


        // ACCELERATION & DECELERATION
        if (isBraking)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, brakeStrength * Time.deltaTime);
        }
        else if (accelerationInput > 0)
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

        // MOVEMENT (Only move if there is speed)
        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        }

        
        // STEERING (Only allow turning if moving)
        if (Mathf.Abs(currentSpeed) > 0.2f /*&& IsGrounded()*/)

        {
            float steerAmount = turnInput * turnSpeed * Time.deltaTime;
            float speedFactor = Mathf.Clamp01(Mathf.Abs(currentSpeed) / maxSpeed); // More speed, harder to turn
            steerAmount *= (1 - speedFactor * speedSteerFactor);

            // Apply braking rotation effect
            if (isBraking && Mathf.Abs(turnInput) > 0.1f)
            {
                steerAmount *= brakingRotationFactor;
            }

            // Reverse the steering direction if moving backward
            transform.Rotate(Vector3.up * steerAmount * direction);
        }

        // Simulate traction (drifting effect)
        Vector3 velocity = transform.forward * currentSpeed;
        Vector3 lateralVelocity = Vector3.Project(velocity, transform.right); // Sideways movement

        // Reduce sideways drift based on traction
        velocity -= lateralVelocity * (1f - traction);

        // Apply the adjusted velocity
        transform.position += velocity * Time.deltaTime;


        //ApplyDownforce();

    }


    void AutoDrive()
    {
        currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed * 0.8f, acceleration * Time.deltaTime);
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * turn);
    }

    void UpdateCamera()
    {
        if (carCamera && cameraFollowPoint)
        {
            Vector3 targetPosition = cameraFollowPoint.position + carCamera.transform.position - transform.position;

            // If braking + turning, introduce slight camera lag
            float lagFactor = isBraking && Mathf.Abs(turnInput) > 0.1f ? slideCameraLag : 1f;

            targetCameraRotation = Quaternion.LookRotation(transform.forward);
            carCamera.transform.position = Vector3.Lerp(carCamera.transform.position, targetPosition, Time.deltaTime * cameraLagSpeed * lagFactor);
            carCamera.transform.rotation = Quaternion.Slerp(carCamera.transform.rotation, targetCameraRotation, Time.deltaTime * cameraLagSpeed);
        }

        // FOV effect based on speed
        float speedPercent = Mathf.Abs(currentSpeed) / maxSpeed;
        carCamera.fieldOfView = Mathf.Lerp(baseFOV, maxSpeedFOV, speedPercent);
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

    //void StabilizeCar()
    //{
    //    // Keep the car upright
    //    Vector3 upVector = transform.up;
    //    Vector3 targetUp = Vector3.up;

    //    // Calculate the rotation needed to align the car upright
    //    Quaternion targetRotation = Quaternion.FromToRotation(upVector, targetUp) * transform.rotation;

    //    // Apply torque to correct the rotation
    //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    //}
    //void ApplyDownforce()
    //{
    //    // Add a downward force to keep the car on the ground
    //    if (!isBraking)
    //    {
    //        float downforce = 10f * (1 - traction); // Less traction = more downforce needed
    //        GetComponent<Rigidbody>().AddForce(Vector3.down * downforce, ForceMode.Acceleration);

    //    }
    //}

    //bool IsGrounded()
    //{
    //    RaycastHit hit;
    //    return Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit, 1.2f);
    //}



}
