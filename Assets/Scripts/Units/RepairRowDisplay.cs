using UnityEngine;
using TMPro;

public class RepairRowDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI unitNameText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private UnityEngine.UI.Button repairButton;

    private string unitName;

    public void SetData(string name, int cost)
    {
        unitName = name;
        unitNameText.text = name;
        costText.text = $"{cost} Credits";

        repairButton.onClick.RemoveAllListeners();
        repairButton.onClick.AddListener(OnRepairClicked);
    }

    private void OnRepairClicked()
    {
        Debug.Log($"Repairing {unitName}...");
        gameObject.SetActive(false);
    }
}