using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Garage
{
    public sealed class UnitDecal
    {
        public string DecalId { get; }
        public string Position { get; private set; }

        public float Scale { get; private set; }
        public float Rotation { get; private set; }

        public bool Enabled { get; private set; }

        public UnitDecal(
            string decalId,
            string position)
        {
            DecalId =
                decalId ?? string.Empty;

            Position =
                position ?? string.Empty;

            Scale = 1f;
            Rotation = 0f;
            Enabled = true;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                DecalId);

        public void SetPosition(
            string position)
        {
            Position =
                position ?? string.Empty;
        }

        public void SetScale(
            float scale)
        {
            Scale =
                Math.Max(0f, scale);
        }

        public void SetRotation(
            float rotation)
        {
            Rotation = rotation;
        }

        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }
    }

    public sealed class DecalSystem
    {
        private readonly Dictionary<
            string,
            List<UnitDecal>> decals =
            new Dictionary<
                string,
                List<UnitDecal>>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterUnit(
            string ownershipId)
        {
            if (string.IsNullOrWhiteSpace(
                    ownershipId) ||
                decals.ContainsKey(ownershipId))
            {
                return false;
            }

            decals.Add(
                ownershipId,
                new List<UnitDecal>());

            return true;
        }

        public bool RemoveUnit(
            string ownershipId)
        {
            if (string.IsNullOrWhiteSpace(
                    ownershipId))
            {
                return false;
            }

            return decals.Remove(ownershipId);
        }

        public bool AddDecal(
            string ownershipId,
            UnitDecal decal)
        {
            if (string.IsNullOrWhiteSpace(
                    ownershipId) ||
                decal == null ||
                !decal.Valid ||
                !decals.TryGetValue(
                    ownershipId,
                    out List<UnitDecal> unitDecals))
            {
                return false;
            }

            foreach (UnitDecal existing in unitDecals)
            {
                if (string.Equals(
                        existing.DecalId,
                        decal.DecalId,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            unitDecals.Add(decal);
            return true;
        }

        public bool RemoveDecal(
            string ownershipId,
            string decalId)
        {
            if (!decals.TryGetValue(
                    ownershipId,
                    out List<UnitDecal> unitDecals) ||
                string.IsNullOrWhiteSpace(decalId))
            {
                return false;
            }

            for (int i = 0; i < unitDecals.Count; i++)
            {
                if (string.Equals(
                        unitDecals[i].DecalId,
                        decalId,
                        StringComparison.OrdinalIgnoreCase))
                {
                    unitDecals.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public IReadOnlyList<UnitDecal>
            GetDecals(
                string ownershipId)
        {
            if (!decals.TryGetValue(
                    ownershipId,
                    out List<UnitDecal> unitDecals))
            {
                return Array.Empty<UnitDecal>();
            }

            return unitDecals;
        }

        public void Clear()
        {
            decals.Clear();
        }
    }
}
