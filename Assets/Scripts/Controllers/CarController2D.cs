using UnityEngine;

public class CarController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform player;
    public Transform exitPoint;
    private bool isDriving = true;

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
        float moveInput = Input.GetAxis("Horizontal");
        transform.position += new Vector3(moveInput * moveSpeed * Time.deltaTime, 0, 0);
    }

    void CheckForExit()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ExitVehicle();
        }
    }

    public void ExitVehicle()
    {
        isDriving = false;
        this.enabled = false;

        Vector3 offset = new Vector3(6f, 2f, 0f);
        player.transform.position = transform.position + offset;

        player.gameObject.layer = 0;
        gameObject.layer = 0;

        player.gameObject.SetActive(true);
        player.GetComponent<TwoDCharacterController>().ExitVehicle(gameObject);

        Camera.main.GetComponent<CameraFollow2D>().SwitchToPlayer(); //  camera switches here
    }

    public void EnterVehicle()
    {
        isDriving = true;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        player.gameObject.layer = LayerMask.NameToLayer("PlayerTemp");
        gameObject.layer = LayerMask.NameToLayer("VehicleTemp");

        player.gameObject.SetActive(false);
        Camera.main.GetComponent<CameraFollow2D>().SwitchToVehicle();

        this.enabled = true;
    }
}