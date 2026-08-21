using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Binds the VR camera rig to the active unit and ensures
    /// the rig updates its anchors based on the unit's pose driver.
    /// </summary>
    public class VRUnitCameraRigBinder : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;
        private VRUnitCameraRig _rig;
        private VRUnitPoseDriver _poseDriver;

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

            if (_unit == null || _rig == null || _poseDriver == null)
                return;

            UpdateRigBinding();
        }

        private void BindToActiveUnit()
        {
            _unit = _session?.ActiveUnit;

            if (_unit == null)
            {
                _rig = null;
                _poseDriver = null;
                return;
            }

            _rig = _unit.GetComponent<VRUnitCameraRig>();
            if (_rig == null)
                _rig = _unit.gameObject.AddComponent<VRUnitCameraRig>();

            _poseDriver = _unit.GetComponent<VRUnitPoseDriver>();
            if (_poseDriver == null)
                _poseDriver = _unit.gameObject.AddComponent<VRUnitPoseDriver>();
        }

        private void UpdateRigBinding()
        {
            Transform rigRoot = _rig.transform;

            rigRoot.position = _unit.transform.position;
            rigRoot.rotation = _unit.transform.rotation;

            Transform headAnchor = _rig.GetHeadAnchor();
            Transform bodyAnchor = _rig.GetBodyAnchor();

            if (headAnchor != null)
            {
                headAnchor.position = _poseDriver.HeadPosition + _rig.headOffset;
                headAnchor.rotation = _poseDriver.HeadRotation;
            }

            if (bodyAnchor != null)
            {
                bodyAnchor.position = _poseDriver.BodyPosition + _rig.bodyOffset;
                bodyAnchor.rotation = _poseDriver.BodyRotation;
            }
        }
    }
}
