using UnityEngine;
using UnityEditor;

public static class InspectHarlequinRig
{
    [MenuItem("Obsidian Protocol/Diagnostics/Inspect Harlequin Rig")]
    public static void Inspect()
    {
        const string path = "Assets/Art/Units/Clown Syndicate/Clown_Assault_Mech/Meshy_AI_Harlequin_War_Machine_1018031030_texture.fbx";
        GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        if (model == null)
        {
            Debug.LogError("Harlequin model not found.");
            return;
        }

        MeshFilter filter = model.GetComponent<MeshFilter>();
        MeshRenderer renderer = model.GetComponent<MeshRenderer>();

        if (filter == null || filter.sharedMesh == null)
        {
            Debug.LogError("Harlequin mesh could not be inspected.");
            return;
        }

        Mesh mesh = filter.sharedMesh;

        Debug.Log(
            "=== HARLEQUIN MESH REPORT ===\n" +
            "Mesh: " + mesh.name + "\n" +
            "Vertices: " + mesh.vertexCount + "\n" +
            "Submeshes: " + mesh.subMeshCount + "\n" +
            "Local Bounds Center: " + mesh.bounds.center + "\n" +
            "Local Bounds Size: " + mesh.bounds.size + "\n" +
            "Model Root Position: " + model.transform.position + "\n" +
            "Renderer Bounds Size: " +
                (renderer != null ? renderer.bounds.size.ToString() : "unavailable")
        );
    }
}
