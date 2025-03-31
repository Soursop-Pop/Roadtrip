using UnityEngine;

public class SnackGravity : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public bool isHeld = false; // Indicates if this snack is being held

    public Transform SecondScreen;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set non-held snacks to be inert initially.
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
    }

    void OnMouseDown()
    {
        isHeld = true;

        // Activate physics for the held snack.
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;

        // Change sorting order so this snack appears on top.
        spriteRenderer.sortingOrder = 1;

        // Reparent the snack so it isn’t affected by disabling the shelf container.
        transform.SetParent(SecondScreen, false);


        // Switch to the arms view.
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.ActivateArmsLayer();
        }
    }

    void OnMouseDrag()
    {
        // Follow the mouse position.
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        transform.position = mousePos;
    }

    void OnMouseUp()
    {
        isHeld = false;
    }

    // Optionally, activate physics on collision for non-held snacks.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isHeld && rb.bodyType == RigidbodyType2D.Kinematic)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1f;
        }
    }
}