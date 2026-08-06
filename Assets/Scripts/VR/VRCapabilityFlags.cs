namespace Obsidian.VR
{
    /// <summary>
    /// Declares whether a unit supports VR features.
    /// Ensures VR remains optional and never required for gameplay.
    /// </summary>
    public enum VRCapability
    {
        None,           // Unit has no VR features
        BasicFPV,       // Unit supports FPV camera only
        FullControl,    // Unit supports FPV + VR input + pose + HUD
        Custom          // Unit provides its own VR integration
    }

    /// <summary>
    /// Attach to any unit prefab to declare VR support level.
    /// VR systems will automatically ignore units with None.
    /// </summary>
    public class VRCapabilityFlags : UnityEngine.MonoBehaviour
    {
        public VRCapability Capability = VRCapability.None;
    }
}

