using UnityEditor;
using UnityEngine;

public static class DeploymentOperationHoloAudit
{
    [MenuItem("Obsidian Protocol/Deployment/Audit Operation Selection Holo Screens")]
    public static void Audit()
    {
        string[] names =
        {
            "[HOLO] OPERATION SELECTION - OPERATION",
            "[HOLO] OPERATION SELECTION - THEATER"
        };

        foreach (string targetName in names)
        {
            GameObject go = GameObject.Find(targetName);

            if (go == null)
            {
                Debug.LogError("[HOLO AUDIT] NOT FOUND: " + targetName);
                continue;
            }

            RectTransform rect = go.GetComponent<RectTransform>();
            Canvas canvas = go.GetComponent<Canvas>();

            Debug.Log(
                "[HOLO AUDIT] " + targetName +
                " | World Position: " + go.transform.position +
                " | World Rotation: " + go.transform.eulerAngles +
                " | World Scale: " + go.transform.lossyScale +
                " | Rect Size: " + (rect != null ? rect.sizeDelta.ToString() : "NONE") +
                " | Canvas: " + (canvas != null ? canvas.renderMode.ToString() : "NONE")
            );
        }
    }
}
