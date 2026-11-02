using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Events;

public static class BuildIntelligenceReturnButton
{
    [MenuItem("Obsidian Protocol/Intelligence/BUILD RETURN TO OPERATIONS")]
    public static void Build()
    {
        const string scenePath =
            "Assets/Scenes/SCN18_INTELLIGENCE/SCN18_INTELLIGENCE.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        GameObject hud = GameObject.Find("INTELLIGENCE HUD");

        if (hud == null)
        {
            Debug.LogError(
                "[INTELLIGENCE RETURN] HUD NOT FOUND."
            );
            return;
        }

        Transform existing =
            hud.transform.Find("RETURN TO OPERATIONS");

        GameObject buttonObject;

        if (existing != null)
        {
            buttonObject = existing.gameObject;

            Debug.Log(
                "[INTELLIGENCE RETURN] EXISTING BUTTON FOUND."
            );
        }
        else
        {
            buttonObject =
                new GameObject(
                    "RETURN TO OPERATIONS",
                    typeof(RectTransform),
                    typeof(CanvasRenderer),
                    typeof(Image),
                    typeof(Button)
                );

            buttonObject.transform.SetParent(
                hud.transform,
                false
            );

            Debug.Log(
                "[INTELLIGENCE RETURN] BUTTON CREATED."
            );
        }

        RectTransform rect =
            buttonObject.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0f, 0f);

        rect.anchorMax =
            new Vector2(0f, 0f);

        rect.pivot =
            new Vector2(0f, 0f);

        rect.anchoredPosition =
            new Vector2(32f, 32f);

        rect.sizeDelta =
            new Vector2(220f, 48f);

        Image image =
            buttonObject.GetComponent<Image>();

        image.raycastTarget = true;

        Button button =
            buttonObject.GetComponent<Button>();

        button.interactable = true;
        button.onClick.RemoveAllListeners();

        IntelligenceHUDController controller =
            hud.GetComponent<IntelligenceHUDController>();

        if (controller == null)
        {
            Debug.LogError(
                "[INTELLIGENCE RETURN] " +
                "IntelligenceHUDController NOT FOUND."
            );
            return;
        }

        while (button.onClick.GetPersistentEventCount() > 0)
        {
            UnityEventTools.RemovePersistentListener(
                button.onClick,
                0
            );
        }

        UnityEventTools.AddPersistentListener(
            button.onClick,
            controller.ReturnToOperations
        );

        GameObject labelObject =
            buttonObject.transform.Find("LABEL")?.gameObject;

        if (labelObject == null)
        {
            labelObject =
                new GameObject(
                    "LABEL",
                    typeof(RectTransform),
                    typeof(CanvasRenderer),
                    typeof(Text)
                );

            labelObject.transform.SetParent(
                buttonObject.transform,
                false
            );
        }

        RectTransform labelRect =
            labelObject.GetComponent<RectTransform>();

        labelRect.anchorMin =
            Vector2.zero;

        labelRect.anchorMax =
            Vector2.one;

        labelRect.offsetMin =
            Vector2.zero;

        labelRect.offsetMax =
            Vector2.zero;

        Text label =
            labelObject.GetComponent<Text>();

        label.text =
            "RETURN TO OPERATIONS";

        label.alignment =
            TextAnchor.MiddleCenter;

        label.fontSize = 16;

        label.fontStyle =
            FontStyle.Bold;

        label.raycastTarget = false;

        label.font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf"
            );

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[INTELLIGENCE RETURN] BUILD COMPLETE."
        );

        Debug.Log(
            "[INTELLIGENCE RETURN] " +
            "BUTTON -> IntelligenceHUDController.ReturnToOperations()"
        );
    }
}


