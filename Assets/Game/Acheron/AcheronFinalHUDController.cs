using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AcheronFinalHUDController : MonoBehaviour
{
    private void Start()
    {
        Wire("ENTER PLANET", EnterPlanet);
        Wire("INTELLIGENCE", OpenIntelligence);
        Wire("OPERATIONS", OpenOperations);
        Wire("DEPLOYMENT", OpenDeployment);
        Wire("LOGISTICS", OpenLogistics);
    }

    private void Wire(string buttonName, UnityEngine.Events.UnityAction action)
    {
        Transform t = FindDeep(transform, buttonName);

        if (t == null)
        {
            Debug.LogWarning("[ACHERON] Button missing: " + buttonName);
            return;
        }

        Button b = t.GetComponent<Button>();

        if (b == null)
        {
            Debug.LogWarning("[ACHERON] Button component missing: " + buttonName);
            return;
        }

        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(action);

        Debug.Log("[ACHERON] WIRED: " + buttonName);
    }

    private void EnterPlanet()
    {
        LoadScene("Planet_Overview");
    }

    private void OpenIntelligence()
    {
        LoadScene("SCN18_INTELLIGENCE");
    }

    private void OpenOperations()
    {
        LoadScene("Operations");
    }

    private void OpenDeployment()
    {
        LoadScene("SCN12_DEPLOYMENT");
    }

    private void OpenLogistics()
    {
        LoadScene("Logistics_Bay");
    }

    private void LoadScene(string sceneName)
    {
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError("[ACHERON] Scene unavailable in Build Settings: " + sceneName);
            return;
        }

        Debug.Log("[ACHERON] Loading scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    private Transform FindDeep(Transform root, string objectName)
    {
        if (root.name == objectName)
            return root;

        foreach (Transform child in root)
        {
            Transform result = FindDeep(child, objectName);

            if (result != null)
                return result;
        }

        return null;
    }
}
