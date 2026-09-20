using UnityEngine;
using ObsidianProtocol.Game.Command;

public class CommandIntentManager : MonoBehaviour
{
    [SerializeField]
    private UnitSelectionManager selectionManager;

    public void IssueIntent(IntentType type)
    {
        Debug.Log("[PLAYER] Intent issued: " + type);

        if (selectionManager == null)
        {
            Debug.LogError(
                "[PLAYER] CommandIntentManager has no Selection Manager.");

            return;
        }

        CommandUnit selectedCommandUnit = null;

        foreach (SelectableUnit unit in selectionManager.SelectedUnits)
        {
            if (unit == null)
                continue;

            CommandUnit commandUnit =
                unit.GetComponent<CommandUnit>();

            if (commandUnit != null)
            {
                selectedCommandUnit = commandUnit;
                break;
            }
        }

        if (selectedCommandUnit == null)
        {
            Debug.LogWarning(
                "[PLAYER] No Command Unit selected. " +
                "Select ARCHIVE before issuing an intent.");

            return;
        }

        Debug.Log(
            "[PLAYER] Sending intent " +
            type +
            " to " +
            selectedCommandUnit.gameObject.name);

        selectedCommandUnit.ReceiveIntent(type);
    }
}
