using UnityEngine;
using System.Collections.Generic;

public class SiegeCoordinator : MonoBehaviour
{
    public List<SiegeAI> siegeUnits = new();

    public void CommandSiege(GameObject targetBuilding)
    {
        Vector3 breachPoint = CalculateGroupBreachPoint(targetBuilding);

        foreach (var s in siegeUnits)
        {
            if (s == null) continue;

            UnitMover mover = s.GetComponent<UnitMover>();
            if (mover != null)
                mover.MoveTo(breachPoint);

            s.SetTarget(targetBuilding);
        }
    }

    Vector3 CalculateGroupBreachPoint(GameObject building)
    {
        return building.transform.position + new Vector3(2f, 0, -2f);
    }
}
