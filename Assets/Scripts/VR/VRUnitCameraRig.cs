using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Manages the VR camera rig attached to the active unit.
    /// Ensures head/body offsets are applied correctly and updated every frame.
    /// </summary>
    public class VRUnitCameraRig : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;
        private VRUnitPoseDriver _poseDriver;

        [Header("Rig Offsets")]
        public Vector3 headOffset = new Vector3(0f, 1.6f, 0f);
        public Vector3 bodyOffset = new Vector3(0f, 1.0f, 0f);

        private Transform _rigRoot;
        private Transform _headAnchor;
        private Transform _bodyAnchor;

        private void Awake()
        {
            if (_session == null)
                _session = Object.FindAnyObjectByType<VRSessionManager>();

            if (_runtime == null)
                _runtime = Object.FindAnyObjectByType<VRRuntimeAdapter>();

            InitializeRig();
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

            UpdateRig();
        }

        private void InitializeRig()
        {
            _rigRoot = new GameObject("VRUnitCameraRigRoot").transform;
            _rigRoot.SetParent(transform, false);

            _headAnchor = new GameObject("VRHeadAnchor").transform;
            _headAnchor.SetParent(_rigRoot, false);

            _bodyAnchor = new GameObject("VRBodyAnchor").transform;
            _bodyAnchor.SetParent(_rigRoot, false);
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
        }

        private void UpdateRig()
        {
            _headAnchor.position = _poseDriver.HeadPosition + headOffset;
            _headAnchor.rotation = _poseDriver.HeadRotation;

            _bodyAnchor.position = _poseDriver.BodyPosition + bodyOffset;
            _bodyAnchor.rotation = _poseDriver.BodyRotation;

            _rigRoot.position = _unit.transform.position;
            _rigRoot.rotation = _unit.transform.rotation;
        }

        public Transform GetHeadAnchor() => _headAnchor;
        public Transform GetBodyAnchor() => _bodyAnchor;
    }
}
