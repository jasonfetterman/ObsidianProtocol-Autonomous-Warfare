using UnityEngine;

namespace Obsidian.VR
{
    public class VRUnitOperatorLink : MonoBehaviour
    {
        [SerializeField] private VRUnitContext _context;

        private void Awake()
        {
            if (_context == null)
                _context = FindAnyObjectByType<VRUnitContext>();
        }

        private void Update()
        {
            if (_context == null || !_context.Valid)
                return;

            var unit = _context.Unit;
            if (unit == null)
                return;

            // Operator → Unit state sync
            unit.SetOperatorPosture(unit.Posture);
            unit.SetOperatorStance(unit.Stance);
            unit.SetOperatorBreathing(unit.BreathingRate);
            unit.SetOperatorStress(unit.StressLevel);
        }
    }
}
