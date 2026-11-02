using UnityEditor;
using UnityEngine;

public static class FixStrategicPlanetTextureImports
{
    [MenuItem("Obsidian Protocol/World Building/Fix Strategic Planet Texture Imports")]
    private static void FixImports()
    {
        string[] folders =
        {
            "Assets/Art/Models/Planets/Karthos",
            "Assets/Art/Models/Planets/Nox Meridian",
            "Assets/Art/Models/Planets/Solara Prime",
            "Assets/Art/Models/Planets/Virellia"
        };

        int changed = 0;

        foreach (string folder in folders)
        {
            string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { folder });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string file = System.IO.Path.GetFileNameWithoutExtension(path);

                bool normal = file.EndsWith("_texture_normal");
                bool dataMap = normal ||
                    file.EndsWith("_texture_metallic") ||
                    file.EndsWith("_texture_roughness");

                if (!normal && !dataMap)
                    continue;

                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer == null)
                {
                    Debug.LogWarning("[PLANET TEXTURES] Could not load importer: " + path);
                    continue;
                }

                bool needsChange =
                    (normal && importer.textureType != TextureImporterType.NormalMap) ||
                    (dataMap && importer.sRGBTexture);

                if (!needsChange)
                {
                    Debug.Log("[PLANET TEXTURES] Already correct: " + path);
                    continue;
                }

                if (normal)
                    importer.textureType = TextureImporterType.NormalMap;

                importer.sRGBTexture = !dataMap;
                importer.SaveAndReimport();
                changed++;

                Debug.Log("[PLANET TEXTURES] Fixed import settings: " + path);
            }
        }

        Debug.Log("[PLANET TEXTURES] Finished. Reimported " + changed +
            " texture(s). No scenes, prefabs, or materials were modified.");
    }
}
