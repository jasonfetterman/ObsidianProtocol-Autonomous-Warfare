using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Syncs unit pose (position + rotation) with VR operator movement.
    /// Used for UGV/UAV/USV/UUV units controlled in VR.
    /// </summary>
    public class VRUnitPoseSync : MonoBehaviour
    {
        [SerializeField] private VRRuntimeAdapter _runtime;
        [SerializeField] private VRSessionManager _session;

        private BaseUnitVRController _unit;

        private void Awake()
        {
            if (_runtime == null)
                _runtime = Object.FindAnyObjectByType<VRRuntimeAdapter>();

            if (_session == null)
                _session = Object.FindAnyObjectByType<VRSessionManager>();
        }

        private void Start()
        {
            _unit = _session?.ActiveUnit;
        }

        private void Update()
        {
            if (_runtime == null || _session == null)
                return;

            if (_unit == null)
                _unit = _session.ActiveUnit;

            if (_unit == null)
                return;

            var pose = _runtime.GetHeadPose();
            if (pose == null)
                return;

            // Sync unit orientation to VR operator
            _unit.transform.rotation = pose.Value.rotation;

            // Optional: position sync if your game uses operator-relative movement
            // Comment out if not needed
            //_unit.transform.position = pose.Value.position;
        }
    }
}
