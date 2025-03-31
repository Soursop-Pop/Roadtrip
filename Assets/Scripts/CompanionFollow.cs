using UnityEngine;

public class CompanionFollow : MonoBehaviour
{
    public Transform target;
    public float followDistance = 1.5f;
    public float followHeight = 0f;
    public float followSpeed = 5f;

    public bool isFollowing = true;

  

    private Renderer[] renderers;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Start()
    {
        
        
    }

    void Update()
    {
        if (target == null) return;

        // Keep updating position always
        float facing = target.eulerAngles.y == 90 ? 1f : -1f;
        Vector3 targetPosition = target.position - new Vector3(facing * followDistance, -followHeight, 0);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    public void SetFollowing(bool follow)
    {
        isFollowing = follow;

        // Toggle only the visuals
        foreach (Renderer r in renderers)
        {
            r.enabled = follow;
        }

        // Optional: also disable Animator, AudioSource, etc.
        Animator anim = GetComponent<Animator>();
        if (anim != null) anim.enabled = follow;
    }
}