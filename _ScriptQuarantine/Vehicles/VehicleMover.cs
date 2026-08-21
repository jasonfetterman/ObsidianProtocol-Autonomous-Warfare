using UnityEngine;
using Obsidian.Vehicles;

public class VehicleMover : MonoBehaviour
{
    private VehicleMoveToPoint movement;

    private void Awake()
    {
        movement = GetComponent<VehicleMoveToPoint>();
    }

    public void MoveTo(Vector3 destination)
    {
        if (movement == null)
            return;

        movement.MoveTo(destination);
    }
}