using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VehicleController : MonoBehaviour
{
    [Header("Speed & Acceleration")]
    public float maxSpeed = 15f;
    public float acceleration = 20f;
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

    [Header("Miscellaneous")]
    private bool isPlayerInside = false;
    private GameObject player;
    private bool isAutoDriving = false;
    private float currentSpeed = 0f;
    private float turnInput = 0f;
    private bool isBraking = false;
    private Quaternion targetCameraRotation;
    private bool isBrokenDown = false;
    private float entryTime = 0f;
    public float exitDelay = 0.2f; // Delay before allowing exit input

    public GameObject repairPrompt;       // UI for "Press F to Fix"
    public ParticleSystem breakdownEffect; // Particle effect for breakdown

    [Header("FMOD & Music")]
    public EventReference carMusicEvent;   // Assign FMOD event path in the Inspector
    private FMOD.Studio.EventInstance carMusicInstance;
    public MusicPlayer musicPlayer;

    [Header("Stabilization & Downforce")]
    public float stabilizationSpeed = 2f;      // How quickly the car rotates back upright
    public float desiredGroundDistance = 0.5f;   // Desired gap from the car's base to the ground
    public float downforceStrength = 2f;         // How strongly to push the car down when airborne

    public Unity.Cinemachine.CinemachineCamera cam1;
    public Unity.Cinemachine.CinemachineCamera cam2;

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
            isAutoDriving = !isAutoDriving;

        //RestartScene();
        //UpdateCamera();

        // Stabilize the car's rotation so it doesn't flip over
        StabilizeCar();

        // Apply a simulated downforce if the car is too high above the ground
        ApplyDownforce();
    }

    void Drive()
    {
        if (isBrokenDown) return; // No movement if broken down

        float accelerationInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
        isBraking = Input.GetKey(KeyCode.Space);

        float direction = Mathf.Sign(currentSpeed);

        if (isBraking)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, brakeStrength * Time.deltaTime);
        }
        else if (accelerationInput > 0)
        {
            // For a better sensation of acceleration, we use Lerp to give a more responsive boost
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        }
        else if (accelerationInput < 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, -reverseSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        // Move forward based on the current speed
        if (Mathf.Abs(currentSpeed) > 0.1f)
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        // Handle steering based on input and speed
        if (Mathf.Abs(currentSpeed) > 0.2f)
        {
            float steerAmount = turnInput * turnSpeed * Time.deltaTime;
            float speedFactor = Mathf.Clamp01(Mathf.Abs(currentSpeed) / maxSpeed);
            steerAmount *= (1 - speedFactor * speedSteerFactor);
            if (isBraking && Mathf.Abs(turnInput) > 0.1f)
                steerAmount *= brakingRotationFactor;

            transform.Rotate(Vector3.up * steerAmount * direction);
        }

        // Simulate traction by adjusting the forward velocity
        Vector3 velocity = transform.forward * currentSpeed;
        Vector3 lateralVelocity = Vector3.Project(velocity, transform.right);
        velocity -= lateralVelocity * (1f - traction);
        transform.position += velocity * Time.deltaTime;
    }

    void AutoDrive()
    {
        if (isBrokenDown) return;

        currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed * 1f, acceleration * Time.deltaTime);
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
        if (Time.time - entryTime < exitDelay)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            ExitVehicle();

        }
    }

    public void EnterVehicle(GameObject playerObj)
    {
        isPlayerInside = true;
        player = playerObj;
        entryTime = Time.time;
        CameraManager.SwitchToCarCamera();

        // Enable the MusicPlayer component
        if (musicPlayer != null)
        {
            musicPlayer.enabled = true;
            Debug.Log("MusicPlayer enabled");
        }
    }

    void ExitVehicle()
    {
        isPlayerInside = false;
        player.transform.SetParent(null);
        player.SetActive(true);
        player.GetComponent<ThirdPersonController>().ExitVehicle(gameObject);
        CameraManager.SwitchToPlayerCamera();

        // Disable the MusicPlayer component
        if (musicPlayer != null)
        {
            musicPlayer.enabled = false;
            Debug.Log("MusicPlayer disabled");
        }
    }

    public void StartCarMusic()
    {
        carMusicInstance = RuntimeManager.CreateInstance(carMusicEvent);
        carMusicInstance.start();
    }

    public void StopCarMusic()
    {
        carMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        carMusicInstance.release();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Breakdown"))
            Breakdown(other);
    }

    void Breakdown(Collider breakdownTrigger)
    {
        if (isBrokenDown) return;

        isBrokenDown = true;
        acceleration = 0f; // Disable further acceleration
        isBraking = true;  // Simulate braking
        if (breakdownEffect && !breakdownEffect.isPlaying)
            breakdownEffect.Play();
        if (repairPrompt)
            repairPrompt.SetActive(true);
        breakdownTrigger.gameObject.SetActive(false);
    }

    public void Repair()
    {
        isBrokenDown = false;
        acceleration = 20f;
        maxSpeed = 15f;
        currentSpeed = 0f;
        if (breakdownEffect && breakdownEffect.isPlaying)
            breakdownEffect.Stop();
        if (repairPrompt)
            repairPrompt.SetActive(false);
    }

    //private void RestartScene()
    //{
    //    if (Input.GetKeyDown(KeyCode.R))
    //        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}

    // This function gently forces the car to remain upright.
    void StabilizeCar()
    {
        Vector3 currentEuler = transform.rotation.eulerAngles;
        // Lerp the X and Z angles back toward 0 (keeping the Y rotation intact).
        float stabilizedX = Mathf.LerpAngle(currentEuler.x, 0, Time.deltaTime * stabilizationSpeed);
        float stabilizedZ = Mathf.LerpAngle(currentEuler.z, 0, Time.deltaTime * stabilizationSpeed);
        transform.rotation = Quaternion.Euler(stabilizedX, currentEuler.y, stabilizedZ);
    }

    // This function casts a ray downward; if the car is too far from the ground, it nudges it downward.
    void ApplyDownforce()
    {
        Ray ray = new Ray(transform.position, -Vector3.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, .5f)) // Check within 5 units
        {
            float distance = hit.distance;
            if (distance > desiredGroundDistance)
            {
                float forceAmount = (distance - desiredGroundDistance) * downforceStrength * Time.deltaTime;
                transform.Translate(Vector3.down * forceAmount, Space.World);
            }
        }
    }
}