using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Campaign
{
    public sealed class CampaignNarrativeEntry
    {
        public string EntryId { get; }

        public string Title { get; }

        public string Text { get; }

        public bool Revealed { get; private set; }

        public CampaignNarrativeEntry(
            string entryId,
            string title,
            string text)
        {
            EntryId =
                entryId ?? string.Empty;

            Title =
                title ?? string.Empty;

            Text =
                text ?? string.Empty;

            Revealed = false;
        }

        public bool Reveal()
        {
            if (Revealed)
            {
                return false;
            }

            Revealed = true;

            return true;
        }
    }

    public sealed class CampaignNarrativeSystems
    {
        private readonly Dictionary<
            string,
            CampaignNarrativeEntry> entries =
            new Dictionary<
                string,
                CampaignNarrativeEntry>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int EntryCount =>
            entries.Count;

        public int RevealedEntryCount
        {
            get
            {
                int count = 0;

                foreach (CampaignNarrativeEntry entry
                         in entries.Values)
                {
                    if (entry.Revealed)
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

            entries.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterEntry(
            string entryId,
            string title,
            string text)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(entryId) ||
                string.IsNullOrWhiteSpace(title))
            {
                return false;
            }

            string id =
                entryId.Trim();

            if (entries.ContainsKey(id))
            {
                return false;
            }

            entries.Add(
                id,
                new CampaignNarrativeEntry(
                    id,
                    title.Trim(),
                    text ?? string.Empty));

            return true;
        }

        public bool RevealEntry(
            string entryId)
        {
            CampaignNarrativeEntry entry =
                GetEntry(entryId);

            return entry != null &&
                   entry.Reveal();
        }

        public CampaignNarrativeEntry GetEntry(
            string entryId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(entryId))
            {
                return null;
            }

            entries.TryGetValue(
                entryId.Trim(),
                out CampaignNarrativeEntry entry);

            return entry;
        }

        public IReadOnlyCollection<
            CampaignNarrativeEntry>
            GetEntries()
        {
            return entries.Values;
        }

        public void Reset()
        {
            entries.Clear();
            Initialized = false;
        }
    }
}
