using UnityEngine;
using UnityEngine.InputSystem;

public class UnitHoverManager : MonoBehaviour
{
    [SerializeField] private Camera battlefieldCamera;

    private SelectableUnit currentlyHovered;

    private void Update()
    {
        Ray ray = battlefieldCamera.ScreenPointToRay(
            Mouse.current.position.ReadValue());

        SelectableUnit hitUnit = null;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            hitUnit = hit.collider.GetComponent<SelectableUnit>();
        }

        if (hitUnit != currentlyHovered)
        {
            if (currentlyHovered != null)
            {
                currentlyHovered.SetHovered(false);
            }

            if (hitUnit != null)
            {
                hitUnit.SetHovered(true);
            }

            currentlyHovered = hitUnit;
        }
    }
}