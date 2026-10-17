using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class AuditIntelligenceReturnButton
{
    [MenuItem("Obsidian Protocol/Intelligence/AUDIT RETURN BUTTON")]
    public static void Audit()
    {
        const string scenePath =
            "Assets/Scenes/SCN18_INTELLIGENCE/SCN18_INTELLIGENCE.unity";

        EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        GameObject hud =
            GameObject.Find("INTELLIGENCE HUD");

        if (hud == null)
        {
            Debug.LogError(
                "[INTELLIGENCE RETURN AUDIT] HUD NOT FOUND."
            );
            return;
        }

        Transform target =
            hud.transform.Find("RETURN TO OPERATIONS");

        if (target == null)
        {
            Debug.LogError(
                "[INTELLIGENCE RETURN AUDIT] BUTTON NOT FOUND."
            );
            return;
        }

        Button button =
            target.GetComponent<Button>();

        RectTransform rect =
            target.GetComponent<RectTransform>();

        Debug.Log(
            "[INTELLIGENCE RETURN AUDIT] " +
            "FOUND | ACTIVE=" +
            target.gameObject.activeSelf
        );

        Debug.Log(
            "[INTELLIGENCE RETURN AUDIT] " +
            "SIZE=" + rect.sizeDelta +
            " | POS=" + rect.anchoredPosition
        );

        Debug.Log(
            "[INTELLIGENCE RETURN AUDIT] " +
            "LISTENERS=" + button.onClick.GetPersistentEventCount()
        );

        for (int i = 0;
             i < button.onClick.GetPersistentEventCount();
             i++)
        {
            Debug.Log(
                "[INTELLIGENCE RETURN AUDIT] " +
                "LISTENER " + i +
                " | TARGET=" +
                button.onClick.GetPersistentTarget(i) +
                " | METHOD=" +
                button.onClick.GetPersistentMethodName(i)
            );
        }

        Debug.Log(
            "[INTELLIGENCE RETURN AUDIT] COMPLETE."
        );
    }
}
