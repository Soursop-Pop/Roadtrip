using UnityEngine;
using EasyRoads3Dv3;

public class RoadManager : MonoBehaviour
{
    public Transform player;
    public GameObject thingToSpawn;

    private ERRoadNetwork roadNetwork;
    private ERRoad road;

    void Start()
    {
        roadNetwork = new ERRoadNetwork();
        ERRoad[] allRoads = roadNetwork.GetRoads();

        if (allRoads.Length == 0)
        {
            Debug.LogError("No roads found in ERRoadNetwork.");
            return;
        }

        road = allRoads[0]; // Change this if you want a specific one by name
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPlayerToNearestRoadPoint();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnObjectAheadOfPlayer(10f); // You can expose this distance as a variable if needed
        }
    }

    public void ResetPlayerToNearestRoadPoint()
    {
        if (road == null || player == null) return;

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

    public void SpawnObjectAheadOfPlayer(float forwardDistance)
    {
        if (road == null || player == null || thingToSpawn == null) return;

        Vector3[] points = road.GetMarkerPositions();
        int closestIndex = 0;
        float closestDist = float.MaxValue;

        // Find nearest point on road
        for (int i = 0; i < points.Length; i++)
        {
            float dist = Vector3.Distance(player.position, points[i]);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestIndex = i;
            }
        }

        // Walk forward along spline
        float walked = 0f;
        Vector3 spawnPoint = points[closestIndex];

        for (int i = closestIndex; i < points.Length - 1; i++)
        {
            float segmentLength = Vector3.Distance(points[i], points[i + 1]);

            if (walked + segmentLength >= forwardDistance)
            {
                float ratio = (forwardDistance - walked) / segmentLength;
                spawnPoint = Vector3.Lerp(points[i], points[i + 1], ratio);
                break;
            }

            walked += segmentLength;
        }

        Instantiate(thingToSpawn, spawnPoint, Quaternion.identity);
    }
}
