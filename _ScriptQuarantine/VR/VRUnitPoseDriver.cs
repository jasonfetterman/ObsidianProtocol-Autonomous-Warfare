using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Provides head and body pose data for VR systems.
    /// This is the central source of positional/rotational data
    /// for camera rigs, HUD anchors, and VR movement logic.
    /// </summary>
    public class VRUnitPoseDriver : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;

        // Exposed pose values
        public Vector3 HeadPosition { get; private set; }
        public Quaternion HeadRotation { get; private set; }

        public Vector3 BodyPosition { get; private set; }
        public Quaternion BodyRotation { get; private set; }

        private void Awake()
        {
            if (_session == null)
                _session = Object.FindAnyObjectByType<VRSessionManager>();

            if (_runtime == null)
                _runtime = Object.FindAnyObjectByType<VRRuntimeAdapter>();
        }

        private void Start()
        {
            BindToActiveUnit();
        }

        private void Update()
        {
            if (_session == null || _runtime == null)
                return;

            if (_unit == null)
                BindToActiveUnit();

            if (_unit == null)
                return;

            UpdatePose();
        }

        private void BindToActiveUnit()
        {
            _unit = _session?.ActiveUnit;

            if (_unit == null)
                return;
        }

        private void UpdatePose()
        {
            // Pull head pose from VR runtime adapter
            HeadPosition = _runtime.GetHeadPosition();
            HeadRotation = _runtime.GetHeadRotation();

            // Pull body pose from VR runtime adapter
            BodyPosition = _runtime.GetBodyPosition();
            BodyRotation = _runtime.GetBodyRotation();
        }
    }
}
