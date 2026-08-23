using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ContentCompletion
{
    public sealed class ContentCompletionStructures
    {
        private readonly HashSet<string> structures =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int StructureCount =>
            structures.Count;

        public bool Complete =>
            StructureCount > 0;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            structures.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterStructure(
            string structureId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(structureId))
            {
                return false;
            }

            return structures.Add(
                structureId.Trim());
        }

        public bool ContainsStructure(
            string structureId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(structureId))
            {
                return false;
            }

            return structures.Contains(
                structureId.Trim());
        }

        public IReadOnlyCollection<string>
            GetStructures()
        {
            return structures;
        }

        public void Reset()
        {
            structures.Clear();
            Initialized = false;
        }
    }
}
