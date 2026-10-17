using UnityEngine;
using System.Collections.Generic;

public class FleetPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject rowTemplate;
    [SerializeField] private Transform rowParent;

    private void OnEnable()
    {
        RefreshList();
    }

    private void RefreshList()
    {
        foreach (Transform child in rowParent)
        {
            if (child.gameObject != rowTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        List<(string name, string status)> sampleUnits = new List<(string, string)>
        {
            ("Alpha Squad 01", "Ready"),
            ("Alpha Squad 02", "Ready"),
            ("Alpha Squad 03", "Damaged"),
            ("Alpha Squad 04", "Ready"),
            ("Command Archive", "Ready"),
        };

        foreach (var unit in sampleUnits)
        {
            GameObject newRow = Instantiate(rowTemplate, rowParent);
            newRow.SetActive(true);

            UnitRowDisplay display = newRow.GetComponent<UnitRowDisplay>();
            if (display != null)
            {
                display.SetSampleData(unit.name, unit.status);
            }
        }

        rowTemplate.SetActive(false);
    }
}