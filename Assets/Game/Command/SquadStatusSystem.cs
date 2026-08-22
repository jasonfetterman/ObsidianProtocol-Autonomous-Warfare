using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Command
{
    public sealed class SquadStatus
    {
        public string SquadId { get; }

        public int UnitCount { get; private set; }
        public int OperationalUnits { get; private set; }
        public int DisabledUnits { get; private set; }

        public float AverageHealth { get; private set; }

        public bool Active { get; private set; }

        public string CurrentOrder { get; private set; }
        public string CurrentIntent { get; private set; }

        public SquadStatus(
            string squadId)
        {
            SquadId = squadId ?? string.Empty;

            UnitCount = 0;
            OperationalUnits = 0;
            DisabledUnits = 0;

            AverageHealth = 0f;

            Active = true;

            CurrentOrder = string.Empty;
            CurrentIntent = string.Empty;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(SquadId);

        public void SetUnitCounts(
            int total,
            int operational,
            int disabled)
        {
            UnitCount = Math.Max(0, total);

            OperationalUnits =
                Math.Max(
                    0,
                    Math.Min(
                        operational,
                        UnitCount));

            DisabledUnits =
                Math.Max(
                    0,
                    Math.Min(
                        disabled,
                        UnitCount));
        }

        public void SetAverageHealth(float health)
        {
            AverageHealth =
                Math.Max(
                    0f,
                    Math.Min(
                        100f,
                        health));
        }

        public void SetOrder(string order)
        {
            CurrentOrder =
                order ?? string.Empty;
        }

        public void SetIntent(string intent)
        {
            CurrentIntent =
                intent ?? string.Empty;
        }

        public void SetActive(bool active)
        {
            Active = active;
        }

        public void Reset()
        {
            UnitCount = 0;
            OperationalUnits = 0;
            DisabledUnits = 0;

            AverageHealth = 0f;

            Active = true;

            CurrentOrder = string.Empty;
            CurrentIntent = string.Empty;
        }
    }

    public sealed class SquadStatusSystem
    {
        private readonly Dictionary<string, SquadStatus> squads =
            new Dictionary<string, SquadStatus>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(SquadStatus squad)
        {
            if (squad == null ||
                !squad.Valid ||
                squads.ContainsKey(squad.SquadId))
            {
                return false;
            }

            squads.Add(
                squad.SquadId,
                squad);

            return true;
        }

        public bool Remove(string squadId)
        {
            if (string.IsNullOrWhiteSpace(squadId))
                return false;

            return squads.Remove(squadId);
        }

        public bool TryGet(
            string squadId,
            out SquadStatus squad)
        {
            return squads.TryGetValue(
                squadId,
                out squad);
        }

        public IReadOnlyCollection<SquadStatus>
            GetSquads()
        {
            return squads.Values;
        }

        public void Clear()
        {
            squads.Clear();
        }
    }
}
