using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class BattlefieldMiddlePanelAudit
{
    [MenuItem("Obsidian Protocol/Battlefield/AUDIT MIDDLE PANELS")]
    public static void Audit()
    {
        const string scenePath =
            "Assets/Scenes/SCN03 _BATTLEFIELD/[HUD] BATTLEFIELD HUD/Battlefield_HUD.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single);

        string[] names =
        {
            "HUD UNIT TACTICAL DETAILS",
            "[WINDOW] COMMAND INTENT",
            "[WINDOW] OBJECTIVE PANEL",
            "[WINDOW] FORCE OVERVIEW",
            "[WINDOW] FORMATION MANAGEMENT",
            "[WINDOW] DOCTRINE",
            "[WINDOW] OPERATIONAL ORDERS",
            "[WINDOW] CONTACT DETAILS",
            "[WINDOW] NETWORK HUD",
            "[WINDOW] SENSOR COVERAGE",
            "[WINDOW] BATTLEFIELD LOGISTICS",
            "[WINDOW] ALERT DETAILS"
        };

        foreach (string name in names)
        {
            GameObject[] all =
                Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject go in all)
            {
                if (go == null ||
                    go.scene != scene ||
                    go.name != name)
                    continue;

                RectTransform rt =
                    go.GetComponent<RectTransform>();

                Debug.Log(
                    "[BATTLEFIELD MIDDLE AUDIT] " +
                    name +
                    " | ACTIVE=" + go.activeSelf +
                    " | PARENT=" +
                    (go.transform.parent != null
                        ? go.transform.parent.name
                        : "<ROOT>") +
                    " | POS=" +
                    (rt != null
                        ? rt.anchoredPosition.ToString()
                        : "NO RECT"));
            }
        }

        Debug.Log("[BATTLEFIELD MIDDLE AUDIT] COMPLETE.");
    }
}
