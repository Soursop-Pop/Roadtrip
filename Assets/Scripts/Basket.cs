using UnityEngine;
using UnityEngine.UI;

public class Basket : MonoBehaviour
{
    // Assign your UI Text element via the Inspector.
    public Text snackNameText;

    // Called each frame a Collider2D remains inside the trigger collider
    void OnTriggerStay2D(Collider2D other)
    {
        // Process only objects tagged as "Snack"
        if (other.CompareTag("Snack"))
        {
            // Get the SnackGravity component to check if it's still held by the player.
            SnackGravity snackScript = other.GetComponent<SnackGravity>();
            if (snackScript != null && !snackScript.isHeld)
            {
                // Get the snack's name (or use a custom property if you have one)
                string snackName = other.gameObject.name;

                // Update the UI Text element with the snack name.
                if (snackNameText != null)
                {
                    snackNameText.text = snackName;
                }

                // Remove the snack from the scene.
                Destroy(other.gameObject);
            }
        }
    }
}