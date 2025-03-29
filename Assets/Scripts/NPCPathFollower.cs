using UnityEngine;
using System.Collections;

public class NPCPathFollower : MonoBehaviour
{
    public Transform[] pathNodes;
    public float moveSpeed = 2f;
    public float reachThreshold = 0.1f;
    public bool startOnTrigger = false;

    public bool isMoving = false;
    private int currentNodeIndex = 0;

    //void OnTriggerEnter(Collider other)
    //{

    //    Debug.Log("NPC trigger collider is happening.");

    //    if (startOnTrigger && other.CompareTag("Vehicle"))
    //    {
    //        Debug.Log("NPC trigger entered by Vehicle. Starting movement.");
    //        StartMovement();
    //    }
    //}

    void Update()
    {
        if (startOnTrigger && !isMoving)
        {
            Debug.Log("NPC trigger entered by Vehicle. Starting movement.");
            StartMovement();
        }
    }

    public void StartMovement()
    {
        if (!isMoving && pathNodes.Length > 0)
        {
            Debug.Log("NPC beginning path movement.");
            isMoving = true;
            StartCoroutine(FollowPath());
        }
        else if (isMoving)
        {
            Debug.Log("NPC is already moving. Ignoring start request.");
        }
        else
        {
            Debug.LogWarning("NPC has no path nodes assigned!");
        }
    }

    IEnumerator FollowPath()
    {
        while (currentNodeIndex < pathNodes.Length)
        {
            Transform targetNode = pathNodes[currentNodeIndex];
            Debug.Log("NPC moving toward node: " + targetNode.name);

            while (Vector3.Distance(transform.position, targetNode.position) > reachThreshold)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetNode.position,
                    moveSpeed * Time.deltaTime
                );
                transform.LookAt(targetNode.position);
                yield return null;
            }

            Debug.Log("NPC reached node: " + targetNode.name);
            currentNodeIndex++;
            yield return null;
        }

        isMoving = false;
        Debug.Log("NPC finished path.");
    }
}