#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;

public static class StorePrefabAudit
{
    [MenuItem("Obsidian Protocol/Store/AUDIT PREFAB SOURCE ROOTS")]
    private static void Audit()
    {
        var scene = SceneManager.GetActiveScene();

        if (scene.path != "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity")
        {
            Debug.LogError("[STORE PREFAB AUDIT] Open SCN16_STORE.unity first. Nothing changed.");
            return;
        }

        var report = new StringBuilder();
        string[] roots = { "STORE_FACILITY", "STORE_AAA_REBUILD" };

        foreach (string rootName in roots)
        {
            GameObject root = GameObject.Find(rootName);
            report.AppendLine("\n===== " + rootName + " =====");

            if (root == null)
            {
                report.AppendLine("NOT FOUND");
                continue;
            }

            var renderers = root.GetComponentsInChildren<Renderer>(true);
            var lights = root.GetComponentsInChildren<Light>(true);
            var colliders = root.GetComponentsInChildren<Collider>(true);
            var behaviours = root.GetComponentsInChildren<MonoBehaviour>(true);

            int enabledRenderers = 0;
            foreach (var renderer in renderers)
                if (renderer.enabled && renderer.gameObject.activeInHierarchy)
                    enabledRenderers++;

            int enabledLights = 0;
            foreach (var light in lights)
                if (light.enabled && light.gameObject.activeInHierarchy)
                    enabledLights++;

            int enabledColliders = 0;
            foreach (var collider in colliders)
                if (collider.enabled && collider.gameObject.activeInHierarchy)
                    enabledColliders++;

            report.AppendLine("Active in hierarchy: " + root.activeInHierarchy);
            report.AppendLine("Child objects: " + root.GetComponentsInChildren<Transform>(true).Length);
            report.AppendLine("Renderers enabled in hierarchy: " + enabledRenderers + " / " + renderers.Length);
            report.AppendLine("Lights enabled in hierarchy: " + enabledLights + " / " + lights.Length);
            report.AppendLine("Colliders enabled in hierarchy: " + enabledColliders + " / " + colliders.Length);
            report.AppendLine("Attached scripts:");

            foreach (var behaviour in behaviours)
                report.AppendLine("  " + (behaviour == null
                    ? "(missing script)"
                    : behaviour.GetType().FullName + " @ " + GetPath(behaviour.transform, root.transform)));

            report.AppendLine("Top-level children:");
            foreach (Transform child in root.transform)
                report.AppendLine("  " + child.name + " [activeSelf=" + child.gameObject.activeSelf + "]");
        }

        Debug.Log("[STORE PREFAB AUDIT]\n" + report);
        EditorUtility.DisplayDialog(
            "Store prefab audit complete",
            "Read-only audit finished. Open Window > General > Console and expand [STORE PREFAB AUDIT]. No scene objects were changed.",
            "OK");
    }

    private static string GetPath(Transform item, Transform root)
    {
        string path = item.name;
        while (item.parent != null && item.parent != root)
        {
            item = item.parent;
            path = item.name + "/" + path;
        }
        return path;
    }
}
#endif
