using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform player;
    public Transform vehicle;
    public float smoothSpeed = 5f;

    private Transform target;
    private float fixedY;
    private float fixedZ;

    void Start()
    {
        target = vehicle != null ? vehicle : player;

        fixedY = transform.position.y;
        fixedZ = transform.position.z;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, fixedY, fixedZ);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }

    public void SwitchToVehicle()
    {
        if (vehicle != null)
        {
            target = vehicle;
        }
    }

    public void SwitchToPlayer()
    {
        if (player != null)
        {
            target = player;
        }
    }
}