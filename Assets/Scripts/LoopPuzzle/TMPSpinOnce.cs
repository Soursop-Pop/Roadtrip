using System.Collections;
using UnityEngine;

public class TMPSpinOnce : MonoBehaviour
{
    public float duration = 1f; // Total time for the spin
    private bool hasSpun = false;

    void OnEnable()
    {
        if (!hasSpun)
        {
            StartCoroutine(SpinY());
        }
    }

    private IEnumerator SpinY()
    {
        hasSpun = true;

        float elapsed = 0f;
        float startY = transform.eulerAngles.y;
        float endY = startY + 360f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float eased = Mathf.SmoothStep(0f, 1f, t);
            float currentY = Mathf.Lerp(startY, endY, eased);

            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                currentY,
                transform.eulerAngles.z
            );

            yield return null;
        }

        // Snap to final angle for precision
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            endY % 360f,
            transform.eulerAngles.z
        );
    }
}