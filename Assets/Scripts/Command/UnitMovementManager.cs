using UnityEngine;
using UnityEngine.InputSystem;

public class UnitMovementManager : MonoBehaviour
{
    [SerializeField] private Camera battlefieldCamera;
    [SerializeField] private UnitSelectionManager selectionManager;

    private void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Ray ray = battlefieldCamera.ScreenPointToRay(
                Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                foreach (SelectableUnit selected in selectionManager.SelectedUnits)
                {
                    UnitMovement mover = selected.GetComponent<UnitMovement>();
                    if (mover != null)
                    {
                        mover.MoveTo(hit.point);
                    }
                }
            }
        }
    }
}