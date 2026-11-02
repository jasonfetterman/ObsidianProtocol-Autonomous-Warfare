using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class ConfigureGarageHUDToggle
{
    [MenuItem("Obsidian Protocol/Scene Editing/Configure Garage HUD Toggle (E)")]
    public static void Configure()
    {
        const string scenePath =
            "Assets/Scenes/SCN20_VR_COMMAND/SCN20_VR_COMMAND.unity";

        if (EditorSceneManager.GetActiveScene().path != scenePath)
        {
            Debug.LogError("Open SCN20_VR_COMMAND first. No changes made.");
            return;
        }

        Canvas[] canvases = Object.FindObjectsByType<Canvas>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        Canvas target = null;

        foreach (Canvas canvas in canvases)
        {
            if (canvas == null) continue;

            string n = canvas.gameObject.name.ToLowerInvariant();

            if (n.Contains("editable garage hud") ||
                n.Contains("garage hud"))
            {
                target = canvas;
                break;
            }
        }

        if (target == null)
        {
            Debug.LogError(
                "Garage HUD Canvas not found. No changes made. " +
                "Check the exact Canvas name in the Hierarchy.");
            return;
        }

        GarageHUDToggle toggle =
            target.GetComponent<GarageHUDToggle>();

        if (toggle == null)
            toggle = Undo.AddComponent<GarageHUDToggle>(target.gameObject);

        Undo.RecordObject(toggle, "Configure Garage HUD Toggle");
        toggle.ConfigureTarget(target.gameObject);

        Undo.RecordObject(target.gameObject, "Hide Garage HUD at Startup");
        target.gameObject.SetActive(false);

        EditorSceneManager.MarkSceneDirty(
            EditorSceneManager.GetActiveScene());

        Debug.Log(
            "Garage HUD configured to start hidden. " +
            "Press E to toggle it during Play mode. Save the scene.");
    }
}

public class GarageHUDToggle : MonoBehaviour
{
    [SerializeField] private GameObject hudRoot;

    public void ConfigureTarget(GameObject target)
    {
        hudRoot = target;
    }

    private void Awake()
    {
        if (hudRoot != null)
            hudRoot.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && hudRoot != null)
            hudRoot.SetActive(!hudRoot.activeSelf);
    }
}
