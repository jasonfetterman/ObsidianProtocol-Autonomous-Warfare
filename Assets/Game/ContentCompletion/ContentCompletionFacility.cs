using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ContentCompletion
{
    public sealed class ContentCompletionFacility
    {
        private readonly HashSet<string> facilitySystems =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int SystemCount =>
            facilitySystems.Count;

        public bool Complete =>
            SystemCount > 0;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            facilitySystems.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterSystem(
            string systemId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(systemId))
            {
                return false;
            }

            return facilitySystems.Add(
                systemId.Trim());
        }

        public bool ContainsSystem(
            string systemId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(systemId))
            {
                return false;
            }

            return facilitySystems.Contains(
                systemId.Trim());
        }

        public IReadOnlyCollection<string>
            GetSystems()
        {
            return facilitySystems;
        }

        public void Reset()
        {
            facilitySystems.Clear();
            Initialized = false;
        }
    }
}
