using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Binds newly spawned units to the VR operator session.
    /// Ensures VR systems attach immediately after spawn.
    /// </summary>
    public class VRUnitSpawnBinder : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;

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

            if (_unit == null)
                BindToActiveUnit();
        }

        private void BindToActiveUnit()
        {
            _unit = _session?.ActiveUnit;

            if (_unit == null)
                return;

            // Attach pose driver
            var poseDriver = _unit.GetComponent<VRUnitPoseDriver>();
            if (poseDriver == null)
                poseDriver = _unit.gameObject.AddComponent<VRUnitPoseDriver>();

            // Attach pose router
            var poseRouter = _unit.GetComponent<VRUnitPoseRouter>();
            if (poseRouter == null)
                poseRouter = _unit.gameObject.AddComponent<VRUnitPoseRouter>();

            // Attach pose sync
            var poseSync = _unit.GetComponent<VRUnitPoseSync>();
            if (poseSync == null)
                poseSync = _unit.gameObject.AddComponent<VRUnitPoseSync>();

            // Attach power state
            var powerState = _unit.GetComponent<VRUnitPowerState>();
            if (powerState == null)
                powerState = _unit.gameObject.AddComponent<VRUnitPowerState>();

            // Attach respawn handler
            var respawnHandler = _unit.GetComponent<VRUnitRespawnHandler>();
            if (respawnHandler == null)
                respawnHandler = _unit.gameObject.AddComponent<VRUnitRespawnHandler>();
        }
    }
}
