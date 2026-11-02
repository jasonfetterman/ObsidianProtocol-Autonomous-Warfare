using UnityEngine;
using UnityEngine.UI;

public class DeploymentOperationSelectionController : MonoBehaviour
{
    [SerializeField] private Button selectOperationButton;
    [SerializeField] private Text operationDisplay;

    private readonly string[] operations =
    {
        "BLACK HORIZON",
        "IRON VEIL",
        "NIGHT VECTOR",
        "SILENT CURRENT"
    };

    private int selectedOperationIndex = 0;

    public string SelectedOperation
    {
        get
        {
            return operations[selectedOperationIndex];
        }
    }

    private void Awake()
    {
        if (selectOperationButton == null)
        {
            Debug.LogError(
                "[OPERATION SELECTION] SELECT OPERATION BUTTON REFERENCE MISSING."
            );
            return;
        }

        if (operationDisplay == null)
        {
            Debug.LogError(
                "[OPERATION SELECTION] OPERATION DISPLAY REFERENCE MISSING."
            );
            return;
        }

        selectOperationButton.onClick.AddListener(
            CycleOperation
        );

        UpdateOperationDisplay();

        Debug.Log(
            "[OPERATION SELECTION] CONTROLLER INITIALIZED // " +
            SelectedOperation
        );
    }

    private void CycleOperation()
    {
        selectedOperationIndex++;

        if (selectedOperationIndex >= operations.Length)
            selectedOperationIndex = 0;

        UpdateOperationDisplay();

        Debug.Log(
            "[OPERATION SELECTION] SELECTED = " +
            SelectedOperation
        );
    }

    private void UpdateOperationDisplay()
    {
        operationDisplay.text =
            "OPERATION // " +
            SelectedOperation;
    }

    public string GetSelectedOperation()
    {
        return SelectedOperation;
    }
}
