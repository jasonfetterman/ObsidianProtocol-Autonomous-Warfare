using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Deployment
{
    public enum DeploymentZoneType
    {
        Player,
        Allied,
        Neutral,
        Restricted,
        Enemy
    }

    public sealed class DeploymentZone
    {
        public string ZoneId { get; }
        public DeploymentZoneType Type { get; }
        public bool Enabled { get; private set; }

        private readonly HashSet<string> allowedUnitIds =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public DeploymentZone(
            string zoneId,
            DeploymentZoneType type)
        {
            ZoneId = zoneId ?? string.Empty;
            Type = type;
            Enabled = true;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(ZoneId);

        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }

        public bool AllowUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
                return false;

            return allowedUnitIds.Add(unitId);
        }

        public bool RemoveUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
                return false;

            return allowedUnitIds.Remove(unitId);
        }

        public bool CanDeployUnit(string unitId)
        {
            if (!Enabled ||
                string.IsNullOrWhiteSpace(unitId))
                return false;

            if (allowedUnitIds.Count == 0)
                return true;

            return allowedUnitIds.Contains(unitId);
        }

        public IReadOnlyCollection<string> GetAllowedUnits()
        {
            return allowedUnitIds;
        }
    }

    public sealed class DeploymentZoneSystem
    {
        private readonly Dictionary<string, DeploymentZone> zones =
            new Dictionary<string, DeploymentZone>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterZone(DeploymentZone zone)
        {
            if (zone == null ||
                !zone.Valid ||
                zones.ContainsKey(zone.ZoneId))
                return false;

            zones.Add(zone.ZoneId, zone);
            return true;
        }

        public bool RemoveZone(string zoneId)
        {
            if (string.IsNullOrWhiteSpace(zoneId))
                return false;

            return zones.Remove(zoneId);
        }

        public bool TryGetZone(
            string zoneId,
            out DeploymentZone zone)
        {
            return zones.TryGetValue(zoneId, out zone);
        }

        public bool EnableZone(string zoneId)
        {
            if (!zones.TryGetValue(
                    zoneId,
                    out DeploymentZone zone))
                return false;

            zone.Enable();
            return true;
        }

        public bool DisableZone(string zoneId)
        {
            if (!zones.TryGetValue(
                    zoneId,
                    out DeploymentZone zone))
                return false;

            zone.Disable();
            return true;
        }

        public bool CanDeploy(
            string zoneId,
            string unitId)
        {
            if (!zones.TryGetValue(
                    zoneId,
                    out DeploymentZone zone))
                return false;

            return zone.CanDeployUnit(unitId);
        }

        public IReadOnlyCollection<DeploymentZone> GetZones()
        {
            return zones.Values;
        }

        public void Clear()
        {
            zones.Clear();
        }
    }
}
