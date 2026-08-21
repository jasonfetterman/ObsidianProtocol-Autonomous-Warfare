using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Binds the VR operator's camera to the active unit.
    /// Ensures the VR camera follows the unit's head/pose driver.
    /// </summary>
    public class VRUnitCameraBinder : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;
        private VRUnitPoseDriver _poseDriver;

        private Camera _vrCamera;

        private void Awake()
        {
            if (_session == null)
                _session = Object.FindAnyObjectByType<VRSessionManager>();

            if (_runtime == null)
                _runtime = Object.FindAnyObjectByType<VRRuntimeAdapter>();

            _vrCamera = Camera.main;
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

            if (_unit == null || _poseDriver == null || _vrCamera == null)
                return;

            FollowPoseDriver();
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

        private void FollowPoseDriver()
        {
            _vrCamera.transform.position = _poseDriver.HeadPosition;
            _vrCamera.transform.rotation = _poseDriver.HeadRotation;
        }
    }
}
