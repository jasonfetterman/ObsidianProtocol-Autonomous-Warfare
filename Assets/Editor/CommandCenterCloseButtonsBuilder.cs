using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class CommandCenterCloseButtonsBuilder
{
    [MenuItem("Obsidian Protocol/Command Center/ADD CLOSE BUTTON TO ALL PANELS")]
    public static void Build()
    {
        string[] panelNames =
        {
            "COMMAND_PANEL",
            "OPERATIONS_PANEL",
            "FLEET_PANEL",
            "DEPLOY_PANEL",
            "LOGISTICS_PANEL",
            "RESEARCH_PANEL",
            "GARAGE_PANEL",
            "FINANCE_PANEL",
            "INTELLIGENCE_PANEL",
            "STORE_PANEL"
        };

        int added = 0;

        foreach (string panelName in panelNames)
        {
            GameObject panel = FindObject(panelName);

            if (panel == null)
            {
                Debug.LogWarning("[COMMAND CENTER] Missing panel: " + panelName);
                continue;
            }

            Transform existing = panel.transform.Find("CLOSE");

            if (existing != null)
            {
                Debug.Log("[COMMAND CENTER] CLOSE already exists: " + panelName);
                continue;
            }

            GameObject close = new GameObject(
                "CLOSE",
                typeof(RectTransform),
                typeof(Image),
                typeof(Button)
            );

            close.transform.SetParent(panel.transform, false);

            RectTransform rect = close.GetComponent<RectTransform>();

            rect.anchorMin = new Vector2(.88f, .91f);
            rect.anchorMax = new Vector2(.97f, .985f);
            rect.offsetMin = new Vector2(0, 0);
            rect.offsetMax = new Vector2(0, 0);

            Image image = close.GetComponent<Image>();
            image.color = new Color(.07f, .10f, .13f, 1f);

            Button button = close.GetComponent<Button>();

            ColorBlock colors = button.colors;
            colors.normalColor = new Color(.07f, .10f, .13f, 1f);
            colors.highlightedColor = new Color(.15f, .20f, .25f, 1f);
            colors.pressedColor = new Color(.20f, .25f, .30f, 1f);
            button.colors = colors;

            GameObject textObject = new GameObject(
                "TEXT",
                typeof(RectTransform),
                typeof(Text)
            );

            textObject.transform.SetParent(close.transform, false);

            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            Text text = textObject.GetComponent<Text>();
            text.text = "CLOSE";
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 11;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;

            // Wire CLOSE directly to CommandCenterFullController.CloseAll()
            GameObject controllerObject = FindObject("COMMAND_CENTER_FULL_INTERACTION");

            if (controllerObject != null)
            {
                CommandCenterFullController controller =
                    controllerObject.GetComponent<CommandCenterFullController>();

                if (controller != null)
                {
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(
                        button.onClick,
                        controller.CloseAll
                    );
                }
                else
                {
                    Debug.LogWarning(
                        "[COMMAND CENTER] Controller component missing on COMMAND_CENTER_FULL_INTERACTION."
                    );
                }
            }
            else
            {
                Debug.LogWarning(
                    "[COMMAND CENTER] COMMAND_CENTER_FULL_INTERACTION not found."
                );
            }

            EditorUtility.SetDirty(panel);
            added++;

            Debug.Log(
                "[COMMAND CENTER] Added and wired CLOSE: " + panelName
            );
        }

        EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );

        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );

        Debug.Log(
            "[COMMAND CENTER] CLOSE BUTTON BUILD COMPLETE. Added: " + added
        );
    }

    private static GameObject FindObject(string name)
    {
        GameObject[] objects =
            Object.FindObjectsByType<GameObject>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        foreach (GameObject obj in objects)
        {
            if (obj.name == name)
                return obj;
        }

        return null;
    }
}
