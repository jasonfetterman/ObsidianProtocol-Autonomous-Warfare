
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Intelligence
{
    [Serializable]
    public sealed class ArchiveMemoryRecord
    {
        public string RecordId;
        public string RecordType;
        public string Source;
        public string Subject;
        public string Details;
        public string Outcome;
        public long RecordedAtUtcTicks;
        public float Confidence;

        public DateTime RecordedAtUtc =>
            new DateTime(RecordedAtUtcTicks, DateTimeKind.Utc);
    }

    public sealed class ArchiveMemorySystem
    {
        private readonly List<ArchiveMemoryRecord> records =
            new List<ArchiveMemoryRecord>();

        public int RecordCount => records.Count;

        public IReadOnlyList<ArchiveMemoryRecord> Records =>
            records;

        public ArchiveMemoryRecord Record(
            string recordType,
            string source,
            string subject,
            string details,
            string outcome,
            float confidence)
        {
            ArchiveMemoryRecord record =
                new ArchiveMemoryRecord
                {
                    RecordId = Guid.NewGuid().ToString(),
                    RecordType = recordType ?? string.Empty,
                    Source = source ?? string.Empty,
                    Subject = subject ?? string.Empty,
                    Details = details ?? string.Empty,
                    Outcome = outcome ?? string.Empty,
                    RecordedAtUtcTicks = DateTime.UtcNow.Ticks,
                    Confidence = Mathf.Clamp01(confidence)
                };

            records.Add(record);

            return record;
        }

        public ArchiveMemoryRecord GetRecord(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId))
                return null;

            return records.Find(
                record => record.RecordId == recordId);
        }

        public List<ArchiveMemoryRecord> GetRecordsByType(
            string recordType)
        {
            return records.FindAll(
                record => string.Equals(
                    record.RecordType,
                    recordType,
                    StringComparison.OrdinalIgnoreCase));
        }

        public List<ArchiveMemoryRecord> GetRecordsBySubject(
            string subject)
        {
            return records.FindAll(
                record => string.Equals(
                    record.Subject,
                    subject,
                    StringComparison.OrdinalIgnoreCase));
        }

        public void RestoreRecords(
            List<ArchiveMemoryRecord> savedRecords)
        {
            records.Clear();

            if (savedRecords == null)
                return;

            foreach (ArchiveMemoryRecord record in savedRecords)
            {
                if (record != null)
                    records.Add(record);
            }
        }

        public void Clear()
        {
            records.Clear();
        }
    }
}
