#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ObsidianStrategicMapHUDBuilder
{
    [MenuItem("Obsidian Protocol/UI/Build MAP-031 to MAP-042")]
    public static void Build()
    {
        GameObject hud = GameObject.Find("HUD");

        if (hud == null)
        {
            Debug.LogError("[STRATEGIC MAP] HUD GameObject not found.");
            return;
        }

        Transform battlefield = GetOrCreate(
            "Battlefield",
            hud.transform
        );

        Transform strategicMap = GetOrCreate(
            "StrategicMap",
            battlefield
        );

        Transform core = GetOrCreate(
            "Core",
            strategicMap
        );

        Transform territory = GetOrCreate(
            "Territory",
            strategicMap
        );

        Transform forces = GetOrCreate(
            "Forces",
            strategicMap
        );

        Transform logistics = GetOrCreate(
            "Logistics",
            strategicMap
        );

        Transform intelligence = GetOrCreate(
            "Intelligence",
            strategicMap
        );

        // CORE
        CreateNode("MAP-031 StrategicMap", core);
        CreateNode("MAP-033 Regions", core);

        // TERRITORY
        CreateNode("MAP-032 TerritoryControl", territory);

        // FORCES
        CreateNode("MAP-034 Bases", forces);
        CreateNode("MAP-035 Armies", forces);
        CreateNode("MAP-036 FrontLines", forces);
        CreateNode("MAP-040 StrategicObjectives", forces);

        // LOGISTICS
        CreateNode("MAP-037 Logistics", logistics);
        CreateNode("MAP-038 Resources", logistics);
        CreateNode("MAP-041 StrategicRoutes", logistics);

        // INTELLIGENCE
        CreateNode("MAP-039 EnemyActivity", intelligence);
        CreateNode("MAP-042 IntelligenceLayer", intelligence);

        EditorUtility.SetDirty(hud);

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            hud.scene
        );

        Selection.activeGameObject = strategicMap.gameObject;

        Debug.Log(
            "[STRATEGIC MAP] MAP-031 through MAP-042 built successfully."
        );
    }

    private static Transform GetOrCreate(
        string objectName,
        Transform parent
    )
    {
        Transform existing = parent.Find(objectName);

        if (existing != null)
            return existing;

        GameObject obj = new GameObject(objectName);
        obj.transform.SetParent(parent, false);

        return obj.transform;
    }

    private static void CreateNode(
        string objectName,
        Transform parent
    )
    {
        if (parent.Find(objectName) != null)
            return;

        GameObject obj = new GameObject(objectName);
        obj.transform.SetParent(parent, false);
    }
}
#endif
