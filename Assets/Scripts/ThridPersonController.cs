using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class ThirdPersonController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 10f;
    public float gravity = 9.81f;
    public float jumpHeight = 2f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isDriving = false;
    public GameObject currentVehicle;  


    void Awake()
    {
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (currentVehicle == null)
        {
            currentVehicle = GameObject.Find("Car"); // Replace with your vehicle's actual name
        }
    }




    void Update()
    {
        if (!isDriving)
        {
            MoveCharacter();
            CheckForVehicleEntry();
        }
        RestartScene();
        
    }

    void MoveCharacter()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, targetAngle, 0), turnSpeed * Time.deltaTime);

            Vector3 move = transform.forward * moveSpeed * Time.deltaTime;
            controller.Move(move);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void CheckForVehicleEntry()
    {
        if (currentVehicle == null) return; // Ensure a vehicle is detected

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed - Entering vehicle");
            EnterVehicle(currentVehicle);
        }
    }


    void EnterVehicle(GameObject vehicle)
    {
        isDriving = true;
        controller.enabled = false;
        gameObject.SetActive(false);
        vehicle.GetComponent<VehicleController>().EnterVehicle(gameObject);

        // Clear current vehicle reference
        currentVehicle = null;

        // Switch to car camera
        CameraManager.SwitchToCarCamera();
    }


    public void ExitVehicle(GameObject vehicle)
    {
        isDriving = false;
        gameObject.SetActive(true);

        Vector3 exitPosition = vehicle.transform.position + vehicle.transform.right * 2;
        RaycastHit hit;

        if (Physics.Raycast(vehicle.transform.position, Vector3.down, out hit, 5f))
        {
            exitPosition.y = hit.point.y + 0.5f; // Adjust Y to sit on top of the ground
        }
        else
        {
            exitPosition.y += 1f; // Fallback if no ground is found
        }

        transform.position = exitPosition;

        controller.enabled = true;
        velocity = Vector3.zero; // Prevent falling through the floor

        CameraManager.SwitchToPlayerCamera();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vehicle"))
        {
            Debug.Log("Entered vehicle trigger: " + other.gameObject.name);
            currentVehicle = other.gameObject;
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Vehicle") && other.gameObject == currentVehicle)
        {
            currentVehicle = null;
        }
    }

    private void RestartScene()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
