using UnityEngine;

public class CharacterBounceVisual : MonoBehaviour
{
    public Transform target; // The root player object to follow
    public float bounceHeight = 0.1f;
    public float bounceSpeed = 8f;
    public float wiggleAmount = 0.05f;
    public float wiggleSpeed = 12f;

    private Vector3 originalLocalPos;
    private float bounceTimer;

    void Start()
    {
        originalLocalPos = transform.localPosition;
    }

    void Update()
    {
        if (target == null) return;

        // Detect if moving
        float horizontalInput = Input.GetAxis("Horizontal");
        bool isMoving = Mathf.Abs(horizontalInput) > 0.01f;

        if (isMoving)
        {
            bounceTimer += Time.deltaTime;

            float bounce = Mathf.Sin(bounceTimer * bounceSpeed) * bounceHeight;
            float wiggle = Mathf.Sin(bounceTimer * wiggleSpeed) * wiggleAmount;

            transform.localPosition = originalLocalPos + new Vector3(wiggle, bounce, 0f);
        }
        else
        {
            // Reset when standing still
            bounceTimer = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalLocalPos, Time.deltaTime * 5f);
        }
    }
}