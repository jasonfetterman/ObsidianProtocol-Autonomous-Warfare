using UnityEditor;
using UnityEngine;

public static class SCN17TronWallMaterialSetup
{
    [MenuItem("Obsidian Protocol/SCN17/Create TRON Wall Material")]
    public static void Create()
    {
        const string texturePath = "Assets/Materials/SCN17/Neon Cyan Futuristic Wall Panels.png";
        const string materialPath = "Assets/Materials/SCN17/SCN17_TRON_Wall.mat";

        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");

        if (!texture || !shader)
        {
            EditorUtility.DisplayDialog("TRON Wall", "Texture or URP Lit shader not found. No material created.", "OK");
            return;
        }

        Material material = new Material(shader);
        material.name = "SCN17 TRON Wall";
        material.SetTexture("_BaseMap", texture);
        material.SetColor("_BaseColor", Color.white);

        if (material.HasProperty("_Smoothness"))
            material.SetFloat("_Smoothness", 0.35f);

        if (material.HasProperty("_Metallic"))
            material.SetFloat("_Metallic", 0.25f);

        AssetDatabase.CreateAsset(material, materialPath);
        AssetDatabase.SaveAssets();
        EditorUtility.DisplayDialog("TRON Wall", "Created SCN17 TRON Wall material. Existing scene objects were not changed.", "OK");
        Selection.activeObject = material;
    }
}

