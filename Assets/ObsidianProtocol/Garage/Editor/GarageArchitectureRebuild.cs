using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class OPAWGarageArchitectureRebuild
{
    private const string RootName = "GARAGE_ARCHITECTURE";
    private static readonly Dictionary<string, Material> Materials =
        new Dictionary<string, Material>();

    [MenuItem("Obsidian Protocol/Build/Preview New Garage Architecture")]
    private static void BuildPreview()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (!scene.IsValid() || !scene.isLoaded)
        {
            EditorUtility.DisplayDialog("Garage Architecture",
                "Open the Garage scene first.", "OK");
            return;
        }

        if (GameObject.Find(RootName) != null)
        {
            EditorUtility.DisplayDialog("Garage Architecture",
                "GARAGE_ARCHITECTURE already exists. Remove the previous preview before rebuilding.",
                "OK");
            return;
        }

        if (!EditorUtility.DisplayDialog("Build Garage Preview",
            "Create the new hangar architecture in the active scene without deleting or replacing any existing objects?",
            "Build Preview", "Cancel"))
            return;

        Materials.Clear();
        GameObject root = new GameObject(RootName);
        Undo.RegisterCreatedObjectUndo(root, "Build Garage Architecture Preview");

        BuildFloor(root.transform);
        BuildExterior(root.transform);
        BuildRoof(root.transform);
        BuildInteriorPartitions(root.transform);
        BuildApron(root.transform);
        BuildIndustrialDetails(root.transform);
        BuildLightingFixtures(root.transform);

        Selection.activeGameObject = root;
        EditorGUIUtility.PingObject(root);
        SceneView.lastActiveSceneView?.FrameSelected();

        EditorSceneManager.MarkSceneDirty(scene);

        EditorUtility.DisplayDialog("Garage Architecture",
            "Preview created. Existing scene objects were not deleted. Inspect the new architecture in Scene view before deciding what to replace.",
            "OK");
    }

    [MenuItem("Obsidian Protocol/Build/Remove New Garage Architecture Preview")]
    private static void RemovePreview()
    {
        GameObject root = GameObject.Find(RootName);

        if (root == null)
        {
            EditorUtility.DisplayDialog("Garage Architecture",
                "No GARAGE_ARCHITECTURE root was found.", "OK");
            return;
        }

        if (!EditorUtility.DisplayDialog("Remove Preview",
            "Remove only GARAGE_ARCHITECTURE and its children? All other scene objects will remain untouched.",
            "Remove Preview", "Cancel"))
            return;

        Undo.DestroyObjectImmediate(root);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    private static void BuildFloor(Transform root)
    {
        Cube(root, "ARCH_FLOOR_CONCRETE", new Vector3(0, -0.35f, 0),
            new Vector3(100, 0.7f, 70), "Concrete");

        Cube(root, "ARCH_INTERIOR_FLOOR", new Vector3(0, 0.015f, 0),
            new Vector3(96, 0.08f, 64), "Floor");

        for (int x = -40; x <= 40; x += 10)
        {
            Cube(root, "FLOOR_BAY_LINE", new Vector3(x, 0.07f, 0),
                new Vector3(0.12f, 0.025f, 55), "SafetyYellow");
        }

        for (int z = -25; z <= 25; z += 10)
        {
            Cube(root, "FLOOR_SERVICE_LINE", new Vector3(0, 0.075f, z),
                new Vector3(88, 0.025f, 0.10f), "FloorMarking");
        }

        Cube(root, "CENTRAL_INSPECTION_LANE", new Vector3(0, 0.09f, 0),
            new Vector3(8, 0.04f, 56), "Lane");

        for (int i = -2; i <= 2; i++)
        {
            Cube(root, "INSPECTION_LANE_DASH", new Vector3(i * 1.3f, 0.12f, 0),
                new Vector3(0.07f, 0.025f, 48), "SafetyYellow");
        }
    }

    private static void BuildExterior(Transform root)
    {
        // Side walls, split front wall, and rear wall.
        Cube(root, "WEST_OUTER_WALL", new Vector3(-49, 8, 0),
            new Vector3(2, 16, 68), "Wall");
        Cube(root, "EAST_OUTER_WALL", new Vector3(49, 8, 0),
            new Vector3(2, 16, 68), "Wall");
        Cube(root, "REAR_OUTER_WALL", new Vector3(0, 8, -33),
            new Vector3(100, 16, 2), "Wall");

        Cube(root, "FRONT_WALL_LEFT", new Vector3(-34, 8, 33),
            new Vector3(32, 16, 2), "Wall");
        Cube(root, "FRONT_WALL_RIGHT", new Vector3(34, 8, 33),
            new Vector3(32, 16, 2), "Wall");
        Cube(root, "FRONT_DOOR_HEADER", new Vector3(0, 14, 33),
            new Vector3(36, 4, 2), "Wall");

        // Framed hangar-door leaves. The central personnel route remains clear.
        for (int side = -1; side <= 1; side += 2)
        {
            float x = side * 9.0f;
            Cube(root, "HANGAR_DOOR_LEAF", new Vector3(x, 6, 32.2f),
                new Vector3(17.5f, 11.5f, 0.55f), "Door");
            for (int y = 1; y <= 11; y += 2)
            {
                Cube(root, "DOOR_RIB", new Vector3(x, y, 31.88f),
                    new Vector3(17.0f, 0.12f, 0.12f), "Steel");
            }
        }

        // Reinforced door surround and upper glazing.
        foreach (float x in new float[] { -18, 0, 18 })
        {
            Cube(root, "DOOR_VERTICAL_FRAME", new Vector3(x, 6, 31.4f),
                new Vector3(0.4f, 12, 0.7f), "Steel");
        }

        // High windows on both long sides.
        for (int z = -24; z <= 24; z += 8)
        {
            Cube(root, "WEST_HIGH_WINDOW", new Vector3(-47.9f, 11.5f, z),
                new Vector3(0.12f, 3, 5), "Glass");
            Cube(root, "EAST_HIGH_WINDOW", new Vector3(47.9f, 11.5f, z),
                new Vector3(0.12f, 3, 5), "Glass");
        }

        // Personnel access doors.
        Cube(root, "PERSONNEL_DOOR_FRAME", new Vector3(42, 2.1f, 32.7f),
            new Vector3(3.0f, 4.2f, 0.5f), "Steel");
        Cube(root, "PERSONNEL_DOOR", new Vector3(42, 2, 32.35f),
            new Vector3(2.5f, 3.8f, 0.2f), "Door");
        Cube(root, "PERSONNEL_DOOR_WINDOW", new Vector3(42, 2.8f, 32.22f),
            new Vector3(0.9f, 1.0f, 0.08f), "Glass");
    }

    private static void BuildRoof(Transform root)
    {
        // Faceted barrel vault: repeated transverse arches with longitudinal
        // purlins and bracing. Segments approximate the curved roof profile.
        for (int z = -30; z <= 30; z += 6)
        {
            for (int segment = 0; segment < 12; segment++)
            {
                float a0 = Mathf.PI - segment * Mathf.PI / 12f;
                float a1 = Mathf.PI - (segment + 1) * Mathf.PI / 12f;

                Vector3 p0 = new Vector3(47 * Mathf.Cos(a0),
                    15 + 10 * Mathf.Sin(a0), z);
                Vector3 p1 = new Vector3(47 * Mathf.Cos(a1),
                    15 + 10 * Mathf.Sin(a1), z);

                Beam(root, "ROOF_ARCH_RIB", p0, p1, 0.38f, "Steel");
            }
        }

        for (int segment = 0; segment <= 12; segment++)
        {
            float a = Mathf.PI - segment * Mathf.PI / 12f;
            Vector3 start = new Vector3(47 * Mathf.Cos(a),
                15 + 10 * Mathf.Sin(a), -32);
            Vector3 end = new Vector3(47 * Mathf.Cos(a),
                15 + 10 * Mathf.Sin(a), 32);
            Beam(root, "ROOF_LONGITUDINAL_PURLIN", start, end, 0.18f, "Roof");
        }

        for (int z = -27; z <= 27; z += 12)
        {
            Beam(root, "ROOF_CROSS_BRACE",
                new Vector3(-40, 20, z), new Vector3(40, 20, z + 6),
                0.13f, "Steel");
            Beam(root, "ROOF_CROSS_BRACE",
                new Vector3(40, 20, z), new Vector3(-40, 20, z + 6),
                0.13f, "Steel");
        }

        Cube(root, "ROOF_RIDGE_SERVICE_TRAY", new Vector3(0, 25, 0),
            new Vector3(1.1f, 0.45f, 60), "Roof");
    }

    private static void BuildInteriorPartitions(Transform root)
    {
        // Six partitioned service bays. All dimensions are positive.
        // The center aisle remains clear for walking and vehicle movement.
        Partition(root, "MAINTENANCE_BAY_WEST", -30, -8, 22, 18, 8, "Wall");
        Partition(root, "DIAGNOSTICS_LAB", 30, -8, 22, 18, 8, "Wall");
        Partition(root, "LOGISTICS_WAREHOUSE", -30, -21, 22, 14, 8, "Wall");
        Partition(root, "PAINT_BOOTH", 30, -21, 22, 14, 8, "Wall");
        Partition(root, "OPERATIONS_CENTER", -30, 10, 22, 14, 8, "Wall");
        Partition(root, "INSPECTION_ZONE", 30, 10, 22, 14, 8, "Wall");

        // Observation windows are placed in the inner-facing side walls.
        Cube(root, "LAB_OBSERVATION_WINDOW", new Vector3(18.8f, 5.2f, -8),
            new Vector3(0.12f, 2.2f, 4), "Glass");
        Cube(root, "OPERATIONS_VIEWING_WINDOW", new Vector3(-18.8f, 5.2f, 10),
            new Vector3(0.12f, 2.2f, 4), "Glass");

        // Work-area floor pads and safety borders.
        ZonePad(root, "VEHICLE_SHOWROOM_PAD", 0, 16, 28, 24);
        ZonePad(root, "HEAVY_REPAIR_PAD", -30, -8, 22, 18);
        ZonePad(root, "DIAGNOSTICS_PAD", 30, -8, 22, 18);
        ZonePad(root, "LOGISTICS_PAD", -30, -20, 22, 14);
        ZonePad(root, "PAINT_PAD", 30, -20, 22, 14);
        ZonePad(root, "OPERATIONS_PAD", -30, 10, 22, 14);
        ZonePad(root, "INSPECTION_PAD", 30, 10, 22, 14);
    }

    private static void Partition(Transform root, string name, float x,
        float z, float width, float depth, float height, string material)
    {
        // Rear and side walls with a wide central opening for vehicle movement.
        Cube(root, name + "_REAR", new Vector3(x, height / 2, z - depth / 2),
            new Vector3(width, height, 0.35f), material);
        Cube(root, name + "_SIDE_A",
            new Vector3(x - width / 2, height / 2, z),
            new Vector3(0.35f, height, depth), material);
        Cube(root, name + "_SIDE_B",
            new Vector3(x + width / 2, height / 2, z),
            new Vector3(0.35f, height, depth), material);

        // Overhead lintel leaves the front entrance open for walkthroughs.
        Cube(root, name + "_FRONT_LINTEL",
            new Vector3(x, height - 0.2f, z + depth / 2),
            new Vector3(width, 0.4f, 0.35f), material);
    }

    private static void ZonePad(Transform root, string name, float x, float z,
        float width, float depth)
    {
        Cube(root, name, new Vector3(x, 0.10f, z),
            new Vector3(width, 0.12f, depth), "Pad");
        Cube(root, name + "_SAFETY_EDGE",
            new Vector3(x, 0.18f, z - depth / 2 + 0.18f),
            new Vector3(width - 0.4f, 0.025f, 0.12f), "SafetyYellow");
    }

    private static void BuildApron(Transform root)
    {
        Cube(root, "EXTERIOR_CONCRETE_APRON", new Vector3(0, -0.28f, 45),
            new Vector3(100, 0.5f, 24), "Concrete");

        for (int x = -40; x <= 40; x += 10)
        {
            Cube(root, "APRON_GUIDE_LINE", new Vector3(x, -0.015f, 45),
                new Vector3(0.12f, 0.025f, 18), "SafetyYellow");
        }

        for (int x = -30; x <= 30; x += 10)
        {
            Cube(root, "APRON_DRAIN", new Vector3(x, -0.005f, 36),
                new Vector3(2.5f, 0.025f, 0.12f), "Steel");
        }

        // Bollards flank the vehicle entrance.
        for (int x = -22; x <= 22; x += 4)
        {
            if (Mathf.Abs(x) < 20) continue;
            Cylinder(root, "APRON_BOLLARD", new Vector3(x, 0.6f, 34.5f),
                new Vector3(0.35f, 1.2f, 0.35f), "SafetyYellow");
        }
    }

    private static void BuildIndustrialDetails(Transform root)
    {
        // Structural columns and protective bases.
        for (int z = -24; z <= 24; z += 12)
        {
            foreach (float x in new float[] { -44, 44 })
            {
                Cube(root, "STEEL_SUPPORT_COLUMN", new Vector3(x, 7.5f, z),
                    new Vector3(0.75f, 15, 0.75f), "Steel");
                Cube(root, "COLUMN_GUARD_BASE", new Vector3(x, 0.6f, z),
                    new Vector3(1.4f, 1.2f, 1.4f), "SafetyYellow");
            }
        }

        // Warehouse shelving.
        for (int row = 0; row < 3; row++)
        {
            float z = -25 + row * 4;
            Cube(root, "WAREHOUSE_SHELF_FRAME",
                new Vector3(-30, 2.2f, z), new Vector3(9, 4.4f, 0.2f), "Steel");

            for (int level = 0; level < 4; level++)
            {
                Cube(root, "WAREHOUSE_SHELF_DECK",
                    new Vector3(-30, 0.7f + level * 1.1f, z),
                    new Vector3(9.2f, 0.12f, 1.4f), "Roof");
            }
        }

        // Repair benches and hoist rails.
        for (int x = -36; x <= -24; x += 4)
        {
            Cube(root, "REPAIR_WORKBENCH", new Vector3(x, 1.1f, -5),
                new Vector3(3.2f, 1.1f, 1.2f), "Steel");
            Cube(root, "WORKBENCH_TOP", new Vector3(x, 1.7f, -5),
                new Vector3(3.3f, 0.12f, 1.3f), "Roof");
        }

        Beam(root, "REPAIR_HOIST_RAIL",
            new Vector3(-40, 9, -14), new Vector3(-20, 9, -14),
            0.35f, "Steel");

        // Paint booth exhaust ducts and diagnostic racks.
        Cube(root, "PAINT_BOOTH_EXHAUST", new Vector3(30, 7, -20),
            new Vector3(8, 0.5f, 1.0f), "Roof");
        for (int x = 25; x <= 35; x += 5)
        {
            Cube(root, "DIAGNOSTIC_RACK", new Vector3(x, 1.8f, -5),
                new Vector3(1.5f, 3.6f, 0.8f), "Steel");
        }

        // Painted hazard markings around work zones.
        for (int i = 0; i < 8; i++)
        {
            Cube(root, "HAZARD_STRIPE", new Vector3(25 + i * 1.2f, 0.22f, -15),
                new Vector3(0.45f, 0.025f, 0.7f), "SafetyYellow");
        }
    }

    private static void BuildLightingFixtures(Transform root)
    {
        for (int z = -24; z <= 24; z += 12)
        {
            for (int x = -30; x <= 30; x += 15)
            {
                Cube(root, "OVERHEAD_LIGHT_FIXTURE",
                    new Vector3(x, 19, z), new Vector3(4, 0.25f, 0.65f), "Light");

                GameObject lightObject = new GameObject("HANGAR_AREA_LIGHT");
                Undo.RegisterCreatedObjectUndo(lightObject, "Create Hangar Light");
                lightObject.transform.SetParent(root, true);
                lightObject.transform.position = new Vector3(x, 18.4f, z);

                Light light = lightObject.AddComponent<Light>();
                light.type = LightType.Point;
                light.range = 13;
                light.intensity = 1.2f;
                light.color = new Color(0.78f, 0.87f, 1f);
            }
        }

        // Perimeter industrial lights.
        for (int z = -24; z <= 24; z += 12)
        {
            Cube(root, "WALL_LIGHT_FIXTURE", new Vector3(-47.7f, 8, z),
                new Vector3(0.2f, 0.5f, 1.6f), "Light");
            Cube(root, "WALL_LIGHT_FIXTURE", new Vector3(47.7f, 8, z),
                new Vector3(0.2f, 0.5f, 1.6f), "Light");
        }
    }

    private static GameObject Cube(Transform parent, string name,
        Vector3 position, Vector3 scale, string materialKey)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = name;
        Undo.RegisterCreatedObjectUndo(obj, "Create Garage Architecture");
        obj.transform.SetParent(parent, false);
        obj.transform.localPosition = position;
        obj.transform.localScale = scale;
        obj.GetComponent<Renderer>().sharedMaterial = GetMaterial(materialKey);
        return obj;
    }

    private static GameObject Cylinder(Transform parent, string name,
        Vector3 position, Vector3 scale, string materialKey)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        obj.name = name;
        Undo.RegisterCreatedObjectUndo(obj, "Create Garage Detail");
        obj.transform.SetParent(parent, false);
        obj.transform.localPosition = position;
        obj.transform.localScale = scale;
        obj.GetComponent<Renderer>().sharedMaterial = GetMaterial(materialKey);
        return obj;
    }

    private static void Beam(Transform parent, string name, Vector3 start,
        Vector3 end, float thickness, string materialKey)
    {
        Vector3 delta = end - start;
        GameObject obj = Cube(parent, name, (start + end) * 0.5f,
            new Vector3(thickness, thickness, delta.magnitude), materialKey);
        obj.transform.rotation = Quaternion.LookRotation(delta.normalized);
    }

    private static Material GetMaterial(string key)
    {
        if (Materials.TryGetValue(key, out Material cached) && cached != null)
            return cached;

        Color color;
        switch (key)
        {
            case "Concrete": color = new Color(0.29f, 0.31f, 0.32f); break;
            case "Floor": color = new Color(0.38f, 0.40f, 0.41f); break;
            case "Wall": color = new Color(0.23f, 0.27f, 0.29f); break;
            case "Steel": color = new Color(0.12f, 0.16f, 0.19f); break;
            case "Roof": color = new Color(0.17f, 0.21f, 0.23f); break;
            case "Door": color = new Color(0.10f, 0.14f, 0.17f); break;
            case "Glass": color = new Color(0.20f, 0.55f, 0.68f, 0.55f); break;
            case "SafetyYellow": color = new Color(0.95f, 0.65f, 0.05f); break;
            case "FloorMarking": color = new Color(0.82f, 0.84f, 0.82f); break;
            case "Lane": color = new Color(0.21f, 0.24f, 0.25f); break;
            case "Pad": color = new Color(0.33f, 0.35f, 0.35f); break;
            case "Light": color = new Color(0.78f, 0.86f, 0.91f); break;
            default: color = Color.gray; break;
        }

        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");

        Material material = new Material(shader);
        material.name = "GarageArchitecture_" + key;
        material.color = color;
        if (key == "Glass")
        {
            material.SetFloat("_Surface", 1f);
            material.SetFloat("_Mode", 3f);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.renderQueue = 3000;
        }

        Materials[key] = material;
        return material;
    }

    [MenuItem("Obsidian Protocol/Build/Hide Old Garage Visuals (TEST SCENE ONLY)")]
    private static void HideOldGarageVisuals()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (!scene.IsValid() || !scene.isLoaded ||
            !scene.path.EndsWith("SCN04_GARAGE_ARCHITECTURE_TEST.unity",
                System.StringComparison.OrdinalIgnoreCase))
        {
            EditorUtility.DisplayDialog("Test Scene Required",
                "Open SCN04_GARAGE_ARCHITECTURE_TEST.unity before running this command.",
                "OK");
            return;
        }

        if (GameObject.Find(RootName) == null)
        {
            EditorUtility.DisplayDialog("Architecture Not Found",
                "GARAGE_ARCHITECTURE was not found. Build the new architecture preview first.",
                "OK");
            return;
        }

        string[] oldNames =
        {
            "FLOOR", "GARAGE_FLOOR", "CENTRAL_GARAGE_DECK", "CROSS_DECK",
            "EAST_WALL", "NORTH_WALL", "SOUTH_WALL", "WEST_WALL",
            "EAST_OUTER_WALL", "WEST_OUTER_WALL", "REAR_OUTER_WALL",
            "FRONT_WALL_LEFT", "FRONT_WALL_RIGHT",
            "GarageArchitecture_Floor", "GarageArchitecture_Wall",
            "GarageArchitecture_FloorMarking", "FLOOR_BAY_LINE",
            "FLOOR_SERVICE_LINE", "FLOOR_LIGHT"
        };

        if (!EditorUtility.DisplayDialog("Hide Old Garage Visuals",
            "This hides renderers on exact-name legacy geometry objects in the TEST scene only. " +
            "It does not delete objects, disable colliders, or change ZONE buttons. Continue?",
            "Hide Legacy Visuals", "Cancel"))
            return;

        int changed = 0;
        foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (go == null || go.scene != scene || go.transform.IsChildOf(
                    GameObject.Find(RootName).transform))
                continue;

            bool isOldGeometry = false;
            foreach (string oldName in oldNames)
            {
                if (go.name == oldName)
                {
                    isOldGeometry = true;
                    break;
                }
            }

            if (!isOldGeometry || go.name.StartsWith("ZONE_"))
                continue;

            foreach (Renderer renderer in go.GetComponents<Renderer>())
            {
                if (renderer.enabled)
                {
                    Undo.RecordObject(renderer, "Hide Old Garage Visuals");
                    renderer.enabled = false;
                    changed++;
                }
            }
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorUtility.DisplayDialog("Legacy Visuals Hidden",
            "Hidden " + changed + " old renderer(s). Objects, scripts, colliders, and zone buttons were preserved. " +
            "Use Edit > Undo if the result is not correct. Save the TEST scene only after reviewing it.",
            "OK");
    }
}



