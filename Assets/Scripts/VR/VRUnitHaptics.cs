using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Sends haptic impulses to VR controllers based on unit events.
    /// Uses VRUnitContext for gating.
    /// </summary>
    public class VRUnitHaptics : MonoBehaviour
    {
        [SerializeField] private VRUnitContextProvider _contextProvider;

        private VRUnitContext _context;

        private void Awake()
        {
            if (_contextProvider == null)
                _contextProvider = GetComponent<VRUnitContextProvider>();

            if (_contextProvider == null)
                _contextProvider = Object.FindAnyObjectByType<VRUnitContextProvider>();
        }

        private void Update()
        {
            if (_contextProvider == null)
                return;

            _context = _contextProvider.Context;
            if (_context == null || !_context.Valid)
                return;

            TickEngineHaptics();
        }

        private void TickEngineHaptics()
        {
            var unit = _context.Unit;
            if (unit == null)
                return;

            var runtime = _context.Runtime;
            if (runtime == null || runtime.Haptics == null)
                return;

            float throttle = unit.GetThrottleLevel();
            float intensity = Mathf.Clamp01(throttle);

            runtime.Haptics.Send(VRControllerHand.Left, intensity, 0.02f);
            runtime.Haptics.Send(VRControllerHand.Right, intensity, 0.02f);
        }

        public void ImpactPulse(float strength)
        {
            if (_context == null || !_context.Valid)
                return;

            var runtime = _context.Runtime;
            if (runtime == null || runtime.Haptics == null)
                return;

            float s = Mathf.Clamp01(strength);

            runtime.Haptics.Send(VRControllerHand.Left, s, 0.05f);
            runtime.Haptics.Send(VRControllerHand.Right, s, 0.05f);
        }
    }
}
