using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AuditBattlefieldCommandIntentWindow
{
    private const string ScenePath =
        "Assets/Scenes/SCN03 _BATTLEFIELD/[HUD] BATTLEFIELD HUD/Battlefield_HUD.unity";

    [MenuItem("Obsidian Protocol/Battlefield/AUDIT COMMAND INTENT WINDOW")]
    public static void Audit()
    {
        EditorSceneManager.OpenScene(ScenePath);

        GameObject hud =
            GameObject.Find("[HUD] BATTLEFIELD HUD");

        Transform window =
            FindChildRecursive(
                hud.transform,
                "[WINDOW] COMMAND INTENT");

        if (window == null)
        {
            Debug.LogError("[COMMAND INTENT AUDIT] WINDOW NOT FOUND.");
            return;
        }

        RectTransform rect =
            window.GetComponent<RectTransform>();

        Debug.Log(
            "[COMMAND INTENT AUDIT] WINDOW = " +
            window.name +
            " | ACTIVE=" +
            window.gameObject.activeSelf);

        if (rect != null)
        {
            Debug.Log(
                "[COMMAND INTENT AUDIT] WINDOW RECT | " +
                "ANCHOR MIN=" + rect.anchorMin +
                " | ANCHOR MAX=" + rect.anchorMax +
                " | POS=" + rect.anchoredPosition +
                " | SIZE=" + rect.sizeDelta +
                " | OFFSET MIN=" + rect.offsetMin +
                " | OFFSET MAX=" + rect.offsetMax);
        }

        Debug.Log(
            "[COMMAND INTENT AUDIT] CHILD COUNT = " +
            window.childCount);

        foreach (Transform child in window)
        {
            RectTransform childRect =
                child.GetComponent<RectTransform>();

            Debug.Log(
                "[COMMAND INTENT AUDIT] CHILD = " +
                child.name +
                " | ACTIVE=" +
                child.gameObject.activeSelf +
                " | COMPONENTS=" +
                GetComponents(child.gameObject));

            if (childRect != null)
            {
                Debug.Log(
                    "[COMMAND INTENT AUDIT] RECT = " +
                    child.name +
                    " | POS=" +
                    childRect.anchoredPosition +
                    " | SIZE=" +
                    childRect.sizeDelta +
                    " | ANCHOR MIN=" +
                    childRect.anchorMin +
                    " | ANCHOR MAX=" +
                    childRect.anchorMax);
            }
        }
    }

    private static string GetComponents(GameObject obj)
    {
        Component[] components =
            obj.GetComponents<Component>();

        System.Text.StringBuilder result =
            new System.Text.StringBuilder();

        foreach (Component component in components)
        {
            if (component == null)
                continue;

            if (result.Length > 0)
                result.Append(", ");

            result.Append(component.GetType().Name);
        }

        return result.ToString();
    }

    private static Transform FindChildRecursive(
        Transform parent,
        string objectName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == objectName)
                return child;

            Transform result =
                FindChildRecursive(
                    child,
                    objectName);

            if (result != null)
                return result;
        }

        return null;
    }
}
