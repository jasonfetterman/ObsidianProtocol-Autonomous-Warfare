using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public static class SCN17HologramBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN17_PERSISTENT_MILITARY/SCN17_PERSISTENT_MILITARY.unity";

    private const string RootName = "SCN17 HOLOGRAM UPGRADE";
    private const string MaterialPath = "Assets/Materials/SCN17_HologramOverlay.mat";

    private static readonly string[] Screens =
    {
        "AI DEVELOPMENT SCREEN",
        "DAMAGE HISTORY SCREEN",
        "FLEET GROWTH SCREEN",
        "REPAIR RECORD A SCREEN",
        "REPAIR RECORD B SCREEN",
        "REPAIR RECORD C SCREEN",
        "MISSION HISTORY SCREEN",
        "EQUIPMENT TIMELINE SCREEN",
        "OPERATIONAL EXPERIENCE SCREEN",
        "COMMANDER DEVELOPMENT SCREEN",
        "RECOVERY STATUS SCREEN",
        "REPAIR HISTORY SCREEN",
        "DOCTRINE DEVELOPMENT SCREEN",
        "MISSION ARCHIVE A SCREEN",
        "MISSION ARCHIVE B SCREEN",
        "MISSION ARCHIVE C SCREEN",
        "COMBAT HISTORY SCREEN",
        "TECHNOLOGY GROWTH SCREEN",
        "LOSS RECORD SCREEN",
        "VETERAN TERMINAL A SCREEN",
        "VETERAN TERMINAL B SCREEN",
        "VETERAN TERMINAL C SCREEN",
        "UNIT HISTORY TERMINAL A SCREEN",
        "UNIT HISTORY TERMINAL B SCREEN",
        "UNIT HISTORY TERMINAL C SCREEN",
        "UNIT RECORDS DISPLAY",
        "SERVICE HISTORY DISPLAY",
        "MILITARY DEVELOPMENT DISPLAY",
        "ARCHIVE CONTROL DISPLAY",
        "UNIT RECORD — BULLDOG-024 SCREEN"
    };

    [MenuItem("Obsidian Protocol/SCN17/Build Holographic Displays")]
    public static void Build()
    {
        if (EditorSceneManager.GetActiveScene().path != ScenePath)
        {
            Debug.LogError("[SCN17 HOLOGRAMS] Open SCN17_PERSISTENT_MILITARY first. No changes made.");
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        GameObject architecture = GameObject.Find("FACILITY ARCHITECTURE");
        if (architecture == null)
        {
            Debug.LogError("[SCN17 HOLOGRAMS] FACILITY ARCHITECTURE not found.");
            return;
        }

        Transform existing = architecture.transform.Find(RootName);
        if (existing != null)
        {
            Debug.Log("[SCN17 HOLOGRAMS] Upgrade already exists; no duplicates created.");
            return;
        }

        Material hologram = LoadOrCreateMaterial();
        if (hologram == null)
        {
            Debug.LogError("[SCN17 HOLOGRAMS] Could not create hologram material.");
            return;
        }

        GameObject root = new GameObject(RootName);
        Undo.RegisterCreatedObjectUndo(root, "Build SCN17 Holograms");
        root.transform.SetParent(architecture.transform, false);

        int built = 0;
        int skipped = 0;

        foreach (string screenName in Screens)
        {
            GameObject screen = FindSceneObject(screenName);

            if (screen == null)
            {
                skipped++;
                Debug.LogWarning("[SCN17 HOLOGRAMS] Screen not found: " + screenName);
                continue;
            }

            Renderer screenRenderer = screen.GetComponent<Renderer>();
            if (screenRenderer == null)
                screenRenderer = screen.GetComponentInChildren<Renderer>();

            if (screenRenderer == null)
            {
                skipped++;
                Debug.LogWarning("[SCN17 HOLOGRAMS] No renderer on: " + screenName);
                continue;
            }

            MeshFilter meshFilter =
                screenRenderer.GetComponent<MeshFilter>();

            if (meshFilter == null || meshFilter.sharedMesh == null)
            {
                skipped++;
                Debug.LogWarning("[SCN17 HOLOGRAMS] No mesh available on: " + screenName);
                continue;
            }

            GameObject overlay = GameObject.CreatePrimitive(PrimitiveType.Quad);
            overlay.name = "HOLOGRAM - " + screenName;
            overlay.transform.SetParent(root.transform, false);

            MeshCollider collider = overlay.GetComponent<MeshCollider>();
            if (collider != null)
                Object.DestroyImmediate(collider);

            Transform surface = screenRenderer.transform;
            Bounds bounds = meshFilter.sharedMesh.bounds;

            overlay.transform.position =
                surface.TransformPoint(bounds.center) +
                surface.forward *
                (bounds.extents.z * surface.lossyScale.z + 0.015f);

            overlay.transform.rotation = surface.rotation;
            overlay.transform.localScale = Vector3.one;

            Vector3 worldScale = surface.lossyScale;
            float width = bounds.size.x * Mathf.Abs(worldScale.x);
            float height = bounds.size.y * Mathf.Abs(worldScale.y);

            overlay.transform.localScale = new Vector3(width, height, 1f);

            Renderer overlayRenderer = overlay.GetComponent<Renderer>();
            overlayRenderer.sharedMaterial = hologram;
            overlayRenderer.shadowCastingMode =
                UnityEngine.Rendering.ShadowCastingMode.Off;
            overlayRenderer.receiveShadows = false;

            built++;
        }

        EditorSceneManager.MarkSceneDirty(
            EditorSceneManager.GetActiveScene());

        EditorSceneManager.SaveScene(
            EditorSceneManager.GetActiveScene());

        Debug.Log("[SCN17 HOLOGRAMS] Built " + built +
                  " holographic overlays. Skipped " + skipped +
                  ". Entrance, windows, interior details and HUD were not modified.");
    }

    private static GameObject FindSceneObject(string objectName)
    {
        foreach (GameObject item in
                 Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (item == null || !item.scene.IsValid())
                continue;

            if (item.scene.path != ScenePath)
                continue;

            if (item.name == objectName)
                return item;
        }

        return null;
    }

    private static Material LoadOrCreateMaterial()
    {
        Material existing = AssetDatabase.LoadAssetAtPath<Material>(
            MaterialPath);

        if (existing != null)
            return existing;

        if (!AssetDatabase.IsValidFolder("Assets/Materials"))
            AssetDatabase.CreateFolder("Assets", "Materials");

        Shader shader = Shader.Find("Standard");
        if (shader == null)
            return null;

        Material material = new Material(shader);
        material.name = "SCN17 Hologram Cyan";

        material.SetFloat("_Mode", 3f);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.EnableKeyword("_EMISSION");
        material.renderQueue = 3000;

        material.color = new Color(0.05f, 0.85f, 1f, 0.22f);
        material.SetColor("_EmissionColor", new Color(0.02f, 1f, 1f, 1f));

        AssetDatabase.CreateAsset(material, MaterialPath);
        AssetDatabase.SaveAssets();

        return material;
    }
}
