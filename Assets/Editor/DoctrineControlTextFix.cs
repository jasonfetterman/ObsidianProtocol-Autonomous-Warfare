using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class DoctrineControlTextFix
{
    [MenuItem("Obsidian Protocol/Deployment/Fix Doctrine Control Text Components")]
    public static void Fix()
    {
        const string scenePath =
            "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        int converted = 0;

        string[] targets =
        {
            "[HOLO] DOCTRINE CONTROL - DOCTRINE",
            "[HOLO] DOCTRINE CONTROL - PARAMETERS"
        };

        foreach (string rootName in targets)
        {
            GameObject root = GameObject.Find(rootName);

            if (root == null)
            {
                Debug.LogError("[DOCTRINE TEXT FIX] Missing root: " + rootName);
                continue;
            }

            Transform[] children =
                root.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in children)
            {
                if (child == root.transform)
                    continue;

                TMP_Text tmp = child.GetComponent<TMP_Text>();

                if (tmp == null)
                    continue;

                RectTransform rect =
                    child.GetComponent<RectTransform>();

                if (rect == null)
                    continue;

                string textValue = tmp.text;
                Color textColor = tmp.color;

                GameObject go = child.gameObject;

                Undo.RegisterCompleteObjectUndo(
                    go,
                    "Fix Doctrine Text"
                );

                Object.DestroyImmediate(tmp);

                Text legacyText = go.GetComponent<Text>();

                if (legacyText == null)
                    legacyText = go.AddComponent<Text>();

                legacyText.text = textValue;
                legacyText.color = textColor;
                legacyText.fontStyle = FontStyle.Normal;
                legacyText.alignment = TextAnchor.MiddleLeft;
                legacyText.horizontalOverflow =
                    HorizontalWrapMode.Overflow;
                legacyText.verticalOverflow =
                    VerticalWrapMode.Overflow;
                legacyText.raycastTarget = false;

                converted++;

                Debug.Log(
                    "[DOCTRINE TEXT FIX] Converted: " +
                    rootName + " / " + go.name
                );
            }
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[DOCTRINE TEXT FIX] COMPLETE. Converted " +
            converted +
            " text components."
        );
    }
}
