using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ApplyMainMenuTextures
{
    private const string SceneGuid = "3a119c7292583d54cbbc2cfac953750c";
    private const string Materials = "Assets/Art/Textures/MainMenuGenerated/";

    [MenuItem("Obsidian Protocol/Main Menu/Apply Room Textures")]
    public static void Apply()
    {
        string scenePath = null;

        foreach (string file in Directory.GetFiles(Application.dataPath, "*.unity", SearchOption.AllDirectories))
        {
            string meta = file + ".meta";
            if (File.Exists(meta) && File.ReadAllText(meta).Contains("guid: " + SceneGuid))
            {
                scenePath = "Assets" + file.Substring(Application.dataPath.Length).Replace('\\', '/');
                break;
            }
        }

        Material wall = AssetDatabase.LoadAssetAtPath<Material>(Materials + "BrushedObsidian.mat");
        Material floor = AssetDatabase.LoadAssetAtPath<Material>(Materials + "ArmoredFloor.mat");
        Material glow = AssetDatabase.LoadAssetAtPath<Material>(Materials + "CyanEmission.mat");

        if (string.IsNullOrEmpty(scenePath) || wall == null || floor == null || glow == null)
        {
            Debug.LogError("[Main Menu Textures] Official scene or generated materials missing. Nothing changed.");
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        string backupDir = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "SceneBackups");
        Directory.CreateDirectory(backupDir);
        string backup = Path.Combine(backupDir, "MainMenu_BeforeTextureApplication.unity");

        if (!File.Exists(backup)) File.Copy(scenePath, backup);

        int applied = 0;
        foreach (GameObject go in scene.GetRootGameObjects())
            applied += ApplyRecursive(go.transform, wall, floor, glow);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();

        Debug.Log("[Main Menu Textures] Applied materials to " + applied +
                  " existing room renderers. HUD preserved. Backup: " + backup);
    }

    private static int ApplyRecursive(Transform root, Material wall, Material floor, Material glow)
    {
        int count = 0;
        string n = root.name.ToUpperInvariant();
        Renderer r = root.GetComponent<Renderer>();

        if (r != null)
        {
            if (n.Contains("FLOOR") && !n.Contains("GRID"))
                r.sharedMaterial = floor;
            else if (n.Contains("ACCENT") || n.Contains("LIGHT") || n.Contains("SERVER_LIGHT"))
                r.sharedMaterial = glow;
            else if (n.Contains("WALL") || n.Contains("CEILING") ||
                     n.Contains("SERVER_COLUMN") || n.Contains("CORE") ||
                     n.Contains("DESK") || n.Contains("TERMINAL"))
                r.sharedMaterial = wall;

            if (n.Contains("FLOOR") || n.Contains("ACCENT") || n.Contains("LIGHT") ||
                n.Contains("WALL") || n.Contains("CEILING") ||
                n.Contains("SERVER_COLUMN") || n.Contains("CORE") ||
                n.Contains("DESK") || n.Contains("TERMINAL"))
                count++;
        }

        for (int i = 0; i < root.childCount; i++)
            count += ApplyRecursive(root.GetChild(i), wall, floor, glow);

        return count;
    }
}
