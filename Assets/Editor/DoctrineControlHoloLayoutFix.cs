using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class DoctrineControlHoloLayoutFix
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    [MenuItem("Obsidian Protocol/Deployment/Fix Doctrine Control Holo Layout")]
    public static void Fix()
    {
        var scene = EditorSceneManager.OpenScene(ScenePath);

        FixRoot("[HOLO] DOCTRINE CONTROL - DOCTRINE");
        FixRoot("[HOLO] DOCTRINE CONTROL - PARAMETERS");

        EditorSceneManager.SaveScene(scene);

        Debug.Log("[HOLO] Doctrine Control layout FIXED.");
    }

    private static void FixRoot(string rootName)
    {
        GameObject root = GameObject.Find(rootName);

        if (root == null)
        {
            Debug.LogError("[HOLO] NOT FOUND: " + rootName);
            return;
        }

        RectTransform canvasRect =
            root.GetComponent<RectTransform>();

        canvasRect.sizeDelta = new Vector2(580f, 240f);

        Transform titleTransform = root.transform.Find("TITLE");

        if (titleTransform != null)
        {
            RectTransform rect =
                titleTransform.GetComponent<RectTransform>();

            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0f, 0.5f);
            rect.sizeDelta = new Vector2(536f, 30f);
            rect.anchoredPosition = new Vector2(-268f, 92f);
        }

        for (int i = 0; i < 8; i++)
        {
            Transform dataTransform =
                root.transform.Find("DATA_" + i.ToString("00"));

            if (dataTransform == null)
                continue;

            RectTransform rect =
                dataTransform.GetComponent<RectTransform>();

            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0f, 0.5f);
            rect.sizeDelta = new Vector2(532f, 18f);

            float y = 58f - (i * 21f);

            rect.anchoredPosition =
                new Vector2(-266f, y);
        }

        Debug.Log("[HOLO] Layout fixed: " + rootName);
    }
}
