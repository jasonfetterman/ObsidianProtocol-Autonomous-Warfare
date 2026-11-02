using System;
using System.Collections.Generic;
using ObsidianProtocol.Game.Units;

namespace ObsidianProtocol.Game.Units.Warden
{
    public sealed class WardenUnitDefinition
    {
        public string UnitId { get; }
        public string DisplayName { get; }
        public UnitCategory Category { get; }

        public WardenUnitDefinition(
            string unitId,
            string displayName,
            UnitCategory category)
        {
            UnitId = unitId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            Category = category;
        }
    }

    public sealed class WardenUnitRoster
    {
        private readonly Dictionary<string, WardenUnitDefinition> units =
            new Dictionary<string, WardenUnitDefinition>(
                StringComparer.OrdinalIgnoreCase);

        public WardenUnitRoster()
        {
            RegisterUnits();
        }

        private void RegisterUnits()
        {
            Register("obs-air-warden", "Warden", UnitCategory.AirUnit);
            Register("obs-air-beacon", "Beacon", UnitCategory.AirUnit);
            Register("obs-air-drop", "Drop", UnitCategory.AirUnit);
            Register("obs-air-iris", "Iris", UnitCategory.AirUnit);
            Register("obs-air-sentinel", "Sentinel", UnitCategory.AirUnit);
            Register("obs-air-scout-eye", "ScoutEye", UnitCategory.AirUnit);
            Register("obs-air-inspect", "Inspect", UnitCategory.AirUnit);
            Register("obs-air-lidar", "Lidar", UnitCategory.AirUnit);
            Register("obs-air-lifeline", "Lifeline", UnitCategory.AirUnit);
            Register("obs-air-link", "Link", UnitCategory.AirUnit);
            Register("obs-air-locator", "Locator", UnitCategory.AirUnit);
            Register("obs-air-cartographer", "Cartographer", UnitCategory.AirUnit);
            Register("obs-air-mesh", "Mesh", UnitCategory.AirUnit);
            Register("obs-air-nightguard", "Nightguard", UnitCategory.AirUnit);
            Register("obs-air-overseer", "Overseer", UnitCategory.AirUnit);
            Register("obs-air-insight", "Insight", UnitCategory.AirUnit);
            Register("obs-air-pulse", "Pulse", UnitCategory.AirUnit);
            Register("obs-air-relay", "Relay", UnitCategory.AirUnit);
            Register("obs-air-sar", "SAR", UnitCategory.AirUnit);
            Register("obs-air-scout", "Scout", UnitCategory.AirUnit);
            Register("obs-air-nexus-grid", "NexusGrid", UnitCategory.AirUnit);
            Register("obs-air-spotter", "Spotter", UnitCategory.AirUnit);
            Register("obs-air-survey", "Survey", UnitCategory.AirUnit);
            Register("obs-air-terrain", "Terrain", UnitCategory.AirUnit);
            Register("obs-air-thermal", "Thermal", UnitCategory.AirUnit);
            Register("obs-air-trailblazer", "Trailblazer", UnitCategory.AirUnit);
            Register("obs-air-tracker", "Tracker", UnitCategory.AirUnit);
            Register("obs-air-vector", "Vector", UnitCategory.AirUnit);
            Register("obs-air-watchtower", "Watchtower", UnitCategory.AirUnit);
            Register("obs-air-skywatch", "Skywatch", UnitCategory.AirUnit);
            Register("obs-air-horizon", "Horizon", UnitCategory.AirUnit);

            Register("obs-ground-bulldog", "Bulldog", UnitCategory.GroundUnit);
            Register("obs-ground-forge", "Forge", UnitCategory.GroundUnit);
            Register("obs-ground-hammer", "Hammer", UnitCategory.GroundUnit);
            Register("obs-ground-ironwalker", "Ironwalker", UnitCategory.GroundUnit);
            Register("obs-ground-mule", "Mule", UnitCategory.GroundUnit);
            Register("obs-ground-patrol", "Patrol", UnitCategory.GroundUnit);
            Register("obs-ground-rescue", "Rescue", UnitCategory.GroundUnit);
            Register("obs-ground-scout", "Scout", UnitCategory.GroundUnit);
            Register("obs-ground-sentinal", "Sentinal", UnitCategory.GroundUnit);
            Register("obs-ground-rover", "Rover", UnitCategory.GroundUnit);
            Register("obs-ground-crusher", "Crusher", UnitCategory.GroundUnit);
            Register("obs-ground-hauler", "Hauler", UnitCategory.GroundUnit);

            Register("obs-sea-surveyor-mk1", "Surveyor Mk1", UnitCategory.Naval);
            Register("obs-sea-current", "Current", UnitCategory.Naval);
            Register("obs-sea-rescue", "Rescue", UnitCategory.Naval);
            Register("obs-sea-scout", "Scout", UnitCategory.Naval);
            Register("obs-sea-sonar", "Sonar", UnitCategory.Naval);
            Register("obs-sea-depthwatch", "Depthwatch", UnitCategory.Naval);
            Register("obs-sea-harbor", "Harbor", UnitCategory.Naval);
            Register("obs-sea-tidebreaker", "Tidebreaker", UnitCategory.Naval);

            Register("obs-command-archive", "Archive", UnitCategory.Station);
            Register("obs-command-worldmap", "Worldmap", UnitCategory.Station);
            Register("obs-command-core", "Command Core", UnitCategory.Station);
            Register("obs-command-fusion", "Fusion", UnitCategory.Station);
            Register("obs-command-nexus", "Nexus", UnitCategory.Station);
            Register("obs-command-insight", "Insight", UnitCategory.Station);
            Register("obs-command-pulse", "Pulse", UnitCategory.Station);
            Register("obs-command-vectorcore", "Vector Core", UnitCategory.Station);

            Register("obs-experimental-echo", "Echo", UnitCategory.Exploration);
            Register("obs-experimental-nullpoint", "Nullpoint", UnitCategory.Exploration);
            Register("obs-experimental-specter", "Specter", UnitCategory.Exploration);
            Register("obs-experimental-shadowgrid", "Shadowgrid", UnitCategory.Exploration);
            Register("obs-experimental-phantom", "Phantom", UnitCategory.Exploration);
            Register("obs-experimental-helix", "Helix", UnitCategory.Exploration);

            Register("obs-sat-orbiter", "Orbiter", UnitCategory.Satellite);
            Register("obs-sat-skylink", "Skylink", UnitCategory.Satellite);
            Register("obs-sat-astral", "Astral", UnitCategory.Satellite);
            Register("obs-sat-starcast", "Starcast", UnitCategory.Satellite);
            Register("obs-sat-cosmos", "Cosmos", UnitCategory.Satellite);
            Register("obs-sat-nebula", "Nebula", UnitCategory.Satellite);
            Register("obs-sat-galaxy", "Galaxy", UnitCategory.Satellite);
        }

        private void Register(
            string unitId,
            string displayName,
            UnitCategory category)
        {
            if (string.IsNullOrWhiteSpace(unitId))
                return;

            units[unitId] = new WardenUnitDefinition(
                unitId,
                displayName,
                category);
        }

        public bool TryGetUnit(
            string unitId,
            out WardenUnitDefinition unit)
        {
            return units.TryGetValue(unitId, out unit);
        }

        public IReadOnlyCollection<WardenUnitDefinition> GetAllUnits()
        {
            return units.Values;
        }

        public int Count => units.Count;

        public int GetCategoryCount(UnitCategory category)
        {
            int count = 0;

            foreach (WardenUnitDefinition unit in units.Values)
            {
                if (unit.Category == category)
                    count++;
            }

            return count;
        }
    }
}
