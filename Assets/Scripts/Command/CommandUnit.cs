using System.Collections.Generic;
using UnityEngine;
using ObsidianProtocol.Game.Command;
using ObsidianProtocol.Game.Command.Autonomy;

public class CommandUnit : MonoBehaviour
{
    [SerializeField]
    private List<SelectableUnit> commandedUnits =
        new List<SelectableUnit>();

    private ArchiveCommandBrain archiveBrain;

    public IReadOnlyList<SelectableUnit> CommandedUnits => commandedUnits;

    public void RegisterCommandedUnit(SelectableUnit unit)
    {
        if (unit == null || commandedUnits.Contains(unit))
            return;

        commandedUnits.Add(unit);
    }
private void Awake()
    {
        archiveBrain = GetComponent<ArchiveCommandBrain>();
    }

    public void ReceiveIntent(IntentType intent)
    {
        if (archiveBrain == null)
        {
            Debug.LogError(
                "[COMMAND UNIT] No ArchiveCommandBrain attached to " +
                gameObject.name);

            return;
        }

        archiveBrain.ReceiveIntent(intent);
    }
}
