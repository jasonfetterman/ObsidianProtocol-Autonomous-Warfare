using UnityEngine;

public class DroneCockpit : MonoBehaviour
{
    [SerializeField] private Transform cockpitPoint;

    public void EnterCockpit()
    {
        if (cockpitPoint == null)
        {
            Debug.LogWarning("CockpitPoint is not assigned.");
            return;
        }

        Debug.Log("Enter cockpit");
    }
}