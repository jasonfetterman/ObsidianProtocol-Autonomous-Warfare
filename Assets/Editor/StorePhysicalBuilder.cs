using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public static class StorePhysicalBuilder
{
    private static Transform root;

    [MenuItem("Obsidian Protocol/Store/BUILD PHYSICAL STORE")]
    public static void Build()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.path != "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity")
        {
            Debug.LogError("[STORE] Open SCN16_STORE.unity before building.");
            return;
        }

        GameObject facility = GameObject.Find("STORE_FACILITY");

        if (facility == null)
        {
            facility = new GameObject("STORE_FACILITY");
            Undo.RegisterCreatedObjectUndo(facility, "Create Store Facility");
        }

        root = facility.transform;

        ClearChildren();

        BuildShell();
        BuildEntrance();
        BuildInterior();
        BuildDisplayBays();
        BuildCommandShowroom();
        BuildEquipmentShowroom();
        BuildCheckout();
        BuildLighting();
        BuildSigns();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[STORE] PHYSICAL STORE BUILT.");
    }

    private static void ClearChildren()
    {
        List<GameObject> children = new List<GameObject>();

        for (int i = 0; i < root.childCount; i++)
            children.Add(root.GetChild(i).gameObject);

        foreach (GameObject child in children)
        {
            if (child.name == "MILITARY STORE HUD")
                continue;

            Object.DestroyImmediate(child);
        }
    }

    private static GameObject Cube(
        string name,
        Vector3 position,
        Vector3 scale,
        Material material)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(root);
        go.transform.localPosition = position;
        go.transform.localScale = scale;

        Renderer r = go.GetComponent<Renderer>();
        if (r != null)
            r.sharedMaterial = material;

        return go;
    }

    private static GameObject Cylinder(
        string name,
        Vector3 position,
        Vector3 scale,
        Material material)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        go.name = name;
        go.transform.SetParent(root);
        go.transform.localPosition = position;
        go.transform.localScale = scale;

        Renderer r = go.GetComponent<Renderer>();
        if (r != null)
            r.sharedMaterial = material;

        return go;
    }

    private static Material Mat(
        string name,
        Color color,
        float metallic = 0f,
        float smoothness = 0.5f)
    {
        Material material =
            new Material(Shader.Find("Universal Render Pipeline/Lit"));

        material.name = name;
        material.color = color;
        material.SetFloat("_Metallic", metallic);
        material.SetFloat("_Smoothness", smoothness);

        return material;
    }

    private static void BuildShell()
    {
        Material floor = Mat(
            "STORE_FLOOR",
            new Color(0.055f, 0.065f, 0.075f),
            0.65f,
            0.35f);

        Material wall = Mat(
            "STORE_WALL",
            new Color(0.075f, 0.085f, 0.10f),
            0.55f,
            0.30f);

        Material structural = Mat(
            "STORE_STRUCTURE",
            new Color(0.025f, 0.03f, 0.035f),
            0.8f,
            0.25f);

        Cube(
            "STORE_FLOOR",
            new Vector3(0, -0.15f, 0),
            new Vector3(34, 0.3f, 24),
            floor);

        Cube(
            "BACK_WALL",
            new Vector3(0, 6, 12),
            new Vector3(34, 12, 0.5f),
            wall);

        Cube(
            "LEFT_WALL",
            new Vector3(-17, 6, 0),
            new Vector3(0.5f, 12, 24),
            wall);

        Cube(
            "RIGHT_WALL",
            new Vector3(17, 6, 0),
            new Vector3(0.5f, 12, 24),
            wall);

        Cube(
            "FRONT_LEFT_WALL",
            new Vector3(-12, 6, -12),
            new Vector3(10, 12, 0.5f),
            wall);

        Cube(
            "FRONT_RIGHT_WALL",
            new Vector3(12, 6, -12),
            new Vector3(10, 12, 0.5f),
            wall);

        Cube(
            "FRONT_HEADER",
            new Vector3(0, 10.5f, -12),
            new Vector3(14, 3, 0.5f),
            structural);

        Cube(
            "ROOF",
            new Vector3(0, 12, 0),
            new Vector3(34, 0.5f, 24),
            structural);

        for (int x = -15; x <= 15; x += 5)
        {
            Cube(
                "STRUCTURAL_COLUMN",
                new Vector3(x, 6, 10.5f),
                new Vector3(0.45f, 12, 0.45f),
                structural);
        }
    }

    private static void BuildEntrance()
    {
        Material trim = Mat(
            "ENTRANCE_TRIM",
            new Color(0.12f, 0.14f, 0.16f),
            0.8f,
            0.4f);

        Material glass = Mat(
            "ENTRANCE_GLASS",
            new Color(0.08f, 0.18f, 0.23f),
            0.2f,
            0.8f);

        Cube(
            "ENTRANCE_LEFT_FRAME",
            new Vector3(-4.5f, 3.5f, -11.65f),
            new Vector3(0.35f, 7, 0.3f),
            trim);

        Cube(
            "ENTRANCE_RIGHT_FRAME",
            new Vector3(4.5f, 3.5f, -11.65f),
            new Vector3(0.35f, 7, 0.3f),
            trim);

        Cube(
            "ENTRANCE_TOP_FRAME",
            new Vector3(0, 7, -11.65f),
            new Vector3(9, 0.35f, 0.3f),
            trim);

        Cube(
            "ENTRANCE_GLASS_LEFT",
            new Vector3(-6.8f, 4, -11.7f),
            new Vector3(3.2f, 8, 0.15f),
            glass);

        Cube(
            "ENTRANCE_GLASS_RIGHT",
            new Vector3(6.8f, 4, -11.7f),
            new Vector3(3.2f, 8, 0.15f),
            glass);
    }

    private static void BuildInterior()
    {
        Material trim = Mat(
            "INTERIOR_TRIM",
            new Color(0.10f, 0.11f, 0.13f),
            0.75f,
            0.35f);

        for (int x = -15; x <= 15; x += 5)
        {
            Cube(
                "INTERIOR_COLUMN",
                new Vector3(x, 4, 8),
                new Vector3(0.35f, 8, 0.35f),
                trim);
        }

        for (int z = -8; z <= 8; z += 4)
        {
            Cube(
                "CEILING_BEAM",
                new Vector3(0, 11.2f, z),
                new Vector3(33, 0.35f, 0.35f),
                trim);
        }
    }

    private static void BuildDisplayBays()
    {
        Material display = Mat(
            "DISPLAY_STRUCTURE",
            new Color(0.035f, 0.045f, 0.055f),
            0.85f,
            0.3f);

        Material accent = Mat(
            "DISPLAY_ACCENT",
            new Color(0.16f, 0.20f, 0.24f),
            0.7f,
            0.5f);

        BuildBay("AIR_OPERATIONS", -12, 4, 4.5f, display, accent);
        BuildBay("GROUND_SYSTEMS", 0, 4, 4.5f, display, accent);
        BuildBay("COMMAND_SYSTEMS", 12, 4, 4.5f, display, accent);

        BuildBay("SEA_SYSTEMS", -12, 9, 4.5f, display, accent);
        BuildBay("EXPERIMENTAL", 0, 9, 4.5f, display, accent);
        BuildBay("SATELLITE_SYSTEMS", 12, 9, 4.5f, display, accent);
    }

    private static void BuildBay(
        string name,
        float x,
        float z,
        float width,
        Material structure,
        Material accent)
    {
        Cube(
            name + "_BACK",
            new Vector3(x, 3.2f, z),
            new Vector3(width, 6.4f, 0.25f),
            structure);

        Cube(
            name + "_COUNTER",
            new Vector3(x, 0.9f, z - 1.4f),
            new Vector3(width, 1.0f, 2.2f),
            accent);

        Cube(
            name + "_TOP",
            new Vector3(x, 6.5f, z),
            new Vector3(width, 0.18f, 0.35f),
            accent);

        Cube(
            name + "_LEFT",
            new Vector3(x - width / 2f, 3.2f, z),
            new Vector3(0.2f, 6.4f, 0.4f),
            accent);

        Cube(
            name + "_RIGHT",
            new Vector3(x + width / 2f, 3.2f, z),
            new Vector3(0.2f, 6.4f, 0.4f),
            accent);
    }

    private static void BuildCommandShowroom()
    {
        Material platform = Mat(
            "COMMAND_PLATFORM",
            new Color(0.07f, 0.08f, 0.09f),
            0.75f,
            0.4f);

        Cube(
            "COMMAND_UNIT_PLATFORM",
            new Vector3(0, 0.35f, 1),
            new Vector3(8, 0.7f, 5),
            platform);

        Cube(
            "COMMAND_UNIT_BACKDROP",
            new Vector3(0, 4.5f, 3.4f),
            new Vector3(8, 8.5f, 0.25f),
            platform);

        for (int x = -3; x <= 3; x += 2)
        {
            Cylinder(
                "SHOWROOM_BOLLARD",
                new Vector3(x, 0.8f, -1.7f),
                new Vector3(0.12f, 0.6f, 0.12f),
                platform);
        }
    }

    private static void BuildEquipmentShowroom()
    {
        Material shelf = Mat(
            "EQUIPMENT_SHELF",
            new Color(0.055f, 0.065f, 0.075f),
            0.8f,
            0.3f);

        for (int row = 0; row < 3; row++)
        {
            float z = -5.5f + row * 2.7f;

            Cube(
                "EQUIPMENT_SHELF_" + row,
                new Vector3(12, 2.2f, z),
                new Vector3(7, 4.4f, 0.45f),
                shelf);
        }
    }

    private static void BuildCheckout()
    {
        Material terminal = Mat(
            "CHECKOUT_TERMINAL",
            new Color(0.04f, 0.055f, 0.065f),
            0.9f,
            0.4f);

        Material counter = Mat(
            "CHECKOUT_COUNTER",
            new Color(0.08f, 0.09f, 0.10f),
            0.75f,
            0.35f);

        Cube(
            "PURCHASE_COUNTER",
            new Vector3(-12, 1.1f, -5.5f),
            new Vector3(7, 2.2f, 2.2f),
            counter);

        Cube(
            "PURCHASE_TERMINAL",
            new Vector3(-12, 2.8f, -5.5f),
            new Vector3(3, 2.5f, 0.5f),
            terminal);

        Cube(
            "FINAL_REVIEW_TERMINAL",
            new Vector3(-4, 1.8f, -8),
            new Vector3(4, 3.6f, 0.8f),
            terminal);
    }

    private static void BuildLighting()
    {
        for (int x = -12; x <= 12; x += 6)
        {
            for (int z = -6; z <= 6; z += 6)
            {
                GameObject lightObject =
                    new GameObject("STORE_LIGHT");

                lightObject.transform.SetParent(root);
                lightObject.transform.localPosition =
                    new Vector3(x, 10.5f, z);

                Light light =
                    lightObject.AddComponent<Light>();

                light.type = LightType.Point;
                light.range = 9f;
                light.intensity = 7f;
                light.color =
                    new Color(0.82f, 0.90f, 1.0f);
            }
        }
    }

    private static void BuildSigns()
    {
        Material sign = Mat(
            "STORE_SIGN",
            new Color(0.025f, 0.035f, 0.045f),
            0.8f,
            0.45f);

        Cube(
            "MILITARY_STORE_SIGN",
            new Vector3(0, 8.8f, -11.8f),
            new Vector3(14, 2.5f, 0.3f),
            sign);

        Cube(
            "COMMAND_CENTER_SIGN",
            new Vector3(0, 7.8f, 3.2f),
            new Vector3(7, 1.2f, 0.25f),
            sign);

        Cube(
            "PURCHASE_SIGN",
            new Vector3(-12, 4.2f, -4.2f),
            new Vector3(6, 1.0f, 0.25f),
            sign);
    }
}
