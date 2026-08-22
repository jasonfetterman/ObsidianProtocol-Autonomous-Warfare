using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Units.Warden
{
    public enum WardenUnitCategory
    {
        Air,
        Ground,
        Sea,
        Command,
        Experimental
    }

    public sealed class WardenUnitDefinition
    {
        public string UnitId { get; }
        public string DisplayName { get; }
        public WardenUnitCategory Category { get; }

        public WardenUnitDefinition(
            string unitId,
            string displayName,
            WardenUnitCategory category)
        {
            UnitId = unitId;
            DisplayName = displayName;
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
            RegisterAirUnits();
            RegisterGroundUnits();
            RegisterSeaUnits();
            RegisterCommandUnits();
            RegisterExperimentalUnits();
        }

        private void RegisterAirUnits()
        {
            string[] names =
            {
                "Warden",
                "Beacon",
                "Drop",
                "Iris",
                "Sentinel",
                "ScoutEye",
                "Inspect",
                "Lidar",
                "Lifeline",
                "Link",
                "Locator",
                "Cartographer",
                "Mesh",
                "Nightguard",
                "Overseer",
                "Trailblazer",
                "Relay",
                "Response",
                "SAR",
                "Scout",
                "NexusGrid",
                "Spotter",
                "Survey",
                "Terrain",
                "Thermal",
                "Tracker",
                "Vector",
                "Watchtower"
            };

            RegisterNames(
                names,
                WardenUnitCategory.Air);
        }

        private void RegisterGroundUnits()
        {
            string[] names =
            {
                "Bulldog",
                "Forge",
                "Hammer",
                "Ironwalker",
                "Mule",
                "Patrol",
                "Rescue",
                "Scout",
                "Sentinel",
                "Rover",
                "Crusher",
                "Hauler"
            };

            RegisterNames(
                names,
                WardenUnitCategory.Ground);
        }

        private void RegisterSeaUnits()
        {
            string[] names =
            {
                "Surveyor Mk1",
                "Current",
                "Rescue",
                "Scout",
                "Sonar",
                "Depthwatch",
                "Harbor",
                "Tidebreaker"
            };

            RegisterNames(
                names,
                WardenUnitCategory.Sea);
        }

        private void RegisterCommandUnits()
        {
            string[] names =
            {
                "Archive",
                "Worldmap",
                "Command Core",
                "Fusion",
                "Nexus",
                "Insight",
                "Pulse",
                "Vector Core"
            };

            RegisterNames(
                names,
                WardenUnitCategory.Command);
        }

        private void RegisterExperimentalUnits()
        {
            string[] names =
            {
                "Echo",
                "Nullpoint",
                "Specter",
                "Shadowgrid",
                "Phantom",
                "Helix"
            };

            RegisterNames(
                names,
                WardenUnitCategory.Experimental);
        }

        private void RegisterNames(
            string[] names,
            WardenUnitCategory category)
        {
            foreach (string name in names)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    continue;
                }

                string unitId =
                    CreateUnitId(name);

                units[unitId] =
                    new WardenUnitDefinition(
                        unitId,
                        name,
                        category);
            }
        }

        private string CreateUnitId(string name)
        {
            return name
                .Replace(" ", string.Empty)
                .Replace("-", string.Empty)
                .ToUpperInvariant();
        }

        public bool TryGetUnit(
            string unitId,
            out WardenUnitDefinition unit)
        {
            return units.TryGetValue(
                unitId,
                out unit);
        }

        public IReadOnlyCollection<WardenUnitDefinition> GetAllUnits()
        {
            return units.Values;
        }

        public int Count =>
            units.Count;

        public int GetCategoryCount(
            WardenUnitCategory category)
        {
            int count = 0;

            foreach (WardenUnitDefinition unit in units.Values)
            {
                if (unit.Category == category)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
