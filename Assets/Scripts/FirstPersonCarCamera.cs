using UnityEngine;

public class FirstPersonCarCamera : MonoBehaviour
{
    public float lookSpeed = 3f;
    private float yaw = 0f;
    private float pitch = 0f;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -80f, 80f); // Prevent flipping

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
