using UnityEditor;
using UnityEngine;

public static class DoctrineLayoutAudit
{
    [MenuItem("Obsidian Protocol/Deployment/Audit Doctrine Layout")]
    public static void Audit()
    {
        string[] names =
        {
            "[HOLO] DOCTRINE CONTROL - DOCTRINE",
            "[HOLO] DOCTRINE CONTROL - PARAMETERS"
        };

        foreach (string name in names)
        {
            GameObject go = GameObject.Find(name);

            if (go == null)
            {
                Debug.LogError("[LAYOUT] HOLO NOT FOUND: " + name);
                continue;
            }

            Canvas canvas = go.GetComponent<Canvas>();

            Debug.Log(
                "[LAYOUT] " + name +
                " | WORLD=" + go.transform.position +
                " | ROT=" + go.transform.eulerAngles +
                " | SCALE=" + go.transform.localScale +
                " | CANVAS=" +
                (canvas == null ? "NONE" :
                    canvas.GetComponent<RectTransform>().sizeDelta.ToString())
            );
        }

        GameObject console =
            GameObject.Find("Console_Doctrine_Control_Terrminals");

        if (console == null)
        {
            Debug.LogError("[LAYOUT] Doctrine console NOT FOUND.");
            return;
        }

        Transform[] children = console.GetComponentsInChildren<Transform>(true);

        foreach (Transform t in children)
        {
            if (t.name == "[SCREEN] LEFT OPERATOR" ||
                t.name == "[SCREEN] RIGHT OPERATOR")
            {
                Debug.Log(
                    "[LAYOUT] PHYSICAL " + t.name +
                    " | WORLD=" + t.position +
                    " | ROT=" + t.eulerAngles +
                    " | LOSSY=" + t.lossyScale
                );
            }
        }
    }
}
