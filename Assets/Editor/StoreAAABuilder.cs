#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Non-destructive first-pass architectural rebuild for SCN16_STORE.
/// Preserves existing Store UI/controllers. Hides only renderers under STORE_FACILITY,
/// then creates a new modular environment and attempts to place the existing Warden preview.
/// </summary>
public static class StoreAAABuilder
{
    private const string ScenePath = "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity";
    private const string OutputRootName = "STORE_AAA_REBUILD";
    private const string WardenPrefabPath = "Assets/Resources/StorePreviews/WardenPreview.prefab";

    [MenuItem("Obsidian Protocol/Store/BUILD AAA STORE ENVIRONMENT (SAFE)")]
    public static void Build()
    {
        if (EditorApplication.isPlaying) { EditorUtility.DisplayDialog("Store rebuild", "Stop Play Mode before rebuilding.", "OK"); return; }
        var scene = SceneManager.GetActiveScene();
        if (scene.path != ScenePath)
        {
            if (!EditorUtility.DisplayDialog("Wrong scene", "Open SCN16_STORE.unity first. No changes were made.", "OK")) return;
            return;
        }
        if (GameObject.Find(OutputRootName) != null)
        {
            if (!EditorUtility.DisplayDialog("Already built", "STORE_AAA_REBUILD already exists. No duplicate build was created.", "OK")) return;
            return;
        }
        if (!EditorUtility.DisplayDialog("Build Store environment", "A backup will be saved. Existing Store UI/controllers remain untouched. Existing physical STORE_FACILITY renderers will be disabled (not deleted), and a new environment will be added. Continue?", "Build", "Cancel")) return;

        string backup = ScenePath.Replace(".unity", "_BEFORE_AAA_REBUILD_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".unity");
        if (!AssetDatabase.CopyAsset(ScenePath, backup))
        {
            EditorUtility.DisplayDialog("Backup failed", "Could not create scene backup. Build cancelled.", "OK"); return;
        }
        EditorSceneManager.MarkSceneDirty(scene);

        var oldFacility = GameObject.Find("STORE_FACILITY");
        if (oldFacility != null)
        {
            foreach (var r in oldFacility.GetComponentsInChildren<Renderer>(true)) r.enabled = false;
            foreach (var l in oldFacility.GetComponentsInChildren<Light>(true)) l.enabled = false;
            foreach (var c in oldFacility.GetComponentsInChildren<Collider>(true)) c.enabled = false;
        }

        var root = new GameObject(OutputRootName);
        Undo.RegisterCreatedObjectUndo(root, "Build AAA Store Environment");
        var mats = CreateMaterials();
        BuildFloor(root.transform, mats);
        BuildShell(root.transform, mats);
        BuildCeiling(root.transform, mats);
        BuildDisplayBays(root.transform, mats);
        BuildProcurementZone(root.transform, mats);
        BuildLighting(root.transform, mats);
        BuildSignage(root.transform, mats);
        PlaceWarden(root.transform, mats);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();
        Selection.activeGameObject = root;
        EditorUtility.DisplayDialog("Store environment built", "Created STORE_AAA_REBUILD and saved a timestamped backup. Existing Store UI/controllers were preserved. Inspect the scene and test the camera before deleting anything.", "OK");
    }

    [MenuItem("Obsidian Protocol/Store/REPAIR STORE COLLIDERS (CURRENT SCENE)")]
    public static void RepairStoreColliders()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("Store repair", "Stop Play Mode first.", "OK");
            return;
        }

        var scene = SceneManager.GetActiveScene();
        if (scene.path != ScenePath)
        {
            EditorUtility.DisplayDialog("Store repair", "Open SCN16_STORE.unity first. No changes made.", "OK");
            return;
        }

        var oldFacility = GameObject.Find("STORE_FACILITY");
        if (oldFacility != null)
        {
            foreach (var c in oldFacility.GetComponentsInChildren<Collider>(true))
                c.enabled = false;
        }

        var rebuilt = GameObject.Find(OutputRootName);
        if (rebuilt == null)
        {
            EditorUtility.DisplayDialog("Store repair", "STORE_AAA_REBUILD was not found. No rebuilt colliders were changed.", "OK");
            return;
        }

        // Ensure the ground immediately outside the Store entrance is solid.
        var approach = rebuilt.transform.Find("Exterior approach slab");
        if (approach == null)
        {
            var slab = GameObject.CreatePrimitive(PrimitiveType.Cube);
            slab.name = "Exterior approach slab";
            slab.transform.SetParent(rebuilt.transform, false);
            approach = slab.transform;
        }

        approach.localPosition = new Vector3(0f, -0.25f, -48f);
        approach.localRotation = Quaternion.identity;
        approach.localScale = new Vector3(38f, 0.5f, 60f);

        var approachCollider = approach.GetComponent<Collider>();
        if (approachCollider == null)
            approachCollider = approach.gameObject.AddComponent<BoxCollider>();
        approachCollider.enabled = true;

        var floor = rebuilt.transform.Find("01_FLOOR_AND_GUIDANCE/Continuous sealed composite floor");
        var floorRenderer = floor != null ? floor.GetComponent<Renderer>() : null;
        var approachRenderer = approach.GetComponent<Renderer>();
        if (floorRenderer != null && approachRenderer != null)
            approachRenderer.sharedMaterial = floorRenderer.sharedMaterial;

