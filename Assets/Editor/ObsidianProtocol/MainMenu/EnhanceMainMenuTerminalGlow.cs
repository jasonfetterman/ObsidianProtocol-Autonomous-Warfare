using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class EnhanceMainMenuTerminalGlow
{
    private const string MaterialPath =
        "Assets/Art/Textures/MainMenuGenerated/CyanEmission.mat";

    [MenuItem("Obsidian Protocol/Main Menu/Enhance Terminal Glow")]
    public static void Apply()
    {
        GameObject terminal =
            GameObject.Find("MAIN_MENU_INTERACTION_TERMINAL");
        Transform screen = terminal != null
            ? terminal.transform.Find("TERMINAL_SCREEN")
            : null;
        Material glow =
            AssetDatabase.LoadAssetAtPath<Material>(MaterialPath);

        if (terminal == null || screen == null || glow == null)
        {
            Debug.LogError(
                "[Terminal Glow] Terminal, screen, or CyanEmission material missing. Nothing changed.");
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        Renderer screenRenderer = screen.GetComponent<Renderer>();
        if (screenRenderer == null)
        {
            Debug.LogError("[Terminal Glow] Screen renderer missing. Nothing changed.");
            return;
        }

        Undo.RecordObject(screenRenderer, "Brighten Terminal Screen");
        screenRenderer.sharedMaterial = glow;

        if (glow.HasProperty("_EmissionColor"))
        {
            Undo.RecordObject(glow, "Increase Terminal Emission");
            glow.EnableKeyword("_EMISSION");
            glow.SetColor("_EmissionColor", new Color(0.02f, 0.8f, 1f) * 5f);
            EditorUtility.SetDirty(glow);
        }

        Transform lightTransform = screen.Find("TERMINAL_CYAN_LIGHT");
        GameObject lightObject;

        if (lightTransform == null)
        {
            lightObject = new GameObject("TERMINAL_CYAN_LIGHT");
            Undo.RegisterCreatedObjectUndo(lightObject, "Add Terminal Cyan Light");
            lightObject.transform.SetParent(screen, false);
            lightObject.transform.localPosition = new Vector3(0f, 0f, -0.15f);
        }
        else
        {
            lightObject = lightTransform.gameObject;
        }

        Light pointLight = lightObject.GetComponent<Light>();
        if (pointLight == null)
            pointLight = Undo.AddComponent<Light>(lightObject);

        pointLight.type = LightType.Point;
        pointLight.color = new Color(0.02f, 0.8f, 1f);
        pointLight.intensity = 3f;
        pointLight.range = 4f;
        pointLight.shadows = LightShadows.None;

        EditorSceneManager.MarkSceneDirty(terminal.scene);
        EditorSceneManager.SaveScene(terminal.scene);
        AssetDatabase.SaveAssets();

        Debug.Log("[Terminal Glow] Bright cyan screen and surrounding light applied.");
    }
}
