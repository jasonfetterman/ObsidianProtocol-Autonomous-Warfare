using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionManager : MonoBehaviour
{
    [SerializeField]
    private Camera battlefieldCamera;

    private List<SelectableUnit> selectedUnits = new List<SelectableUnit>();

    public IReadOnlyList<SelectableUnit> SelectedUnits => selectedUnits;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TrySingleSelect();
        }
    }

    private void TrySingleSelect()
    {
        Ray ray = battlefieldCamera.ScreenPointToRay(
            Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            SelectableUnit unit =
                hit.collider.GetComponent<SelectableUnit>();

            if (unit != null)
            {
                ClearSelection();
                AddToSelection(unit);
                return;
            }
        }

        ClearSelection();
    }

    public void ClearSelection()
    {
        foreach (SelectableUnit unit in selectedUnits)
        {
            unit.Deselect();
        }
        selectedUnits.Clear();
    }

    public void AddToSelection(SelectableUnit unit)
    {
        if (!selectedUnits.Contains(unit))
        {
            selectedUnits.Add(unit);
            unit.Select();
        }
    }
}