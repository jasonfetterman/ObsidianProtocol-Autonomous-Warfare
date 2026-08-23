using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Release
{
    public enum BugSeverity
    {
        Critical,
        High,
        Medium,
        Low
    }

    public sealed class ReleaseBug
    {
        public string BugId { get; }

        public BugSeverity Severity { get; }

        public string Description { get; }

        public bool Fixed { get; private set; }

        public ReleaseBug(
            string bugId,
            BugSeverity severity,
            string description)
        {
            BugId =
                bugId ?? string.Empty;

            Severity = severity;

            Description =
                description ?? string.Empty;

            Fixed = false;
        }

        public void MarkFixed()
        {
            Fixed = true;
        }
    }

    public sealed class FinalBugFixing
    {
        private readonly Dictionary<
            string,
            ReleaseBug> bugs =
            new Dictionary<
                string,
                ReleaseBug>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int BugCount =>
            bugs.Count;

        public int FixedCount
        {
            get
            {
                int count = 0;

                foreach (ReleaseBug bug
                         in bugs.Values)
                {
                    if (bug.Fixed)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public int OpenBugCount =>
            BugCount - FixedCount;

        public bool CriticalBugsResolved
        {
            get
            {
                foreach (ReleaseBug bug
                         in bugs.Values)
                {
                    if (bug.Severity ==
                            BugSeverity.Critical &&
                        !bug.Fixed)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public bool ReadyForFinalBalance
        {
            get
            {
                return Initialized &&
                       CriticalBugsResolved;
            }
        }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            bugs.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterBug(
            string bugId,
            BugSeverity severity,
            string description)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(bugId) ||
                string.IsNullOrWhiteSpace(description))
            {
                return false;
            }

            string id =
                bugId.Trim();

            if (bugs.ContainsKey(id))
            {
                return false;
            }

            bugs.Add(
                id,
                new ReleaseBug(
                    id,
                    severity,
                    description.Trim()));

            return true;
        }

        public bool MarkBugFixed(
            string bugId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(bugId))
            {
                return false;
            }

            if (!bugs.TryGetValue(
                    bugId.Trim(),
                    out ReleaseBug bug))
            {
                return false;
            }

            bug.MarkFixed();

            return true;
        }

        public ReleaseBug GetBug(
            string bugId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(bugId))
            {
                return null;
            }

            bugs.TryGetValue(
                bugId.Trim(),
                out ReleaseBug bug);

            return bug;
        }

        public IReadOnlyCollection<ReleaseBug>
            GetBugs()
        {
            return bugs.Values;
        }

        public void Reset()
        {
            bugs.Clear();
            Initialized = false;
        }
    }
}
