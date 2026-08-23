using UnityEngine;

public class HUDSingleUnitSelection : MonoBehaviour
{
    public GameObject SelectedUnit { get; private set; }

    public bool HasSelection => SelectedUnit != null;

    public void Select(GameObject unit)
    {
        SelectedUnit = unit;
    }

    public void Clear()
    {
        SelectedUnit = null;
    }
}
