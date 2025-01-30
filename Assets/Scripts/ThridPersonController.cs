using UnityEngine;
using UnityEngine.Rendering;

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
    private GameObject currentVehicle;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!isDriving)
        {
            MoveCharacter();
            CheckForVehicleEntry();
        }
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
        if (Input.GetKeyDown(KeyCode.E) && currentVehicle != null)
        {
            EnterVehicle(currentVehicle);
        }
    }

    void EnterVehicle(GameObject vehicle)
    {
        isDriving = true;
        controller.enabled = false;
        gameObject.SetActive(false);
        vehicle.GetComponent<VehicleController>().EnterVehicle(gameObject);

        // Switch to car camera
        CameraManager.SwitchToCarCamera();
    }

    public void ExitVehicle(GameObject vehicle)
    {
        isDriving = false;
        gameObject.SetActive(true);
        transform.position = vehicle.transform.position + vehicle.transform.right * 2;
        controller.enabled = true;

        // Switch back to player camera
        CameraManager.SwitchToPlayerCamera();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vehicle"))
        {
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
}
