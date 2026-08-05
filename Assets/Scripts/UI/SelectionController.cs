using UnityEngine;
using System.Collections.Generic;

public class SelectionController : MonoBehaviour
{
    public List<UnitMover> selectedUnits = new();
    FormationManager formation;

    void Awake()
    {
        formation = new FormationManager();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                formation.MoveUnits(selectedUnits, hit.point);
            }
        }
    }

    public void Select(UnitMover mover)
    {
        if (!selectedUnits.Contains(mover))
        {
            selectedUnits.Add(mover);

            UnitSelectable sel = mover.GetComponent<UnitSelectable>();
            if (sel != null)
                sel.SetSelected(true);
        }
    }

    public void DeselectAll()
    {
        foreach (var m in selectedUnits)
        {
            if (m == null) continue;
            UnitSelectable sel = m.GetComponent<UnitSelectable>();
            if (sel != null)
                sel.SetSelected(false);
        }

        selectedUnits.Clear();
    }
}
