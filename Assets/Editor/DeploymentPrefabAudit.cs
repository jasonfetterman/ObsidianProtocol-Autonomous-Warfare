using UnityEditor;
using UnityEngine;

public static class DeploymentPrefabAudit
{
    [MenuItem("Obsidian Protocol/Deployment/Audit Operation Holo Surface")]
    public static void Audit()
    {
        var all = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (var go in all)
        {
            if (!go.scene.IsValid()) continue;
            if (go.name != "Console_Operation_Selection_Terrminals") continue;

            var renderers = go.GetComponentsInChildren<Renderer>(true);

            Debug.Log("[AUDIT] ===== OPERATION HOLO SURFACE =====");
            Debug.Log("[AUDIT] Renderer Count: " + renderers.Length);

            foreach (var r in renderers)
            {
                Debug.Log("[AUDIT] Renderer: " + r.name);
                Debug.Log("[AUDIT] Bounds Center: " + r.bounds.center);
                Debug.Log("[AUDIT] Bounds Size: " + r.bounds.size);
                Debug.Log("[AUDIT] Bounds Min: " + r.bounds.min);
                Debug.Log("[AUDIT] Bounds Max: " + r.bounds.max);
            }

            return;
        }

        Debug.LogError("[AUDIT] Console object not found.");
    }
}
