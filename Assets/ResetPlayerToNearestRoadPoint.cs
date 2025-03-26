using UnityEngine;
using EasyRoads3Dv3;

public class ResetPlayerToNearestRoadPoint : MonoBehaviour
{
    public Transform player;

    private ERRoadNetwork roadNetwork;
    private ERRoad road;

    void Start()
    {
        roadNetwork = new ERRoadNetwork();

        // Get all road objects from the scene (from the Road Network)
        ERRoad[] allRoads = roadNetwork.GetRoads();

        if (allRoads.Length == 0)
        {
            Debug.LogError("No roads found in ERRoadNetwork.");
            return;
        }

        road = allRoads[0]; // Or pick by name
       
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayer();
        }
    }

    void ResetPlayer()
    {
        Vector3[] markers = road.GetMarkerPositions();

        float closestDistance = float.MaxValue;
        Vector3 closest = Vector3.zero;
        int index = 0;

        for (int i = 0; i < markers.Length; i++)
        {
            float dist = Vector3.Distance(player.position, markers[i]);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = markers[i];
                index = i;
            }
        }

        player.position = closest;

        if (index < markers.Length - 1)
        {
            Vector3 forward = (markers[index + 1] - closest).normalized;
            player.rotation = Quaternion.LookRotation(forward);
        }
    }
}