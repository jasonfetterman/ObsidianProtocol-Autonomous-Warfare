using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Online
{
    public sealed class OnlineUnitState
    {
        public string UnitId { get; }

        public string OwnerPlayerId { get; private set; }

        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        public float Health { get; private set; }

        public bool Active { get; private set; }

        public long LastUpdateTick { get; private set; }

        public OnlineUnitState(
            string unitId,
            string ownerPlayerId)
        {
            UnitId = unitId ?? string.Empty;
            OwnerPlayerId = ownerPlayerId ?? string.Empty;
        }

        public bool Update(
            string ownerPlayerId,
            float x,
            float y,
            float z,
            float health,
            bool active,
            long tick)
        {
            if (string.IsNullOrWhiteSpace(UnitId))
            {
                return false;
            }

            OwnerPlayerId =
                ownerPlayerId ?? string.Empty;

            X = x;
            Y = y;
            Z = z;

            Health =
                Math.Max(0f, health);

            Active = active;
            LastUpdateTick = tick;

            return true;
        }
    }

    public sealed class OnlineUnitSynchronization
    {
        private readonly Dictionary<string, OnlineUnitState> units =
            new Dictionary<string, OnlineUnitState>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int UnitCount =>
            units.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            units.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterUnit(
            string unitId,
            string ownerPlayerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            string id =
                unitId.Trim();

            if (units.ContainsKey(id))
            {
                return false;
            }

            units.Add(
                id,
                new OnlineUnitState(
                    id,
                    ownerPlayerId));

            return true;
        }

        public bool SynchronizeUnit(
            string unitId,
            string ownerPlayerId,
            float x,
            float y,
            float z,
            float health,
            bool active,
            long tick)
        {
            OnlineUnitState unit =
                GetUnit(unitId);

            return unit != null &&
                   unit.Update(
                       ownerPlayerId,
                       x,
                       y,
                       z,
                       health,
                       active,
                       tick);
        }

        public OnlineUnitState GetUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return null;
            }

            units.TryGetValue(
                unitId.Trim(),
                out OnlineUnitState unit);

            return unit;
        }

        public bool RemoveUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return units.Remove(
                unitId.Trim());
        }

        public IReadOnlyCollection<OnlineUnitState>
            GetUnits()
        {
            return units.Values;
        }

        public void Reset()
        {
            units.Clear();
            Initialized = false;
        }
    }
}
