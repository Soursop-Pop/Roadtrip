using UnityEngine;

public class SnackGravity : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool isHeld = false; // Tracks if the snack is currently being held by the player

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Ensure gravity is off at the start
        rb.gravityScale = 0f;
    }

    void OnMouseDown()
    {
        isHeld = true;
        // Enable gravity when the snack is first clicked (if not already enabled)
        if (rb.gravityScale == 0f)
        {
            rb.gravityScale = 1f;  // Adjust this value to suit your game
        }
    }

    void OnMouseDrag()
    {
        // Update the snack’s position to follow the mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // Ensure the object remains in the 2D plane
        transform.position = mousePos;
    }

    void OnMouseUp()
    {
        // When the player releases the mouse button, mark the snack as no longer held.
        isHeld = false;
    }
}