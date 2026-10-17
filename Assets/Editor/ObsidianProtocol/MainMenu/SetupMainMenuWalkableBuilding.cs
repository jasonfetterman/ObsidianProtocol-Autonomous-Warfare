using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SetupMainMenuWalkableBuilding
{
    private const string SceneGuid = "3a119c7292583d54cbbc2cfac953750c";

    [MenuItem("Obsidian Protocol/Main Menu/Setup Walkable Building")]
    public static void Setup()
    {
        string scenePath = null;

        foreach (string file in Directory.GetFiles(
            Application.dataPath, "*.unity", SearchOption.AllDirectories))
        {
            string meta = file + ".meta";
            if (File.Exists(meta) &&
                File.ReadAllText(meta).Contains("guid: " + SceneGuid))
            {
                scenePath = "Assets" +
                    file.Substring(Application.dataPath.Length).Replace('\\', '/');
                break;
            }
        }

        if (string.IsNullOrEmpty(scenePath))
        {
            Debug.LogError("[Main Menu Setup] Official scene GUID not found. Nothing changed.");
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

        GameObject cameraObject = GameObject.Find("MAIN_MENU_CAMERA");
        GameObject canvas = GameObject.Find("MAIN_MENU_CANVAS");

        if (cameraObject == null || cameraObject.GetComponent<Camera>() == null ||
            canvas == null)
        {
            Debug.LogError("[Main Menu Setup] Camera or HUD canvas missing. Scene not modified.");
            return;
        }

        if (GameObject.Find("MAIN_MENU_WALK_PLAYER") != null ||
            GameObject.Find("MAIN_MENU_INTERACTION_TERMINAL") != null)
        {
            Debug.LogError("[Main Menu Setup] Player or terminal already exists. Nothing changed.");
            return;
        }

        string backupFolder = Path.Combine(
            Directory.GetParent(Application.dataPath).FullName, "SceneBackups");
        Directory.CreateDirectory(backupFolder);
        File.Copy(scenePath, Path.Combine(
            backupFolder, "MainMenu_BeforeWalkableSetup.unity"), true);

        GameObject player = new GameObject("MAIN_MENU_WALK_PLAYER");
        Undo.RegisterCreatedObjectUndo(player, "Create Main Menu Walking Player");
        player.transform.position = new Vector3(0f, 0f, -7f);

        CharacterController cc = Undo.AddComponent<CharacterController>(player);
        cc.height = 1.8f;
        cc.radius = 0.3f;
        cc.center = new Vector3(0f, 0.9f, 0f);

        Undo.SetTransformParent(cameraObject.transform, player.transform,
            "Attach Main Menu Camera");
        cameraObject.transform.localPosition = new Vector3(0f, 1.6f, 0f);
        cameraObject.transform.localRotation = Quaternion.identity;

        SCN17FirstPersonWalkController movement =
            Undo.AddComponent<SCN17FirstPersonWalkController>(player);
        movement.playerCamera = cameraObject.GetComponent<Camera>();

        GameObject terminal = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Undo.RegisterCreatedObjectUndo(terminal, "Create Main Menu Terminal");
        terminal.name = "MAIN_MENU_INTERACTION_TERMINAL";
        terminal.transform.position = new Vector3(0f, 1.1f, 2f);
        terminal.transform.localScale = new Vector3(1.2f, 1.8f, 0.8f);

        Renderer renderer = terminal.GetComponent<Renderer>();
        Material material = new Material(Shader.Find("Standard"));
        material.color = new Color(0.025f, 0.08f, 0.10f);
        renderer.sharedMaterial = material;

        GameObject screen = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Undo.RegisterCreatedObjectUndo(screen, "Create Terminal Screen");
        screen.name = "TERMINAL_SCREEN";
        screen.transform.SetParent(terminal.transform, false);
        screen.transform.localPosition = new Vector3(0f, 0.25f, -0.51f);
        screen.transform.localScale = new Vector3(0.8f, 0.45f, 0.025f);
        Object.DestroyImmediate(screen.GetComponent<Collider>());

        MainMenuTerminalInteraction interaction =
            Undo.AddComponent<MainMenuTerminalInteraction>(terminal);
        interaction.player = player.transform;
        interaction.menu = canvas;

        canvas.SetActive(false);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();

        Debug.Log("[Main Menu Setup] Walkable player and terminal added. HUD preserved and hidden until E interaction. Backup saved outside Assets.");
    }
}
