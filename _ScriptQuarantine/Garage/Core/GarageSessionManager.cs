using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class GarageSessionManager : MonoBehaviour
    {
        [Header("Session")]
        [SerializeField]
        private GarageSessionState session =
            new GarageSessionState();

        public GarageSessionState Session => session;

        public event Action SessionChanged;

        public void SetActiveUnit(
            string instanceId,
            string definitionId)
        {
            session.activeUnitInstanceId = instanceId;
            session.activeUnitDefinitionId = definitionId;

            MarkDirty();
        }

        public void SetWorld(
            string worldId,
            string regionId)
        {
            session.worldId = worldId;
            session.regionId = regionId;

            MarkDirty();
        }

        public void SetOperatingMode(
            GarageOperatingMode mode)
        {
            session.operatingMode = mode;
            MarkDirty();
        }

        public void SetSessionMode(
            GarageSessionMode mode)
        {
            session.sessionMode = mode;
            MarkDirty();
        }

        public void EnterGarage()
        {
            session.inGarage = true;
            session.deployed = false;

            MarkDirty();
        }

        public void Deploy()
        {
            session.inGarage = false;
            session.deployed = true;

            MarkDirty();
        }

        public void Pause(bool value)
        {
            session.paused = value;

            MarkDirty();
        }

        public void Spectate(bool value)
        {
            session.spectating = value;

            MarkDirty();
        }

        public void MarkDirty()
        {
            session.dirty = true;
            SessionChanged?.Invoke();
        }

        public void MarkSaved()
        {
            session.dirty = false;
            session.lastSaveTimestamp =
                DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            SessionChanged?.Invoke();
        }
    }
}
