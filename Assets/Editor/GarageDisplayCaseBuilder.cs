
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GarageDisplayCaseBuilder
{
    private const string MaterialFolder = "Assets/Materials";

    private static Material metal;
    private static Material trim;
    private static Material glass;
    private static Material cyan;

    private static readonly string[] Terminals =
    {
        "TERMINAL_DOCTRINE",
        "TERMINAL_FINANCE",
        "TERMINAL_DEPLOYMENT_BUDGET",
        "TERMINAL_SELECT_FORCE",
        "TERMINAL_COMMAND_INTENT",
        "TERMINAL_RULES_OF_ENGAGEMENT",
        "TERMINAL_LOGISTICS",
        "TERMINAL_AI_COMMAND",
        "TERMINAL_FINAL_DEPLOYMENT",
        "TERMINAL_DEPLOYMENT",
        "TERMINAL_SELECT_OPERATION",
        "TERMINAL_FORMATION",
        "TERMINAL_FLEET_MANAGEMENT",
        "TERMINAL_RESEARCH"
    };

    [MenuItem("OPAW/Garage/Build Enclosed Display Cases")]
    public static void Build()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name != "SCN04_GARAGE")
        {
            Debug.LogError("Open SCN04_GARAGE before building display cases.");
            return;
        }

        EnsureMaterials();

        int built = 0;
        int repaired = 0;

        foreach (string terminalName in Terminals)
        {
            GameObject terminal = GameObject.Find(terminalName);

            if (terminal == null)
            {
                Debug.LogWarning("Terminal not found: " + terminalName);
                continue;
            }

            Transform display = FindDescendant(terminal.transform, "DISPLAY");

            if (display == null)
            {
                Debug.LogWarning("DISPLAY not found: " + terminalName);
                continue;
            }

            Transform rootTransform = terminal.transform.Find("[DISPLAY CASE]");

            GameObject root;

            if (rootTransform == null)
            {
                root = new GameObject("[DISPLAY CASE]");
                Undo.RegisterCreatedObjectUndo(root, "Build Display Case");
                root.transform.SetParent(terminal.transform, false);
                root.transform.localPosition = display.localPosition;
                root.transform.localRotation = display.localRotation;
                root.transform.localScale = Vector3.one;

                built++;
            }
            else
            {
                root = rootTransform.gameObject;
                repaired++;

                // Remove previously generated parts so the new
                // collision layout is clean and predictable.
                for (int i = root.transform.childCount - 1; i >= 0; i--)
                {
                    Undo.DestroyObjectImmediate(root.transform.GetChild(i).gameObject);
                }
            }

            float width = Mathf.Max(4.8f, display.localScale.x * 2f + 0.8f);
            float height = Mathf.Max(4.2f, display.localScale.y * 2f + 1.2f);
            float depth = Mathf.Max(3.6f, display.localScale.z * 2f + 3.2f);

            BuildCase(root.transform, width, height, depth);
        }

        EditorSceneManager.MarkSceneDirty(scene);
        AssetDatabase.SaveAssets();
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "DISPLAY CASE BUILD COMPLETE. New: " + built +
            " | Repaired: " + repaired +
            " | All cases have dedicated solid collision volumes. " +
            "Resource crates were not modified."
        );
    }

    private static void BuildCase(
        Transform root,
        float width,
        float height,
        float depth)
    {
        float bottom = -height * 0.5f;
        float top = height * 0.5f;

        float wallHeight = height - 0.9f;
        float wallCenter = 0f;

        float wallThickness = 0.16f;
        float sideOffsetX = width * 0.5f - wallThickness * 0.5f;
        float frontOffsetZ = depth * 0.5f - wallThickness * 0.5f;

        // ARMORED BASE

        Part(root, "Lower Armored Footing",
            new Vector3(0, bottom + 0.08f, 0),
            new Vector3(width + 0.25f, 0.16f, depth + 0.25f),
            metal);

        Part(root, "Main Armored Plinth",
            new Vector3(0, bottom + 0.32f, 0),
            new Vector3(width, 0.42f, depth),
            metal);

        Part(root, "Inset Base Deck",
            new Vector3(0, bottom + 0.55f, 0),
            new Vector3(width - 0.3f, 0.06f, depth - 0.3f),
            trim);

        Part(root, "Base Front Light",
            new Vector3(0, bottom + 0.61f, -depth * 0.5f - 0.025f),
            new Vector3(width - 0.55f, 0.035f, 0.04f),
            cyan);

        // TOP CANOPY

        Part(root, "Upper Armored Canopy",
            new Vector3(0, top - 0.30f, 0),
            new Vector3(width, 0.42f, depth),
            metal);

        Part(root, "Raised Canopy Cap",
            new Vector3(0, top - 0.06f, 0),
            new Vector3(width + 0.25f, 0.12f, depth + 0.25f),
            trim);

        Part(root, "Top Inset Armor",
            new Vector3(0, top + 0.01f, 0),
            new Vector3(width * 0.42f, 0.035f, depth * 0.42f),
            metal);

        Part(root, "Canopy Front Light",
            new Vector3(0, top - 0.55f, -depth * 0.5f - 0.025f),
            new Vector3(width - 0.55f, 0.035f, 0.04f),
            cyan);

        // FOUR CORNER SUPPORTS

        float px = width * 0.5f - 0.10f;
        float pz = depth * 0.5f - 0.10f;

        foreach (float x in new float[] { -px, px })
        {
            foreach (float z in new float[] { -pz, pz })
            {
                Part(root, "Reinforced Corner Column",
                    new Vector3(x, wallCenter, z),
                    new Vector3(0.20f, wallHeight, 0.20f),
                    metal);

                Part(root, "Corner Armor Collar",
                    new Vector3(x, top - 0.58f, z),
                    new Vector3(0.34f, 0.22f, 0.34f),
                    trim);
            }
        }

        // GLASS WALL VISUALS

        Part(root, "Front Glass",
            new Vector3(0, wallCenter, -frontOffsetZ),
            new Vector3(width - 0.42f, wallHeight, 0.035f),
            glass);

        Part(root, "Rear Glass",
            new Vector3(0, wallCenter, frontOffsetZ),
            new Vector3(width - 0.42f, wallHeight, 0.035f),
            glass);

        Part(root, "Left Glass",
            new Vector3(-sideOffsetX, wallCenter, 0),
            new Vector3(0.035f, wallHeight, depth - 0.42f),
            glass);

        Part(root, "Right Glass",
            new Vector3(sideOffsetX, wallCenter, 0),
            new Vector3(0.035f, wallHeight, depth - 0.42f),
            glass);

        // DEDICATED COLLISION VOLUMES
        // These are separate from the glass render objects.
        // They are thick enough to prevent walking through the walls.

        ColliderPart(root, "COLLISION_FRONT",
            new Vector3(0, wallCenter, -frontOffsetZ),
            new Vector3(width - 0.25f, wallHeight, 0.24f));

        ColliderPart(root, "COLLISION_REAR",
            new Vector3(0, wallCenter, frontOffsetZ),
            new Vector3(width - 0.25f, wallHeight, 0.24f));

        ColliderPart(root, "COLLISION_LEFT",
            new Vector3(-sideOffsetX, wallCenter, 0),
            new Vector3(0.24f, wallHeight, depth - 0.25f));

        ColliderPart(root, "COLLISION_RIGHT",
            new Vector3(sideOffsetX, wallCenter, 0),
            new Vector3(0.24f, wallHeight, depth - 0.25f));

        // Solid floor and roof collision surfaces.

        ColliderPart(root, "COLLISION_FLOOR",
            new Vector3(0, bottom + 0.55f, 0),
            new Vector3(width - 0.2f, 0.18f, depth - 0.2f));

        ColliderPart(root, "COLLISION_ROOF",
            new Vector3(0, top - 0.20f, 0),
            new Vector3(width - 0.1f, 0.24f, depth - 0.1f));
    }

    private static void Part(
        Transform root,
        string name,
        Vector3 position,
        Vector3 size,
        Material material)
    {
        GameObject part = GameObject.CreatePrimitive(PrimitiveType.Cube);

        part.name = name;
        part.transform.SetParent(root, false);
        part.transform.localPosition = position;
        part.transform.localRotation = Quaternion.identity;
        part.transform.localScale = size;

        MeshRenderer renderer = part.GetComponent<MeshRenderer>();
        renderer.sharedMaterial = material;

        // Visual meshes do not provide collision.
        // Dedicated collision objects handle the case boundaries.
        Collider collider = part.GetComponent<Collider>();

        if (collider != null)
            Object.DestroyImmediate(collider);
    }

    private static void ColliderPart(
        Transform root,
        string name,
        Vector3 position,
        Vector3 size)
    {
        GameObject part = new GameObject(name);
        part.transform.SetParent(root, false);
        part.transform.localPosition = position;
        part.transform.localRotation = Quaternion.identity;
        part.transform.localScale = Vector3.one;

        BoxCollider box = part.AddComponent<BoxCollider>();
        box.center = Vector3.zero;
        box.size = size;
        box.isTrigger = false;
        box.enabled = true;
    }

    private static Transform FindDescendant(
        Transform parent,
        string targetName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == targetName)
                return child;

            Transform result = FindDescendant(child, targetName);

            if (result != null)
                return result;
        }

        return null;
    }

    private static void EnsureMaterials()
    {
        if (!AssetDatabase.IsValidFolder(MaterialFolder))
            AssetDatabase.CreateFolder("Assets", "Materials");

        metal = GetMaterial(
            "OPAW_Case_Gunmetal",
            new Color(0.025f, 0.04f, 0.055f), false);

        trim = GetMaterial(
            "OPAW_Case_ArmorTrim",
            new Color(0.10f, 0.15f, 0.19f), false);

        cyan = GetMaterial(
            "OPAW_Case_Cyan",
            new Color(0.02f, 0.65f, 1f), false);

        glass = GetMaterial(
            "OPAW_Case_Glass",
            new Color(0.25f, 0.75f, 0.9f, 0.12f), true);

        cyan.EnableKeyword("_EMISSION");

        if (cyan.HasProperty("_EmissionColor"))
        {
            cyan.SetColor(
                "_EmissionColor",
                new Color(0.02f, 0.65f, 1f) * 2.5f);
        }
    }

    private static Material GetMaterial(
        string name,
        Color color,
        bool transparent)
    {
        string path = MaterialFolder + "/" + name + ".mat";

        Material material =
            AssetDatabase.LoadAssetAtPath<Material>(path);

        if (material != null)
            return material;

        Shader shader =
            Shader.Find("Universal Render Pipeline/Lit");

        if (shader == null)
            shader = Shader.Find("Standard");

        material = new Material(shader);
        material.name = name;

        if (material.HasProperty("_BaseColor"))
            material.SetColor("_BaseColor", color);

        if (material.HasProperty("_Color"))
            material.SetColor("_Color", color);

        if (transparent)
        {
            if (material.HasProperty("_Surface"))
                material.SetFloat("_Surface", 1f);

            if (material.HasProperty("_ZWrite"))
                material.SetFloat("_ZWrite", 0f);

            material.SetOverrideTag("RenderType", "Transparent");
            material.renderQueue = 3000;
            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.EnableKeyword("_ALPHABLEND_ON");
        }

        AssetDatabase.CreateAsset(material, path);

        return material;
    }
}
