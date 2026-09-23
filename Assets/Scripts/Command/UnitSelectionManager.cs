using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class UnitSelectionManager : MonoBehaviour
{
    [SerializeField]
    private Camera battlefieldCamera;

    private List<SelectableUnit> selectedUnits =
        new List<SelectableUnit>();

    public IReadOnlyList<SelectableUnit> SelectedUnits =>
        selectedUnits;

    private void Update()
    {
        if (Mouse.current == null)
            return;

        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        // UI clicks must never clear battlefield selection.
        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        TrySingleSelect();
    }

    private void TrySingleSelect()
    {
        if (battlefieldCamera == null)
            return;

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
            if (unit != null)
                unit.Deselect();
        }

        selectedUnits.Clear();
    }

    public void AddToSelection(SelectableUnit unit)
    {
        if (unit == null)
            return;

        if (!selectedUnits.Contains(unit))
        {
            selectedUnits.Add(unit);
            unit.Select();
        }
    }
}
