using UnityEngine;
using System.Collections.Generic;

public class FormationManager : MonoBehaviour
{
    public float spacing = 1.5f;

    public void MoveUnits(List<UnitMover> units, Vector3 target)
    {
        int count = units.Count;
        int rows = Mathf.CeilToInt(Mathf.Sqrt(count));
        int cols = rows;

        int index = 0;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (index >= count) return;

                Vector3 offset = new Vector3(r * spacing, 0, c * spacing);
                units[index].MoveTo(target + offset);

                index++;
            }
        }
    }
}
