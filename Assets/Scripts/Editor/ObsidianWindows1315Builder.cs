#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class ObsidianWindows1315Builder
{
    [MenuItem("Obsidian Protocol/UI/Build PHASE 13-15 Windows")]
    public static void Build()
    {
        GameObject hud = GameObject.Find("HUD");

        if (hud == null)
        {
            Debug.LogError("[UI] HUD GameObject not found.");
            return;
        }

        Transform windows = GetOrCreate("Windows", hud.transform);

        // =====================================================
        // PHASE 13 — LOGISTICS
        // =====================================================

        Transform logistics = GetOrCreate("Logistics", windows);

        Transform logisticsCore =
            GetOrCreate("Core", logistics);

        Transform logisticsStatus =
            GetOrCreate("Status", logistics);

        Transform logisticsTransport =
            GetOrCreate("Transport", logistics);

        Transform logisticsRoutes =
            GetOrCreate("Routes", logistics);

        Transform logisticsWarnings =
            GetOrCreate("Warnings", logistics);

        CreateNode("WIN-058 LogisticsOverview", logisticsCore);

        CreateNode("WIN-059 SupplyStatus", logisticsStatus);
        CreateNode("WIN-060 FuelStatus", logisticsStatus);
        CreateNode("WIN-061 AmmunitionStatus", logisticsStatus);
        CreateNode("WIN-062 RepairStatus", logisticsStatus);
        CreateNode("WIN-066 DepotStatus", logisticsStatus);

        CreateNode("WIN-063 ResourceTransport", logisticsTransport);
        CreateNode("WIN-065 Convoys", logisticsTransport);

        CreateNode("WIN-064 SupplyRoutes", logisticsRoutes);

        CreateNode("WIN-067 LogisticsWarnings", logisticsWarnings);
        CreateNode("WIN-068 RouteDisruptionDisplay", logisticsWarnings);

        // =====================================================
        // PHASE 14 — INTELLIGENCE
        // =====================================================

        Transform intelligence =
            GetOrCreate("Intelligence", windows);

        Transform intelligenceCore =
            GetOrCreate("Core", intelligence);

        Transform intelligenceContacts =
            GetOrCreate("Contacts", intelligence);

        Transform intelligenceAnalysis =
            GetOrCreate("Analysis", intelligence);

        Transform intelligenceTracking =
            GetOrCreate("Tracking", intelligence);

        CreateNode("WIN-069 IntelligenceWindow", intelligenceCore);

        CreateNode("WIN-070 ContactList", intelligenceContacts);
        CreateNode("WIN-071 KnownEnemyUnits", intelligenceContacts);
        CreateNode("WIN-072 UnknownContacts", intelligenceContacts);

        CreateNode("WIN-073 ContactClassification", intelligenceAnalysis);
        CreateNode("WIN-074 ThreatLevel", intelligenceAnalysis);
        CreateNode("WIN-077 ConfidenceLevel", intelligenceAnalysis);

        CreateNode("WIN-075 LastKnownPosition", intelligenceTracking);
        CreateNode("WIN-076 SensorSource", intelligenceTracking);
        CreateNode("WIN-078 TrackingStatus", intelligenceTracking);
        CreateNode("WIN-079 IntelligenceHistory", intelligenceTracking);

        // =====================================================
        // PHASE 15 — TECHNOLOGY
        // =====================================================

        Transform technology =
            GetOrCreate("Technology", windows);

        Transform technologyCore =
            GetOrCreate("Core", technology);

        Transform technologyTree =
            GetOrCreate("Tree", technology);

        Transform technologyResearch =
            GetOrCreate("Research", technology);

        Transform technologyDetails =
            GetOrCreate("Details", technology);

        CreateNode("WIN-080 TechnologyWindow", technologyCore);

        CreateNode("WIN-081 TechnologyTree", technologyTree);
        CreateNode("WIN-082 TechnologyCategories", technologyTree);

        CreateNode("WIN-083 ResearchRequirements", technologyResearch);
        CreateNode("WIN-084 ResearchCost", technologyResearch);
        CreateNode("WIN-085 ResearchProgress", technologyResearch);
        CreateNode("WIN-086 LockedTechnology", technologyResearch);
        CreateNode("WIN-087 UnlockedTechnology", technologyResearch);

        CreateNode("WIN-088 TechnologyDetails", technologyDetails);

        // =====================================================
        // SAVE
        // =====================================================

        EditorUtility.SetDirty(hud);

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            hud.scene
        );

        Selection.activeGameObject = windows.gameObject;

        Debug.Log(
            "[UI] PHASE 13-15 hierarchy built successfully. WIN-058 through WIN-088."
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
