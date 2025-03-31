using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    public Transform target; // Your character
    public Vector3 offset = new Vector3(0, 2.5f, -4f); // Above and behind
    public float followSpeed = 5f; // Smoothing for position
    public float lookSpeed = 10f; // How quickly the camera looks at the player

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (!target) return;

        // 1️⃣ Smoothly move the camera to the desired position behind the player
        Vector3 targetPosition = target.position + target.TransformDirection(offset);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 1f / followSpeed);

        // 2️⃣ Always look at the player
        Vector3 lookDirection = target.position - transform.position;
        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lookSpeed);
        }
    }
}