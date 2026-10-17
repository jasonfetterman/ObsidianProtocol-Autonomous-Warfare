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
            Register("obs-air-warden", "Warden", UnitCategory.Air);
            Register("obs-air-beacon", "Beacon", UnitCategory.Air);
            Register("obs-air-drop", "Drop", UnitCategory.Air);
            Register("obs-air-iris", "Iris", UnitCategory.Air);
            Register("obs-air-sentinel", "Sentinel", UnitCategory.Air);
            Register("obs-air-scout-eye", "ScoutEye", UnitCategory.Air);
            Register("obs-air-inspect", "Inspect", UnitCategory.Air);
            Register("obs-air-lidar", "Lidar", UnitCategory.Air);
            Register("obs-air-lifeline", "Lifeline", UnitCategory.Air);
            Register("obs-air-link", "Link", UnitCategory.Air);
            Register("obs-air-locator", "Locator", UnitCategory.Air);
            Register("obs-air-cartographer", "Cartographer", UnitCategory.Air);
            Register("obs-air-mesh", "Mesh", UnitCategory.Air);
            Register("obs-air-nightguard", "Nightguard", UnitCategory.Air);
            Register("obs-air-overseer", "Overseer", UnitCategory.Air);
            Register("obs-air-insight", "Insight", UnitCategory.Air);
            Register("obs-air-pulse", "Pulse", UnitCategory.Air);
            Register("obs-air-relay", "Relay", UnitCategory.Air);
            Register("obs-air-sar", "SAR", UnitCategory.Air);
            Register("obs-air-scout", "Scout", UnitCategory.Air);
            Register("obs-air-nexus-grid", "NexusGrid", UnitCategory.Air);
            Register("obs-air-spotter", "Spotter", UnitCategory.Air);
            Register("obs-air-survey", "Survey", UnitCategory.Air);
            Register("obs-air-terrain", "Terrain", UnitCategory.Air);
            Register("obs-air-thermal", "Thermal", UnitCategory.Air);
            Register("obs-air-trailblazer", "Trailblazer", UnitCategory.Air);
            Register("obs-air-tracker", "Tracker", UnitCategory.Air);
            Register("obs-air-vector", "Vector", UnitCategory.Air);
            Register("obs-air-watchtower", "Watchtower", UnitCategory.Air);
            Register("obs-air-skywatch", "Skywatch", UnitCategory.Air);
            Register("obs-air-horizon", "Horizon", UnitCategory.Air);

            Register("obs-ground-bulldog", "Bulldog", UnitCategory.Ground);
            Register("obs-ground-forge", "Forge", UnitCategory.Ground);
            Register("obs-ground-hammer", "Hammer", UnitCategory.Ground);
            Register("obs-ground-ironwalker", "Ironwalker", UnitCategory.Ground);
            Register("obs-ground-mule", "Mule", UnitCategory.Ground);
            Register("obs-ground-patrol", "Patrol", UnitCategory.Ground);
            Register("obs-ground-rescue", "Rescue", UnitCategory.Ground);
            Register("obs-ground-scout", "Scout", UnitCategory.Ground);
            Register("obs-ground-sentinal", "Sentinal", UnitCategory.Ground);
            Register("obs-ground-rover", "Rover", UnitCategory.Ground);
            Register("obs-ground-crusher", "Crusher", UnitCategory.Ground);
            Register("obs-ground-hauler", "Hauler", UnitCategory.Ground);

            Register("obs-sea-surveyor-mk1", "Surveyor Mk1", UnitCategory.Sea);
            Register("obs-sea-current", "Current", UnitCategory.Sea);
            Register("obs-sea-rescue", "Rescue", UnitCategory.Sea);
            Register("obs-sea-scout", "Scout", UnitCategory.Sea);
            Register("obs-sea-sonar", "Sonar", UnitCategory.Sea);
            Register("obs-sea-depthwatch", "Depthwatch", UnitCategory.Sea);
            Register("obs-sea-harbor", "Harbor", UnitCategory.Sea);
            Register("obs-sea-tidebreaker", "Tidebreaker", UnitCategory.Sea);

            Register("obs-command-archive", "Archive", UnitCategory.Command);
            Register("obs-command-worldmap", "Worldmap", UnitCategory.Command);
            Register("obs-command-core", "Command Core", UnitCategory.Command);
            Register("obs-command-fusion", "Fusion", UnitCategory.Command);
            Register("obs-command-nexus", "Nexus", UnitCategory.Command);
            Register("obs-command-insight", "Insight", UnitCategory.Command);
            Register("obs-command-pulse", "Pulse", UnitCategory.Command);
            Register("obs-command-vectorcore", "Vector Core", UnitCategory.Command);

            Register("obs-experimental-echo", "Echo", UnitCategory.Experimental);
            Register("obs-experimental-nullpoint", "Nullpoint", UnitCategory.Experimental);
            Register("obs-experimental-specter", "Specter", UnitCategory.Experimental);
            Register("obs-experimental-shadowgrid", "Shadowgrid", UnitCategory.Experimental);
            Register("obs-experimental-phantom", "Phantom", UnitCategory.Experimental);
            Register("obs-experimental-helix", "Helix", UnitCategory.Experimental);

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
