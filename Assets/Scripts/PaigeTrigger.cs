using Unity.VisualScripting;
using UnityEngine;

public class PaigeTrigger : MonoBehaviour
{
    public bool paigeMove = false;
    public NPCPathFollower npcPathFollower;

    void Start()
    {
        npcPathFollower = FindAnyObjectByType<NPCPathFollower>();
        paigeMove = FindAnyObjectByType<NPCPathFollower>().startOnTrigger;
        Debug.Log("PaigeTrigger initialized. StartOnTrigger: " + paigeMove);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!paigeMove && other.CompareTag("Vehicle"))
        {
            paigeMove = true;
            npcPathFollower.startOnTrigger = true;
            Debug.Log("Player triggered PaigeTrigger. NPC StartOnTrigger set to true.");
        }
    }
}