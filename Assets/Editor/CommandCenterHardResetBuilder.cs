using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class CommandCenterButtonController : MonoBehaviour
{
    private readonly Dictionary<string, GameObject> panels =
        new Dictionary<string, GameObject>();

    public void RegisterPanel(string key, GameObject panel)
    {
        panels[key] = panel;

        if (panel != null)
            panel.SetActive(false);
    }

    public void OpenCommand()
    {
        Open("COMMAND");
    }

    public void OpenOperations()
    {
        Open("OPERATIONS");
    }

    public void OpenFleet()
    {
        Open("FLEET");
    }

    public void OpenDeploy()
    {
        Open("DEPLOY");
    }

    public void OpenLogistics()
    {
        Open("LOGISTICS");
    }

    public void OpenResearch()
    {
        Open("RESEARCH");
    }

    public void OpenGarage()
    {
        Open("GARAGE");
    }

    public void OpenFinance()
    {
        Open("FINANCE");
    }

    public void OpenIntelligence()
    {
        Open("INTELLIGENCE");
    }

    public void OpenStore()
    {
        Open("STORE");
    }

    public void ClosePanel()
    {
        foreach (GameObject panel in panels.Values)
        {
            if (panel != null)
                panel.SetActive(false);
        }

        Debug.Log("[COMMAND CENTER] CLOSE");
    }

    private void Open(string key)
    {
        foreach (KeyValuePair<string, GameObject> item in panels)
        {
            if (item.Value != null)
                item.Value.SetActive(item.Key == key);
        }

        Debug.Log("[COMMAND CENTER] OPEN: " + key);
    }
}

public static class CommandCenterHardResetBuilder
{
    [MenuItem("Obsidian Protocol/Command Center/BUILD ONCLICK CONNECTIONS")]
    public static void Build()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name != "Command_Center")
        {
            Debug.LogError(
                "[COMMAND CENTER] Open Command_Center first."
            );

            return;
        }

        Canvas canvas = Object.FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError(
                "[COMMAND CENTER] Canvas not found."
            );

            return;
        }

        Transform rootTransform =
            canvas.transform.Find("COMMAND_CENTER_HARD_TEST");

        if (rootTransform == null)
        {
            Debug.LogError(
                "[COMMAND CENTER] COMMAND_CENTER_HARD_TEST not found."
            );

            return;
        }

        CommandCenterButtonController controller =
            rootTransform.GetComponent<CommandCenterButtonController>();

        if (controller == null)
            controller =
                rootTransform.gameObject.AddComponent<CommandCenterButtonController>();

        string[] names =
        {
            "COMMAND",
            "OPERATIONS",
            "FLEET",
            "DEPLOY",
            "LOGISTICS",
            "RESEARCH",
            "GARAGE",
            "FINANCE",
            "INTELLIGENCE",
            "STORE"
        };

        string[] methods =
        {
            "OpenCommand",
            "OpenOperations",
            "OpenFleet",
            "OpenDeploy",
            "OpenLogistics",
            "OpenResearch",
            "OpenGarage",
            "OpenFinance",
            "OpenIntelligence",
            "OpenStore"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Transform buttonTransform =
                rootTransform.Find("CC_TEST_" + names[i]);

            if (buttonTransform == null)
            {
                Debug.LogWarning(
                    "[COMMAND CENTER] Missing button: " +
                    names[i]
                );

                continue;
            }

            Button button =
                buttonTransform.GetComponent<Button>();

            if (button == null)
                continue;

            button.onClick.RemoveAllListeners();

            int index = i;

            button.onClick.AddListener(() =>
            {
                switch (index)
                {
                    case 0: controller.OpenCommand(); break;
                    case 1: controller.OpenOperations(); break;
                    case 2: controller.OpenFleet(); break;
                    case 3: controller.OpenDeploy(); break;
                    case 4: controller.OpenLogistics(); break;
                    case 5: controller.OpenResearch(); break;
                    case 6: controller.OpenGarage(); break;
                    case 7: controller.OpenFinance(); break;
                    case 8: controller.OpenIntelligence(); break;
                    case 9: controller.OpenStore(); break;
                }
            });

            Debug.Log(
                "[COMMAND CENTER] ONCLICK CONNECTED: " +
                names[i] +
                " -> " +
                methods[i]
            );
        }

        EditorSceneManager.MarkSceneDirty(scene);

        Debug.Log(
            "[COMMAND CENTER] ALL 10 PRIMARY ONCLICKS CONNECTED."
        );
    }
}

