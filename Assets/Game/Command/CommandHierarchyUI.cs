using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Command
{
    public enum CommandHierarchyLevel
    {
        Commander,
        Formation,
        Squad,
        Unit
    }

    public sealed class CommandHierarchyEntry
    {
        public string EntryId { get; }
        public string ParentId { get; private set; }
        public string DisplayName { get; }
        public CommandHierarchyLevel Level { get; }

        public bool Selected { get; private set; }
        public bool Active { get; private set; }

        public CommandHierarchyEntry(
            string entryId,
            string parentId,
            string displayName,
            CommandHierarchyLevel level)
        {
            EntryId = entryId ?? string.Empty;
            ParentId = parentId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            Level = level;

            Selected = false;
            Active = true;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(EntryId);

        public void SetParent(string parentId)
        {
            ParentId = parentId ?? string.Empty;
        }

        public void SetSelected(bool selected)
        {
            Selected = selected;
        }

        public void SetActive(bool active)
        {
            Active = active;
        }
    }

    public sealed class CommandHierarchyUI
    {
        private readonly Dictionary<
            string,
            CommandHierarchyEntry> entries =
            new Dictionary<
                string,
                CommandHierarchyEntry>(
                StringComparer.OrdinalIgnoreCase);

        public bool Visible { get; private set; }

        public CommandHierarchyUI()
        {
            Visible = true;
        }

        public void Show()
        {
            Visible = true;
        }

        public void Hide()
        {
            Visible = false;
        }

        public bool Register(
            CommandHierarchyEntry entry)
        {
            if (entry == null ||
                !entry.Valid ||
                entries.ContainsKey(entry.EntryId))
            {
                return false;
            }

            entries.Add(
                entry.EntryId,
                entry);

            return true;
        }

        public bool Remove(
            string entryId)
        {
            if (string.IsNullOrWhiteSpace(
                    entryId))
            {
                return false;
            }

            return entries.Remove(entryId);
        }

        public bool TryGet(
            string entryId,
            out CommandHierarchyEntry entry)
        {
            return entries.TryGetValue(
                entryId,
                out entry);
        }

        public bool SetSelected(
            string entryId,
            bool selected)
        {
            if (!entries.TryGetValue(
                    entryId,
                    out CommandHierarchyEntry entry))
            {
                return false;
            }

            entry.SetSelected(selected);
            return true;
        }

        public bool SetActive(
            string entryId,
            bool active)
        {
            if (!entries.TryGetValue(
                    entryId,
                    out CommandHierarchyEntry entry))
            {
                return false;
            }

            entry.SetActive(active);
            return true;
        }

        public IReadOnlyCollection<
            CommandHierarchyEntry>
            GetEntries()
        {
            return entries.Values;
        }

        public void Clear()
        {
            entries.Clear();
        }

        public void Reset()
        {
            Visible = true;
            entries.Clear();
        }
    }
}
