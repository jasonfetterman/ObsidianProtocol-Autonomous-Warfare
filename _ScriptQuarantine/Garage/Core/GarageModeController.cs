using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class GarageModeController : MonoBehaviour
    {
        [Header("Current State")]
        [SerializeField]
        private GarageModeState state = new GarageModeState();

        public GarageOperatingMode OperatingMode =>
            state.operatingMode;

        public GarageSessionMode SessionMode =>
            state.sessionMode;

        public GarageModeState State =>
            state;

        public bool SetOperatingMode(
            GarageOperatingMode mode)
        {
            state.operatingMode = mode;
            return true;
        }

        public bool SetSessionMode(
            GarageSessionMode mode)
        {
            state.sessionMode = mode;
            return true;
        }

        public void EnterGarage()
        {
            state.isInGarage = true;
            state.isDeployed = false;
        }

        public void ExitGarage()
        {
            state.isInGarage = false;
        }

        public void Deploy()
        {
            state.isDeployed = true;
            state.isInGarage = false;
        }

        public void Recall()
        {
            state.isDeployed = false;
        }

        public void SetPaused(bool paused)
        {
            state.isPaused = paused;
        }

        public void SetSpectating(bool spectating)
        {
            state.isSpectating = spectating;
        }
    }
}
