using UnityEngine;
using UnityEditor;
using System.IO;

public static class MainMenuTextureBuilder
{
    private const string Folder = "Assets/Art/Textures/MainMenuGenerated";

    [MenuItem("Obsidian Protocol/Main Menu/Build Textured Materials")]
    public static void Build()
    {
        if (!AssetDatabase.IsValidFolder(Folder))
        {
            if (!AssetDatabase.IsValidFolder("Assets/Art/Textures"))
                AssetDatabase.CreateFolder("Assets/Art", "Textures");
            AssetDatabase.CreateFolder("Assets/Art/Textures", "MainMenuGenerated");
        }

        CreateTexture("WallMetal", 256, (x,y) =>
        {
            float seam = (x % 64 < 3 || y % 64 < 3) ? 0.48f : 1f;
            float noise = 0.94f + Mathf.PerlinNoise(x * .08f, y * .08f) * .12f;
            return new Color(.105f * seam * noise, .14f * seam * noise, .16f * seam * noise, 1);
        });

        CreateTexture("FloorPlates", 256, (x,y) =>
        {
            float seam = (x % 32 < 2 || y % 32 < 2) ? .35f : 1f;
            float stripe = (x % 64 < 5 && y % 64 < 5) ? .18f : 1f;
            float noise = .92f + Mathf.PerlinNoise(x * .11f, y * .11f) * .12f;
            float c = .13f * seam * stripe * noise;
            return new Color(c, c * 1.12f, c * 1.2f, 1);
        });

        CreateTexture("CyanEmission", 64, (x,y) =>
        {
            float pulse = (x % 16 < 2 || y % 16 < 2) ? .3f : 1f;
            return new Color(.02f * pulse, .65f * pulse, .8f * pulse, 1);
        });

        Material wall = CreateMaterial("BrushedObsidian", "WallMetal", .78f, .72f);
        Material floor = CreateMaterial("ArmoredFloor", "FloorPlates", .62f, .55f);
        Material glow = CreateMaterial("CyanEmission", "CyanEmission", .25f, .1f);
        if (glow.HasProperty("_EmissionColor"))
        {
            glow.EnableKeyword("_EMISSION");
            glow.SetColor("_EmissionColor", new Color(.02f, .8f, 1f) * 5f);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Main Menu Textures] Created wall, floor and cyan emissive materials at " + Folder +
                  ". Inspect these assets before applying them to the scene.");
    }

    private static void CreateTexture(string name, int size, System.Func<int,int,Color> pixel)
    {
        string path = Folder + "/" + name + ".png";
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, true);
        texture.wrapMode = TextureWrapMode.Repeat;
        texture.filterMode = FilterMode.Trilinear;

        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                texture.SetPixel(x, y, pixel(x, y));

        texture.Apply(true, false);
        File.WriteAllBytes(path, texture.EncodeToPNG());
        Object.DestroyImmediate(texture);

        AssetDatabase.ImportAsset(path);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Default;
            importer.wrapMode = TextureWrapMode.Repeat;
            importer.SaveAndReimport();
        }
    }

    private static Material CreateMaterial(string name, string textureName, float metallic, float smoothness)
    {
        string path = Folder + "/" + name + ".mat";
        Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (material == null)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            material = new Material(shader);
            AssetDatabase.CreateAsset(material, path);
        }

        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(Folder + "/" + textureName + ".png");
        if (material.HasProperty("_BaseMap")) material.SetTexture("_BaseMap", texture);
        if (material.HasProperty("_MainTex")) material.SetTexture("_MainTex", texture);
        if (material.HasProperty("_Metallic")) material.SetFloat("_Metallic", metallic);
        if (material.HasProperty("_Smoothness")) material.SetFloat("_Smoothness", smoothness);
        EditorUtility.SetDirty(material);
        return material;
    }
}

