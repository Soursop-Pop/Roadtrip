using UnityEngine;

public class SnackCollision : MonoBehaviour
{
    private bool delivered = false;  // Ensure delivery is only processed once

    public FriendArms friendArms;

    void Start()
    {
        // Find the FriendArms script in the scene.
        friendArms = FindFirstObjectByType<FriendArms>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (delivered)
            return;

        if (other.CompareTag("FriendArms"))
        {
            delivered = true;

            // Update friend arms UI with the snack name.
            if (friendArms == null)
                friendArms = FindFirstObjectByType<FriendArms>();
            if (friendArms != null)
                friendArms.AddSnack(gameObject.name);

            // Notify the GameFlowManager that a snack has been delivered.
            if (GameFlowManager.Instance != null)
                GameFlowManager.Instance.OnSnackDelivered();

            // Reparent the delivered snack to the ArmsContainer.
            if (GameFlowManager.Instance != null && GameFlowManager.Instance.armsContainer != null)
            {
                transform.SetParent(GameFlowManager.Instance.armsContainer.transform, false);
                // Optionally, you might adjust the local position if needed:
                // transform.localPosition = new Vector3(desiredX, desiredY, 0);
            }
            else
            {
                Debug.LogWarning("ArmsContainer not set on GameFlowManager");
            }
        }
    }
}