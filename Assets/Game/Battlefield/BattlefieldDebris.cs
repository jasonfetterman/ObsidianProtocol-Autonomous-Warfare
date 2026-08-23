using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Battlefield
{
    public enum DebrisType
    {
        VehicleWreckage,
        BuildingRubble,
        BridgeFragments,
        RoadFragments,
        Equipment,
        StructuralFragments
    }

    public sealed class DebrisObject
    {
        public string DebrisId { get; }

        public DebrisType Type { get; }

        public float ObstructionLevel { get; private set; }

        public bool Persistent { get; }

        public bool Active { get; private set; }

        public DebrisObject(
            string debrisId,
            DebrisType type,
            float obstructionLevel,
            bool persistent)
        {
            DebrisId =
                debrisId ?? string.Empty;

            Type = type;

            ObstructionLevel =
                ClampObstruction(obstructionLevel);

            Persistent = persistent;

            Active =
                ObstructionLevel > 0f;
        }

        public bool SetObstructionLevel(
            float obstructionLevel)
        {
            ObstructionLevel =
                ClampObstruction(obstructionLevel);

            Active =
                ObstructionLevel > 0f;

            return true;
        }

        public bool Clear()
        {
            if (!Active)
            {
                return false;
            }

            ObstructionLevel = 0f;
            Active = false;

            return true;
        }

        private static float ClampObstruction(
            float value)
        {
            return Math.Max(
                0f,
                Math.Min(1f, value));
        }
    }

    public sealed class BattlefieldDebris
    {
        private readonly Dictionary<
            string,
            DebrisObject> debris =
            new Dictionary<
                string,
                DebrisObject>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int DebrisCount =>
            debris.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            debris.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterDebris(
            string debrisId,
            DebrisType type,
            float obstructionLevel,
            bool persistent)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(debrisId) ||
                obstructionLevel < 0f)
            {
                return false;
            }

            string id =
                debrisId.Trim();

            if (debris.ContainsKey(id))
            {
                return false;
            }

            debris.Add(
                id,
                new DebrisObject(
                    id,
                    type,
                    obstructionLevel,
                    persistent));

            return true;
        }

        public bool SetObstructionLevel(
            string debrisId,
            float obstructionLevel)
        {
            DebrisObject objectData =
                GetDebris(debrisId);

            return objectData != null &&
                   objectData.SetObstructionLevel(
                       obstructionLevel);
        }

        public bool ClearDebris(
            string debrisId)
        {
            DebrisObject objectData =
                GetDebris(debrisId);

            return objectData != null &&
                   objectData.Clear();
        }

        public DebrisObject GetDebris(
            string debrisId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(debrisId))
            {
                return null;
            }

            debris.TryGetValue(
                debrisId.Trim(),
                out DebrisObject objectData);

            return objectData;
        }

        public IReadOnlyCollection<DebrisObject>
            GetDebrisObjects()
        {
            return debris.Values;
        }

        public void Reset()
        {
            debris.Clear();

            Initialized = false;
        }
    }
}
