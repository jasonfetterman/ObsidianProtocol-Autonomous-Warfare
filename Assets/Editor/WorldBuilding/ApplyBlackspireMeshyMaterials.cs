using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ApplyBlackspireMeshyMaterials
{
    private const string Folder =
        "Assets/Art/Models/Map designs/Blackspire Expanse/";
    private const string Prefix =
        "Meshy_AI_Realistic_sci_fi_mili_1015104137_texture";
    private const string ScenePath =
        "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity";
    private const string MaterialPath =
        Folder + "Blackspire_Meshy_Terrain.mat";
    private const string PackedPath =
        Folder + "Blackspire_Metallic_Smoothness.png";

    [MenuItem("Obsidian Protocol/World Building/Apply Blackspire Meshy Materials")]
    public static void Apply()
    {
        string colorPath = Folder + Prefix + ".png";
        string normalPath = Folder + Prefix + "_normal.png";
        string metallicPath = Folder + Prefix + "_metallic.png";
        string roughnessPath = Folder + Prefix + "_roughness.png";

        foreach (string path in new[]
        {
            ScenePath, colorPath, normalPath, metallicPath, roughnessPath
        })
        {
            if (!File.Exists(path))
            {
                Debug.LogError("[BLACKSPIRE MATERIALS] Missing file: " + path);
                return;
            }
        }

        Scene scene = SceneManager.GetSceneByPath(ScenePath);
        if (!scene.IsValid() || !scene.isLoaded)
            scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Additive);

        GameObject terrainRoot = null;
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            if (root.name == "BLACKSPIRE MESHY TERRAIN")
            {
                terrainRoot = root;
                break;
            }
        }

        if (terrainRoot == null)
        {
            Debug.LogError("[BLACKSPIRE MATERIALS] Intended terrain parent not found. Nothing assigned.");
            return;
        }

        Transform model = terrainRoot.transform.Find("MESHY BLACKSPIRE TERRAIN MODEL");
        if (model == null)
        {
            Debug.LogError("[BLACKSPIRE MATERIALS] Intended Meshy model child not found. Nothing assigned.");
            return;
        }

        Renderer renderer = model.GetComponentInChildren<Renderer>(true);
        if (renderer == null)
        {
            Debug.LogError("[BLACKSPIRE MATERIALS] No renderer found on the intended model.");
            return;
        }

        TextureImporter normalImporter =
            AssetImporter.GetAtPath(normalPath) as TextureImporter;
        if (normalImporter != null &&
            normalImporter.textureType != TextureImporterType.NormalMap)
        {
            normalImporter.textureType = TextureImporterType.NormalMap;
            normalImporter.SaveAndReimport();
        }

        Texture2D metallic = MakeReadableLinear(metallicPath);
        Texture2D roughness = MakeReadableLinear(roughnessPath);

        if (metallic == null || roughness == null)
        {
            Debug.LogError("[BLACKSPIRE MATERIALS] Could not load metallic or roughness texture.");
            return;
        }

        int width = metallic.width;
        int height = metallic.height;
        Color[] metalPixels = metallic.GetPixels();
        Color[] roughPixels = roughness.GetPixels();
        Color[] packedPixels = new Color[metalPixels.Length];

        for (int y = 0; y < height; y++)
        {
            int ry = Mathf.Min(roughness.height - 1,
                y * roughness.height / height);

            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;
                int rx = Mathf.Min(roughness.width - 1,
                    x * roughness.width / width);
                Color m = metalPixels[index];
                Color r = roughPixels[ry * roughness.width + rx];

                float metalValue = m.r;
                float smoothness = 1f - r.r;

                packedPixels[index] = new Color(
                    metalValue, metalValue, metalValue, smoothness);
            }
        }

        Texture2D packed = new Texture2D(
            width, height, TextureFormat.RGBA32, true, true);
        packed.SetPixels(packedPixels);
        packed.Apply(true, false);

        File.WriteAllBytes(PackedPath, packed.EncodeToPNG());
        Object.DestroyImmediate(packed);

        AssetDatabase.Refresh();
        AssetDatabase.ImportAsset(PackedPath, ImportAssetOptions.ForceUpdate);

        TextureImporter packedImporter =
            AssetImporter.GetAtPath(PackedPath) as TextureImporter;
        if (packedImporter != null)
        {
            packedImporter.sRGBTexture = false;
            packedImporter.textureType = TextureImporterType.Default;
            packedImporter.SaveAndReimport();
        }

        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
        {
            Debug.LogError("[BLACKSPIRE MATERIALS] URP/Lit shader not found.");
            return;
        }

        Material material = AssetDatabase.LoadAssetAtPath<Material>(MaterialPath);
        if (material == null)
        {
            material = new Material(shader);
            AssetDatabase.CreateAsset(material, MaterialPath);
        }
        else
        {
            material.shader = shader;
        }

        material.SetTexture("_BaseMap",
            AssetDatabase.LoadAssetAtPath<Texture2D>(colorPath));
        material.SetTexture("_BumpMap",
            AssetDatabase.LoadAssetAtPath<Texture2D>(normalPath));
        material.SetFloat("_BumpScale", 1f);
        material.SetTexture("_MetallicGlossMap",
            AssetDatabase.LoadAssetAtPath<Texture2D>(PackedPath));
        material.SetFloat("_Metallic", 1f);
        material.SetFloat("_Smoothness", 1f);
        material.SetFloat("_SmoothnessTextureChannel", 0f);
        material.EnableKeyword("_NORMALMAP");
        material.EnableKeyword("_METALLICSPECGLOSSMAP");

        EditorUtility.SetDirty(material);
        renderer.sharedMaterial = material;
        EditorUtility.SetDirty(renderer);
        EditorSceneManager.MarkSceneDirty(scene);

        AssetDatabase.SaveAssets();
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[BLACKSPIRE MATERIALS] Applied material to " +
            renderer.gameObject.name +
            ". Color, normal, metallic, and inverted roughness packed as smoothness. Scene saved.");
    }

    private static Texture2D MakeReadableLinear(string path)
    {
        TextureImporter importer =
            AssetImporter.GetAtPath(path) as TextureImporter;

        if (importer != null &&
            (importer.sRGBTexture || !importer.isReadable))
        {
            importer.sRGBTexture = false;
            importer.isReadable = true;
            importer.SaveAndReimport();
        }

        return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
    }
}
