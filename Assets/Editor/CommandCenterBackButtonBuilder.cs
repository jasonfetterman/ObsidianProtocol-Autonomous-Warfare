using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class CommandCenterBackButtonBuilder
{
    [MenuItem("Obsidian Protocol/Command Center/ADD BACK BUTTON")]
    public static void Build()
    {
        GameObject nav = GameObject.Find("PRIMARY_NAVIGATION");

        if (nav == null)
        {
            Debug.LogError("[COMMAND CENTER] PRIMARY_NAVIGATION NOT FOUND");
            return;
        }

        Transform existing = nav.transform.Find("BUTTON_BACK");

        if (existing != null)
        {
            Debug.Log("[COMMAND CENTER] BACK BUTTON ALREADY EXISTS");
            return;
        }

        GameObject buttonObject = new GameObject(
            "BUTTON_BACK",
            typeof(RectTransform),
            typeof(Image),
            typeof(Button)
        );

        buttonObject.transform.SetParent(nav.transform, false);

        RectTransform rect = buttonObject.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.05f, 0.02f);
        rect.anchorMax = new Vector2(0.95f, 0.10f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image = buttonObject.GetComponent<Image>();
        image.color = new Color(0.07f, 0.10f, 0.13f, 1f);

        Button button = buttonObject.GetComponent<Button>();

        GameObject textObject = new GameObject(
            "LABEL",
            typeof(RectTransform),
            typeof(Text)
        );

        textObject.transform.SetParent(buttonObject.transform, false);

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        Text text = textObject.GetComponent<Text>();
        text.text = "BACK";
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 16;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;

        CommandCenterFullController controller =
            GameObject.Find("COMMAND_CENTER_FULL_INTERACTION")
            ?.GetComponent<CommandCenterFullController>();

        if (controller != null)
            UnityEditor.Events.UnityEventTools.AddPersistentListener(
                button.onClick,
                controller.BackToMainMenu
            );

        EditorUtility.SetDirty(nav);

        Scene scene =
            UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[COMMAND CENTER] BACK BUTTON ADDED AND WIRED");
    }
}

