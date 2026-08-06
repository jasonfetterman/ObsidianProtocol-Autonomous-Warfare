using UnityEngine;

namespace Obsidian.VR
{
    public class VRUnitDeathHandler : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private void Awake()
        {
            if (_session == null)
                _session = Object.FindAnyObjectByType<VRSessionManager>();

            if (_runtime == null)
                _runtime = Object.FindAnyObjectByType<VRRuntimeAdapter>();
        }

        public void OnUnitDeath(BaseUnitVRController controller)
        {
            if (controller == null)
                return;

            if (_session == null || _runtime == null)
                return;

            if (_session.StateData.ActiveUnitId == controller.UnitId)
            {
                _session.SetActiveUnit(-1, string.Empty);
                _runtime.SetActiveCamera(null);
            }

            controller.DeactivateVRControl();
        }
    }
}
