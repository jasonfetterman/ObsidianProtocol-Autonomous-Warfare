using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class BlackspireDisplaySurfaceInspector
{
    [MenuItem("Obsidian Protocol/Diagnostics/Inspect Blackspire Display Surface")]
    public static void Inspect()
    {
        const string scenePath =
            "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath, OpenSceneMode.Single);

        GameObject surface = null;

        foreach (GameObject go in scene.GetRootGameObjects())
        {
            foreach (Transform t in go.GetComponentsInChildren<Transform>(true))
            {
                if (t.name == "[MAP] DISPLAY SURFACE")
                {
                    surface = t.gameObject;
                    break;
                }
            }

            if (surface != null) break;
        }

        if (surface == null)
        {
            Debug.LogError("[BLACKSPIRE INSPECT] Display surface not found.");
            return;
        }

        Transform tr = surface.transform;
        Renderer renderer = surface.GetComponent<Renderer>();
        MeshFilter filter = surface.GetComponent<MeshFilter>();

        Debug.Log("[BLACKSPIRE SURFACE] Name=" + surface.name +
                  " | Scene=" + scene.path +
                  " | Parent=" + (tr.parent ? tr.parent.name : "(root)") +
                  " | LocalPosition=" + tr.localPosition +
                  " | WorldPosition=" + tr.position +
                  " | WorldRotation=" + tr.rotation.eulerAngles +
                  " | LocalScale=" + tr.localScale +
                  " | WorldScale=" + tr.lossyScale +
                  " | TopCenter=" + tr.TransformPoint(new Vector3(0f, 0.5f, 0f)));

        if (renderer != null)
        {
            Debug.Log("[BLACKSPIRE RENDERER] Bounds=" + renderer.bounds +
                      " | Enabled=" + renderer.enabled);

            foreach (Material mat in renderer.sharedMaterials)
                if (mat != null)
                    Debug.Log("[BLACKSPIRE MATERIAL] " + mat.name +
                              " | Shader=" + (mat.shader ? mat.shader.name : "(none)"));
        }

        if (filter != null && filter.sharedMesh != null)
            Debug.Log("[BLACKSPIRE MESH] " + filter.sharedMesh.name +
                      " | Bounds=" + filter.sharedMesh.bounds);

        Debug.Log("[BLACKSPIRE INSPECT] Read-only inspection complete.");
    }
}
