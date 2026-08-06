using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Routes VR head pose data to the active unit's pose driver.
    /// Ensures the correct unit receives head tracking updates.
    /// </summary>
    public class VRUnitPoseRouter : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;
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
            RefreshUnitBinding();
        }

        private void Update()
        {
            if (_session == null || _runtime == null)
                return;

            if (_unit == null || _poseDriver == null)
                RefreshUnitBinding();
        }

        private void RefreshUnitBinding()
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
    }
}
