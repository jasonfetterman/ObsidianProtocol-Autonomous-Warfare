using UnityEngine;

public class Builder : MonoBehaviour
{
    BuildingConstruction target;

    public void SetTarget(BuildingConstruction bc)
    {
        target = bc;
        // You can expand this later with your building logic
    }
}
