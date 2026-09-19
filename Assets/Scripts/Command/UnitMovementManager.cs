using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitMovementManager : MonoBehaviour
{
    [SerializeField] private Camera battlefieldCamera;
    [SerializeField] private UnitSelectionManager selectionManager;
    [SerializeField] private float formationSpacing = 4f;

    private void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Ray ray = battlefieldCamera.ScreenPointToRay(
                Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                IReadOnlyList<SelectableUnit> selected = selectionManager.SelectedUnits;

                if (selected.Count == 0)
                {
                    return;
                }

                List<SelectableUnit> unitsToMove = new List<SelectableUnit>();

                foreach (SelectableUnit unit in selected)
                {
                    CommandUnit commandUnit = unit.GetComponent<CommandUnit>();

                    if (commandUnit != null)
                    {
                        unitsToMove.AddRange(commandUnit.CommandedUnits);
                    }
                    else
                    {
                        unitsToMove.Add(unit);
                    }
                }

                if (unitsToMove.Count == 1)
                {
                    MoveUnit(unitsToMove[0], hit.point);
                }
                else if (unitsToMove.Count > 1)
                {
                    MoveGroupInFormation(unitsToMove, hit.point);
                }
            }
        }
    }

    private void MoveUnit(SelectableUnit unit, Vector3 destination)
    {
        UnitMovement mover = unit.GetComponent<UnitMovement>();
        if (mover != null)
        {
            mover.MoveTo(destination);
        }
    }

    private void MoveGroupInFormation(List<SelectableUnit> units, Vector3 center)
    {
        int count = units.Count;
        int columns = Mathf.CeilToInt(Mathf.Sqrt(count));

        for (int i = 0; i < count; i++)
        {
            int row = i / columns;
            int col = i % columns;

            float offsetX = (col - (columns - 1) / 2f) * formationSpacing;
            float offsetZ = (row - (count / columns) / 2f) * formationSpacing;

            Vector3 destination = center + new Vector3(offsetX, 0f, offsetZ);

            MoveUnit(units[i], destination);
        }
    }
}