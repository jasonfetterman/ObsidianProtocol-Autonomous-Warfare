using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class SurveillanceFeed
    {
        public string FeedId { get; }
        public string SensorId { get; }
        public bool Active { get; internal set; }
        public DateTime LastUpdate { get; internal set; }

        public SurveillanceFeed(string feedId, string sensorId)
        {
            FeedId = feedId ?? string.Empty;
            SensorId = sensorId ?? string.Empty;
            Active = false;
            LastUpdate = DateTime.UtcNow;
        }
    }

    public sealed class IntelligenceSurveillanceFeed
    {
        private readonly Dictionary<string, SurveillanceFeed> feeds =
            new Dictionary<string, SurveillanceFeed>(StringComparer.OrdinalIgnoreCase);

        public void RegisterFeed(string feedId, string sensorId)
        {
            if (string.IsNullOrWhiteSpace(feedId) ||
                string.IsNullOrWhiteSpace(sensorId))
                return;

            feeds[feedId] = new SurveillanceFeed(feedId, sensorId);
        }

        public bool ActivateFeed(string feedId)
        {
            if (!feeds.TryGetValue(feedId, out SurveillanceFeed feed))
                return false;

            feed.Active = true;
            feed.LastUpdate = DateTime.UtcNow;
            return true;
        }

        public bool DeactivateFeed(string feedId)
        {
            if (!feeds.TryGetValue(feedId, out SurveillanceFeed feed))
                return false;

            feed.Active = false;
            feed.LastUpdate = DateTime.UtcNow;
            return true;
        }

        public bool TryGetFeed(string feedId, out SurveillanceFeed feed)
        {
            return feeds.TryGetValue(feedId, out feed);
        }

        public IReadOnlyCollection<SurveillanceFeed> GetFeeds()
        {
            return feeds.Values;
        }

        public int Count()
        {
            return feeds.Count;
        }

        public void Clear()
        {
            feeds.Clear();
        }
    }
}