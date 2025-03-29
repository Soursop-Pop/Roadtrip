using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TwoDCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = 9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isDriving = false;
    public GameObject currentVehicle;

    public List<GameObject> companions = new List<GameObject>();

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
        Vector3 moveDirection = new Vector3(horizontal, 0, 0).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            transform.rotation = Quaternion.Euler(0, horizontal > 0 ? 90 : -90, 0);
            Vector3 move = moveDirection * moveSpeed * Time.deltaTime;
            controller.Move(move);
        }
        else
        {
            
            
            // Face forward toward the screen when idle
            transform.rotation = Quaternion.Euler(0, 180, 0);
            

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
        if (currentVehicle == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            EnterVehicle(currentVehicle);
        }
    }

    void EnterVehicle(GameObject vehicle)
    {
        isDriving = true;
        controller.enabled = false;
        gameObject.SetActive(false);
        vehicle.GetComponent<CarController2D>().EnterVehicle();

        currentVehicle = null;
        Camera.main.GetComponent<CameraFollow2D>().SwitchToVehicle();

        if (companions != null)
        {
            foreach (GameObject companion in companions)
            {
                if (companion != null)
                    companion.SetActive(false);
            }
        }
    }

    public void ExitVehicle(GameObject vehicle)
    {
        isDriving = false;
        gameObject.SetActive(true);

        Vector3 exitPosition = vehicle.transform.position + new Vector3(6f, 2f, 0);
        transform.position = exitPosition;

        controller.enabled = true;
        velocity = Vector3.zero;

        Camera.main.GetComponent<CameraFollow2D>().SwitchToPlayer(); //  should switch here

        if (companions != null)
        {
            foreach (GameObject companion in companions)
            {
                if (companion != null)
                    companion.SetActive(true);
            }
        }
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

    private void RestartScene()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
