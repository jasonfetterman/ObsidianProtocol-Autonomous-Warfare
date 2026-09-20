using UnityEngine;

public class GaragePanelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;

    public void ShowPanel(GameObject panelToShow)
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(panel == panelToShow);
        }
    }
}