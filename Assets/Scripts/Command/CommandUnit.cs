using System.Collections.Generic;
using UnityEngine;

public class CommandUnit : MonoBehaviour
{
    [SerializeField] private List<SelectableUnit> commandedUnits = new List<SelectableUnit>();

    public IReadOnlyList<SelectableUnit> CommandedUnits => commandedUnits;
}