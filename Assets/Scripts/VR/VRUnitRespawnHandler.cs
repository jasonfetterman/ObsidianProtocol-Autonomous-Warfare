using UnityEngine;

namespace Obsidian.VR
{
    public class VRUnitRespawnHandler : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;

        private VRUnitPoseDriver _poseDriver;
        private VRUnitPoseRouter _poseRouter;
        private VRUnitPoseSync _poseSync;
        private VRUnitPowerState _powerState;

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
            if (_session == null)
                return;

            if (_unit == null || !_unit.IsAlive())
                BindToActiveUnit();
        }

        private void BindToActiveUnit()
        {
            _unit = _session?.ActiveUnit;

            if (_unit == null)
            {
                ClearBindings();
                return;
            }

            _poseDriver = _unit.GetComponent<VRUnitPoseDriver>();
            if (_poseDriver == null)
                _poseDriver = _unit.gameObject.AddComponent<VRUnitPoseDriver>();

            _poseRouter = _unit.GetComponent<VRUnitPoseRouter>();
            if (_poseRouter == null)
                _poseRouter = _unit.gameObject.AddComponent<VRUnitPoseRouter>();

            _poseSync = _unit.GetComponent<VRUnitPoseSync>();
            if (_poseSync == null)
                _poseSync = _unit.gameObject.AddComponent<VRUnitPoseSync>();

            _powerState = _unit.GetComponent<VRUnitPowerState>();
            if (_powerState == null)
                _powerState = _unit.gameObject.AddComponent<VRUnitPowerState>();
        }

        private void ClearBindings()
        {
            _poseDriver = null;
            _poseRouter = null;
            _poseSync = null;
            _powerState = null;
        }
    }
}
