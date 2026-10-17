using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class FixStorePinkMaterials
{
    private const string DoneKey = "ObsidianProtocol.StorePinkMaterialsFixed";

    static FixStorePinkMaterials()
    {
        EditorApplication.delayCall += Fix;
    }

    private static void Fix()
    {
        if (EditorPrefs.GetBool(DoneKey, false))
            return;

        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
        {
            Debug.LogError("[STORE PINK FIX] URP/Lit shader not found. No materials changed.");
            return;
        }

        string[] ids = AssetDatabase.FindAssets(
            "t:Material", new[] { "Assets/Materials/StoreExtracted" });

        int fixedCount = 0;

        foreach (string id in ids)
        {
            string path = AssetDatabase.GUIDToAssetPath(id);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (material == null || material.shader == shader)
                continue;

            Undo.RecordObject(material, "Fix Store Pink Material");
            material.shader = shader;
            EditorUtility.SetDirty(material);
            fixedCount++;
        }

        AssetDatabase.SaveAssets();
        EditorPrefs.SetBool(DoneKey, true);

        Debug.Log($"[STORE PINK FIX] Complete. Materials switched to URP/Lit: {fixedCount}. Check the Store in Scene view and Game view.");
    }
}
