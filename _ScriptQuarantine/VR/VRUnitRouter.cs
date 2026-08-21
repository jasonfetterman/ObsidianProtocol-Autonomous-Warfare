using UnityEngine;

namespace Obsidian.VR
{
    public class VRUnitRouter : MonoBehaviour
    {
        private VRSessionManager _session;

        private void Awake()
        {
            _session = Object.FindAnyObjectByType<VRSessionManager>();
        }

        public BaseUnitVRController GetActiveUnit()
        {
            if (_session == null)
                return null;

            return _session.ActiveUnit;
        }

        public Camera GetActiveUnitCamera()
        {
            var unit = GetActiveUnit();
            if (unit == null)
                return null;

            return unit.UnitPOVCamera;
        }

        public void ActivateUnitForVR(int unitId, string tag)
        {
            if (_session == null)
                return;

            var units = Object.FindObjectsByType<BaseUnitVRController>();
            foreach (var u in units)
            {
                if (u.UnitId == unitId)
                {
                    _session.SetActiveUnit(u);
                    u.enabled = true;
                    return;
                }
            }
        }

        public void DeactivateAllUnits()
        {
            var units = Object.FindObjectsByType<BaseUnitVRController>();
            foreach (var u in units)
                u.enabled = false;
        }
    }
}
