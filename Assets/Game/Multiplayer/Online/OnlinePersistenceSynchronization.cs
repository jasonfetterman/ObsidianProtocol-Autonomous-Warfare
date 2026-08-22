using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Online
{
    public sealed class OnlinePersistenceSnapshot
    {
        private readonly Dictionary<string, string> values =
            new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase);

        public string SnapshotId { get; }

        public long SimulationTick { get; private set; }

        public DateTime CreatedAtUtc { get; private set; }

        public int ValueCount =>
            values.Count;

        public OnlinePersistenceSnapshot(
            string snapshotId)
        {
            SnapshotId =
                snapshotId ?? string.Empty;

            CreatedAtUtc =
                DateTime.UtcNow;
        }

        public bool SetValue(
            string key,
            string value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            values[key.Trim()] =
                value ?? string.Empty;

            return true;
        }

        public bool TryGetValue(
            string key,
            out string value)
        {
            value = string.Empty;

            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            return values.TryGetValue(
                key.Trim(),
                out value);
        }

        public bool SetSimulationTick(
            long simulationTick)
        {
            if (simulationTick < 0)
            {
                return false;
            }

            SimulationTick =
                simulationTick;

            return true;
        }

        public IReadOnlyDictionary<string, string>
            GetValues()
        {
            return values;
        }
    }

    public sealed class OnlinePersistenceSynchronization
    {
        private OnlinePersistenceSnapshot snapshot;

        public bool Initialized { get; private set; }

        public bool HasSnapshot =>
            snapshot != null;

        public OnlinePersistenceSnapshot Snapshot =>
            snapshot;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            snapshot = null;
            Initialized = true;

            return true;
        }

        public bool CreateSnapshot(
            string snapshotId,
            long simulationTick)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(snapshotId) ||
                simulationTick < 0)
            {
                return false;
            }

            snapshot =
                new OnlinePersistenceSnapshot(
                    snapshotId.Trim());

            return snapshot.SetSimulationTick(
                simulationTick);
        }

        public bool SynchronizeValue(
            string key,
            string value)
        {
            if (!Initialized ||
                snapshot == null)
            {
                return false;
            }

            return snapshot.SetValue(
                key,
                value);
        }

        public bool TryGetValue(
            string key,
            out string value)
        {
            value = string.Empty;

            if (!Initialized ||
                snapshot == null)
            {
                return false;
            }

            return snapshot.TryGetValue(
                key,
                out value);
        }

        public bool HasValue(
            string key)
        {
            return TryGetValue(
                key,
                out _);
        }

        public void ClearSnapshot()
        {
            snapshot = null;
        }

        public void Reset()
        {
            snapshot = null;
            Initialized = false;
        }
    }
}
