using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class BlackspireTerrainRaycastDiagnostic
{
    [MenuItem("Obsidian Protocol/Battlefield/DIAGNOSE TERRAIN RAYCAST")]
    public static void Diagnose()
    {
        GameObject terrain = GameObject.Find("MAP001_BLACKSPIRE_EXPANSE_TERRAIN");

        if (terrain == null)
        {
            Debug.LogError("[TERRAIN DIAGNOSTIC] Terrain object not found.");
            return;
        }

        MeshCollider collider = terrain.GetComponent<MeshCollider>();

        if (collider == null)
        {
            Debug.LogError("[TERRAIN DIAGNOSTIC] MeshCollider missing.");
            return;
        }

        Debug.Log("[TERRAIN DIAGNOSTIC] Scene: " + SceneManager.GetActiveScene().path);
        Debug.Log("[TERRAIN DIAGNOSTIC] Enabled: " + collider.enabled +
                  " | Active: " + collider.gameObject.activeInHierarchy +
                  " | Convex: " + collider.convex +
                  " | Mesh: " + (collider.sharedMesh != null));

        Debug.Log("[TERRAIN DIAGNOSTIC] Transform position: " + terrain.transform.position +
                  " | Scale: " + terrain.transform.lossyScale +
                  " | Collider bounds: " + collider.bounds);

        if (collider.sharedMesh != null)
            Debug.Log("[TERRAIN DIAGNOSTIC] Mesh local bounds: " + collider.sharedMesh.bounds);

        Physics.SyncTransforms();

        Vector3[] samples =
        {
            new Vector3(450f, 500f, 490f),
            new Vector3(470f, 500f, 490f),
            new Vector3(490f, 500f, 490f),
            new Vector3(540f, 500f, 535f),
            new Vector3(560f, 500f, 535f),
            new Vector3(580f, 500f, 535f),
            collider.bounds.center + Vector3.up * 500f
        };
        foreach (Vector3 origin in samples)
        {
            Ray ray = new Ray(origin, Vector3.down);
            RaycastHit directHit;

            bool direct = collider.Raycast(ray, out directHit, 1000f);

            RaycastHit[] hits = Physics.RaycastAll(ray, 1000f, ~0, QueryTriggerInteraction.Ignore);

            Debug.Log("[TERRAIN RAY TEST] Origin: " + origin +
                      " | Collider.Raycast: " + direct +
                      (direct ? " | Direct hit: " + directHit.point : "") +
                      " | Physics hits: " + hits.Length);

            foreach (RaycastHit hit in hits)
                Debug.Log("[TERRAIN PHYSICS HIT] Object: " + hit.collider.name +
                          " | Is terrain: " + (hit.collider == collider) +
                          " | Point: " + hit.point);
        }

        Debug.Log("[TERRAIN DIAGNOSTIC] Complete. No scene changes made.");
    }
}

