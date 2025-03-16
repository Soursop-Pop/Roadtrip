using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    public Transform target;  // Player transform
    public Vector3 offset = new Vector3(0, 5, -7);
    public float followSpeed = 5f;

    void LateUpdate()
    {
        if (target)
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
            transform.LookAt(target);
        }
    }
}