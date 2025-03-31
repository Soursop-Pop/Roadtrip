using UnityEngine;

public class CarCameraFollow : MonoBehaviour
{
    public Transform target;  // Assign your car (or better, cameraFollowPoint)
    public Vector3 offset = new Vector3(0, 3, -6); // Position behind & above car
    public float followSpeed = 5f; // How smoothly the camera follows
    public float turnLag = 2f; // How much delay in camera rotation
    public float maxTurnLag = 30f; // Max angle lag before catching up

    private Vector3 velocity = Vector3.zero; // Used for smooth damp
    private Quaternion currentRotation;
    private float yawAngle = 0f;

    void LateUpdate()
    {
        if (!target) return;

        // 1️ Smoothly move the camera toward the target position
        Vector3 targetPosition = target.position + target.TransformDirection(offset);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 1 / followSpeed);

        // 2️ Camera LAGGED Rotation: Delay camera's yaw rotation for a cinematic feel
        float targetYaw = target.eulerAngles.y;
        yawAngle = Mathf.LerpAngle(yawAngle, targetYaw, Time.deltaTime * turnLag);

        // 3️ Prevent too much lag in fast turns (catches up faster if car turns quickly)
        float angleDiff = Mathf.DeltaAngle(transform.eulerAngles.y, yawAngle);
        if (Mathf.Abs(angleDiff) > maxTurnLag)
        {
            yawAngle = targetYaw; // Snap closer if turning too fast
        }

        // 4️ Apply rotation with lag
        currentRotation = Quaternion.Euler(0, yawAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, currentRotation, Time.deltaTime * turnLag);
    }
}