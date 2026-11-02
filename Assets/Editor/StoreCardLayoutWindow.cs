using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StoreCardLayoutWindow : EditorWindow
{
    private int columns = 3;
    private float left = 0.025f;
    private float right = 0.975f;
    private float top = 0.835f;
    private float cardHeight = 0.245f;
    private float gapX = 0.018f;
    private float gapY = 0.018f;

    [MenuItem("Obsidian Protocol/Store/Adjust Store Cards")]
    private static void Open()
    {
        GetWindow<StoreCardLayoutWindow>("Adjust Store Cards");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Store Card Layout", EditorStyles.boldLabel);
        columns = EditorGUILayout.IntSlider("Columns", columns, 1, 6);
        left = EditorGUILayout.Slider("Left Margin", left, 0f, 0.2f);
        right = EditorGUILayout.Slider("Right Edge", right, 0.8f, 1f);
        top = EditorGUILayout.Slider("Top Position", top, 0.4f, 1f);
        cardHeight = EditorGUILayout.Slider("Card Height", cardHeight, 0.08f, 0.4f);
        gapX = EditorGUILayout.Slider("Horizontal Gap", gapX, 0f, 0.08f);
        gapY = EditorGUILayout.Slider("Vertical Gap", gapY, 0f, 0.08f);

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(
            "Applies layout to existing ASSET_CARD objects only. Does not rebuild the HUD or delete units.",
            MessageType.Info);

        if (GUILayout.Button("Apply Layout"))
        {
            ApplyLayout();
        }
    }

    private void ApplyLayout()
    {
        GameObject first = GameObject.Find("ASSET_CARD_1");
        if (first == null)
        {
            EditorUtility.DisplayDialog("Store Cards Not Found",
                "Open the store scene containing ASSET_CARD_1 first.", "OK");
            return;
        }

        Transform parent = first.transform.parent;
        int count = 0;
        float width = (right - left - gapX * (columns - 1)) / columns;

        if (width <= 0f)
        {
            EditorUtility.DisplayDialog("Invalid Layout",
                "Margins and gaps leave no room for cards.", "OK");
            return;
        }

        Undo.RecordObjects(parent.GetComponentsInChildren<RectTransform>(true),
            "Adjust Store Card Layout");

        for (int i = 1; i <= 12; i++)
        {
            Transform card = parent.Find("ASSET_CARD_" + i);
            if (card == null) continue;

            int row = (i - 1) / columns;
            int col = (i - 1) % columns;

            RectTransform rect = card as RectTransform;
            if (rect == null) continue;

            float xMin = left + col * (width + gapX);
            float xMax = xMin + width;
            float yMax = top - row * (cardHeight + gapY);
            float yMin = yMax - cardHeight;

            rect.anchorMin = new Vector2(xMin, yMin);
            rect.anchorMax = new Vector2(xMax, yMax);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            count++;
        }

        EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        EditorUtility.DisplayDialog("Store Cards Updated",
            "Adjusted " + count + " existing cards. Review the scene before saving.",
            "OK");
    }
}

