using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackspireTreePainter : EditorWindow
{
    [Serializable]
    private class PaintStamp
    {
        public Vector3 position;
        public float radius;
    }

    private class PaintData : ScriptableObject
    {
        public List<PaintStamp> stamps = new List<PaintStamp>();
    }

    private const string DataPath =
        "Assets/Editor/BlackspireTreePaintData.asset";
    private const string TreeFolder =
        "Assets/Realistic Tree/Prefabs/URP";
    private const string GeneratedRootName =
        "BLACKSPIRE_GENERATED_TREES";

    private MeshCollider target;
    private PaintData data;
    private float brushRadius = 20f;
    private float spacing = 8f;
    private float density = 1f;
    private float minScale = 0.8f;
    private float maxScale = 1.2f;
    private float maxSlope = 35f;
    private int seed = 12345;
    private bool eraseMode;
    private bool painting;
    private bool showBrush = true;
    private List<GameObject> prefabs = new List<GameObject>();

    [MenuItem("Obsidian Protocol/World Building/Blackspire Tree Painter")]
    private static void Open()
    {
        GetWindow<BlackspireTreePainter>("Blackspire Tree Painter");
    }

    private void OnEnable()
    {
        LoadData();
        LoadPrefabs();
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void LoadData()
    {
        data = AssetDatabase.LoadAssetAtPath<PaintData>(DataPath);
    }

    private void EnsureData()
    {
        if (data != null) return;

        data = ScriptableObject.CreateInstance<PaintData>();
        AssetDatabase.CreateAsset(data, DataPath);
        AssetDatabase.SaveAssets();
    }

    private void LoadPrefabs()
    {
        prefabs.Clear();

        foreach (string guid in AssetDatabase.FindAssets("t:Prefab",
                     new[] { TreeFolder }))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string file = System.IO.Path.GetFileNameWithoutExtension(path);

            if (file.IndexOf("Group", StringComparison.OrdinalIgnoreCase) >= 0)
                continue;

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab != null)
                prefabs.Add(prefab);
        }

        prefabs.Sort((a, b) => string.Compare(
            a.name, b.name, StringComparison.OrdinalIgnoreCase));
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("BLACKSPIRE TREE PAINTER",
            EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "Paint forest zones on the Blackspire mesh. Painting stores " +
            "brush stamps; it does not change the scene until you generate trees.",
            MessageType.Info);

        if (GUILayout.Button("Find Blackspire Mesh Collider"))
            FindTarget();

        target = (MeshCollider)EditorGUILayout.ObjectField(
            "Target Mesh Collider", target, typeof(MeshCollider), true);

        if (target == null || target.sharedMesh == null)
            EditorGUILayout.HelpBox(
                "Assign the terrain's enabled MeshCollider before painting.",
                MessageType.Warning);

        brushRadius = EditorGUILayout.Slider("Brush Radius (m)",
            brushRadius, 2f, 100f);
        eraseMode = EditorGUILayout.Toggle("Erase Painted Zones", eraseMode);
        showBrush = EditorGUILayout.Toggle("Show Brush in Scene View", showBrush);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tree Generation", EditorStyles.boldLabel);
        spacing = EditorGUILayout.Slider("Minimum Spacing (m)", spacing, 2f, 30f);
        density = EditorGUILayout.Slider("Density", density, 0.1f, 3f);
        minScale = EditorGUILayout.Slider("Minimum Scale", minScale, 0.3f, 2f);
        maxScale = EditorGUILayout.Slider("Maximum Scale", maxScale, minScale, 3f);
        maxSlope = EditorGUILayout.Slider("Maximum Slope (degrees)", maxSlope, 0f, 60f);
        seed = EditorGUILayout.IntField("Random Seed", seed);

        EditorGUILayout.Space();
        int stampCount = data == null ? 0 : data.stamps.Count;
        EditorGUILayout.LabelField("Paint stamps: " + stampCount);
        EditorGUILayout.LabelField("Available tree prefabs: " + prefabs.Count);

        using (new EditorGUI.DisabledScope(target == null ||
                   target.sharedMesh == null))
        {
            if (GUILayout.Button("Enable Scene Painting"))
            {
                Selection.activeGameObject = target.gameObject;
                SceneView.lastActiveSceneView?.FrameSelected();
                SceneView.RepaintAll();
            }
        }

        using (new EditorGUI.DisabledScope(stampCount == 0))
        {
            if (GUILayout.Button("Clear All Painted Zones"))
            {
                if (EditorUtility.DisplayDialog("Clear Painted Zones",
                        "Remove all saved paint stamps? Existing trees will remain.",
                        "Clear Zones", "Cancel"))
                {
                    Undo.RecordObject(data, "Clear Blackspire Paint Zones");
                    data.stamps.Clear();
                    EditorUtility.SetDirty(data);
                    SceneView.RepaintAll();
                }
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(
            "Generate and Clear Trees operate only on the dedicated " +
            GeneratedRootName + " object. Existing scene objects are untouched.",
            MessageType.Warning);

        using (new EditorGUI.DisabledScope(stampCount == 0 ||
                   target == null || prefabs.Count == 0))
        {
            if (GUILayout.Button("Generate Trees in Painted Zones"))
                GenerateTrees();
        }

        if (GUILayout.Button("Clear Generated Trees Only"))
            ClearGeneratedTrees();
    }

    private void FindTarget()
    {
        MeshCollider best = null;

        foreach (MeshCollider col in FindObjectsByType<MeshCollider>(
                     FindObjectsSortMode.None))
        {
            if (!col.enabled || col.sharedMesh == null) continue;

            string objectName = col.gameObject.name;
            string hierarchy = objectName;
            Transform parent = col.transform.parent;

            while (parent != null)
            {
                hierarchy += " " + parent.name;
                parent = parent.parent;
            }

            if (hierarchy.IndexOf("BLACKSPIRE MESHY TERRAIN",
                    StringComparison.OrdinalIgnoreCase) >= 0)
            {
                best = col;
                break;
            }
        }

        target = best;

        if (target == null)
            EditorUtility.DisplayDialog("Terrain Collider Not Found",
                "Open Blackspire_Continent and make sure its terrain " +
                "MeshCollider is enabled. You can also assign it manually.",
                "OK");
    }

    private void OnSceneGUI(SceneView view)
    {
        if (target == null || !target.enabled || target.sharedMesh == null)
            return;

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        bool hitTerrain = target.Raycast(ray, out RaycastHit hit, 100000f);

        if (showBrush && hitTerrain)
        {
            Handles.color = eraseMode ? Color.red : Color.green;
            Handles.DrawWireDisc(hit.point, hit.normal, brushRadius);
        }

        if (!hitTerrain) return;

        if (e.type == EventType.Layout)
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(
                FocusType.Passive));

        if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
        {
            painting = true;
            ApplyBrush(hit.point);
            e.Use();
        }
        else if (e.type == EventType.MouseDrag && e.button == 0 && painting)
        {
            ApplyBrush(hit.point);
            e.Use();
        }
        else if (e.type == EventType.MouseUp && e.button == 0 && painting)
        {
            painting = false;
            e.Use();
        }
    }

    private void ApplyBrush(Vector3 point)
    {
        EnsureData();
        Undo.RecordObject(data, eraseMode
            ? "Erase Blackspire Forest Zone"
            : "Paint Blackspire Forest Zone");

        if (eraseMode)
        {
            data.stamps.RemoveAll(s =>
                Vector3.Distance(s.position, point) <= brushRadius + s.radius);
        }
        else
        {
            data.stamps.Add(new PaintStamp
            {
                position = point,
                radius = brushRadius
            });
        }

        EditorUtility.SetDirty(data);
        SceneView.RepaintAll();
        Repaint();
    }

    private bool IsPainted(Vector3 point)
    {
        if (data == null) return false;

        foreach (PaintStamp stamp in data.stamps)
        {
            Vector2 a = new Vector2(point.x, point.z);
            Vector2 b = new Vector2(stamp.position.x, stamp.position.z);

            if (Vector2.Distance(a, b) <= stamp.radius)
                return true;
        }

        return false;
    }

    private void GenerateTrees()
    {
        if (EditorApplication.isPlaying || target == null ||
            !target.enabled || prefabs.Count == 0 || data == null)
            return;

        if (!EditorUtility.DisplayDialog("Generate Blackspire Trees",
                "Generate trees inside the painted zones? This replaces " +
                "only previously generated trees in " + GeneratedRootName + ".",
                "Generate", "Cancel"))
            return;

        Transform root = GetGeneratedRoot();
        if (root == null)
        {
            GameObject rootObject = new GameObject(GeneratedRootName);
            Undo.RegisterCreatedObjectUndo(rootObject, "Create Tree Container");
            root = rootObject.transform;
        }

        Undo.RecordObject(root.gameObject, "Regenerate Blackspire Trees");

        for (int i = root.childCount - 1; i >= 0; i--)
            Undo.DestroyObjectImmediate(root.GetChild(i).gameObject);

        UnityEngine.Random.State oldState = UnityEngine.Random.state;
        UnityEngine.Random.InitState(seed);

        Bounds bounds = target.bounds;
        float step = Mathf.Max(1f, spacing / Mathf.Sqrt(density));
        int generated = 0;
        int attempts = 0;
        int maxAttempts = Mathf.CeilToInt(
            bounds.size.x * bounds.size.z / (step * step) * 5f);
        maxAttempts = Mathf.Clamp(maxAttempts, 1000, 500000);

        try
        {
            for (float x = bounds.min.x; x <= bounds.max.x; x += step)
            for (float z = bounds.min.z; z <= bounds.max.z; z += step)
            {
                if (++attempts > maxAttempts) break;
                Vector3 origin = new Vector3(
                    x + UnityEngine.Random.Range(-step * 0.45f, step * 0.45f),
                    bounds.max.y + 10f,
                    z + UnityEngine.Random.Range(-step * 0.45f, step * 0.45f));

                if (!target.Raycast(new Ray(origin, Vector3.down),
                        out RaycastHit hit, bounds.size.y + 100f))
                    continue;

                if (!IsPainted(hit.point)) continue;
                if (Vector3.Angle(hit.normal, Vector3.up) > maxSlope) continue;

                GameObject prefab = prefabs[
                    UnityEngine.Random.Range(0, prefabs.Count)];

                GameObject tree = (GameObject)PrefabUtility.InstantiatePrefab(
                    prefab, root.gameObject.scene);

                if (tree == null) continue;

                Undo.RegisterCreatedObjectUndo(tree, "Generate Blackspire Tree");
                tree.transform.SetParent(root, true);
                tree.transform.position = hit.point;
                tree.transform.rotation = Quaternion.Euler(
                    0f, UnityEngine.Random.Range(0f, 360f), 0f);

                float scale = UnityEngine.Random.Range(minScale, maxScale);
                tree.transform.localScale = tree.transform.localScale * scale;
                generated++;
            }
        }
        finally
        {
            UnityEngine.Random.state = oldState;
            EditorUtility.SetDirty(root.gameObject);
            EditorSceneManager.MarkSceneDirty(root.gameObject.scene);
        }

        Debug.Log("[BLACKSPIRE TREES] Generated " + generated +
                  " trees inside painted zones.");
        EditorUtility.DisplayDialog("Tree Generation Complete",
            "Generated " + generated + " trees.", "OK");
    }

    private Transform GetGeneratedRoot()
    {
        foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
            if (go.name == GeneratedRootName)
                return go.transform;

        return null;
    }

    private void ClearGeneratedTrees()
    {
        Transform root = GetGeneratedRoot();
        if (root == null)
        {
            EditorUtility.DisplayDialog("No Generated Trees",
                "The dedicated generated-tree container does not exist.", "OK");
            return;
        }

        if (!EditorUtility.DisplayDialog("Clear Generated Trees",
                "Delete only the children of " + GeneratedRootName + "?",
                "Clear Trees", "Cancel"))
            return;

        Undo.RegisterFullObjectHierarchyUndo(root.gameObject,
            "Clear Blackspire Generated Trees");

        for (int i = root.childCount - 1; i >= 0; i--)
            Undo.DestroyObjectImmediate(root.GetChild(i).gameObject);

        EditorSceneManager.MarkSceneDirty(root.gameObject.scene);
    }
}
