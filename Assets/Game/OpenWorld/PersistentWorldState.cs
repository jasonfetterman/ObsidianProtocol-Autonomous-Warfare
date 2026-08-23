using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OpenWorld
{
    public sealed class PersistentWorldState
    {
        private readonly Dictionary<string, string> values =
            new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, long> revisions =
            new Dictionary<string, long>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ValueCount =>
            values.Count;

        public long Revision { get; private set; }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            values.Clear();
            revisions.Clear();

            Revision = 0;
            Initialized = true;

            return true;
        }

        public bool Set(
            string key,
            string value)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            string normalizedKey =
                key.Trim();

            values[normalizedKey] =
                value ?? string.Empty;

            Revision++;

            revisions[normalizedKey] =
                Revision;

            return true;
        }

        public bool Remove(
            string key)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            string normalizedKey =
                key.Trim();

            bool removed =
                values.Remove(normalizedKey);

            if (!removed)
            {
                return false;
            }

            Revision++;

            revisions[normalizedKey] =
                Revision;

            return true;
        }

        public bool Contains(
            string key)
        {
            return Initialized &&
                   !string.IsNullOrWhiteSpace(key) &&
                   values.ContainsKey(key.Trim());
        }

        public bool TryGet(
            string key,
            out string value)
        {
            value = string.Empty;

            if (!Initialized ||
                string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            return values.TryGetValue(
                key.Trim(),
                out value);
        }

        public bool TryGetRevision(
            string key,
            out long revision)
        {
            revision = 0;

            if (!Initialized ||
                string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            return revisions.TryGetValue(
                key.Trim(),
                out revision);
        }

        public IReadOnlyDictionary<string, string>
            GetValues()
        {
            return values;
        }

        public IReadOnlyDictionary<string, long>
            GetRevisions()
        {
            return revisions;
        }

        public void Clear()
        {
            values.Clear();
            revisions.Clear();

            Revision++;
        }

        public void Reset()
        {
            values.Clear();
            revisions.Clear();

            Revision = 0;
            Initialized = false;
        }
    }
}
