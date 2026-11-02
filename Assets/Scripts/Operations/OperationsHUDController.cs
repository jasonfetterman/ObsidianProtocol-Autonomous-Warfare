using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OperationsHUDController : MonoBehaviour
{
    private void Start()
    {
        WireButtons();
        ShowSection("MISSION");

        Debug.Log("[OPERATIONS] INTERACTION ONLINE.");
    }

    private void WireButtons()
    {
        Wire("NAV_0", MissionOverview);
        Wire("NAV_1", Battlefield);
        Wire("NAV_2", ForceManagement);
        Wire("NAV_3", AICommand);
        Wire("NAV_4", Reconnaissance);
        Wire("NAV_5", Logistics);
        Wire("NAV_6", Intelligence);
        Wire("NAV_7", Deployment);
        Wire("NAV_8", AfterAction);

        Debug.Log("[OPERATIONS] 9 NAVIGATION CONTROLS WIRED.");
    }

    private void Wire(string objectName, UnityEngine.Events.UnityAction action)
    {
        Transform target = FindObject(objectName);

        if (target == null)
        {
            Debug.LogWarning(
                "[OPERATIONS] NAV OBJECT NOT FOUND: " +
                objectName
            );
            return;
        }

        Button button = target.GetComponent<Button>();

        if (button == null)
        {
            button = target.gameObject.AddComponent<Button>();
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);

        Image image = target.GetComponent<Image>();

        if (image != null)
            button.targetGraphic = image;
    }

    private void MissionOverview()
    {
        Debug.Log("[OPERATIONS] MISSION OVERVIEW");
        ShowSection("MISSION");
    }

    private void Battlefield()
    {
        Debug.Log("[OPERATIONS] BATTLEFIELD");
        ShowSection("TACTICAL_BATTLEFIELD");
    }

    private void ForceManagement()
    {
        Debug.Log("[OPERATIONS] FORCE MANAGEMENT");
        ShowSection("SELECTED_UNIT");
    }

    private void AICommand()
    {
        Debug.Log("[OPERATIONS] AI COMMAND");
        ShowSection("COMMAND_BOX");
    }

    private void Reconnaissance()
    {
        Debug.Log("[OPERATIONS] RECONNAISSANCE");
        ShowSection("MAP_STATUS");
    }

    private void Logistics()
    {
        Debug.Log("[OPERATIONS] LOGISTICS");

        LoadScene(
            "Assets/Scenes/SCN08_LOGISTICS/" +
            "[HUD] LOGISTICS HUD/Logistics_Bay.unity"
        );
    }

    private void Intelligence()
    {
        Debug.Log("[OPERATIONS] INTELLIGENCE");

        LoadScene(
            "Assets/Scenes/SCN18_INTELLIGENCE/" +
            "SCN18_INTELLIGENCE.unity"
        );
    }

    private void Deployment()
    {
        Debug.Log("[OPERATIONS] DEPLOYMENT");

        LoadScene(
            "Assets/Scenes/SCN12_DEPLOYMENT/" +
            "SCN12_DEPLOYMENT.unity"
        );
    }

    private void AfterAction()
    {
        Debug.Log("[OPERATIONS] AFTER ACTION");

        LoadScene(
            "Assets/Scenes/SCN10_AFTER_ACTION/" +
            "After_Action.unity"
        );
    }

    private void ShowSection(string sectionName)
    {
        string[] sections =
        {
            "MISSION",
            "TACTICAL_BATTLEFIELD",
            "SELECTED_UNIT",
            "COMMAND_BOX",
            "MAP_STATUS"
        };

        foreach (string name in sections)
        {
            Transform section = FindObject(name);

            if (section != null)
                section.gameObject.SetActive(
                    name == sectionName
                );
        }

        Transform target = FindObject(sectionName);

        if (target == null)
        {
            Debug.LogWarning(
                "[OPERATIONS] SECTION NOT FOUND: " +
                sectionName
            );
            return;
        }

        target.gameObject.SetActive(true);

        Debug.Log(
            "[OPERATIONS] SECTION OPENED: " +
            sectionName
        );
    }

    private Transform FindObject(string objectName)
    {
        Transform[] all =
            GetComponentsInChildren<Transform>(true);

        foreach (Transform t in all)
        {
            if (t.name == objectName)
                return t;
        }

        return null;
    }

    private void LoadScene(string scenePath)
    {
        Debug.Log(
            "[OPERATIONS] LOADING SCENE: " +
            scenePath
        );

        SceneManager.LoadScene(scenePath);
    }
}




