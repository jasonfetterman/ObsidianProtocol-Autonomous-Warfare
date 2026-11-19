using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public static class BlackspireGroundUnitBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN03_BATTLEFIELD_AAA/SCN03_BATTLEFIELD_AAA.unity";

    private const string RootName = "BLACKSPIRE_FIELD_UNITS";

    private class UnitPlacement
    {
        public string name;
        public string asset;
        public bool friendly;
        public float x;
        public float z;

        public UnitPlacement(string name, string asset, bool friendly, float x, float z)
        {
            this.name = name;
            this.asset = asset;
            this.friendly = friendly;
            this.x = x;
            this.z = z;
        }
    }

    [MenuItem("Obsidian Protocol/Battlefield/BUILD BLACKSPIRE GROUND UNITS")]
    public static void Build()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.path != ScenePath)
        {
            Debug.LogError("[BLACKSPIRE UNITS] Aborted. Open SCN03_BATTLEFIELD_AAA first.");
            return;
        }

        GameObject terrain = GameObject.Find("MAP001_BLACKSPIRE_EXPANSE_TERRAIN");

        if (terrain == null)
        {
            Debug.LogError("[BLACKSPIRE UNITS] Terrain object not found. Nothing created.");
            return;
        }

        MeshCollider terrainCollider = terrain.GetComponent<MeshCollider>();

        if (terrainCollider == null || !terrainCollider.enabled)
        {
            Debug.LogError("[BLACKSPIRE UNITS] Terrain MeshCollider missing or disabled.");
            return;
        }

        GameObject existingRoot = GameObject.Find(RootName);


        GameObject root = existingRoot;

        if (root == null)
        {
            root = new GameObject(RootName);
            Undo.RegisterCreatedObjectUndo(root, "Create Blackspire Unit Group");
        }

        root.transform.position = Vector3.zero;
        root.transform.rotation = Quaternion.identity;
        root.transform.localScale = Vector3.one;

        List<UnitPlacement> placements = new List<UnitPlacement>
        {
            new UnitPlacement(
                "FRIENDLY_IRON_SENTINEL_ROVER_01",
                "Assets/Models/others/Meshy_AI_Iron_Sentinel_Rover_1015181555_texture_fbx/Meshy_AI_Iron_Sentinel_Rover_1015181555_texture_fbx/Meshy_AI_Iron_Sentinel_Rover_1015181555_texture.prefab",
                true, 450f, 490f),

            new UnitPlacement(
                "FRIENDLY_IRON_HOUND_01",
                "Assets/Models/others/Meshy_AI_Iron_Hound_1015181916_texture_fbx/Meshy_AI_Iron_Hound_1015181916_texture_fbx/Meshy_AI_Iron_Hound_1015181916_texture.prefab",
                true, 470f, 490f),

            new UnitPlacement(
                "FRIENDLY_IRON_MULE_01",
                "Assets/Models/others/Meshy_AI_Iron_Mule_Transport_1015183422_texture_fbx/Meshy_AI_Iron_Mule_Transport_1015183422_texture_fbx/Meshy_AI_Iron_Mule_Transport_1015183422_texture.prefab",
                true, 490f, 490f),

            new UnitPlacement(
                "ENEMY_IRONCLAD_MARAUDER_01",
                "Assets/Models/others/Meshy_AI_Ironclad_Marauder_1015181433_texture_fbx/Meshy_AI_Ironclad_Marauder_1015181433_texture_fbx/Meshy_AI_Ironclad_Marauder_1015181433_texture.prefab",
                false, 540f, 490f),

            new UnitPlacement(
                "ENEMY_IRONCLAD_VIPER_01",
                "Assets/Models/others/Meshy_AI_Ironclad_Viper_1015181751_texture_fbx/Meshy_AI_Ironclad_Viper_1015181751_texture_fbx/Meshy_AI_Ironclad_Viper_1015181751_texture.prefab",
                false, 560f, 490f),

            new UnitPlacement(
                "ENEMY_IRONCLAD_DOZER_01",
                "Assets/Models/others/Meshy_AI_Ironclad_Dozer_1013083154_texture_fbx/Meshy_AI_Ironclad_Dozer_1013083154_texture_fbx/Meshy_AI_Ironclad_Dozer_1013083154_texture.prefab",
                false, 580f, 490f)
        };

        int created = 0;
        int failed = 0;

        foreach (UnitPlacement placement in placements)
        {
            if (root.transform.Find(placement.name) != null)
            {
                Debug.Log("[BLACKSPIRE UNITS] Already exists. Skipping: " + placement.name);
                continue;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(placement.asset);

            if (prefab == null)
            {
                Debug.LogError("[BLACKSPIRE UNITS] Missing prefab: " + placement.asset);
                failed++;
                continue;
            }

            GameObject instance = PrefabUtility.InstantiatePrefab(prefab, scene) as GameObject;

            if (instance == null)
            {
                Debug.LogError("[BLACKSPIRE UNITS] Could not instantiate: " + prefab.name);
                failed++;
                continue;
            }

            Undo.RegisterCreatedObjectUndo(instance, "Build Blackspire Ground Unit");

            instance.name = placement.name;
            instance.transform.SetParent(root.transform, true);

            Renderer[] renderers = instance.GetComponentsInChildren<Renderer>(true);

            if (renderers.Length == 0)
            {
                Debug.LogError("[BLACKSPIRE UNITS] No renderer found: " + prefab.name);
                Object.DestroyImmediate(instance);
                failed++;
                continue;
            }

            Bounds bounds = renderers[0].bounds;

            foreach (Renderer renderer in renderers)
                bounds.Encapsulate(renderer.bounds);

            Ray ray = new Ray(
                new Vector3(placement.x, 500f, placement.z),
                Vector3.down);

            RaycastHit hit;

            if (!terrainCollider.Raycast(ray, out hit, 1000f))
            {
                Debug.LogError("[BLACKSPIRE UNITS] Terrain sample failed for " + placement.name);
                Object.DestroyImmediate(instance);
                failed++;
                continue;
            }

            Vector3 position = new Vector3(
                placement.x,
                hit.point.y - bounds.min.y,
                placement.z);

            instance.transform.position = position;

            System.Type markerType = System.AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly =>
                {
                    try { return assembly.GetTypes(); }
                    catch { return new System.Type[0]; }
                })
                .FirstOrDefault(type => type.Name == "BattlefieldMinimapUnit");

            if (markerType == null)
            {
                Debug.LogError("[BLACKSPIRE UNITS] BattlefieldMinimapUnit type not found. Aborting this unit.");
                Object.DestroyImmediate(instance);
                failed++;
                continue;
            }

            Component marker = instance.GetComponent(markerType);

            if (marker == null)
                marker = instance.AddComponent(markerType);

            System.Reflection.MethodInfo setFriendly =
                markerType.GetMethod("SetFriendly");

            if (setFriendly == null)
            {
                Debug.LogError("[BLACKSPIRE UNITS] SetFriendly method not found.");
                Object.DestroyImmediate(instance);
                failed++;
                continue;
            }

            setFriendly.Invoke(marker, new object[] { placement.friendly });

            Debug.Log(
                "[BLACKSPIRE UNIT CREATED] " + instance.name +
                " | Team: " + (placement.friendly ? "FRIENDLY / BLUE" : "ENEMY / RED") +
                " | Ground Y: " + hit.point.y.ToString("F2") +
                " | Position: " + position);

            created++;
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[BLACKSPIRE UNITS] BUILD COMPLETE | Created: " +
            created + " | Failed: " + failed +
            " | Scene: " + scene.path);
    }
}





