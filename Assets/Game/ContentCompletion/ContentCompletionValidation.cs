using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ContentCompletion
{
    public sealed class ContentCompletionValidation
    {
        private readonly Dictionary<string, bool> sections =
            new Dictionary<string, bool>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public bool Complete
        {
            get
            {
                if (!Initialized ||
                    sections.Count == 0)
                {
                    return false;
                }

                foreach (bool complete in sections.Values)
                {
                    if (!complete)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public int SectionCount =>
            sections.Count;

        public int CompletedSectionCount
        {
            get
            {
                int count = 0;

                foreach (bool complete in sections.Values)
                {
                    if (complete)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            sections.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterSection(
            string sectionId,
            bool complete)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(sectionId))
            {
                return false;
            }

            string id =
                sectionId.Trim();

            if (sections.ContainsKey(id))
            {
                return false;
            }

            sections.Add(
                id,
                complete);

            return true;
        }

        public bool SetSectionComplete(
            string sectionId,
            bool complete)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(sectionId))
            {
                return false;
            }

            string id =
                sectionId.Trim();

            if (!sections.ContainsKey(id))
            {
                return false;
            }

            sections[id] = complete;

            return true;
        }

        public bool IsSectionComplete(
            string sectionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(sectionId))
            {
                return false;
            }

            return sections.TryGetValue(
                sectionId.Trim(),
                out bool complete) &&
                   complete;
        }

        public IReadOnlyDictionary<string, bool>
            GetSections()
        {
            return sections;
        }

        public void Reset()
        {
            sections.Clear();
            Initialized = false;
        }
    }
}
