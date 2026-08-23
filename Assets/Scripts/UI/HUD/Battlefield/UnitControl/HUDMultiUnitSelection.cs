using System.Collections.Generic;
using UnityEngine;

public class HUDMultiUnitSelection : MonoBehaviour
{
    private readonly List<GameObject> selectedUnits = new();

    public IReadOnlyList<GameObject> SelectedUnits => selectedUnits;
    public int SelectionCount => selectedUnits.Count;

    public void SetSelection(List<GameObject> units)
    {
        selectedUnits.Clear();

        if (units != null)
            selectedUnits.AddRange(units);
    }

    public void AddUnit(GameObject unit)
    {
        if (unit != null && !selectedUnits.Contains(unit))
            selectedUnits.Add(unit);
    }

    public void RemoveUnit(GameObject unit)
    {
        selectedUnits.Remove(unit);
    }

    public void Clear()
    {
        selectedUnits.Clear();
    }
}
