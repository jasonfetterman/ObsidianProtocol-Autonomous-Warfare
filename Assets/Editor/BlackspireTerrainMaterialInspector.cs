using UnityEditor;
using UnityEngine;

public static class BlackspireTerrainMaterialInspector
{
    [MenuItem("Obsidian Protocol/Diagnostics/Inspect Blackspire Terrain Material")]
    public static void Inspect()
    {
        const string path =
            "Assets/Art/Models/Map designs/Blackspire Expanse/Meshy_AI_Realistic_sci_fi_mili_1015104137_texture.fbx";

        GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        if (model == null)
        {
            Debug.LogError("[BLACKSPIRE MATERIAL CHECK] FBX not found: " + path);
            return;
        }

        foreach (Renderer renderer in model.GetComponentsInChildren<Renderer>(true))
        {
            Debug.Log("[BLACKSPIRE MODEL RENDERER] " + renderer.name +
                      " | Bounds=" + renderer.bounds +
                      " | MaterialCount=" + renderer.sharedMaterials.Length);

            foreach (Material material in renderer.sharedMaterials)
            {
                if (material == null) continue;

                Debug.Log("[BLACKSPIRE MODEL MATERIAL] Name=" + material.name +
                          " | Shader=" + (material.shader ? material.shader.name : "(none)") +
                          " | RenderQueue=" + material.renderQueue +
                          " | HasBaseColor=" + material.HasProperty("_BaseColor") +
                          " | HasColor=" + material.HasProperty("_Color") +
                          " | HasEmission=" + material.HasProperty("_EmissionColor") +
                          " | HasBaseMap=" + material.HasProperty("_BaseMap") +
                          " | HasMainTexture=" + material.HasProperty("_MainTex"));

                if (material.HasProperty("_BaseColor"))
                    Debug.Log("[BLACKSPIRE BASE COLOR] " + material.GetColor("_BaseColor"));

                if (material.HasProperty("_Color"))
                    Debug.Log("[BLACKSPIRE COLOR] " + material.GetColor("_Color"));

                if (material.HasProperty("_EmissionColor"))
                    Debug.Log("[BLACKSPIRE EMISSION] " + material.GetColor("_EmissionColor"));

                if (material.HasProperty("_BaseMap"))
                    Debug.Log("[BLACKSPIRE BASE MAP] " +
                              (material.GetTexture("_BaseMap")
                                  ? material.GetTexture("_BaseMap").name
                                  : "(none)"));
            }
        }

        Debug.Log("[BLACKSPIRE MATERIAL CHECK] Read-only inspection complete.");
    }
}
