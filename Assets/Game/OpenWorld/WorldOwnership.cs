using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OpenWorld
{
    public enum WorldOwnerType
    {
        Unclaimed,
        Player,
        Alliance,
        AI
    }

    public sealed class WorldOwnershipRecord
    {
        public string RegionId { get; }

        public WorldOwnerType OwnerType { get; private set; }

        public string OwnerId { get; private set; }

        public long OwnershipRevision { get; private set; }

        public WorldOwnershipRecord(
            string regionId)
        {
            RegionId =
                regionId ?? string.Empty;

            OwnerType =
                WorldOwnerType.Unclaimed;

            OwnerId =
                string.Empty;
        }

        public bool SetOwner(
            WorldOwnerType ownerType,
            string ownerId,
            long revision)
        {
            if (string.IsNullOrWhiteSpace(RegionId) ||
                revision < 0)
            {
                return false;
            }

            if (ownerType == WorldOwnerType.Unclaimed)
            {
                OwnerId = string.Empty;
            }
            else if (string.IsNullOrWhiteSpace(ownerId))
            {
                return false;
            }
            else
            {
                OwnerId = ownerId.Trim();
            }

            OwnerType = ownerType;
            OwnershipRevision = revision;

            return true;
        }
    }

    public sealed class WorldOwnership
    {
        private readonly Dictionary<
            string,
            WorldOwnershipRecord> regions =
            new Dictionary<
                string,
                WorldOwnershipRecord>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int RegionCount =>
            regions.Count;

        public long Revision { get; private set; }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            regions.Clear();
            Revision = 0;
            Initialized = true;

            return true;
        }

        public bool RegisterRegion(
            string regionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(regionId))
            {
                return false;
            }

            string id =
                regionId.Trim();

            if (regions.ContainsKey(id))
            {
                return false;
            }

            regions.Add(
                id,
                new WorldOwnershipRecord(id));

            return true;
        }

        public bool AssignOwner(
            string regionId,
            WorldOwnerType ownerType,
            string ownerId)
        {
            WorldOwnershipRecord record =
                GetRegion(regionId);

            if (record == null)
            {
                return false;
            }

            Revision++;

            return record.SetOwner(
                ownerType,
                ownerId,
                Revision);
        }

        public bool ClearOwner(
            string regionId)
        {
            return AssignOwner(
                regionId,
                WorldOwnerType.Unclaimed,
                string.Empty);
        }

        public bool IsOwnedBy(
            string regionId,
            WorldOwnerType ownerType,
            string ownerId)
        {
            WorldOwnershipRecord record =
                GetRegion(regionId);

            if (record == null ||
                record.OwnerType != ownerType)
            {
                return false;
            }

            if (ownerType ==
                WorldOwnerType.Unclaimed)
            {
                return true;
            }

            return string.Equals(
                record.OwnerId,
                ownerId?.Trim(),
                StringComparison.OrdinalIgnoreCase);
        }

        public WorldOwnershipRecord GetRegion(
            string regionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(regionId))
            {
                return null;
            }

            regions.TryGetValue(
                regionId.Trim(),
                out WorldOwnershipRecord record);

            return record;
        }

        public IReadOnlyCollection<
            WorldOwnershipRecord>
            GetRegions()
        {
            return regions.Values;
        }

        public void Reset()
        {
            regions.Clear();
            Revision = 0;
            Initialized = false;
        }
    }
}
