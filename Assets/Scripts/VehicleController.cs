using UnityEngine;
using UnityEngine.SceneManagement;

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
    public float speedSteerFactor = 0.4f;
    public float brakingRotationFactor = 1.5f;
    public float traction = 0.9f;

    [Header("Camera Settings")]
    public Camera carCamera;
    public Transform cameraFollowPoint;
    public float baseFOV = 60f;
    public float maxSpeedFOV = 80f;
    public float cameraLagSpeed = 2f;
    public float slideCameraLag = 0.3f;

    private bool isPlayerInside = false;
    private GameObject player;
    private bool isAutoDriving = false;
    private float currentSpeed = 0f;
    private float turnInput = 0f;
    private bool isBraking = false;
    private Quaternion targetCameraRotation;

    private bool isBrokenDown = false; // Car breakdown state
    public GameObject repairPrompt; // UI for "Press F to Fix"

    public ParticleSystem breakdownEffect; // Assign your particle effect in the Inspector

    public AudioSource carMusic; // Assign in Inspector


    void Start()
    {
        if (repairPrompt) repairPrompt.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInside)
        {
            if (!isBrokenDown)
            {
                if (isAutoDriving)
                    AutoDrive();
                else
                    Drive();
            }

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
        if (isBrokenDown) return; // Prevent movement if broken down

        float accelerationInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
        isBraking = Input.GetKey(KeyCode.Space);

        float direction = Mathf.Sign(currentSpeed);

        //if (!IsGrounded())
        //{
        //    GetComponent<Rigidbody>().AddForce(Vector3.down * 20f, ForceMode.Acceleration);
        //}

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

        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        }

        if (Mathf.Abs(currentSpeed) > 0.2f /*&& IsGrounded()*/)
        {
            float steerAmount = turnInput * turnSpeed * Time.deltaTime;
            float speedFactor = Mathf.Clamp01(Mathf.Abs(currentSpeed) / maxSpeed);
            steerAmount *= (1 - speedFactor * speedSteerFactor);

            if (isBraking && Mathf.Abs(turnInput) > 0.1f)
            {
                steerAmount *= brakingRotationFactor;
            }

            transform.Rotate(Vector3.up * steerAmount * direction);
        }

        Vector3 velocity = transform.forward * currentSpeed;
        Vector3 lateralVelocity = Vector3.Project(velocity, transform.right);
        velocity -= lateralVelocity * (1f - traction);
        transform.position += velocity * Time.deltaTime;

        //ApplyDownforce();
    }

    void AutoDrive()
    {
        if (isBrokenDown) return; // Prevent auto-driving if broken down

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
            float lagFactor = isBraking && Mathf.Abs(turnInput) > 0.1f ? slideCameraLag : 1f;
            targetCameraRotation = Quaternion.LookRotation(transform.forward);
            carCamera.transform.position = Vector3.Lerp(carCamera.transform.position, targetPosition, Time.deltaTime * cameraLagSpeed * lagFactor);
            carCamera.transform.rotation = Quaternion.Slerp(carCamera.transform.rotation, targetCameraRotation, Time.deltaTime * cameraLagSpeed);
        }

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

        // Resume or start music
        if (carMusic && !carMusic.isPlaying)
        {
            carMusic.Play();
        }
    }


    void ExitVehicle()
    {
        isPlayerInside = false;
        player.GetComponent<ThirdPersonController>().ExitVehicle(gameObject);
        CameraManager.SwitchToPlayerCamera();

        // Pause the music so it resumes when re-entering
        if (carMusic)
        {
            carMusic.Pause();
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Breakdown"))
        {
            Breakdown(other); // Pass the trigger object to disable it
        }
    }



    void Breakdown(Collider breakdownTrigger)
    {
        if (isBrokenDown) return; // Prevent multiple triggers

        isBrokenDown = true;

        // Disable acceleration but allow the car to decelerate naturally
        acceleration = 0f;
        isBraking = true; // Simulates braking without a sudden stop

        // Play breakdown particle effect
        if (breakdownEffect && !breakdownEffect.isPlaying)
        {
            breakdownEffect.Play();
        }

        // Show repair UI
        if (repairPrompt) repairPrompt.SetActive(true);

        // Disable the breakdown trigger box so it cannot be triggered again
        breakdownTrigger.gameObject.SetActive(false);
    }



    public void Repair()
    {
        isBrokenDown = false;

        // Restore car acceleration and max speed
        acceleration = 8f;
        maxSpeed = 25f;

        // Reset speed to 0 so it doesn't suddenly jump when repaired
        currentSpeed = 0f;

        // Stop breakdown particle effect
        if (breakdownEffect && breakdownEffect.isPlaying)
        {
            breakdownEffect.Stop();
        }

        // Hide repair UI
        if (repairPrompt) repairPrompt.SetActive(false);
    }



    //void StabilizeCar()
    //{
    //    Vector3 upVector = transform.up;
    //    Vector3 targetUp = Vector3.up;
    //    Quaternion targetRotation = Quaternion.FromToRotation(upVector, targetUp) * transform.rotation;
    //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    //}

    //void ApplyDownforce()
    //{
    //    if (!isBraking)
    //    {
    //        float downforce = 10f * (1 - traction);
    //        GetComponent<Rigidbody>().AddForce(Vector3.down * downforce, ForceMode.Acceleration);
    //    }
    //}

    //bool IsGrounded()
    //{
    //    RaycastHit hit;
    //    return Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit, 1.2f);
    //}
}
