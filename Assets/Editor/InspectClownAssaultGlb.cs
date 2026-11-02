using UnityEditor;
using UnityEngine;

public static class InspectClownAssaultGlb
{
    [MenuItem("Obsidian Protocol/Diagnostics/Inspect Clown Assault GLB")]
    public static void Inspect()
    {
        const string path = "Assets/Art/Units/Clown Syndicate/Clown_Assault_Mech/textures/autorig-7e7ecd87-v4-glb.glb";
        GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        Debug.Log("[GLB INSPECT] Loaded model: " + (model != null));

        if (model == null)
        {
            Debug.LogError("[GLB INSPECT] Unity did not expose this GLB as a GameObject.");
            return;
        }

        Debug.Log("[GLB INSPECT] Name: " + model.name +
                  " | Children: " + model.transform.childCount +
                  " | Asset: " + AssetDatabase.GetAssetPath(model));

        foreach (MeshFilter mf in model.GetComponentsInChildren<MeshFilter>(true))
            Debug.Log("[GLB INSPECT] Mesh: " + mf.name +
                      " | Vertices: " + (mf.sharedMesh != null ? mf.sharedMesh.vertexCount : 0));

        foreach (SkinnedMeshRenderer smr in model.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            Debug.Log("[GLB INSPECT] Skinned mesh: " + smr.name);
    }
}
