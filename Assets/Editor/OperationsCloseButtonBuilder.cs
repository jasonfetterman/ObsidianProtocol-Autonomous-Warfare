using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class OperationsCloseButtonBuilder
{
    [MenuItem("Obsidian Protocol/Command Center/ADD OPERATIONS CLOSE BUTTON")]
    public static void Build()
    {
        GameObject panel = FindObject("OPERATIONS_PANEL");

        if (panel == null)
        {
            Debug.LogError("[OPERATIONS] OPERATIONS_PANEL not found.");
            return;
        }

        Transform existing = panel.transform.Find("CLOSE");

        if (existing != null)
        {
            Debug.Log("[OPERATIONS] CLOSE already exists.");
            return;
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
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

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

        GameObject controllerObject =
            FindObject("COMMAND_CENTER_FULL_INTERACTION");

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

                Debug.Log("[OPERATIONS] CLOSE wired to CloseAll().");
            }
            else
            {
                Debug.LogError(
                    "[OPERATIONS] CommandCenterFullController not found."
                );
            }
        }
        else
        {
            Debug.LogError(
                "[OPERATIONS] COMMAND_CENTER_FULL_INTERACTION not found."
            );
        }

        EditorUtility.SetDirty(panel);

        EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );

        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );

        Debug.Log("[OPERATIONS] CLOSE BUTTON ADDED.");
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
