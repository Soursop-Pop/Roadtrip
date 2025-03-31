using UnityEngine;
using UnityEditor;

public class DropTreesToTerrain : MonoBehaviour
{
    [MenuItem("Tools/Drop Selected Objects To Terrain")]
    static void DropToTerrain()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            RaycastHit hit;
            Vector3 origin = obj.transform.position + Vector3.up * 1000f;

            if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity))
            {
                string tag = hit.collider.tag;

                if (tag == "Road" || tag == "Water" || tag == "House")
                {
                    Debug.Log($"Deleted {obj.name} - landed on {tag}.");
                    Undo.DestroyObjectImmediate(obj);
                }
                else
                {
                    Undo.RecordObject(obj.transform, "Drop to Terrain");
                    obj.transform.position = new Vector3(
                        obj.transform.position.x,
                        hit.point.y,
                        obj.transform.position.z
                    );
                }
            }
            else
            {
                Debug.Log($"Deleted {obj.name} - no terrain found below.");
                Undo.DestroyObjectImmediate(obj);
            }
        }
    }
}