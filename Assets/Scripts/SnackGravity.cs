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
            rb.gravityScale = 1f;  // Adjust this value as needed
        }
    }

    void OnMouseDrag()
    {
        // Update the snack’s position to follow the mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // Keep the object in the 2D plane
        transform.position = mousePos;
    }

    void OnMouseUp()
    {
        // Mark the snack as no longer held
        isHeld = false;
    }

    // This method triggers when the snack collides with another object.
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object we collided with is also a snack.
        if (collision.gameObject.CompareTag("Snack"))
        {
            // If gravity hasn't been enabled yet, turn it on.
            if (rb.gravityScale == 0f)
            {
                rb.gravityScale = 1f;
            }
        }
    }
}