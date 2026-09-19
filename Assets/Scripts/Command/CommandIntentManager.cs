using UnityEngine;
using ObsidianProtocol.Game.Command;

public class CommandIntentManager : MonoBehaviour
{
    [SerializeField]
    private UnitSelectionManager selectionManager;

    public void IssueIntent(IntentType type)
    {
        if (selectionManager == null)
        {
            Debug.LogWarning("CommandIntentManager: UnitSelectionManager is not assigned.");
            return;
        }

        bool issued = false;

        foreach (SelectableUnit unit in selectionManager.SelectedUnits)
        {
            if (unit == null)
                continue;

            SquadIntentController controller =
                unit.GetComponent<SquadIntentController>();

            if (controller == null)
                controller =
                    unit.GetComponentInChildren<SquadIntentController>();

            if (controller == null)
                continue;

            Intent intent = new Intent(
                type,
                unit.transform.position
            );

            controller.SetIntent(intent);

            Debug.Log(
                $"Intent issued: {type} -> {unit.gameObject.name}"
            );

            issued = true;
        }

        if (!issued)
        {
            Debug.Log(
                "No selected unit with SquadIntentController found. Cannot issue intent."
            );
        }
    }
}
