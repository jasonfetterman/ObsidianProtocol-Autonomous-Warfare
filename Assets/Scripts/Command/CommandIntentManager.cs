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

        foreach (SelectableUnit unit in selectionManager.SelectedUnits)
        {
            if (unit == null)
                continue;

            CommandUnit commandUnit =
                unit.GetComponent<CommandUnit>();

            if (commandUnit != null)
            {
                Debug.Log(
                    "[PLAYER] Sending intent " +
                    type +
                    " to ARCHIVE command unit " +
                    commandUnit.gameObject.name);

                commandUnit.ReceiveIntent(type);
                return;
            }

            WardenCommandReceiver wardenReceiver =
                unit.GetComponent<WardenCommandReceiver>();

            if (wardenReceiver != null)
            {
                Debug.Log(
                    "[PLAYER] Sending intent " +
                    type +
                    " to WARDEN " +
                    wardenReceiver.gameObject.name);

                wardenReceiver.ReceiveIntent(type);
                return;
            }
        }

        Debug.LogWarning(
            "[PLAYER] No commandable unit selected. " +
            "Select ARCHIVE or a deployed WARDEN.");
    }
}
