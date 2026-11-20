using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RebuildStorePrefabFromSource
{
    private const string SourceScene = "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity";
    private const string MaterialFolder = "Assets/Materials/StoreExtracted";
    private const string OutputPrefab = "Assets/Prefabs/Buildings/Store/STORE_FACILITY.prefab";

    [MenuItem("Obsidian Protocol/Store/Build Production Store Prefab")]
    private static void BuildProductionPrefab()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogError("[STORE BUILD] Stop Play Mode first.");
            return;
        }

        if (File.Exists(OutputPrefab))
        {
            Debug.LogError("[STORE BUILD] Output already exists; refusing to overwrite: " + OutputPrefab);
            return;
        }

        Scene source = SceneManager.GetSceneByPath(SourceScene);
        bool openedHere = !source.IsValid() || !source.isLoaded;
        GameObject copy = null;

        try
        {
            if (openedHere)
                source = EditorSceneManager.OpenScene(SourceScene, OpenSceneMode.Additive);

            if (!source.IsValid() || !source.isLoaded)
            {
                Debug.LogError("[STORE BUILD] Could not load source scene.");
                return;
            }

            GameObject original = null;
            foreach (GameObject candidate in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (candidate.name == "STORE_FACILITY" &&
                    candidate.scene == source &&
                    candidate.scene.IsValid())
                {
                    original = candidate;
                    break;
                }
            }

            if (original == null)
            {
                Debug.LogError("[STORE BUILD] Root STORE_FACILITY not found in source scene.");
                return;
            }

            if (!AssetDatabase.IsValidFolder("Assets/Materials"))
                AssetDatabase.CreateFolder("Assets", "Materials");

            if (!AssetDatabase.IsValidFolder(MaterialFolder))
                AssetDatabase.CreateFolder("Assets/Materials", "StoreExtracted");

            var materialsByName = new Dictionary<string, Material>();
            foreach (string guid in AssetDatabase.FindAssets("t:Material", new[] { MaterialFolder }))
            {
                string materialPath = AssetDatabase.GUIDToAssetPath(guid);
                Material existing = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
                if (existing != null && !materialsByName.ContainsKey(existing.name))
                    materialsByName.Add(existing.name, existing);
            }

            var extracted = new Dictionary<Material, Material>();
            int rendererCount = 0;
            int slotsBefore = 0;
            int emptyBefore = 0;
            int createdMaterials = 0;

            copy = Object.Instantiate(original);
            copy.name = "STORE_FACILITY";
            copy.transform.SetParent(null, true);

            foreach (Renderer renderer in copy.GetComponentsInChildren<Renderer>(true))
            {
                rendererCount++;
                Material[] slots = renderer.sharedMaterials;
                bool changed = false;

                for (int i = 0; i < slots.Length; i++)
                {
                    slotsBefore++;
                    Material material = slots[i];

                    if (material == null)
                    {
                        emptyBefore++;
                        continue;
                    }

                    if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(material)))
                        continue;

                    if (!extracted.TryGetValue(material, out Material asset))
                    {
                        if (!materialsByName.TryGetValue(material.name, out asset))
                        {
                            string safeName = string.Join("_", material.name.Split(Path.GetInvalidFileNameChars()));
                            if (string.IsNullOrWhiteSpace(safeName))
                                safeName = "StoreMaterial";

                            string materialPath = AssetDatabase.GenerateUniqueAssetPath(
                                MaterialFolder + "/" + safeName + ".mat");

                            asset = new Material(material);
                            asset.name = material.name;
                            AssetDatabase.CreateAsset(asset, materialPath);
                            materialsByName[asset.name] = asset;
                            createdMaterials++;
                        }

                        extracted.Add(material, asset);
                    }

                    slots[i] = asset;
                    changed = true;
                }

                if (changed)
                    renderer.sharedMaterials = slots;
            }

            AssetDatabase.SaveAssets();

            GameObject saved = PrefabUtility.SaveAsPrefabAsset(copy, OutputPrefab);
            if (saved == null)
            {
                Debug.LogError("[STORE BUILD] Prefab save failed.");
                return;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(OutputPrefab, ImportAssetOptions.ForceSynchronousImport);

            GameObject verified = AssetDatabase.LoadAssetAtPath<GameObject>(OutputPrefab);
            if (verified == null)
            {
                Debug.LogError("[STORE BUILD] Saved prefab could not be reloaded.");
                return;
            }

            int savedRenderers = 0;
            int slotsAfter = 0;
            int emptyAfter = 0;
            int nonAssetMaterials = 0;

            foreach (Renderer renderer in verified.GetComponentsInChildren<Renderer>(true))
            {
                savedRenderers++;
                foreach (Material material in renderer.sharedMaterials)
                {
                    slotsAfter++;
                    if (material == null)
                        emptyAfter++;
                    else if (string.IsNullOrEmpty(AssetDatabase.GetAssetPath(material)))
                        nonAssetMaterials++;
                }
            }

            Debug.Log(
                "[STORE BUILD RESULT] Prefab=" + OutputPrefab +
                " | Renderers before=" + rendererCount +
                " | Renderers saved=" + savedRenderers +
                " | Slots before=" + slotsBefore +
                " | Empty before=" + emptyBefore +
                " | Slots saved=" + slotsAfter +
                " | Empty saved=" + emptyAfter +
                " | Non-asset materials=" + nonAssetMaterials +
                " | New materials=" + createdMaterials +
                " | Source position=" + original.transform.position +
                " | Source rotation=" + original.transform.rotation +
                " | Source scale=" + original.transform.lossyScale);

            if (savedRenderers == 0 ||
                savedRenderers != rendererCount ||
                slotsAfter != slotsBefore ||
                emptyAfter != 0 ||
                nonAssetMaterials != 0)
            {
                Debug.LogError("[STORE BUILD] VALIDATION FAILED. Do not place in Blackspire.");
                return;
            }

            Debug.Log("[STORE BUILD] VALIDATION PASSED. Prefab is ready for placement review.");
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
        }
        finally
        {
            if (copy != null)
                Object.DestroyImmediate(copy);

            if (openedHere && source.IsValid() && source.isLoaded)
                EditorSceneManager.CloseScene(source, true);
        }
    }
}

