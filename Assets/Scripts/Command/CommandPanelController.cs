using UnityEngine;

public class CommandPanelController : MonoBehaviour
{
    [SerializeField] private UnitSelectionManager selectionManager;
    [SerializeField] private HUDUnitCommandPanel commandPanel;

    private void Update()
    {
        bool commandUnitSelected = false;

        foreach (SelectableUnit unit in selectionManager.SelectedUnits)
        {
            if (unit.GetComponent<CommandUnit>() != null)
            {
                commandUnitSelected = true;
                break;
            }
        }

        if (commandUnitSelected && !commandPanel.IsOpen)
        {
            commandPanel.Open();
        }
        else if (!commandUnitSelected && commandPanel.IsOpen)
        {
            commandPanel.Close();
        }
    }
}