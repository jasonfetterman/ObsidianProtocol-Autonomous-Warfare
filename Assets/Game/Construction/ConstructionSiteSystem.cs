using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Construction
{
    public enum ConstructionSiteState
    {
        Planned,
        Active,
        Paused,
        Completed,
        Abandoned,
        Destroyed
    }

    public sealed class ConstructionSite
    {
        public string SiteId { get; }

        public string StructureId { get; }

        public string OwnerId { get; }

        public float Progress { get; private set; }

        public ConstructionSiteState State { get; private set; }

        public ConstructionSite(
            string siteId,
            string structureId,
            string ownerId)
        {
            SiteId =
                siteId ?? string.Empty;

            StructureId =
                structureId ?? string.Empty;

            OwnerId =
                ownerId ?? string.Empty;

            Progress =
                0f;

            State =
                ConstructionSiteState.Planned;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(SiteId) &&
            !string.IsNullOrWhiteSpace(StructureId);

        public bool Active =>
            State ==
                ConstructionSiteState.Active ||
            State ==
                ConstructionSiteState.Paused;

        public void Activate()
        {
            if (State ==
                ConstructionSiteState.Planned)
            {
                State =
                    ConstructionSiteState.Active;
            }
        }

        public void Pause()
        {
            if (State ==
                ConstructionSiteState.Active)
            {
                State =
                    ConstructionSiteState.Paused;
            }
        }

        public void Resume()
        {
            if (State ==
                ConstructionSiteState.Paused)
            {
                State =
                    ConstructionSiteState.Active;
            }
        }

        public void AddProgress(
            float amount)
        {
            if (State !=
                ConstructionSiteState.Active)
            {
                return;
            }

            Progress =
                Math.Max(
                    0f,
                    Math.Min(
                        1f,
                        Progress + Math.Max(
                            0f,
                            amount)));

            if (Progress >= 1f)
            {
                Progress =
                    1f;

                State =
                    ConstructionSiteState.Completed;
            }
        }

        public void Abandon()
        {
            if (State !=
                ConstructionSiteState.Completed)
            {
                State =
                    ConstructionSiteState.Abandoned;
            }
        }

        public void Destroy()
        {
            if (State !=
                ConstructionSiteState.Completed)
            {
                State =
                    ConstructionSiteState.Destroyed;
            }
        }
    }

    public sealed class ConstructionSiteSystem
    {
        private readonly Dictionary<string, ConstructionSite>
            sites =
                new Dictionary<string, ConstructionSite>(
                    StringComparer.OrdinalIgnoreCase);

        public bool RegisterSite(
            ConstructionSite site)
        {
            if (site == null ||
                !site.Valid ||
                sites.ContainsKey(
                    site.SiteId))
            {
                return false;
            }

            sites.Add(
                site.SiteId,
                site);

            return true;
        }

        public bool RemoveSite(
            string siteId)
        {
            if (string.IsNullOrWhiteSpace(
                    siteId))
            {
                return false;
            }

            return sites.Remove(
                siteId);
        }

        public bool TryGetSite(
            string siteId,
            out ConstructionSite site)
        {
            return sites.TryGetValue(
                siteId,
                out site);
        }

        public bool ActivateSite(
            string siteId)
        {
            if (!sites.TryGetValue(
                    siteId,
                    out ConstructionSite site))
            {
                return false;
            }

            site.Activate();

            return true;
        }

        public bool PauseSite(
            string siteId)
        {
            if (!sites.TryGetValue(
                    siteId,
                    out ConstructionSite site))
            {
                return false;
            }

            site.Pause();

            return true;
        }

        public bool ResumeSite(
            string siteId)
        {
            if (!sites.TryGetValue(
                    siteId,
                    out ConstructionSite site))
            {
                return false;
            }

            site.Resume();

            return true;
        }

        public bool AddProgress(
            string siteId,
            float amount)
        {
            if (!sites.TryGetValue(
                    siteId,
                    out ConstructionSite site))
            {
                return false;
            }

            site.AddProgress(
                amount);

            return true;
        }

        public bool AbandonSite(
            string siteId)
        {
            if (!sites.TryGetValue(
                    siteId,
                    out ConstructionSite site))
            {
                return false;
            }

            site.Abandon();

            return true;
        }

        public bool DestroySite(
            string siteId)
        {
            if (!sites.TryGetValue(
                    siteId,
                    out ConstructionSite site))
            {
                return false;
            }

            site.Destroy();

            return true;
        }

        public IReadOnlyCollection<ConstructionSite>
            GetSites()
        {
            return sites.Values;
        }

        public IReadOnlyCollection<ConstructionSite>
            GetActiveSites()
        {
            List<ConstructionSite> active =
                new List<ConstructionSite>();

            foreach (
                ConstructionSite site
                in sites.Values)
            {
                if (site.Active)
                {
                    active.Add(
                        site);
                }
            }

            return active;
        }

        public void Clear()
        {
            sites.Clear();
        }
    }
}
