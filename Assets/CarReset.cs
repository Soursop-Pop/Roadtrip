using UnityEngine;

public class CarReset : MonoBehaviour
{
    // Height offset to start the raycast above the car.
    public float rayHeight = 10f;
    // Maximum raycast distance.
    public float rayDistance = 100f;

    // Variables to store the last hit point for Gizmos.
    private Vector3 lastHitPoint;
    private bool hitDetected = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Calculate the ray origin above the car.
            Vector3 rayOrigin = transform.position + Vector3.up * rayHeight;

            // Draw a red debug ray in the Scene view for 2 seconds.
            Debug.DrawRay(rayOrigin, Vector3.down * rayDistance, Color.red, 2f);

            // Perform the raycast downward.
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayDistance))
            {
                Debug.Log("Searching for Road");
                // Check if the hit collider belongs to the road (ensure the road has the "Road" tag).
                if (hit.collider.CompareTag("Road"))
                {
                    // Reset the car's position to the hit point on the road.
                    transform.position = hit.point;

                    // Store hit data for Gizmos.
                    lastHitPoint = hit.point;
                    hitDetected = true;
                    Debug.Log("Found Road");
                }
                else
                {
                    hitDetected = false;
                }
            }
            else
            {
                hitDetected = false;
            }
        }
    }

    // Draw Gizmos in the editor for additional debugging visualization.
    void OnDrawGizmos()
    {
        // Define the ray origin for Gizmos.
        Vector3 rayOrigin = transform.position + Vector3.up * rayHeight;

        // Draw the full ray as a red line.
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rayOrigin, rayOrigin + Vector3.down * rayDistance);

        // If a hit was detected, draw a green sphere at the hit point.
        if (hitDetected)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(lastHitPoint, 0.5f);
        }
    }
}
