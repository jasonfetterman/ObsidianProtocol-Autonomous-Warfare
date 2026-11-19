using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using ObsidianProtocol.Game.Deployment;

public static class BlackspireAutonomousPatrolInstaller
{
    private const string ScenePath =
        "Assets/Scenes/SCN03_BATTLEFIELD_AAA/SCN03_BATTLEFIELD_AAA.unity";

    private const int UnitsPerType = 10;
    private const float UnitScale = 300f;
    private const float MinimumSpacing = 24f;

    private class UnitType
    {
        public string Name;
        public string Guid;
        public string RootName;
        public bool Airborne;
        public float Speed;
        public float Radius;
        public float Altitude;
        public float TurnSpeed;

        public UnitType(
            string name, string guid, string rootName,
            bool airborne, float speed, float radius,
            float altitude, float turnSpeed)
        {
            Name = name;
            Guid = guid;
            RootName = rootName;
            Airborne = airborne;
            Speed = speed;
            Radius = radius;
            Altitude = altitude;
            TurnSpeed = turnSpeed;
        }
    }

    private static readonly UnitType[] UnitTypes =
    {
        new UnitType(
            "FRIENDLY_IRON_SENTINEL_ROVER",
            "50c76074946fd9b4f87c1f63b7f76ad5",
            "BLACKSPIRE_FIELD_UNITS",
            false, 3.2f, 30f, 0f, 120f),

        new UnitType(
            "FRIENDLY_IRON_HOUND",
            "409606fb53473ba4abc250d9e507cbf6",
            "BLACKSPIRE_FIELD_UNITS",
            false, 2.2f, 25f, 0f, 120f),

        new UnitType(
            "FRIENDLY_IRON_MULE",
            "9b3183a1008306d468c9afa5468ae6a1",
            "BLACKSPIRE_FIELD_UNITS",
            false, 1.8f, 25f, 0f, 90f),

        new UnitType(
            "ENEMY_IRONCLAD_MARAUDER",
            "975cfc7f333a00a4f8b9f773734c96ba",
            "BLACKSPIRE_FIELD_UNITS",
            false, 3.8f, 35f, 0f, 140f),

        new UnitType(
            "ENEMY_IRONCLAD_DOZER",
            "37e6175989b841244a405d4885207487",
            "BLACKSPIRE_FIELD_UNITS",
            false, 1.5f, 25f, 0f, 80f),

        new UnitType(
            "ENEMY_IRONCLAD_VIPER",
            "fa57ffa988ffe0e4faab59dec9539a22",
            "BLACKSPIRE_AIR_UNITS",
            true, 10f, 55f, 30f, 160f)
    };

