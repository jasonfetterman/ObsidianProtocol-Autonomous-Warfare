using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public static class OPAWSettingsInputNeonRepair
{
    private const string ScenePath =
        "Assets/Scenes/SCN14_SETTINGS/[HUD] SETTINGS HUD/Settings.unity";

    private static readonly Color Neon =
        new Color(0.251f, 0.796f, 0.922f, 0.96f);

    [MenuItem("Obsidian Protocol/Settings/FIX NEON AND RESTORE ALL CONTROLS")]
    public static void Repair()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (!scene.IsValid() || scene.path != ScenePath)
            scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

        int controls = 0;
        int canvases = 0;
        int groups = 0;
        int frames = 0;

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            Transform[] transforms = root.GetComponentsInChildren<Transform>(true);

            foreach (Transform t in transforms)
            {
                if (t == null) continue;

                GameObject go = t.gameObject;

                CanvasGroup group = go.GetComponent<CanvasGroup>();
                if (group != null)
                {
                    group.interactable = true;
                    group.blocksRaycasts = true;
                    EditorUtility.SetDirty(group);
                    groups++;
                }

                Canvas canvas = go.GetComponent<Canvas>();
                if (canvas != null)
                {
                    GraphicRaycaster raycaster = go.GetComponent<GraphicRaycaster>();

                    if (raycaster == null)
                        Undo.AddComponent<GraphicRaycaster>(go);
                    else
                        raycaster.enabled = true;

                    canvas.enabled = true;
                    canvases++;
                }

                OPAWSettingsBoxFrameGraphic frame =
                    go.GetComponent<OPAWSettingsBoxFrameGraphic>();

                if (frame != null)
                {
                    frame.enabled = true;
                    frame.raycastTarget = false;
                    frame.color = Neon;
                    frame.SetAllDirty();
                    go.transform.SetAsLastSibling();
                    EditorUtility.SetDirty(frame);
                    frames++;
                }

                OPAWSettingsBoxHighlight oldHighlight =
                    go.GetComponent<OPAWSettingsBoxHighlight>();

                if (oldHighlight != null)
                    Undo.DestroyObjectImmediate(oldHighlight);

                Selectable selectable = go.GetComponent<Selectable>();

                if (selectable == null)
                    continue;

                selectable.enabled = true;
                selectable.interactable = true;
                selectable.transition = Selectable.Transition.ColorTint;

                ColorBlock colorBlock = selectable.colors;
                colorBlock.normalColor = Color.white;
                colorBlock.highlightedColor = new Color(0.88f, 0.96f, 1f, 1f);
                colorBlock.pressedColor = new Color(0.72f, 0.88f, 0.96f, 1f);
                colorBlock.selectedColor = new Color(0.88f, 0.96f, 1f, 1f);
                colorBlock.disabledColor = new Color(0.45f, 0.52f, 0.57f, 0.65f);
                colorBlock.fadeDuration = 0.08f;
                colorBlock.colorMultiplier = 1f;
                selectable.colors = colorBlock;

                Graphic target = selectable.targetGraphic;
                if (target != null)
                    target.raycastTarget = true;

                // Make slider tracks, handles, input fields and button surfaces
                // receive pointer events. Decorative neon frames remain non-blocking.
                Graphic[] childGraphics = go.GetComponentsInChildren<Graphic>(true);

                foreach (Graphic graphic in childGraphics)
                {
                    if (graphic == null ||
                        graphic.GetComponent<OPAWSettingsBoxFrameGraphic>() != null)
                        continue;

                    graphic.raycastTarget = true;
                    EditorUtility.SetDirty(graphic);
                }

                EditorUtility.SetDirty(selectable);
                controls++;
            }
        }

        // Ensure an active EventSystem and UI input module exist.
        EventSystem eventSystem = UnityEngine.Object.FindObjectOfType<EventSystem>();

        if (eventSystem == null)
        {
            GameObject eventObject = new GameObject("EventSystem");
            Undo.RegisterCreatedObjectUndo(eventObject, "Create Settings EventSystem");
            eventSystem = Undo.AddComponent<EventSystem>(eventObject);
        }

        eventSystem.enabled = true;
        eventSystem.sendNavigationEvents = true;

        BaseInputModule inputModule =
            eventSystem.GetComponent<BaseInputModule>();

        if (inputModule == null)
        {
            Type inputSystemType = Type.GetType(
                "UnityEngine.InputSystem.UI.InputSystemUIInputModule, Unity.InputSystem"
            );

            if (inputSystemType != null)
                Undo.AddComponent(eventSystem.gameObject, inputSystemType);
            else
                Undo.AddComponent<StandaloneInputModule>(eventSystem.gameObject);
        }
        else
        {
            inputModule.enabled = true;
        }

        EditorUtility.SetDirty(eventSystem);
        EditorSceneManager.MarkSceneDirty(scene);

        if (EditorSceneManager.SaveScene(scene))
        {
            Debug.Log(
                "SETTINGS REPAIR SAVED | Controls enabled: " + controls +
                " | Canvas raycasters checked: " + canvases +
                " | CanvasGroups restored: " + groups +
                " | Static neon frames restored: " + frames
            );
        }
        else
        {
            Debug.LogError("Could not save repaired Settings scene.");
        }
    }
}
