using UnityEditor;
using UnityEngine;

public static class BlackspireModelBoundsInspector
{
    [MenuItem("Obsidian Protocol/Diagnostics/Inspect Blackspire FBX Bounds")]
    public static void Inspect()
    {
        const string path =
            "Assets/Art/Models/Map designs/Blackspire Expanse/Meshy_AI_Realistic_sci_fi_mili_1015104137_texture.fbx";

        GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        if (model == null)
        {
            Debug.LogError("[BLACKSPIRE BOUNDS] FBX could not be loaded.");
            return;
        }

        Debug.Log("[BLACKSPIRE ROOT] Name=" + model.name +
                  " | Position=" + model.transform.localPosition +
                  " | Rotation=" + model.transform.localEulerAngles +
                  " | Scale=" + model.transform.localScale);

        foreach (MeshFilter filter in model.GetComponentsInChildren<MeshFilter>(true))
        {
            if (filter.sharedMesh == null) continue;

            Debug.Log("[BLACKSPIRE MESH] " + filter.name +
                      " | Local center=" + filter.sharedMesh.bounds.center.ToString("F3") +
                      " | Local size=" + filter.sharedMesh.bounds.size.ToString("F3") +
                      " | Child position=" + filter.transform.localPosition +
                      " | Child rotation=" + filter.transform.localEulerAngles +
                      " | Child scale=" + filter.transform.localScale);
        }
    }
}