    [MenuItem("Obsidian Protocol/Battlefield/POPULATE BLACKSPIRE WITH 60 AUTONOMOUS UNITS")]
    public static void PopulateBlackspire()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.path != ScenePath)
        {
            Debug.LogError("[BLACKSPIRE] Open the Blackspire Expanse battlefield first.");
            return;
        }

        GameObject terrainObject =
            GameObject.Find("MAP001_BLACKSPIRE_EXPANSE_TERRAIN");

        GameObject groundRoot =
            GameObject.Find("BLACKSPIRE_FIELD_UNITS");

        GameObject airRoot =
            GameObject.Find("BLACKSPIRE_AIR_UNITS");

        if (terrainObject == null || groundRoot == null || airRoot == null)
        {
            Debug.LogError("[BLACKSPIRE] Terrain or unit group is missing.");
            return;
        }

        MeshCollider terrain = terrainObject.GetComponent<MeshCollider>();

        if (terrain == null || !terrain.enabled)
        {
            Debug.LogError("[BLACKSPIRE] Terrain collider unavailable.");
            return;
        }

        Bounds mapBounds = terrain.bounds;
        List<Vector3> occupied = new List<Vector3>();

        foreach (Transform child in groundRoot.transform)
            occupied.Add(child.position);

        foreach (Transform child in airRoot.transform)
            occupied.Add(child.position);

        int created = 0;
        int configured = 0;
        int failed = 0;

        foreach (UnitType type in UnitTypes)
        {
            GameObject root = type.Airborne ? airRoot : groundRoot;

            string prefabPath = AssetDatabase.GUIDToAssetPath(type.Guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (prefab == null)
            {
                Debug.LogError("[BLACKSPIRE] Prefab unavailable: " + type.Name);
                failed++;
                continue;
            }

            for (int number = 1; number <= UnitsPerType; number++)
            {
                string unitName = type.Name + "_" + number.ToString("00");
                Transform existing = root.transform.Find(unitName);

                GameObject unitObject;

                if (existing != null)
                {
                    unitObject = existing.gameObject;
                    configured++;
                }
                else
                {
                    Vector3 position;

                    if (!TryFindPlacement(terrain, mapBounds, occupied,
                        type.Airborne, out position))
                    {
                        Debug.LogError("[BLACKSPIRE] No valid placement found for " + unitName);
                        failed++;
                        continue;
                    }

                    unitObject = PrefabUtility.InstantiatePrefab(prefab, scene) as GameObject;

                    if (unitObject == null)
                    {
                        Debug.LogError("[BLACKSPIRE] Could not instantiate " + unitName);
                        failed++;
                        continue;
                    }

                    Undo.RegisterCreatedObjectUndo(unitObject, "Create Blackspire Patrol Unit");

                    unitObject.name = unitName;
                    unitObject.transform.SetParent(root.transform, true);
                    unitObject.transform.localScale = Vector3.one * UnitScale;
                    unitObject.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);

                    Renderer[] renderers =
                        unitObject.GetComponentsInChildren<Renderer>(true);

                    if (renderers.Length == 0)
                    {
                        Debug.LogError("[BLACKSPIRE] No renderers on " + unitName);
                        Undo.DestroyObjectImmediate(unitObject);
                        failed++;
                        continue;
                    }

                    Bounds modelBounds = renderers[0].bounds;

                    foreach (Renderer renderer in renderers)
                        modelBounds.Encapsulate(renderer.bounds);

                    float bottomOffset =
                        modelBounds.min.y - unitObject.transform.position.y;

                    position.y = position.y +
                        (type.Airborne ? type.Altitude : 0f) - bottomOffset;

                    unitObject.transform.position = position;

                    occupied.Add(position);
                    created++;
                }

                BlackspireAutonomousPatrol patrol =
                    unitObject.GetComponent<BlackspireAutonomousPatrol>();

                if (patrol == null)
                    patrol = Undo.AddComponent<BlackspireAutonomousPatrol>(unitObject);

                patrol.Configure(
                    terrain,
                    type.Airborne,
                    type.Altitude,
                    type.Radius,
                    type.Speed,
                    type.TurnSpeed);

                EditorUtility.SetDirty(unitObject);
                EditorUtility.SetDirty(patrol);
            }

            Debug.Log(
                "[BLACKSPIRE] " + type.Name +
                " | Target: 10 | Speed: " + type.Speed + " m/s");
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[BLACKSPIRE] POPULATION COMPLETE | Created: " +
            created + " | Existing/configured: " + configured +
            " | Failed: " + failed +
            " | Target: 60 units.");
    }

    private static bool TryFindPlacement(
        MeshCollider terrain,
        Bounds bounds,
        List<Vector3> occupied,
        bool airborne,
        out Vector3 position)
    {
        for (int attempt = 0; attempt < 3000; attempt++)
        {
            float x = Random.Range(bounds.min.x + 30f, bounds.max.x - 30f);
            float z = Random.Range(bounds.min.z + 30f, bounds.max.z - 30f);

            Ray ray = new Ray(new Vector3(x, 500f, z), Vector3.down);

            if (!terrain.Raycast(ray, out RaycastHit hit, 1000f))
                continue;

            if (Vector3.Angle(hit.normal, Vector3.up) > 18f)
                continue;

            Vector3 candidate = new Vector3(x, hit.point.y, z);

            bool tooClose = false;

            foreach (Vector3 other in occupied)
            {
                Vector2 a = new Vector2(candidate.x, candidate.z);
                Vector2 b = new Vector2(other.x, other.z);

                if (Vector2.Distance(a, b) < MinimumSpacing)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose)
                continue;

            position = candidate;
            return true;
        }

        position = Vector3.zero;
        return false;
    }
}
