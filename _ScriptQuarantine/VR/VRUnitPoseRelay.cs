using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Relays VR head/body pose from the runtime adapter to the unit.
    /// This ensures the unit's head and body follow the VR operator.
    /// </summary>
    public class VRUnitPoseRelay : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;
        private VRUnitPoseDriver _poseDriver;

        [Header("Pose Targets")]
        public Transform headTarget;
        public Transform bodyTarget;

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

        private void LateUpdate()
        {
            if (_session == null || _runtime == null)
                return;

            if (_unit == null)
                BindToActiveUnit();

            if (_unit == null || _poseDriver == null)
                return;

            ApplyPose();
        }

        private void BindToActiveUnit()
        {
            _unit = _session?.ActiveUnit;

            if (_unit == null)
            {
                _poseDriver = null;
                return;
            }

            _poseDriver = _unit.GetComponent<VRUnitPoseDriver>();
            if (_poseDriver == null)
                _poseDriver = _unit.gameObject.AddComponent<VRUnitPoseDriver>();

            if (headTarget == null)
                headTarget = _unit.transform.Find("Head");

            if (bodyTarget == null)
                bodyTarget = _unit.transform.Find("Body");
        }

        private void ApplyPose()
        {
            if (headTarget != null)
            {
                headTarget.position = _poseDriver.HeadPosition;
                headTarget.rotation = _poseDriver.HeadRotation;
            }

            if (bodyTarget != null)
            {
                bodyTarget.position = _poseDriver.BodyPosition;
                bodyTarget.rotation = _poseDriver.BodyRotation;
            }
        }
    }
}
