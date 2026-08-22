using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.GroundWarfare
{
    public enum GroundTerrainType
    {
        Road,
        OpenGround,
        Mud,
        Sand,
        Rock,
        Forest,
        Urban,
        Water
    }

    public sealed class TerrainTraversalProfile
    {
        public string UnitId { get; }

        private readonly Dictionary<GroundTerrainType, float> movementMultipliers =
            new Dictionary<GroundTerrainType, float>();

        public TerrainTraversalProfile(string unitId)
        {
            UnitId = unitId ?? string.Empty;

            foreach (GroundTerrainType terrain in
                     Enum.GetValues(typeof(GroundTerrainType)))
            {
                movementMultipliers[terrain] = 1f;
            }
        }

        public void SetMovementMultiplier(
            GroundTerrainType terrain,
            float multiplier)
        {
            movementMultipliers[terrain] =
                Math.Max(0f, multiplier);
        }

        public float GetMovementMultiplier(
            GroundTerrainType terrain)
        {
            return movementMultipliers.TryGetValue(
                       terrain,
                       out float multiplier)
                ? multiplier
                : 0f;
        }

        public bool CanTraverse(
            GroundTerrainType terrain)
        {
            return GetMovementMultiplier(terrain) > 0f;
        }
    }

    public sealed class TerrainTraversalSystem
    {
        private readonly Dictionary<string, TerrainTraversalProfile> profiles =
            new Dictionary<string, TerrainTraversalProfile>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterVehicle(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!profiles.ContainsKey(unitId))
            {
                profiles.Add(
                    unitId,
                    new TerrainTraversalProfile(unitId));
            }
        }

        public void SetMovementMultiplier(
            string unitId,
            GroundTerrainType terrain,
            float multiplier)
        {
            RegisterVehicle(unitId);

            profiles[unitId].SetMovementMultiplier(
                terrain,
                multiplier);
        }

        public float GetMovementMultiplier(
            string unitId,
            GroundTerrainType terrain)
        {
            return profiles.TryGetValue(
                       unitId,
                       out TerrainTraversalProfile profile)
                ? profile.GetMovementMultiplier(terrain)
                : 0f;
        }

        public bool CanTraverse(
            string unitId,
            GroundTerrainType terrain)
        {
            return profiles.TryGetValue(
                       unitId,
                       out TerrainTraversalProfile profile) &&
                   profile.CanTraverse(terrain);
        }

        public bool TryGetProfile(
            string unitId,
            out TerrainTraversalProfile profile)
        {
            return profiles.TryGetValue(
                unitId,
                out profile);
        }

        public void RemoveVehicle(string unitId)
        {
            profiles.Remove(unitId);
        }

        public void Clear()
        {
            profiles.Clear();
        }
    }
}
