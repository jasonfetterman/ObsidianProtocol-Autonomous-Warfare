using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class BuildOperationsHUD
{
    [MenuItem("Obsidian Protocol/Operations/BUILD OPERATIONS INTERACTION")]
    public static void Build()
    {
        const string scenePath =
            "Assets/Scenes/SCN05_OPERATIONS/" +
            "[HUD] OPERATIONS HUD/Operations.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        GameObject root =
            GameObject.Find("Canvas_OperationsHUD");

        if (root == null)
        {
            Debug.LogError(
                "[OPERATIONS] Canvas_OperationsHUD NOT FOUND."
            );
            return;
        }

        OperationsHUDController controller =
            root.GetComponent<OperationsHUDController>();

        if (controller == null)
            root.AddComponent<OperationsHUDController>();

        string[] navNames =
        {
            "NAV_0",
            "NAV_1",
            "NAV_2",
            "NAV_3",
            "NAV_4",
            "NAV_5",
            "NAV_6",
            "NAV_7",
            "NAV_8"
        };

        int added = 0;
        int existing = 0;

        foreach (string navName in navNames)
        {
            Transform nav = FindChild(
                root.transform,
                navName
            );

            if (nav == null)
            {
                Debug.LogWarning(
                    "[OPERATIONS] NAV NOT FOUND: " +
                    navName
                );
                continue;
            }

            Button button =
                nav.GetComponent<Button>();

            if (button == null)
            {
                button =
                    nav.gameObject.AddComponent<Button>();

                added++;
            }
            else
            {
                existing++;
            }

            Image image =
                nav.GetComponent<Image>();

            if (image != null)
                button.targetGraphic = image;

            button.interactable = true;
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[OPERATIONS] BUILD COMPLETE | " +
            "NAV BUTTONS ADDED=" + added +
            " | EXISTING=" + existing
        );
    }

    private static Transform FindChild(
        Transform root,
        string objectName)
    {
        foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == objectName)
                return child;
        }

        return null;
    }
}
