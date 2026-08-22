using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.CommandUnits
{
    public enum CommandDataType
    {
        UnitStatus,
        Battlefield,
        Reconnaissance,
        Intelligence,
        Mission,
        Logistics,
        Fleet,
        Network,
        Analytics
    }

    public sealed class CommandDataRecord
    {
        public string RecordId { get; }
        public CommandDataType Type { get; }

        public string SourceId { get; }
        public string Data { get; private set; }

        public DateTime LastUpdated { get; private set; }

        public CommandDataRecord(
            string recordId,
            CommandDataType type,
            string sourceId,
            string data)
        {
            RecordId =
                recordId ?? string.Empty;

            Type =
                type;

            SourceId =
                sourceId ?? string.Empty;

            Data =
                data ?? string.Empty;

            LastUpdated =
                DateTime.UtcNow;
        }

        public void Update(
            string data)
        {
            Data =
                data ?? string.Empty;

            LastUpdated =
                DateTime.UtcNow;
        }
    }

    public sealed class CommandDataStorageSystem
    {
        private readonly Dictionary<string, CommandDataRecord> records =
            new Dictionary<string, CommandDataRecord>(
                StringComparer.OrdinalIgnoreCase);

        public void Store(
            string recordId,
            CommandDataType type,
            string sourceId,
            string data)
        {
            if (string.IsNullOrWhiteSpace(recordId))
            {
                return;
            }

            if (records.TryGetValue(
                    recordId,
                    out CommandDataRecord existing))
            {
                existing.Update(data);
                return;
            }

            records.Add(
                recordId,
                new CommandDataRecord(
                    recordId,
                    type,
                    sourceId,
                    data));
        }

        public bool TryGet(
            string recordId,
            out CommandDataRecord record)
        {
            return records.TryGetValue(
                recordId,
                out record);
        }

        public bool Contains(
            string recordId)
        {
            return records.ContainsKey(recordId);
        }

        public int Count()
        {
            return records.Count;
        }

        public void Remove(
            string recordId)
        {
            records.Remove(recordId);
        }

        public void Clear()
        {
            records.Clear();
        }
    }
}