        int repaired = 0;
        foreach (var t in rebuilt.GetComponentsInChildren<Transform>(true))
        {
            bool solid =
                t.name == "Continuous sealed composite floor" ||
                t.name == "Rear armored wall" ||
                t.name == "Left armored wall" ||
                t.name == "Right armored wall" ||
                t.name == "Entrance left monolith" ||
                t.name == "Entrance right monolith" ||
                t.name == "AIRFRAME SYSTEMS raised plinth" ||
                t.name == "GROUND SYSTEMS raised plinth" ||
                t.name == "AIRFRAME SYSTEMS rear display screen" ||
                t.name == "GROUND SYSTEMS rear display screen" ||
                t.name == "Procurement desk base" ||
                t.name == "Procurement desk armored face" ||
                t.name == "Procurement counter surface" ||
                t.name == "Procurement terminal pedestal" ||
                t.name == "Low bay safety bollard" ||
                t.name == "Reinforced column" ||
                t.name == "Column foot" ||
                t.name == "Warden model inspection turntable";

            if (!solid) continue;

            var c = t.GetComponent<Collider>();
            if (c == null) c = t.gameObject.AddComponent<BoxCollider>();
            c.enabled = true;
            repaired++;
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        EditorUtility.DisplayDialog(
            "Store repair",
            "Collider repair complete. Created missing BoxColliders where needed and enabled colliders on " + repaired +
            " intended solid objects. Decorative floor lights, markings, seams, and trim were left non-colliding.",
            "OK");
    }    private class Mats { public Material floor, dark, armor, edge, glass, white, emissive, yellow, rubber; }
    [MenuItem("Obsidian Protocol/Store/FIX STORE WINDOWS AND WALL SCREENS")]
    public static void FixStoreWindowsAndScreens()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("Store repair", "Stop Play Mode first.", "OK");
            return;
        }

        var scene = SceneManager.GetActiveScene();
        if (scene.path != ScenePath)
        {
            EditorUtility.DisplayDialog("Store repair", "Open SCN16_STORE.unity first. No changes made.", "OK");
            return;
        }

        var rebuilt = GameObject.Find(OutputRootName);
        if (rebuilt == null)
        {
            EditorUtility.DisplayDialog("Store repair", "STORE_AAA_REBUILD was not found. No changes made.", "OK");
            return;
        }

        var existing = rebuilt.transform.Find("STORE_WINDOW_SCREEN_FIX");
        if (existing != null)
        {
            EditorUtility.DisplayDialog("Store repair", "Window/screen repair already exists. No duplicate was created.", "OK");
            return;
        }

        var shell = rebuilt.transform.Find("02_REINFORCED_ARCHITECTURE");
        var bays = rebuilt.transform.Find("04_HEAVY_UNIT_DISPLAY_BAYS");
        if (shell == null || bays == null)
        {
            EditorUtility.DisplayDialog("Store repair", "Required Store architecture/display objects are missing. No changes made.", "OK");
            return;
        }

        Material dark = FindStoreMaterial("Rear armored wall");
        Material armor = FindStoreMaterial("Left wall armor panel");
        Material glassSource = FindStoreMaterial("AIRFRAME SYSTEMS rear display screen");
        Material glow = FindStoreMaterial("AIRFRAME SYSTEMS screen header");

        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        if (shader == null) throw new System.Exception("No supported shader found.");

        if (dark == null) dark = new Material(shader) { name = "Store_Repair_Graphite", color = new Color(.055f,.075f,.09f) };

        if (armor == null) armor = new Material(shader) { name = "Store_Repair_Titanium", color = new Color(.28f,.34f,.37f) };
        if (glassSource == null) glassSource = new Material(shader) { name = "Store_Repair_Smartglass", color = new Color(.035f,.18f,.22f,.28f) };
        if (glow == null) glow = new Material(shader) { name = "Store_Repair_Cyan", color = new Color(.01f,.55f,.75f) };

        var fix = new GameObject("STORE_WINDOW_SCREEN_FIX");
        fix.transform.SetParent(rebuilt.transform, false);

        // Replace each solid side wall with collidable wall sections around three real window openings.
        string[] wallNames = { "Left armored wall", "Right armored wall" };
        string[] panelNames = { "Left wall armor panel", "Right wall armor panel" };

        foreach (string n in wallNames)
        {
            var wall = FindChildRecursive(shell, n);
            if (wall != null)
            {
                var renderer = wall.GetComponent<Renderer>();
                var collider = wall.GetComponent<Collider>();
                if (renderer != null) renderer.enabled = false;
                if (collider != null) collider.enabled = false;
            }
        }

        // Old decorative wall panels would cover the new glazing.
        foreach (Transform t in shell.GetComponentsInChildren<Transform>(true))
        {
            if (t.name != panelNames[0] && t.name != panelNames[1]) continue;
            var r = t.GetComponent<Renderer>();
            if (r != null) r.enabled = false;
        }

        float[] windowZ = { -8f, 4f, 15f };
        const float wallX = 19f;
        const float wallDepth = 1f;
        const float windowWidth = 5.2f;
        const float windowBottom = 4.0f;
        const float windowTop = 7.4f;
        const float windowHeight = windowTop - windowBottom;
        const float windowY = (windowTop + windowBottom) * 0.5f;

        for (int side = -1; side <= 1; side += 2)
        {
            float x = side * wallX;
            float paneX = x - side * 0.02f;
            float cursor = -17f;

            foreach (float z in windowZ)
            {
                float start = z - windowWidth * 0.5f;
                float end = z + windowWidth * 0.5f;

                if (start > cursor)
                    StoreSolid(fix.transform, "Window wall pier", new Vector3(x, 6f, (cursor + start) * 0.5f),
                        new Vector3(wallDepth, 12f, start - cursor), dark);

                StoreSolid(fix.transform, "Window lower armored wall",
                    new Vector3(x, windowBottom * 0.5f, z),
                    new Vector3(wallDepth, windowBottom, windowWidth), dark);

                StoreSolid(fix.transform, "Window upper armored wall",
                    new Vector3(x, (windowTop + 12f) * 0.5f, z),
                    new Vector3(wallDepth, 12f - windowTop, windowWidth), dark);

                // Glass is a thin, non-walkable visual pane; the frame and surrounding wall are solid.
                StoreVisual(fix.transform, "Reinforced smartglass window",
                    new Vector3(paneX, windowY, z),
                    new Vector3(0.10f, windowHeight - 0.18f, windowWidth - 0.18f),
                    MakeWindowMaterial(glassSource));

                float frameX = x - side * 0.56f;
                StoreSolid(fix.transform, "Window frame left",
                    new Vector3(frameX, windowY, z - windowWidth * 0.5f),
                    new Vector3(0.18f, windowHeight, 0.16f), armor);
                StoreSolid(fix.transform, "Window frame right",
                    new Vector3(frameX, windowY, z + windowWidth * 0.5f),
                    new Vector3(0.18f, windowHeight, 0.16f), armor);
                StoreSolid(fix.transform, "Window armored sill",
                    new Vector3(frameX, windowBottom + 0.04f, z),
                    new Vector3(0.18f, 0.16f, windowWidth), armor);
                StoreSolid(fix.transform, "Window armored lintel",
                    new Vector3(frameX, windowTop - 0.04f, z),
                    new Vector3(0.18f, 0.16f, windowWidth), armor);

                // Thin cyan perimeter accents, deliberately non-colliding.
                StoreVisual(fix.transform, "Window cyan sill",
                    new Vector3(frameX - side * 0.10f, windowBottom + 0.14f, z),
                    new Vector3(0.035f, 0.035f, windowWidth - 0.3f), glow);

                cursor = end;
            }

            if (cursor < 21f)
                StoreSolid(fix.transform, "Window rear wall pier",
                    new Vector3(x, 6f, (cursor + 21f) * 0.5f),
                    new Vector3(wallDepth, 12f, 21f - cursor), dark);
        }

        // Give the two large rear display screens a proper bezel and restrained interface graphics.
        string[] labels = { "AIRFRAME SYSTEMS", "GROUND SYSTEMS" };
        float[] xs = { -12f, 12f };

        for (int i = 0; i < labels.Length; i++)
        {
            float x = xs[i];
            float frontZ = 12.76f;

            StoreVisual(fix.transform, labels[i] + " screen left bezel",
                new Vector3(x - 3.72f, 4.2f, frontZ),
                new Vector3(0.16f, 4.55f, 0.14f), armor);
            StoreVisual(fix.transform, labels[i] + " screen right bezel",
                new Vector3(x + 3.72f, 4.2f, frontZ),
                new Vector3(0.16f, 4.55f, 0.14f), armor);
            StoreVisual(fix.transform, labels[i] + " screen bottom bezel",
                new Vector3(x, 1.94f, frontZ),
                new Vector3(7.55f, 0.16f, 0.14f), armor);

            // Screen graphics sit just in front of the existing glass surface.
            StoreVisual(fix.transform, labels[i] + " interface title strip",
                new Vector3(x, 5.95f, 12.755f),
                new Vector3(6.7f, 0.055f, 0.025f), glow);

            for (int row = 0; row < 4; row++)
            {
                float y = 5.35f - row * 0.72f;
                StoreVisual(fix.transform, labels[i] + " interface data line",
                    new Vector3(x - 0.35f, y, 12.755f),
                    new Vector3(4.7f, 0.035f, 0.025f), glow);

                StoreVisual(fix.transform, labels[i] + " interface status block",
                    new Vector3(x + 2.65f, y, 12.755f),
                    new Vector3(0.55f, 0.24f, 0.025f), armor);
            }

            StoreVisual(fix.transform, labels[i] + " interface side column",
                new Vector3(x - 2.85f, 3.95f, 12.755f),
                new Vector3(0.055f, 1.15f, 0.025f), glow);
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();
        Selection.activeGameObject = fix;

        EditorUtility.DisplayDialog("Store repair",
            "Side-wall window openings, reinforced frames, smartglass panes, and wall-screen bezels/interface graphics were added. Existing Store exit and controllers were left untouched. Inspect Scene view and Game view.",
            "OK");
    }

    [MenuItem("Obsidian Protocol/Store/CLEAN STORE DISPLAY CLUTTER AND RELOCATE SCREENS")]
    public static void CleanStoreDisplayClutterAndRelocateScreens()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("Store cleanup", "Stop Play Mode first.", "OK");
            return;
        }

        var rebuilt = GameObject.Find(OutputRootName);
        if (rebuilt == null)
        {
            EditorUtility.DisplayDialog("Store cleanup", "STORE_AAA_REBUILD was not found. No changes made.", "OK");
            return;
        }

        var bays = rebuilt.transform.Find("04_HEAVY_UNIT_DISPLAY_BAYS");
        if (bays == null)
        {
            EditorUtility.DisplayDialog("Store cleanup", "Display bay parent was not found. No changes made.", "OK");
            return;
        }

        int removed = 0;

        // Remove obsolete display platforms, trim, lights, and side pillars.
        for (int i = bays.childCount - 1; i >= 0; i--)
        {
            var child = bays.GetChild(i);
            string n = child.name;

            bool unwanted =
                n.Contains("raised plinth") ||
                n.Contains("armored perimeter") ||
                n.Contains("platform light") ||
                n.Contains("side information pillar") ||
                n.Contains("pillar status") ||
                n == "Deck grip inset" ||
                n == "Low bay safety bollard";

            if (unwanted)
            {
                Undo.DestroyObjectImmediate(child.gameObject);
                removed++;
            }
        }

        // Move the existing screens and their cyan headers onto the inside face
        // of the rear wall. Keep their original materials and X/Y positions.
        int moved = 0;
        foreach (Transform child in bays.GetComponentsInChildren<Transform>(true))
        {
            if (child == null) continue;

            if (child.name.EndsWith("rear display screen", System.StringComparison.Ordinal))
            {
                Undo.RecordObject(child, "Relocate Store display screen");
                var p = child.localPosition;
                p.z = 20.35f;
                child.localPosition = p;
                moved++;
            }
            else if (child.name.EndsWith("screen header", System.StringComparison.Ordinal))
            {
                Undo.RecordObject(child, "Relocate Store screen neon");
                var p = child.localPosition;
                p.z = 20.22f;
                child.localPosition = p;
                moved++;
            }
        }

        // Remove the extra screen bezels/interface graphics added by the window repair.
        var fix = rebuilt.transform.Find("STORE_WINDOW_SCREEN_FIX");
        if (fix != null)
        {
            for (int i = fix.childCount - 1; i >= 0; i--)
            {
                var child = fix.GetChild(i);
                string n = child.name;

                if (n.Contains("screen ") ||
                    n.Contains("interface") ||
                    n == "Window cyan sill")
                {
                    Undo.DestroyObjectImmediate(child.gameObject);
                    removed++;
                }
            }
        }

        // Remove the obsolete floating AIR/GROUND labels, not the main Store signage.
        var signage = rebuilt.transform.Find("07_BRANDED_SIGNAGE");
        if (signage != null)
        {
            for (int i = signage.childCount - 1; i >= 0; i--)
            {
                var child = signage.GetChild(i);
                if (child.name == "AIR SYSTEMS" || child.name == "GROUND SYSTEMS")
                {
                    Undo.DestroyObjectImmediate(child.gameObject);
                    removed++;
                }
            }
        }

        EditorSceneManager.MarkSceneDirty(rebuilt.scene);
        EditorSceneManager.SaveScene(rebuilt.scene);
        AssetDatabase.SaveAssets();

        Selection.activeGameObject = rebuilt;
        EditorUtility.DisplayDialog(
            "Store cleanup complete",
            "Removed obsolete display clutter: " + removed +
            "\nRelocated existing screens/neon: " + moved +
            "\nWindows, entrance, and exterior approach were left in place.",
            "OK");
    }
    [MenuItem("Obsidian Protocol/Store/REPAIR STORE REAR WALL SCREENS AND NEON")]
    public static void RepairStoreRearWallScreensAndNeon()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("Store screens", "Stop Play Mode first.", "OK");
            return;
        }

        var root = GameObject.Find("STORE_AAA_REBUILD");
        if (root == null)
        {
            EditorUtility.DisplayDialog("Store screens", "STORE_AAA_REBUILD was not found. No changes made.", "OK");
            return;
        }

        var all = root.GetComponentsInChildren<Transform>(true);
        Transform airScreen = null, groundScreen = null;
        Transform airHeader = null, groundHeader = null;

        foreach (var t in all)
        {
            if (t == null) continue;
            if (t.name == "AIRFRAME SYSTEMS rear display screen") airScreen = t;
            else if (t.name == "GROUND SYSTEMS rear display screen") groundScreen = t;
            else if (t.name == "AIRFRAME SYSTEMS screen header") airHeader = t;
            else if (t.name == "GROUND SYSTEMS screen header") groundHeader = t;
        }

        if (airScreen == null || groundScreen == null ||
            airHeader == null || groundHeader == null)
        {
            EditorUtility.DisplayDialog(
                "Store screens",
                "One or more original screen/header objects are missing. No screen changes made.",
                "OK");
            return;
        }

        Material neonMaterial = null;
        var headerRenderer = airHeader.GetComponent<Renderer>();
        if (headerRenderer != null) neonMaterial = headerRenderer.sharedMaterial;
        if (neonMaterial == null)
        {
            headerRenderer = groundHeader.GetComponent<Renderer>();
            if (headerRenderer != null) neonMaterial = headerRenderer.sharedMaterial;
        }

        if (neonMaterial == null)
        {
            EditorUtility.DisplayDialog(
                "Store screens",
                "Could not find the original neon material. No screen changes made.",
                "OK");
            return;
        }

        // Remove only this repair's previous neon geometry, so the command is repeatable.
        var previous = root.transform.Find("STORE_REAR_WALL_SCREEN_NEON");
        if (previous != null) Undo.DestroyObjectImmediate(previous.gameObject);

        // Parent directly to the Store root so bay-parent transforms cannot offset them.
        Transform[] screens = { airScreen, groundScreen };
        Transform[] headers = { airHeader, groundHeader };
        float[] xs = { -9f, 9f };

        for (int i = 0; i < 2; i++)
        {
            Undo.RecordObject(screens[i], "Place Store rear screen");
            screens[i].SetParent(root.transform, false);
            screens[i].localPosition = new Vector3(xs[i], 4.2f, 20.32f);
            screens[i].localRotation = Quaternion.identity;
            screens[i].localScale = new Vector3(7.2f, 4.3f, 0.22f);

            Undo.RecordObject(headers[i], "Place Store rear screen neon");
            headers[i].SetParent(root.transform, false);
            headers[i].localPosition = new Vector3(xs[i], 6.5f, 20.17f);
            headers[i].localRotation = Quaternion.identity;
            headers[i].localScale = new Vector3(7.2f, 0.18f, 0.12f);
        }

        var neonRoot = new GameObject("STORE_REAR_WALL_SCREEN_NEON");
        Undo.RegisterCreatedObjectUndo(neonRoot, "Create Store screen neon");
        neonRoot.transform.SetParent(root.transform, false);

        for (int i = 0; i < 2; i++)
        {
            float x = xs[i];

            // Slim vertical cyan strips, aligned to each screen's sides.
            CreateNeonBar(neonRoot.transform, "Screen neon left " + i,
                new Vector3(x - 3.72f, 4.2f, 20.18f),
                new Vector3(0.055f, 4.45f, 0.055f), neonMaterial);

            CreateNeonBar(neonRoot.transform, "Screen neon right " + i,
                new Vector3(x + 3.72f, 4.2f, 20.18f),
                new Vector3(0.055f, 4.45f, 0.055f), neonMaterial);

            CreateNeonBar(neonRoot.transform, "Screen neon lower " + i,
                new Vector3(x, 1.96f, 20.18f),
                new Vector3(7.45f, 0.055f, 0.055f), neonMaterial);
        }

        // Remove the obsolete floating system labels if they remain.
        var signage = root.transform.Find("07_BRANDED_SIGNAGE");
        if (signage != null)
        {
            for (int i = signage.childCount - 1; i >= 0; i--)
            {
                var t = signage.GetChild(i);
                if (t.name == "AIR SYSTEMS" || t.name == "GROUND SYSTEMS")
                    Undo.DestroyObjectImmediate(t.gameObject);
            }
        }

        EditorSceneManager.MarkSceneDirty(root.scene);
        EditorSceneManager.SaveScene(root.scene);
        AssetDatabase.SaveAssets();
        Selection.activeGameObject = neonRoot.gameObject;

        EditorUtility.DisplayDialog(
            "Store screens",
            "Moved both screens and header neons to the rear wall and added matching neon side/lower accents.",
            "OK");
    }

    private static void CreateNeonBar(
        Transform parent, string objectName, Vector3 position, Vector3 scale, Material material)
    {
        var bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bar.name = objectName;
        bar.transform.SetParent(parent, false);
        bar.transform.localPosition = position;
        bar.transform.localRotation = Quaternion.identity;
        bar.transform.localScale = scale;

        var collider = bar.GetComponent<Collider>();
        if (collider != null) Object.DestroyImmediate(collider);

        var renderer = bar.GetComponent<Renderer>();
        if (renderer != null) renderer.sharedMaterial = material;
    }
    [MenuItem("Obsidian Protocol/Store/REBUILD STORE SIDE WINDOWS AND SIX SCREENS")]
    public static void RebuildStoreSideWindowsAndSixScreens()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("Store layout", "Stop Play Mode first.", "OK");
            return;
        }

        var root = GameObject.Find("STORE_AAA_REBUILD");
        if (root == null)
        {
            EditorUtility.DisplayDialog("Store layout", "STORE_AAA_REBUILD not found. No changes made.", "OK");
            return;
        }

        var fix = root.transform.Find("STORE_WINDOW_SCREEN_FIX");
        if (fix == null)
        {
            EditorUtility.DisplayDialog("Store layout", "STORE_WINDOW_SCREEN_FIX not found. No changes made.", "OK");
            return;
        }

        Material dark = FindStoreMaterial("Rear armored wall");
        Material armor = FindStoreMaterial("Left wall armor panel");
        Material glass = FindStoreMaterial("AIRFRAME SYSTEMS rear display screen");
        Material glow = FindStoreMaterial("AIRFRAME SYSTEMS screen header");

        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        if (shader == null)
        {
            EditorUtility.DisplayDialog("Store layout", "No supported shader found. No changes made.", "OK");
            return;
        }

        if (dark == null) dark = new Material(shader) { name = "Store_WindowWall_Graphite", color = new Color(.055f, .075f, .09f) };
        if (armor == null) armor = new Material(shader) { name = "Store_WindowWall_Frame", color = new Color(.28f, .34f, .37f) };
        if (glass == null) glass = new Material(shader) { name = "Store_Screen_Glass", color = new Color(.035f, .18f, .22f, 1f) };
        if (glow == null) glow = new Material(shader) { name = "Store_Screen_Neon", color = new Color(.01f, .65f, .9f) };

        // Remove only the previous window-repair geometry; keep its parent and the rest of the Store.
        for (int i = fix.childCount - 1; i >= 0; i--)
            Undo.DestroyObjectImmediate(fix.GetChild(i).gameObject);

        // Remove the two original display screens/headers wherever the previous repair left them.
        var all = root.GetComponentsInChildren<Transform>(true);
        var oldObjects = new System.Collections.Generic.List<GameObject>();
        foreach (var t in all)
        {
            if (t == null || t == root.transform || t == fix) continue;
            if (t.name == "AIRFRAME SYSTEMS rear display screen" ||
                t.name == "GROUND SYSTEMS rear display screen" ||
                t.name == "AIRFRAME SYSTEMS screen header" ||
                t.name == "GROUND SYSTEMS screen header")
                oldObjects.Add(t.gameObject);
        }
        foreach (var go in oldObjects)
            if (go != null) Undo.DestroyObjectImmediate(go);

        var oldRearNeon = root.transform.Find("STORE_REAR_WALL_SCREEN_NEON");
        if (oldRearNeon != null) Undo.DestroyObjectImmediate(oldRearNeon.gameObject);
        var oldSideNeon = root.transform.Find("STORE_SIDE_WALL_SCREEN_NEON");
        if (oldSideNeon != null) Undo.DestroyObjectImmediate(oldSideNeon.gameObject);

        // Disable original solid side walls and armor panels; the replacement wall
        // sections below preserve the wall while leaving real window openings.
        var shell = root.transform.Find("02_REINFORCED_ARCHITECTURE");
        if (shell == null)
        {
            EditorUtility.DisplayDialog("Store layout", "Architecture group not found. No changes made.", "OK");
            return;
        }

        string[] wallNames = { "Left armored wall", "Right armored wall" };
        string[] panelNames = { "Left wall armor panel", "Right wall armor panel" };
        foreach (string n in wallNames)
        {
            var wall = FindChildRecursive(shell, n);
            if (wall != null)
            {
                var r = wall.GetComponent<Renderer>();
                var c = wall.GetComponent<Collider>();
                if (r != null) r.enabled = false;
                if (c != null) c.enabled = false;
            }
        }
        foreach (string n in panelNames)
        {
            var panel = FindChildRecursive(shell, n);
            if (panel != null)
            {
                var r = panel.GetComponent<Renderer>();
                if (r != null) r.enabled = false;
            }
        }

        // Four narrow, tall windows on each side wall.
        float[] windowZ = { -12f, -3f, 6f, 15f };
        const float wallX = 19f;
        const float wallDepth = 1f;
        const float windowWidth = 3.2f;
        const float windowBottom = 3.4f;
        const float windowTop = 8.4f;
        const float windowHeight = windowTop - windowBottom;
        const float windowY = (windowTop + windowBottom) * 0.5f;

        for (int side = -1; side <= 1; side += 2)
        {
            float x = side * wallX;
            float cursor = -17f;

            foreach (float z in windowZ)
            {
                float start = z - windowWidth * 0.5f;
                float end = z + windowWidth * 0.5f;

                if (start > cursor)
                    StoreSolid(fix, "Window wall pier", new Vector3(x, 6f, (cursor + start) * 0.5f),
                        new Vector3(wallDepth, 12f, start - cursor), dark);

                StoreSolid(fix, "Window lower wall", new Vector3(x, windowBottom * 0.5f, z),
                    new Vector3(wallDepth, windowBottom, windowWidth), dark);

                StoreSolid(fix, "Window upper wall", new Vector3(x, (windowTop + 12f) * 0.5f, z),
                    new Vector3(wallDepth, 12f - windowTop, windowWidth), dark);

                float paneX = x - side * 0.56f;
                StoreVisual(fix, "Tall narrow smartglass window", new Vector3(paneX, windowY, z),
                    new Vector3(0.10f, windowHeight - 0.16f, windowWidth - 0.16f),
                    MakeWindowMaterial(glass));

                float frameX = x - side * 0.56f;
                StoreSolid(fix, "Window frame left", new Vector3(frameX, windowY, z - windowWidth * 0.5f),
                    new Vector3(0.14f, windowHeight, 0.12f), armor);
                StoreSolid(fix, "Window frame right", new Vector3(frameX, windowY, z + windowWidth * 0.5f),
                    new Vector3(0.14f, windowHeight, 0.12f), armor);
                StoreSolid(fix, "Window armored sill", new Vector3(frameX, windowBottom, z),
                    new Vector3(0.14f, 0.12f, windowWidth), armor);
                StoreSolid(fix, "Window armored lintel", new Vector3(frameX, windowTop, z),
                    new Vector3(0.14f, 0.12f, windowWidth), armor);

                cursor = end;
            }

            if (cursor < 21f)
                StoreSolid(fix, "Window end wall pier", new Vector3(x, 6f, (cursor + 21f) * 0.5f),
                    new Vector3(wallDepth, 12f, 21f - cursor), dark);
        }

        // Three screens per side, centered in the three gaps between the four windows.
        var neonRoot = new GameObject("STORE_SIDE_WALL_SCREEN_NEON");
        Undo.RegisterCreatedObjectUndo(neonRoot, "Create Store screen neon");
        neonRoot.transform.SetParent(root.transform, false);

        float[] screenZ = { -7.5f, 1.5f, 10.5f };
        int screenCount = 0;

        for (int side = -1; side <= 1; side += 2)
        {
            foreach (float z in screenZ)
            {
                float screenX = side * 18.32f;
                float neonX = side * 18.22f;
                string id = side < 0 ? "LEFT" : "RIGHT";
                id += "_SCREEN_" + (++screenCount);

                StoreVisual(root.transform, id + "_DISPLAY",
                    new Vector3(screenX, 5.7f, z),
                    new Vector3(0.12f, 2.6f, 4.65f), glass);

                // Neon frame sits around the display, inside the window gap.
                CreateNeonBar(neonRoot.transform, id + "_NEON_TOP",
                    new Vector3(neonX, 7.05f, z),
                    new Vector3(0.055f, 0.055f, 4.75f), glow);
                CreateNeonBar(neonRoot.transform, id + "_NEON_BOTTOM",
                    new Vector3(neonX, 4.35f, z),
                    new Vector3(0.055f, 0.055f, 4.75f), glow);
                CreateNeonBar(neonRoot.transform, id + "_NEON_A",
                    new Vector3(neonX, 5.7f, z - 2.38f),
                    new Vector3(0.055f, 2.75f, 0.055f), glow);
                CreateNeonBar(neonRoot.transform, id + "_NEON_B",
                    new Vector3(neonX, 5.7f, z + 2.38f),
                    new Vector3(0.055f, 2.75f, 0.055f), glow);
            }
        }

        // Remove obsolete floating AIR/GROUND labels.
        var signage = root.transform.Find("07_BRANDED_SIGNAGE");
        if (signage != null)
        {
            for (int i = signage.childCount - 1; i >= 0; i--)
            {
                var t = signage.GetChild(i);
                if (t.name == "AIR SYSTEMS" || t.name == "GROUND SYSTEMS")
                    Undo.DestroyObjectImmediate(t.gameObject);
            }
        }

        EditorSceneManager.MarkSceneDirty(root.scene);
        EditorSceneManager.SaveScene(root.scene);
        AssetDatabase.SaveAssets();
        Selection.activeGameObject = neonRoot;

        EditorUtility.DisplayDialog("Store layout complete",
            "Created 4 windows on each side wall and 3 screens with neon between them on each side. Entrance and exterior were not rebuilt.",
            "OK");
    }
    private static Material FindStoreMaterial(string objectName)
    {
        var all = Resources.FindObjectsOfTypeAll<Renderer>();
        foreach (var r in all)
        {
            if (r == null || r.gameObject.name != objectName || r.sharedMaterial == null) continue;
            if (r.gameObject.scene.IsValid()) return r.sharedMaterial;
        }
        return null;
    }

    private static Transform FindChildRecursive(Transform parent, string objectName)
    {
        foreach (Transform t in parent.GetComponentsInChildren<Transform>(true))
            if (t.name == objectName) return t;
        return null;
    }

    private static GameObject StoreSolid(Transform parent, string name, Vector3 pos, Vector3 scale, Material material)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        var r = go.GetComponent<Renderer>();
        if (r != null) r.sharedMaterial = material;
        var c = go.GetComponent<Collider>();
        if (c == null) c = go.AddComponent<BoxCollider>();
        c.enabled = true;
        return go;
    }

    private static GameObject StoreVisual(Transform parent, string name, Vector3 pos, Vector3 scale, Material material)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        var r = go.GetComponent<Renderer>();
        if (r != null) r.sharedMaterial = material;
        var c = go.GetComponent<Collider>();
        if (c != null) c.enabled = false;
        return go;
    }

    private static Material MakeWindowMaterial(Material source)
    {
        var m = new Material(source);
        m.name = "Store_Reinforced_Smartglass_Window";
        Color c = m.color;
        c.a = 0.28f;
        m.color = c;

        if (m.HasProperty("_Surface"))
        {
            m.SetFloat("_Surface", 1f);
            if (m.HasProperty("_Blend")) m.SetFloat("_Blend", 0f);
            if (m.HasProperty("_AlphaClip")) m.SetFloat("_AlphaClip", 0f);
            if (m.HasProperty("_SrcBlend")) m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            if (m.HasProperty("_DstBlend")) m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            if (m.HasProperty("_ZWrite")) m.SetInt("_ZWrite", 0);
            m.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            m.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }
        else if (m.HasProperty("_Mode"))
        {
            m.SetFloat("_Mode", 3f);
            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            m.SetInt("_ZWrite", 0);
            m.DisableKeyword("_ALPHATEST_ON");
            m.EnableKeyword("_ALPHABLEND_ON");
            m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            m.renderQueue = 3000;
        }

        return m;
    }
    private static Mats CreateMaterials()
    {
        return new Mats {
            floor = Mat("Store_Floor_Charcoal", new Color(.105f,.13f,.15f), .78f, .24f),
            dark = Mat("Store_Architectural_Graphite", new Color(.055f,.075f,.09f), .7f, .32f),
            armor = Mat("Store_Armor_Titanium", new Color(.28f,.34f,.37f), .82f, .28f),
            edge = Mat("Store_Edge_Ceramic", new Color(.48f,.54f,.56f), .65f, .26f),
            glass = Mat("Store_SmartGlass", new Color(.035f,.18f,.22f), .75f, .18f),
            white = Mat("Store_Sign_White", new Color(.72f,.84f,.88f), .35f, .35f),
            emissive = Mat("Store_Cyan_Emission", new Color(.01f,.55f,.75f), .25f, .3f, true),
            yellow = Mat("Store_Safety_Amber", new Color(1f,.48f,.06f), .35f, .35f, true),
            rubber = Mat("Store_Platform_Rubber", new Color(.035f,.045f,.05f), .25f, .75f)
        };
    }
    private static Material Mat(string name, Color c, float metallic, float smooth, bool emission=false)
    {
        var shader = Shader.Find("Universal Render Pipeline/Lit"); if (shader == null) shader = Shader.Find("Standard");
        var m = new Material(shader) { name=name };
        if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", c); else if (m.HasProperty("_Color")) m.SetColor("_Color", c);
        if (m.HasProperty("_Metallic")) m.SetFloat("_Metallic", metallic);
        if (m.HasProperty("_Smoothness")) m.SetFloat("_Smoothness", smooth);
        if (emission && m.HasProperty("_EmissionColor")) { m.EnableKeyword("_EMISSION"); m.SetColor("_EmissionColor", c * 2.5f); }
        return m;
    }
    private static GameObject Box(Transform parent, string name, Vector3 pos, Vector3 scale, Material mat)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube); go.name=name; go.transform.SetParent(parent, false); go.transform.localPosition=pos; go.transform.localScale=scale;
        var r=go.GetComponent<Renderer>(); if (r) r.sharedMaterial=mat;
        var c=go.GetComponent<Collider>(); if (c) c.enabled = false;
        return go;
    }
    private static GameObject SolidBox(Transform parent, string name, Vector3 pos, Vector3 scale, Material mat)
    {
        var go = Box(parent, name, pos, scale, mat);
        var c = go.GetComponent<Collider>();
        if (c != null) c.enabled = true;
        return go;
    }
    private static void BuildFloor(Transform root, Mats m)
    {
        var g=new GameObject("01_FLOOR_AND_GUIDANCE"); g.transform.SetParent(root,false);
        SolidBox(g.transform,"Continuous sealed composite floor",new Vector3(0,-.25f,2),new Vector3(38,.5f,42),m.floor);
        Box(g.transform,"Central aisle inset",new Vector3(0,.012f,-1),new Vector3(5,.025f,30),m.dark);
        for(int z=-14;z<=14;z+=4) { Box(g.transform,"Aisle cyan guide",new Vector3(-2.42f,.035f,z),new Vector3(.055f,.025f,2.4f),m.emissive); Box(g.transform,"Aisle cyan guide",new Vector3(2.42f,.035f,z),new Vector3(.055f,.025f,2.4f),m.emissive); }
        for(int x=-16;x<=16;x+=4) Box(g.transform,"Floor expansion joint",new Vector3(x,.013f,2),new Vector3(.035f,.012f,39),m.rubber);
        for(int z=-14;z<=16;z+=2) { Box(g.transform,"Bay safety dash",new Vector3(-10,.03f,z),new Vector3(1.2f,.025f,.055f),m.yellow); Box(g.transform,"Bay safety dash",new Vector3(10,.03f,z),new Vector3(1.2f,.025f,.055f),m.yellow); }
    }
    private static void BuildShell(Transform root, Mats m)
    {
        var g=new GameObject("02_REINFORCED_ARCHITECTURE"); g.transform.SetParent(root,false);
        SolidBox(g.transform,"Rear armored wall",new Vector3(0,6,21),new Vector3(38,12,1),m.dark);
        SolidBox(g.transform,"Left armored wall",new Vector3(-19,6,2),new Vector3(1,12,38),m.dark);
        SolidBox(g.transform,"Right armored wall",new Vector3(19,6,2),new Vector3(1,12,38),m.dark);
        Box(g.transform,"Rear upper fascia",new Vector3(0,10.9f,20.35f),new Vector3(37,.6f,.25f),m.armor);
        Box(g.transform,"Left upper fascia",new Vector3(-18.35f,10.9f,2),new Vector3(.25f,.6f,37),m.armor);
        Box(g.transform,"Right upper fascia",new Vector3(18.35f,10.9f,2),new Vector3(.25f,.6f,37),m.armor);
        for(int x=-16;x<=16;x+=4) { Box(g.transform,"Rear wall armor panel",new Vector3(x,5.7f,20.42f),new Vector3(3.65f,9.8f,.12f),m.armor); Box(g.transform,"Rear panel seam",new Vector3(x+1.85f,5.7f,20.32f),new Vector3(.045f,9.8f,.035f),m.emissive); }
        for(int z=-14;z<=18;z+=4) { Box(g.transform,"Left wall armor panel",new Vector3(-18.42f,5.7f,z),new Vector3(.12f,9.8f,3.65f),m.armor); Box(g.transform,"Right wall armor panel",new Vector3(18.42f,5.7f,z),new Vector3(.12f,9.8f,3.65f),m.armor); }
        // Layered entry portal at front; deliberately leaves a wide central opening.
        SolidBox(g.transform,"Entrance left monolith",new Vector3(-8,5, -16.8f),new Vector3(1.2f,10,1.2f),m.armor);
        SolidBox(g.transform,"Entrance right monolith",new Vector3(8,5,-16.8f),new Vector3(1.2f,10,1.2f),m.armor);
        Box(g.transform,"Entrance header",new Vector3(0,10,-16.8f),new Vector3(17.2f,1.2f,1.4f),m.dark);
        Box(g.transform,"Entrance cyan reveal",new Vector3(0,9.32f,-16.05f),new Vector3(15.6f,.07f,.06f),m.emissive);
        for(int x=-16;x<=16;x+=8) { SolidBox(g.transform,"Reinforced column",new Vector3(x,5,18.8f),new Vector3(.55f,10,.7f),m.edge); SolidBox(g.transform,"Column foot",new Vector3(x,.35f,18.8f),new Vector3(1.1f,.7f,1.1f),m.dark); }
    }
    private static void BuildCeiling(Transform root, Mats m)
    {
        var g=new GameObject("03_OVERHEAD_TRUSS_AND_SERVICES"); g.transform.SetParent(root,false);
        Box(g.transform,"Ceiling service deck",new Vector3(0,12.2f,2),new Vector3(38,.45f,39),m.dark);
        for(int z=-14;z<=18;z+=4) { Box(g.transform,"Transverse truss",new Vector3(0,11.75f,z),new Vector3(37,.34f,.48f),m.armor); Box(g.transform,"Truss lower light channel",new Vector3(0,11.53f,z-.25f),new Vector3(30,.07f,.06f),m.emissive); }
        for(int x=-16;x<=16;x+=4) Box(g.transform,"Longitudinal ceiling rail",new Vector3(x,11.85f,2),new Vector3(.22f,.35f,37),m.edge);
        for(int x=-12;x<=12;x+=8) for(int z=-10;z<=14;z+=8) { Box(g.transform,"Ceiling light housing",new Vector3(x,11.48f,z),new Vector3(3.2f,.16f,1.2f),m.white); }
    }
    private static void BuildDisplayBays(Transform root, Mats m)
    {
        var g=new GameObject("04_HEAVY_UNIT_DISPLAY_BAYS"); g.transform.SetParent(root,false);
        float[] xs={-12f,12f}; string[] labels={"AIRFRAME SYSTEMS","GROUND SYSTEMS"};
        for(int i=0;i<2;i++) {
            float x=xs[i];
            SolidBox(g.transform,labels[i]+" raised plinth",new Vector3(x,.22f,5),new Vector3(10, .42f,15),m.dark);
            Box(g.transform,labels[i]+" armored perimeter",new Vector3(x,.47f,-2.35f),new Vector3(10,.08f,.12f),m.edge);
            Box(g.transform,labels[i]+" armored perimeter",new Vector3(x,.47f,12.35f),new Vector3(10,.08f,.12f),m.edge);
            Box(g.transform,labels[i]+" platform light front",new Vector3(x,.48f,-2.1f),new Vector3(8,.04f,.06f),m.emissive);
            Box(g.transform,labels[i]+" platform light rear",new Vector3(x,.48f,12.1f),new Vector3(8,.04f,.06f),m.emissive);
            for(int dx=-4;dx<=4;dx+=2) { Box(g.transform,"Deck grip inset",new Vector3(x+dx,.445f,5),new Vector3(.035f,.018f,13.5f),m.edge); }
            Box(g.transform,labels[i]+" rear display screen",new Vector3(x,4.2f,12.9f),new Vector3(7.2f,4.3f,.22f),m.glass);
            Box(g.transform,labels[i]+" screen header",new Vector3(x,6.5f,12.72f),new Vector3(7.2f,.18f,.12f),m.emissive);
            SolidBox(g.transform,labels[i]+" side information pillar",new Vector3(x+4.15f,1.8f,4.8f),new Vector3(.45f,3.3f,.55f),m.armor);
            Box(g.transform,labels[i]+" pillar status",new Vector3(x+3.9f,2.65f,4.49f),new Vector3(.14f,.8f,.05f),m.emissive);
        }
        // Keep the center aisle open; low guide bollards only at bay corners.
        foreach(float x in new float[]{-6.4f,6.4f}) foreach(float z in new float[]{-2f,12f}) { var b=Box(g.transform,"Low bay safety bollard",new Vector3(x,.5f,z),new Vector3(.18f,1f,.18f),m.yellow); }
    }
    private static void BuildProcurementZone(Transform root, Mats m)
    {
        var g=new GameObject("05_PROCUREMENT_AND_CHECKOUT"); g.transform.SetParent(root,false);
        SolidBox(g.transform,"Procurement desk base",new Vector3(0,1.05f,16.1f),new Vector3(12,2.1f,1.7f),m.dark);
        SolidBox(g.transform,"Procurement desk armored face",new Vector3(0,1.05f,15.2f),new Vector3(11.6f,1.45f,.08f),m.armor);
        SolidBox(g.transform,"Procurement counter surface",new Vector3(0,2.16f,16.05f),new Vector3(12.4f,.18f,2),m.edge);
        Box(g.transform,"Counter cyan edge",new Vector3(0,2.05f,15.02f),new Vector3(11.8f,.045f,.05f),m.emissive);
        for(int x=-4;x<=4;x+=4) {
            SolidBox(g.transform,"Procurement terminal pedestal",new Vector3(x,2.9f,16.05f),new Vector3(.7f,1.35f,.7f),m.dark);
            var screen=Box(g.transform,"Procurement terminal glass",new Vector3(x,3.85f,15.83f),new Vector3(1.7f,1.45f,.12f),m.glass);
            screen.transform.localRotation=Quaternion.Euler(-12,0,0);
            Box(g.transform,"Terminal UI light",new Vector3(x,4.57f,15.74f),new Vector3(1.2f,.045f,.04f),m.emissive);
        }
        Box(g.transform,"Checkout back wall feature",new Vector3(0,5.4f,20.35f),new Vector3(14,5.2f,.16f),m.dark);
        Box(g.transform,"Checkout feature cyan frame top",new Vector3(0,8.05f,20.2f),new Vector3(14,.07f,.08f),m.emissive);
    }
    private static void BuildLighting(Transform root, Mats m)
    {
        var g=new GameObject("06_LIGHTING_RIG"); g.transform.SetParent(root,false);
        for(int x=-12;x<=12;x+=8) for(int z=-8;z<=12;z+=10) {
            var go=new GameObject("Neutral industrial area light"); go.transform.SetParent(g.transform,false); go.transform.localPosition=new Vector3(x,10.8f,z);
            var l=go.AddComponent<Light>(); l.type=LightType.Rectangle; l.intensity=8; l.range=16; l.color=new Color(.68f,.82f,1f); l.enabled=true;
            // Area lights are not supported by all Unity render paths; add a point-light fallback.
            l.type=LightType.Point; l.intensity=5.5f; l.range=14; l.shadows=LightShadows.Soft;
        }
        AddSpot(g.transform,"Entrance key light",new Vector3(0,9,-12),new Vector3(0,1,4),new Color(.28f,.72f,1f),850f,35f);
        AddSpot(g.transform,"Air bay key light",new Vector3(-12,9,5),new Vector3(-12,0,5),new Color(.65f,.82f,1f),1100f,45f);
        AddSpot(g.transform,"Ground bay key light",new Vector3(12,9,5),new Vector3(12,0,5),new Color(1f,.64f,.32f),1000f,45f);
    }
    private static void AddSpot(Transform parent,string name,Vector3 pos,Vector3 target,Color color,float intensity,float angle)
    {
        var go=new GameObject(name); go.transform.SetParent(parent,false); go.transform.localPosition=pos; go.transform.LookAt(target);
        var l=go.AddComponent<Light>(); l.type=LightType.Spot; l.color=color; l.intensity=intensity; l.range=28; l.spotAngle=angle; l.shadows=LightShadows.Soft;
    }
    private static void BuildSignage(Transform root, Mats m)
    {
        var g=new GameObject("07_BRANDED_SIGNAGE"); g.transform.SetParent(root,false);
        Box(g.transform,"Main store identity plaque",new Vector3(0,8.5f,20.05f),new Vector3(12,2.2f,.25f),m.dark);
        Text(g.transform,"OBSIDIAN PROTOCOL",new Vector3(0,8.9f,19.86f),new Vector3(10,.72f,1),2.6f,m.white,TextAlignmentOptions.Center);
        Text(g.transform,"MILITARY PROCUREMENT  //  AUTHORIZED PERSONNEL",new Vector3(0,8.25f,19.85f),new Vector3(10,.36f,1),.75f,m.emissive,TextAlignmentOptions.Center);
        Text(g.transform,"AIR SYSTEMS",new Vector3(-12,7.1f,12.65f),new Vector3(6,.45f,1),1.1f,m.white,TextAlignmentOptions.Center);
        Text(g.transform,"GROUND SYSTEMS",new Vector3(12,7.1f,12.65f),new Vector3(6,.45f,1),1.1f,m.white,TextAlignmentOptions.Center);
        Text(g.transform,"PROCUREMENT / CHECKOUT",new Vector3(0,7.8f,20.05f),new Vector3(10,.4f,1),.9f,m.emissive,TextAlignmentOptions.Center);
    }
    private static TextMeshPro Text(Transform parent,string value,Vector3 pos,Vector3 scale,float size,Material mat,TextAlignmentOptions align)
    {
        var go=new GameObject(value.Replace('/','-')); go.transform.SetParent(parent,false); go.transform.localPosition=pos; go.transform.localScale=Vector3.one*scale.y;
        var t=go.AddComponent<TextMeshPro>(); t.text=value; t.fontSize=size; t.alignment=align; t.color=Color.white; t.textWrappingMode=TextWrappingModes.NoWrap; t.rectTransform.sizeDelta=new Vector2(10f*scale.x/scale.y,1f);
        if(mat!=null && mat.HasProperty("_FaceColor")) t.fontMaterial.SetColor("_FaceColor",mat.color);
        return t;
    }
    private static void PlaceWarden(Transform root, Mats m)
    {
        var prefab=AssetDatabase.LoadAssetAtPath<GameObject>(WardenPrefabPath);
        if(prefab==null) prefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Game/Units/Models/Air/WardenPreview.prefab");
        if(prefab==null) { Debug.LogWarning("Store AAA build: Warden preview prefab not found; environment built without the model."); return; }
        var bay=new GameObject("WARDEN_FIRST_MODEL_TEST"); bay.transform.SetParent(root,false); bay.transform.localPosition=new Vector3(-12,.5f,4.5f);
        var instance=(GameObject)PrefabUtility.InstantiatePrefab(prefab, SceneManager.GetActiveScene());
        if(instance==null) instance=Object.Instantiate(prefab);
        instance.name="WardenPreview_STORE_TEST"; instance.transform.SetParent(bay.transform,false); instance.transform.localPosition=Vector3.zero; instance.transform.localRotation=Quaternion.identity; instance.transform.localScale=Vector3.one;
        SolidBox(bay.transform,"Warden model inspection turntable",new Vector3(0,-.2f,0),new Vector3(4.5f,.25f,4.5f),m.rubber);
        Box(bay.transform,"Warden turntable cyan ring",new Vector3(0,-.055f,0),new Vector3(4.2f,.035f,4.2f),m.emissive);
        var label=Text(bay.transform,"WARDEN // AIRFRAME",new Vector3(0,2.7f,-2.15f),new Vector3(3,.35f,1),.65f,m.white,TextAlignmentOptions.Center);
        label.transform.localRotation=Quaternion.Euler(0,0,0);
    }
}
#endif















