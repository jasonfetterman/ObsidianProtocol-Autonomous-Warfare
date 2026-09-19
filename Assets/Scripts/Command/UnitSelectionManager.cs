using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionManager : MonoBehaviour
{
    [SerializeField]
    private Camera battlefieldCamera;

    public SelectableUnit SelectedUnit { get; private set; }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            SelectUnit();
        }
    }

    private void SelectUnit()
    {
        Ray ray = battlefieldCamera.ScreenPointToRay(
            Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            SelectableUnit unit =
                hit.collider.GetComponent<SelectableUnit>();

            if (unit != null)
            {
                if (SelectedUnit != null)
                {
                    SelectedUnit.Deselect();
                }

                SelectedUnit = unit;
                SelectedUnit.Select();
                return;
            }
        }

        if (SelectedUnit != null)
        {
            SelectedUnit.Deselect();
            SelectedUnit = null;
        }
    }
}