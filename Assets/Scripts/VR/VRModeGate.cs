using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Centralized helper for gating VR logic.
    /// Ensures VR is optional and never interferes with non‑VR gameplay.
    /// </summary>
    public static class VRModeGate
    {
        public static bool VRReady(VRSessionManager session, VRRuntimeAdapter runtime)
        {
            if (session == null || runtime == null)
                return false;

            // VR is active ONLY when VRState.Active
            if (session.State != VRSessionManager.VRState.Active)
                return false;

            if (session.Info.Mode != VRMode.Operator)
                return false;

            return true;
        }

        public static bool UnitSupportsVR(BaseUnitVRController unit)
        {
            if (unit == null)
                return false;

            var flags = unit.GetComponent<VRCapabilityFlags>();
            if (flags == null)
                return false;

            return flags.Capability != VRCapability.None;
        }

        public static bool CanRunVR(BaseUnitVRController unit, VRSessionManager session, VRRuntimeAdapter runtime)
        {
            if (!VRReady(session, runtime))
                return false;

            if (!UnitSupportsVR(unit))
                return false;

            return true;
        }
    }
}
