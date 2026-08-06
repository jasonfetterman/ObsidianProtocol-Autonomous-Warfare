using UnityEngine;
using Obsidian.VR;   // ⭐ REQUIRED — fixes your UnitMover error

namespace Obsidian.Squad
{
    public class FormationManager : MonoBehaviour
    {
        [SerializeField] private UnitMover[] units;

        public void SetFormation(Vector3[] positions)
        {
            if (units == null || positions == null) return;

            int count = Mathf.Min(units.Length, positions.Length);

            for (int i = 0; i < count; i++)
            {
                UnitMover mover = units[i];
                if (mover != null)
                    mover.SetMoveInput(positions[i] - mover.transform.position);
            }
        }
    }
}
