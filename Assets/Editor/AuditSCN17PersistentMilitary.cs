using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class AuditSCN17PersistentMilitary
{
    private const string ScenePath =
        "Assets/Scenes/SCN-17 PERSISTENT MILITARY/Persistent_Military.unity";

    private const string OutputPath =
        "Assets/_Audit/SCN-17_PersistentMilitary_UNITY_COMPONENT_AUDIT.txt";

    [MenuItem("Tools/Obsidian Protocol/Audit/SCN-17 Persistent Military")]
    public static void Run()
    {
        Directory.CreateDirectory(
            Path.Combine(Application.dataPath, "_Audit")
        );

        var scene = EditorSceneManager.OpenScene(
            ScenePath,
            OpenSceneMode.Single
        );

        if (!scene.IsValid())
        {
            Debug.LogError("[SCN-17 AUDIT] Scene could not be opened: " + ScenePath);
            return;
        }

        var sb = new StringBuilder();

        sb.AppendLine("============================================================");
        sb.AppendLine(" SCN-17 PERSISTENT MILITARY");
        sb.AppendLine(" UNITY COMPONENT / HIERARCHY AUDIT");
        sb.AppendLine("============================================================");
        sb.AppendLine();
        sb.AppendLine("SCENE: " + ScenePath);
        sb.AppendLine();

        GameObject[] roots = scene.GetRootGameObjects();

        int totalObjects = 0;
        int totalComponents = 0;
        int buttonCount = 0;
        int emptyButtonCount = 0;
        int eventSystemCount = 0;
        int cameraCount = 0;
        int canvasCount = 0;

        sb.AppendLine("ROOT OBJECT COUNT: " + roots.Length);
        sb.AppendLine();

        foreach (GameObject root in roots)
        {
            AuditObject(
                root,
                0,
                sb,
                ref totalObjects,
                ref totalComponents,
                ref buttonCount,
                ref emptyButtonCount,
                ref eventSystemCount,
                ref cameraCount,
                ref canvasCount
            );
        }

        sb.AppendLine();
        sb.AppendLine("============================================================");
        sb.AppendLine(" SUMMARY");
        sb.AppendLine("============================================================");
        sb.AppendLine("GAMEOBJECTS: " + totalObjects);
        sb.AppendLine("COMPONENTS: " + totalComponents);
        sb.AppendLine("BUTTONS: " + buttonCount);
        sb.AppendLine("BUTTONS WITH EMPTY ONCLICK: " + emptyButtonCount);
        sb.AppendLine("EVENT SYSTEMS: " + eventSystemCount);
        sb.AppendLine("CAMERAS: " + cameraCount);
        sb.AppendLine("CANVASES: " + canvasCount);

        sb.AppendLine();
        sb.AppendLine("============================================================");
        sb.AppendLine(" BUTTON AUDIT");
        sb.AppendLine("============================================================");

        foreach (GameObject root in roots)
        {
            AuditButtons(root, sb);
        }

        sb.AppendLine();
        sb.AppendLine("============================================================");
        sb.AppendLine(" EVENT SYSTEM AUDIT");
        sb.AppendLine("============================================================");

        foreach (GameObject root in roots)
        {
            AuditEventSystems(root, sb);
        }

        sb.AppendLine();
        sb.AppendLine("============================================================");
        sb.AppendLine(" CAMERA AUDIT");
        sb.AppendLine("============================================================");

        foreach (GameObject root in roots)
        {
            AuditCameras(root, sb);
        }

        sb.AppendLine();
        sb.AppendLine("============================================================");
        sb.AppendLine(" CANVAS AUDIT");
        sb.AppendLine("============================================================");

        foreach (GameObject root in roots)
        {
            AuditCanvases(root, sb);
        }

        sb.AppendLine();
        sb.AppendLine("============================================================");
        sb.AppendLine(" END");
        sb.AppendLine("============================================================");

        string projectRoot =
            Directory.GetParent(Application.dataPath).FullName;

        string absolutePath =
            Path.Combine(projectRoot, OutputPath);

        File.WriteAllText(
            absolutePath,
            sb.ToString(),
            Encoding.UTF8
        );

        AssetDatabase.Refresh();

        Debug.Log(
            "[SCN-17 AUDIT] COMPLETE\n" +
            "Objects: " + totalObjects + "\n" +
            "Components: " + totalComponents + "\n" +
            "Buttons: " + buttonCount + "\n" +
            "Empty OnClick: " + emptyButtonCount + "\n" +
            "Report: " + OutputPath
        );
    }

    private static void AuditObject(
        GameObject go,
        int depth,
        StringBuilder sb,
        ref int totalObjects,
        ref int totalComponents,
        ref int buttonCount,
        ref int emptyButtonCount,
        ref int eventSystemCount,
        ref int cameraCount,
        ref int canvasCount
    )
    {
        totalObjects++;

        string indent = new string(' ', depth * 4);

        sb.AppendLine(
            indent +
            "[OBJECT] " +
            go.name +
            " | ACTIVE=" +
            go.activeSelf
        );

        Component[] components = go.GetComponents<Component>();

        foreach (Component component in components)
        {
            totalComponents++;

            if (component == null)
            {
                sb.AppendLine(
                    indent + "    [MISSING COMPONENT]"
                );

                continue;
            }

            sb.AppendLine(
                indent +
                "    [COMPONENT] " +
                component.GetType().FullName
            );

            if (component is MonoBehaviour mono)
            {
                MonoScript script =
                    MonoScript.FromMonoBehaviour(mono);

                if (script != null)
                {
                    sb.AppendLine(
                        indent +
                        "        SCRIPT=" +
                        AssetDatabase.GetAssetPath(script)
                    );
                }
            }

            if (component is Button button)
            {
                buttonCount++;

                int listeners =
                    button.onClick.GetPersistentEventCount();

                sb.AppendLine(
                    indent +
                    "        BUTTON_INTERACTABLE=" +
                    button.interactable
                );

                sb.AppendLine(
                    indent +
                    "        ONCLICK_LISTENERS=" +
                    listeners
                );

                if (listeners == 0)
                {
                    emptyButtonCount++;
                }

                for (int i = 0; i < listeners; i++)
                {
                    UnityEngine.Object target =
                        button.onClick.GetPersistentTarget(i);

                    sb.AppendLine(
                        indent +
                        "        ONCLICK[" +
                        i +
                        "] TARGET=" +
                        (target != null ? target.name : "<NULL>") +
                        " METHOD=" +
                        button.onClick.GetPersistentMethodName(i)
                    );
                }
            }

            if (component is EventSystem)
            {
                eventSystemCount++;
            }

            if (component is Camera)
            {
                cameraCount++;
            }

            if (component is Canvas)
            {
                canvasCount++;
            }
        }

        foreach (Transform child in go.transform)
        {
            AuditObject(
                child.gameObject,
                depth + 1,
                sb,
                ref totalObjects,
                ref totalComponents,
                ref buttonCount,
                ref emptyButtonCount,
                ref eventSystemCount,
                ref cameraCount,
                ref canvasCount
            );
        }
    }

    private static void AuditButtons(
        GameObject go,
        StringBuilder sb
    )
    {
        Button button = go.GetComponent<Button>();

        if (button != null)
        {
            sb.AppendLine(
                "BUTTON: " + GetHierarchyPath(go)
            );

            sb.AppendLine(
                "    ACTIVE: " + go.activeInHierarchy
            );

            sb.AppendLine(
                "    INTERACTABLE: " + button.interactable
            );

            int listeners =
                button.onClick.GetPersistentEventCount();

            sb.AppendLine(
                "    ONCLICK LISTENERS: " + listeners
            );

            for (int i = 0; i < listeners; i++)
            {
                UnityEngine.Object target =
                    button.onClick.GetPersistentTarget(i);

                sb.AppendLine(
                    "    [" +
                    i +
                    "] TARGET=" +
                    (target != null ? target.name : "<NULL>") +
                    " METHOD=" +
                    button.onClick.GetPersistentMethodName(i)
                );
            }

            sb.AppendLine();
        }

        foreach (Transform child in go.transform)
        {
            AuditButtons(child.gameObject, sb);
        }
    }

    private static void AuditEventSystems(
        GameObject go,
        StringBuilder sb
    )
    {
        EventSystem es =
            go.GetComponent<EventSystem>();

        if (es != null)
        {
            sb.AppendLine(
                "EVENT SYSTEM: " + GetHierarchyPath(go)
            );

            foreach (Component component in go.GetComponents<Component>())
            {
                if (component != null)
                {
                    sb.AppendLine(
                        "    " + component.GetType().FullName
                    );
                }
            }

            sb.AppendLine();
        }

        foreach (Transform child in go.transform)
        {
            AuditEventSystems(child.gameObject, sb);
        }
    }

    private static void AuditCameras(
        GameObject go,
        StringBuilder sb
    )
    {
        Camera camera =
            go.GetComponent<Camera>();

        if (camera != null)
        {
            sb.AppendLine(
                "CAMERA: " + GetHierarchyPath(go)
            );

            sb.AppendLine(
                "    ENABLED: " + camera.enabled
            );

            sb.AppendLine(
                "    TAG: " + go.tag
            );

            sb.AppendLine(
                "    DEPTH: " + camera.depth
            );

            sb.AppendLine();
        }

        foreach (Transform child in go.transform)
        {
            AuditCameras(child.gameObject, sb);
        }
    }

    private static void AuditCanvases(
        GameObject go,
        StringBuilder sb
    )
    {
        Canvas canvas =
            go.GetComponent<Canvas>();

        if (canvas != null)
        {
            sb.AppendLine(
                "CANVAS: " + GetHierarchyPath(go)
            );

            sb.AppendLine(
                "    RENDER MODE: " + canvas.renderMode
            );

            sb.AppendLine(
                "    SORTING ORDER: " + canvas.sortingOrder
            );

            sb.AppendLine(
                "    WORLD CAMERA: " +
                (canvas.worldCamera != null
                    ? canvas.worldCamera.name
                    : "<NULL>")
            );

            sb.AppendLine();
        }

        foreach (Transform child in go.transform)
        {
            AuditCanvases(child.gameObject, sb);
        }
    }

    private static string GetHierarchyPath(
        GameObject go
    )
    {
        string path = go.name;

        Transform current =
            go.transform.parent;

        while (current != null)
        {
            path =
                current.name +
                "/" +
                path;

            current = current.parent;
        }

        return path;
    }
}
