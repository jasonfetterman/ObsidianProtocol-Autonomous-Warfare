using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Coordinates lifecycle events across all VR subsystems.
    /// Ensures spawn, death, and revive events propagate correctly.
    /// </summary>
    public class VRUnitLifeCycleManager : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;
        private VRUnitLifeCycle _lifeCycle;
        private VRUnitRespawnHandler _respawnHandler;

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
        }

        private void BindToActiveUnit()
        {
            _unit = _session?.ActiveUnit;

            if (_unit == null)
            {
                _lifeCycle = null;
                _respawnHandler = null;
                return;
            }

            // Bind lifecycle
            _lifeCycle = _unit.GetComponent<VRUnitLifeCycle>();
            if (_lifeCycle == null)
                _lifeCycle = _unit.gameObject.AddComponent<VRUnitLifeCycle>();

            // Bind respawn handler
            _respawnHandler = _unit.GetComponent<VRUnitRespawnHandler>();
            if (_respawnHandler == null)
                _respawnHandler = _unit.gameObject.AddComponent<VRUnitRespawnHandler>();

            // Hook lifecycle events
            _lifeCycle.OnUnitSpawned += HandleSpawn;
            _lifeCycle.OnUnitDied += HandleDeath;
            _lifeCycle.OnUnitRevived += HandleRevive;
        }

        private void HandleSpawn()
        {
            Debug.Log("[VR] Unit spawned.");
        }

        private void HandleDeath()
        {
            Debug.Log("[VR] Unit died.");
        }

        private void HandleRevive()
        {
            Debug.Log("[VR] Unit revived.");
        }
    }
}
