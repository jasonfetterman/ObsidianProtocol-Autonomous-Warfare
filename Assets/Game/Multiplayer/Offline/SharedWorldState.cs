using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Offline
{
    public sealed class SharedWorldState
    {
        private readonly HashSet<string> activeUnits =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        private readonly HashSet<string> activeObjectives =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, string>
            unitControllers =
                new Dictionary<string, string>(
                    StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public bool WorldPaused { get; private set; }

        public long SimulationTick { get; private set; }

        public int ActiveUnitCount =>
            activeUnits.Count;

        public int ActiveObjectiveCount =>
            activeObjectives.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            activeUnits.Clear();
            activeObjectives.Clear();
            unitControllers.Clear();

            SimulationTick = 0;
            WorldPaused = false;
            Initialized = true;

            return true;
        }

        public bool RegisterUnit(
            string unitId,
            OfflinePlayerId controller)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId) ||
                controller == OfflinePlayerId.None)
            {
                return false;
            }

            string normalizedId =
                unitId.Trim();

            if (!activeUnits.Add(normalizedId))
            {
                return false;
            }

            unitControllers[normalizedId] =
                controller.ToString();

            return true;
        }

        public bool RemoveUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            string normalizedId =
                unitId.Trim();

            unitControllers.Remove(
                normalizedId);

            return activeUnits.Remove(
                normalizedId);
        }

        public bool RegisterObjective(
            string objectiveId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(objectiveId))
            {
                return false;
            }

            return activeObjectives.Add(
                objectiveId.Trim());
        }

        public bool RemoveObjective(
            string objectiveId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(objectiveId))
            {
                return false;
            }

            return activeObjectives.Remove(
                objectiveId.Trim());
        }

        public bool SetPaused(
            bool paused)
        {
            if (!Initialized)
            {
                return false;
            }

            WorldPaused = paused;

            return true;
        }

        public bool AdvanceSimulationTick()
        {
            if (!Initialized ||
                WorldPaused)
            {
                return false;
            }

            SimulationTick++;

            return true;
        }

        public bool ContainsUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return activeUnits.Contains(
                unitId.Trim());
        }

        public bool ContainsObjective(
            string objectiveId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(objectiveId))
            {
                return false;
            }

            return activeObjectives.Contains(
                objectiveId.Trim());
        }

        public OfflinePlayerId
            GetUnitController(
                string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return OfflinePlayerId.None;
            }

            string normalizedId =
                unitId.Trim();

            if (!unitControllers.TryGetValue(
                    normalizedId,
                    out string controller))
            {
                return OfflinePlayerId.None;
            }

            if (Enum.TryParse(
                    controller,
                    out OfflinePlayerId player))
            {
                return player;
            }

            return OfflinePlayerId.None;
        }

        public IReadOnlyCollection<string>
            GetActiveUnits()
        {
            return activeUnits;
        }

        public IReadOnlyCollection<string>
            GetActiveObjectives()
        {
            return activeObjectives;
        }

        public void Reset()
        {
            activeUnits.Clear();
            activeObjectives.Clear();
            unitControllers.Clear();

            SimulationTick = 0;
            WorldPaused = false;
            Initialized = false;
        }
    }
}
