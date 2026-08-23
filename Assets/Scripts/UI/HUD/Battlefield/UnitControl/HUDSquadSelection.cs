using UnityEngine;

public class HUDSquadSelection : MonoBehaviour
{
    public GameObject SelectedSquad { get; private set; }
    public bool HasSelection => SelectedSquad != null;

    public void SelectSquad(GameObject squad)
    {
        SelectedSquad = squad;
    }

    public void Clear()
    {
        SelectedSquad = null;
    }
}
