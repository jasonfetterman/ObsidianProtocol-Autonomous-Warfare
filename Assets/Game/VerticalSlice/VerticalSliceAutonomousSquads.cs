using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VerticalSlice
{
    public enum VerticalSliceSquadIntent
    {
        Idle,
        Move,
        Attack,
        Defend,
        Flank,
        Retreat,
        Reinforce
    }

    public sealed class VerticalSliceAutonomousSquad
    {
        private readonly List<string> unitIds =
            new List<string>();

        public string SquadId { get; }

        public VerticalSliceSquadIntent Intent
        {
            get;
            private set;
        }

        public bool Active { get; private set; }

        public int UnitCount =>
            unitIds.Count;

        public VerticalSliceAutonomousSquad(
            string squadId)
        {
            SquadId =
                squadId ?? string.Empty;

            Intent =
                VerticalSliceSquadIntent.Idle;

            Active = false;
        }

        public bool AddUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId) ||
                unitIds.Contains(unitId.Trim()))
            {
                return false;
            }

            unitIds.Add(unitId.Trim());

            return true;
        }

        public bool RemoveUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return unitIds.Remove(unitId.Trim());
        }

        public bool SetIntent(
            VerticalSliceSquadIntent intent)
        {
            Intent = intent;
            Active = true;

            return true;
        }

        public bool Deactivate()
        {
            Active = false;
            Intent = VerticalSliceSquadIntent.Idle;

            return true;
        }

        public IReadOnlyList<string> GetUnitIds()
        {
            return unitIds;
        }
    }

    public sealed class VerticalSliceAutonomousSquads
    {
        private readonly Dictionary<
            string,
            VerticalSliceAutonomousSquad> squads =
            new Dictionary<
                string,
                VerticalSliceAutonomousSquad>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int SquadCount =>
            squads.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            squads.Clear();
            Initialized = true;

            return true;
        }

        public bool CreateSquad(string squadId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(squadId))
            {
                return false;
            }

            string id = squadId.Trim();

            if (squads.ContainsKey(id))
            {
                return false;
            }

            squads.Add(
                id,
                new VerticalSliceAutonomousSquad(id));

            return true;
        }

        public bool AddUnit(
            string squadId,
            string unitId)
        {
            VerticalSliceAutonomousSquad squad =
                GetSquad(squadId);

            return squad != null &&
                   squad.AddUnit(unitId);
        }

        public bool SetIntent(
            string squadId,
            VerticalSliceSquadIntent intent)
        {
            VerticalSliceAutonomousSquad squad =
                GetSquad(squadId);

            return squad != null &&
                   squad.SetIntent(intent);
        }

        public VerticalSliceAutonomousSquad GetSquad(
            string squadId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(squadId))
            {
                return null;
            }

            squads.TryGetValue(
                squadId.Trim(),
                out VerticalSliceAutonomousSquad squad);

            return squad;
        }

        public IReadOnlyCollection<
            VerticalSliceAutonomousSquad>
            GetSquads()
        {
            return squads.Values;
        }

        public void Reset()
        {
            squads.Clear();
            Initialized = false;
        }
    }
}
