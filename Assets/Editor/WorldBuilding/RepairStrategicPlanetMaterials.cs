using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public static class RepairStrategicPlanetMaterials
{
    private sealed class Planet
    {
        public string Name;
        public string Folder;
        public string PrefabPath;
        public string AlbedoPath;
        public string NormalPath;
        public string MetallicPath;
        public string RoughnessPath;
        public string MaterialPath;
        public GameObject Root;
        public Material Material;
        public MeshRenderer[] Renderers;
    }

    private static readonly Planet[] Planets =
    {
        new Planet {
            Name = "Karthos",
            Folder = "Assets/Art/Models/Planets/Karthos",
            PrefabPath = "Assets/Art/Models/Planets/Karthos/KARTHOS_PLANET.prefab",
            MaterialPath = "Assets/Art/Models/Planets/Karthos/Materials/Karthos_Surface.mat"
        },
        new Planet {
            Name = "Nox Meridian",
            Folder = "Assets/Art/Models/Planets/Nox Meridian",
            PrefabPath = "Assets/Art/Models/Planets/Nox Meridian/NOX_MERIDIAN_PLANET.prefab",
            MaterialPath = "Assets/Art/Models/Planets/Nox Meridian/Materials/NoxMeridian_Surface.mat"
        },
        new Planet {
            Name = "Solara Prime",
            Folder = "Assets/Art/Models/Planets/Solara Prime",
            PrefabPath = "Assets/Art/Models/Planets/Solara Prime/SOLARA_PRIME_PLANET.prefab",
            MaterialPath = "Assets/Art/Models/Planets/Solara Prime/Materials/SolaraPrime_Surface.mat"
        },
        new Planet {
            Name = "Virellia",
            Folder = "Assets/Art/Models/Planets/Virellia",
            PrefabPath = "Assets/Art/Models/Planets/Virellia/VIRELLIA_PLANET.prefab",
            MaterialPath = "Assets/Art/Models/Planets/Virellia/Materials/Virellia_Surface.mat"
        }
    };

    [MenuItem("Obsidian Protocol/World Building/Repair Strategic Planet Materials")]
    private static void Repair()
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
        {
            Debug.LogError("[PLANET REPAIR] URP/Lit shader missing. Nothing changed.");
            return;
        }

        // Preflight all textures and prefab renderers before saving anything.
        foreach (Planet p in Planets)
        {
            if (!File.Exists(p.PrefabPath))
            {
                Debug.LogError("[PLANET REPAIR] Missing prefab: " + p.PrefabPath);
                return;
            }

            string[] albedoFiles = Directory.GetFiles(p.Folder, "*_texture.png");
            Array.Sort(albedoFiles, StringComparer.OrdinalIgnoreCase);

            List<string> validAlbedos = new List<string>();
            foreach (string file in albedoFiles)
            {
                string name = Path.GetFileNameWithoutExtension(file);
                if (!name.EndsWith("_metallic", StringComparison.OrdinalIgnoreCase) &&
                    !name.EndsWith("_normal", StringComparison.OrdinalIgnoreCase) &&
                    !name.EndsWith("_roughness", StringComparison.OrdinalIgnoreCase))
                    validAlbedos.Add(file);
            }

            if (validAlbedos.Count != 1)
            {
                Debug.LogError("[PLANET REPAIR] Expected one albedo texture for " +
                    p.Name + "; found " + validAlbedos.Count + ". Nothing changed.");
                return;
            }

            string albedo = validAlbedos[0].Replace('\\', '/');
            string stem = Path.GetFileNameWithoutExtension(albedo);
            string prefix = stem.Substring(0, stem.Length - "_texture".Length);

            p.AlbedoPath = albedo;
            p.NormalPath = p.Folder + "/" + prefix + "_texture_normal.png";
            p.MetallicPath = p.Folder + "/" + prefix + "_texture_metallic.png";
            p.RoughnessPath = p.Folder + "/" + prefix + "_texture_roughness.png";

            if (!File.Exists(p.NormalPath) ||
                !File.Exists(p.MetallicPath) ||
                !File.Exists(p.RoughnessPath))
            {
                Debug.LogError("[PLANET REPAIR] Incomplete texture set for " +
                    p.Name + ". Expected albedo, normal, metallic, and roughness PNGs. Nothing changed.");
                return;
            }

            if (AssetDatabase.LoadAssetAtPath<Texture2D>(p.AlbedoPath) == null ||
                AssetDatabase.LoadAssetAtPath<Texture2D>(p.NormalPath) == null ||
                AssetDatabase.LoadAssetAtPath<Texture2D>(p.MetallicPath) == null ||
                AssetDatabase.LoadAssetAtPath<Texture2D>(p.RoughnessPath) == null)
            {
                Debug.LogError("[PLANET REPAIR] A texture failed to load for " +
                    p.Name + ". Let Unity finish importing assets, then retry.");
                return;
            }

            p.Root = PrefabUtility.LoadPrefabContents(p.PrefabPath);
            if (p.Root == null)
            {
                Debug.LogError("[PLANET REPAIR] Could not load " + p.PrefabPath);
                UnloadAll();
                return;
            }

            p.Renderers = p.Root.GetComponentsInChildren<MeshRenderer>(true);
            if (p.Renderers == null || p.Renderers.Length == 0)
            {
                Debug.LogError("[PLANET REPAIR] No MeshRenderer found in " +
                    p.Name + ". No prefabs were saved.");
                UnloadAll();
                return;
            }

            Debug.Log("[PLANET REPAIR] Validated " + p.Name +
                ": " + p.Renderers.Length + " mesh renderer(s); four textures found.");
        }

        string backupFolder = Path.GetFullPath("Temp/PlanetMaterialRepairBackups");
        Directory.CreateDirectory(backupFolder);

        try
        {
            // Back up all wrapper prefabs before any changes are saved.
            foreach (Planet p in Planets)
            {
                string backupPath = Path.Combine(backupFolder,
                    Path.GetFileNameWithoutExtension(p.PrefabPath) + ".prefab.bak");
                File.Copy(Path.GetFullPath(p.PrefabPath), backupPath, true);
            }

            // Create external surface materials using each planet's own maps.
            foreach (Planet p in Planets)
            {
                string materialDirectory = Path.GetDirectoryName(p.MaterialPath).Replace('\\', '/');
                if (!AssetDatabase.IsValidFolder(materialDirectory))
                {
                    string parent = Path.GetDirectoryName(materialDirectory).Replace('\\', '/');
                    string folderName = Path.GetFileName(materialDirectory);
                    if (!AssetDatabase.IsValidFolder(parent) ||
                        AssetDatabase.CreateFolder(parent, folderName).Length == 0)
                        throw new InvalidOperationException("Could not create " + materialDirectory);
                }

                Material mat = AssetDatabase.LoadAssetAtPath<Material>(p.MaterialPath);
                if (mat == null)
                {
                    mat = new Material(shader);
                    AssetDatabase.CreateAsset(mat, p.MaterialPath);
                }

                mat.shader = shader;
                mat.SetTexture("_BaseMap", AssetDatabase.LoadAssetAtPath<Texture2D>(p.AlbedoPath));
                mat.SetTexture("_MainTex", AssetDatabase.LoadAssetAtPath<Texture2D>(p.AlbedoPath));
                mat.SetTexture("_BumpMap", AssetDatabase.LoadAssetAtPath<Texture2D>(p.NormalPath));
                mat.SetTexture("_MetallicGlossMap", AssetDatabase.LoadAssetAtPath<Texture2D>(p.MetallicPath));
                mat.SetColor("_BaseColor", Color.white);
                mat.SetFloat("_Metallic", 1f);
                mat.SetFloat("_Smoothness", 0.5f);
                mat.EnableKeyword("_NORMALMAP");
                mat.EnableKeyword("_METALLICGLOSSMAP");
                EditorUtility.SetDirty(mat);
                p.Material = mat;
            }

            AssetDatabase.SaveAssets();

            foreach (Planet p in Planets)
            {
                foreach (MeshRenderer renderer in p.Renderers)
                {
                    Material[] slots = renderer.sharedMaterials;
                    if (slots == null || slots.Length == 0)
                        slots = new Material[1];

                    for (int i = 0; i < slots.Length; i++)
                        slots[i] = p.Material;

                    renderer.sharedMaterials = slots;
                    EditorUtility.SetDirty(renderer);
                }

                PrefabUtility.SaveAsPrefabAsset(p.Root, p.PrefabPath);
                Debug.Log("[PLANET REPAIR] Saved " + p.Name +
                    " using its own albedo, normal, and metallic maps. Roughness map retained; URP/Lit uses smoothness, not a direct roughness slot.");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[PLANET REPAIR] SUCCESS. Four planet wrapper prefabs repaired. Scene layouts and rotation scripts were not intentionally changed. Prefab backups: " + backupFolder);
        }
        catch (Exception ex)
        {
            Debug.LogError("[PLANET REPAIR] Repair stopped: " + ex);
        }
        finally
        {
            UnloadAll();
        }
    }

    private static void UnloadAll()
    {
        foreach (Planet p in Planets)
        {
            if (p.Root != null)
            {
                PrefabUtility.UnloadPrefabContents(p.Root);
                p.Root = null;
            }
        }
    }
}
